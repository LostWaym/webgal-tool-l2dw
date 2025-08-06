


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using live2d;
using live2d.framework;
using UnityEngine;

public class MyGOLive2DEx : MonoBehaviour
{
    private Live2DModelUnity live2DModel;
    public Live2DModelUnity Live2DModel => live2DModel;
    private L2DPhysics physics;
    
    public List<MotionPair> motionPairs = new List<MotionPair>();
    public MotionQueueManager motionMgr;
    
    public List<ExpPair> expPairs = new List<ExpPair>();
    public ExpQueueManager expMgr;

    private MyGOBlink eyeBlink = new MyGOBlink();

    public string curMotionName, curExpName;

    public ModelDisplayMode displayMode = ModelDisplayMode.Normal;

    public float left, up;
    
    public MygoConfig myGOConfig;
    
    // 是否为主渲染循环
    public bool isMainRenderLoop = true;
    public MeshRenderer meshRenderer;

    public List<PartsData> m_partsDataList = new List<PartsData>();
    
    public void LoadConfig(MygoConfig config)
    {
        myGOConfig = config;
        live2DModel = Live2DModelUnity.loadModel(config.model);
        for (int i = 0; i < config.textures.Count; i++)
        {
            var texture = config.textures[i];
            live2DModel.setTexture(i, texture);
        }
        physics = null;
        if (config.physics != null)
        {
            physics = L2DPhysics.load(config.physics);
        }
        motionMgr = new MotionQueueManager();
        expMgr = new ExpQueueManager();
        LoadMotionPairs(config);
        LoadExpPairs(config);

        var plane = meshRenderer.transform;
        plane.localPosition = new Vector3(
            live2DModel.getCanvasWidth(),
            live2DModel.getCanvasHeight() * -1.0f,
            0.0f
        );
        plane.localRotation = Quaternion.Euler(90.0f, 0.0f, 180.0f);
        plane.localScale = new Vector3(
            live2DModel.getCanvasWidth() * 0.2f,
            1.0f,
            live2DModel.getCanvasHeight() * 0.2f
        );

        InitPartsDataList();
        ApplyInitOpacities(live2DModel);
    }

    private void InitPartsDataList()
    {
        var type = live2DModel.getModelContext().GetType();
        var field = type.GetField("partsDataList", BindingFlags.NonPublic | BindingFlags.Instance);
        m_partsDataList = field.GetValue(live2DModel.getModelContext()) as List<PartsData>;
    }

    private void ApplyInitOpacities(ALive2DModel model)
    {
        foreach (var item in m_partsDataList)
        {
            var id = item.getPartsDataID();
            if (myGOConfig.init_opacities.TryGetValue(id.ToString(), out var value))
            {
                model.setPartsOpacity(id.ToString(), value);
            }
        }
    }

    public void ReloadTextures()
    {
        myGOConfig.ReloadTextures();
        for (int i = 0; i < myGOConfig.textures.Count; i++)
        {
            var texture = myGOConfig.textures[i];
            live2DModel.setTexture(i, texture);
        }
    }

    private void LoadMotionPairs(MygoConfig config)
    {
        motionPairs.Clear();
        foreach (var item in config.motions)
        {
            motionPairs.Add(new MotionPair()
            {
                name = item.Key,
                motion = Live2DMotion.loadMotion(item.Value)
            });
        }
        motionPairs.Sort((a, b) =>
        {
            return a.name.CompareTo(b.name);
        });
    }

    private void LoadExpPairs(MygoConfig config)
    {
        expPairs.Clear();
        foreach (var item in config.expressions)
        {
            expPairs.Add(new ExpPair()
            {
                name = item.Key,
                exp = new MygoExp()
                {
                    data = item.Value,
                }
            });
        }
        expPairs.Sort((a, b) =>
        {
            return a.name.CompareTo(b.name);
        });
    }

    void Start()
    {
    }

    public void UpdateLive2D()
    {
        if (live2DModel == null)
            return;
        
        if (!Application.isPlaying)
        {
            live2DModel.update();
            return;
        }
        
        live2DModel.setMatrix(
            Matrix4x4.TRS(
                new Vector3(left, up, 0),
                Quaternion.identity,
                Vector3.one
            )
        );
        
        if (displayMode == ModelDisplayMode.Normal)
            NormalUpdate();
        else if (displayMode == ModelDisplayMode.EmotionEditor)
            EmotionEditorUpdate();
        else if (displayMode == ModelDisplayMode.MotionEditor)
            MotionUpdate();
    }

    private void NormalUpdate()
    {
        live2DModel.loadParam();
        motionMgr.updateParam(live2DModel);
        live2DModel.saveParam();

        expMgr.updateParam(live2DModel);
        if (MainControl.AllowBlink)
            eyeBlink.setParam(live2DModel);

        if (physics != null)
            physics.updateParam(live2DModel);

        double timeSec = UtSystem.getUserTimeMSec() / 1000.0;
        double t = timeSec * 2 * Math.PI;
        live2DModel.setParamFloat("PARAM_BREATH", (float)(0.5f + 0.5f * Math.Sin(t / 3.0)));

        live2DModel.update();
    }

    private void MotionUpdate()
    {
        live2DModel.update();
    }

    private void EmotionEditorUpdate()
    {
        live2DModel.update();
    }

    void OnRenderObject()
    {
        if (
            isMainRenderLoop
            || live2DModel == null
            || live2DModel.getRenderMode() != Live2D.L2D_RENDER_DRAW_MESH_NOW
            )
            return;
    
        live2DModel.draw();
    }

    public void PlayMotion(string name)
    {
        var pair = motionPairs.Find(x => x.name == name);
        if (pair == null)
        {
            return;
        }
        motionMgr.startMotion(pair.motion);
    }

    public void PlayExp(string name)
    {
        var pair = expPairs.Find(x => x.name == name);
        if (pair == null)
        {
            return;
        }
        expMgr.startExp(pair.exp, live2DModel);
    }
}

public static class MyGOLive2DExExtensions
{
    public static string GetOutputText(this MyGOLive2DEx mygo)
    {
        List<string> list = new List<string>();
        if (!string.IsNullOrEmpty(mygo.curMotionName))
        {
            list.Add($"-motion={mygo.curMotionName}");
        }
        if (!string.IsNullOrEmpty(mygo.curExpName))
        {
            list.Add($"-expression={mygo.curExpName}");
        }
        string output = string.Join(" ", list);
        return output;
    }
}

public class Live2DParamInfo
{
    public string name;
    public float min;
    public float max;
    public float value;
}

public class Live2DParamInfoList
{
    public List<Live2DParamInfo> list = new List<Live2DParamInfo>();
    public Dictionary<string, float> paramDefDict = new Dictionary<string, float>();
    public Dictionary<string, float> realParamDefDict = new Dictionary<string, float>();

    public void ReadFrom(ALive2DModel model)
    {
        list.Clear();
        paramDefDict.Clear();
        var context = model.getModelContext();
        var paramIDList = GetParamIDListReflection(context);
        foreach (var paramID in paramIDList)
        {
            if (paramID == null)
                continue;

            var paramIndex = context.getParamIndex(paramID);
            var param = context.getParamFloat(paramIndex);
            list.Add(new Live2DParamInfo()
            {
                name = paramID.ToString(),
                min = context.getParamMin(paramIndex),
                max = context.getParamMax(paramIndex),
                value = param,
            });
            paramDefDict.Add(paramID.ToString(), param);
        }
    }

    public void CombineParamInfoList(IEnumerable<ALive2DModel> models)
    {
        list.Clear();
        paramDefDict.Clear();
        realParamDefDict.Clear();

        HashSet<string> paramSet = new HashSet<string>();

        foreach (var model in models)
        {
            var context = model.getModelContext();
            var paramIDList = GetParamIDListReflection(context);
            foreach (var paramID in paramIDList)
            {
                if (paramID == null)
                    continue;

                if (paramSet.Contains(paramID.ToString()))
                    continue;

                paramSet.Add(paramID.ToString());

                var paramIndex = context.getParamIndex(paramID);
                var param = context.getParamFloat(paramIndex);
                list.Add(new Live2DParamInfo()
                {
                    name = paramID.ToString(),
                    min = context.getParamMin(paramIndex),
                    max = context.getParamMax(paramIndex),
                    value = param,
                });
                paramDefDict.Add(paramID.ToString(), param);
                realParamDefDict.Add(paramID.ToString(), param);
            }
        }
    }

    public void SetDefParam(string name, float value)
    {
        paramDefDict[name] = value;
    }

    public void RemoveDefParam(string name)
    {
        if (realParamDefDict.TryGetValue(name, out var value))
        {
            paramDefDict[name] = value;
        }
    }

    public void ApplyInitParams(MygoConfig config)
    {
        foreach (var item in config.init_params)
        {
            paramDefDict[item.Key] = item.Value;
        }
    }

    public void CombineInitParams(IEnumerable<MygoConfig> configs)
    {
        Dictionary<string, float> dict = new Dictionary<string, float>();
        foreach (var config in configs)
        {
            foreach (var item in config.init_params)
            {
                if (dict.ContainsKey(item.Key))
                    continue;

                dict[item.Key] = item.Value;
            }
        }

        foreach (var item in dict)
        {
            paramDefDict[item.Key] = item.Value;
        }
    }

    private List<ParamID> GetParamIDListReflection(ModelContext context)
    {
        var type = context.GetType();
        var field = type.GetField("floatParamIDList", BindingFlags.NonPublic | BindingFlags.Instance);
        if (field == null)
            return new List<ParamID>();

        var list = field.GetValue(context) as ParamID[];
        return list.ToList();
    }
}

public class AnimationEditor
{

}

public class EmotionEditor
{
    public Live2DParamInfoList list = new();

    public Dictionary<string, float> paramApplyDict = new Dictionary<string, float>();
    public HashSet<string> paramSet = new HashSet<string>();
    public Dictionary<string, string> paramCalcDict = new Dictionary<string, string>();

    public void Start()
    {
        Reset();
    }

    public void Reset()
    {
        foreach (var item in list.list)
        {
            paramApplyDict[item.name] = list.paramDefDict[item.name];
        }

        paramSet.Clear();
        paramCalcDict.Clear();
    }

    public void SetParam(string name, float value)
    {
        paramApplyDict[name] = value;
    }

    public void SetDefaultParam(string name)
    {
        if (list.paramDefDict.TryGetValue(name, out var value))
        {
            paramApplyDict[name] = value;
        }
    }

    public void AddControl(string name)
    {
        paramSet.Add(name);
        SetCalcType(name, MygoExp.CALC_TYPE_SET);
    }

    public void RemoveControl(string name)
    {
        paramSet.Remove(name);
        SetDefaultParam(name);
    }

    public string GetCalcType(string name)
    {
        if (paramCalcDict.TryGetValue(name, out var value))
        {
            return value;
        }

        return MygoExp.CALC_TYPE_ADD;
    }

    public void SetCalcType(string name, string type)
    {
        paramCalcDict[name] = type;
    }

    public void CopyFromExp(MygoExp exp)
    {
        Reset();
        foreach (var item in exp.data.keyDatas)
        {
            AddControl(item.id);
            SetParam(item.id, item.val);
            SetCalcType(item.id, item.calc);
        }
    }

    public void ApplyValue(ALive2DModel model)
    {
        foreach (var item in paramApplyDict)
        {
            var inSet = paramSet.Contains(item.Key);
            float value;
            if (inSet)
            {
                value = item.Value;
            }
            else if (!list.paramDefDict.TryGetValue(item.Key, out value))
            {
                continue;
            }

            string calcType = inSet ? GetCalcType(item.Key) : MygoExp.CALC_TYPE_SET;
            MygoExp.Apply(model, item.Key, calcType, value, 1.0f);
        }
    }

    public void ApplyModelDefaultValues(ALive2DModel model)
    {
        foreach (var item in list.paramDefDict)
        {
            MygoExp.Apply(model, item.Key, MygoExp.CALC_TYPE_SET, item.Value, 1.0f);
        }
    }

    public MygoExpJson ToMygoExpJson()
    {
        var ret = new MygoExpJson()
        {
            type = "Live2D Expression",
            fade_in = 500,
            fade_out = 500,
            keyDatas = new List<MygoExpKeyData>()
        };

        foreach (var item in paramApplyDict)
        {
            if (paramSet.Contains(item.Key))
            {
                var data = new MygoExpKeyData()
                {
                    id = item.Key,
                    val = item.Value,
                };
                if (paramCalcDict.TryGetValue(item.Key, out var type))
                {
                    data.calc = type;
                }
                else
                {
                    data.calc = MygoExp.CALC_TYPE_DEFAULT;
                }

                ret.keyDatas.Add(data);
            }
        }

        return ret;
    }
}

public enum ModelDisplayMode
{
    Normal,
    EmotionEditor,
    MotionEditor,
}