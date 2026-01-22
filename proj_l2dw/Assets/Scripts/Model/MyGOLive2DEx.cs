


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
    public SpeakTween speakTween = new SpeakTween();
    
    // 是否为主渲染循环
    public bool isMainRenderLoop = true;
    public MeshRenderer meshRenderer;

    public List<PartsData> m_partsDataList = new List<PartsData>();
    // Live2D 参数默认信息列表
    public List<Live2DParamInfo> defaultParamInfoList = new List<Live2DParamInfo>();

    // Live2D模型的边界框
    // Todo：应当找一个更合理的方式来获取模型的边界框
    public float[] live2dBounds = new float[] { 0, 0, 0, 0 }; // left, top, right, bottom

    public Action<MyGOLive2DEx> onModelDisplayParamSet = null;
    
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
            getModifiedWidth(),
            getModifiedHeight() * -1.0f,
            0.0f
        );
        plane.localRotation = Quaternion.Euler(90.0f, 0.0f, 180.0f);
        plane.localScale = new Vector3(
            getModifiedWidth() * 0.2f,
            1.0f,
            getModifiedHeight() * 0.2f
        );

        InitPartsDataList();
        ApplyInitOpacities(live2DModel);
        RefreshDefaultParamInfoList();
        ResetAllParams();
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
    
    private void RefreshDefaultParamInfoList()
    {
        var context = live2DModel.getModelContext();
        var contextType = context.GetType();
        var idListField = contextType.GetField("floatParamIDList", BindingFlags.NonPublic | BindingFlags.Instance);
        if (idListField == null)
            return;

        var list = idListField.GetValue(context) as ParamID[];
        if (list == null)
            return;

        foreach (var paramID in list)
        {
            if (paramID == null)
                continue;

            var paramIndex = context.getParamIndex(paramID);
            
            defaultParamInfoList.Add(new Live2DParamInfo()
            {
                name = paramID.ToString(),
                min = context.getParamMin(paramIndex),
                max = context.getParamMax(paramIndex),
                value = context.getParamFloat(paramIndex),
            });
        }
    }
    
    public void ResetAllParams()
    {
        foreach (var item in defaultParamInfoList)
        {
            if (myGOConfig.init_params.TryGetValue(item.name, out var value))
            {
                live2DModel.setParamFloat(item.name, value);
            } else
            {
                live2DModel.setParamFloat(item.name, item.value);
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

    public void ReloadTexturesIfDirty()
    {
        if (myGOConfig.IsFileTimeHashDirty())
        {
            ReloadTextures();
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

    public void Speak(float expiredTime)
    {
        speakTween.expiredTime = expiredTime;
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
        speakTween.Update(live2DModel);

        onModelDisplayParamSet?.Invoke(this);

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
    
    // 获取 Live2D 模型更改边界后的宽度
    public float getModifiedWidth()
    {
        if (live2DModel == null)
        {
            Debug.LogError("模型不存在，无法获取宽度");
            return 0.0f;
        }

        return live2DModel.getCanvasWidth() - live2dBounds[0] + live2dBounds[2];
    }
    
    // 获取 Live2D 模型更改边界后的高度
    public float getModifiedHeight()
    {
        if (live2DModel == null)
        {
            Debug.LogError("模型不存在，无法获取高度");
            return 0.0f;
        }

        return live2DModel.getCanvasHeight() - live2dBounds[1] + live2dBounds[3];
    }

    public JSONObject FixOpacitiesJsonObject(JSONObject opacities)
    {
        var clone = opacities.Copy();
        var partsNames = m_partsDataList.Select(x => x.getPartsDataID().ToString()).ToHashSet();
        clone.list.RemoveAll(item => !partsNames.Contains(item.GetField("id").str));
        return clone;
    }

    public JSONObject FixParamsJsonObject(JSONObject paramsObject)
    {
        var clone = paramsObject.Copy();
        var paramNames = defaultParamInfoList.Select(x => x.name).ToHashSet();
        clone.list.RemoveAll(item => !paramNames.Contains(item.GetField("id").str));
        return clone;
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
    public Dictionary<string, Live2DParamInfo> paramInfoDict = new Dictionary<string, Live2DParamInfo>();
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
            var paramInfo = new Live2DParamInfo()
            {
                name = paramID.ToString(),
                min = context.getParamMin(paramIndex),
                max = context.getParamMax(paramIndex),
                value = param,
            };
            list.Add(paramInfo);
            paramInfoDict[paramID.ToString()] = paramInfo;
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
                var paramInfo = new Live2DParamInfo()
                {
                    name = paramID.ToString(),
                    min = context.getParamMin(paramIndex),
                    max = context.getParamMax(paramIndex),
                    value = param,
                };
                list.Add(paramInfo);
                paramInfoDict[paramID.ToString()] = paramInfo;
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

    // public void ApplyInitParams(MygoConfig config)
    // {
    //     foreach (var item in config.init_params)
    //     {
    //         paramDefDict[item.Key] = item.Value;
    //     }
    // }

    private Dictionary<string, float> initParamsDict = new Dictionary<string, float>();
    public void CombineInitParams(IEnumerable<MygoConfig> configs)
    {
        initParamsDict.Clear();
        foreach (var config in configs)
        {
            foreach (var item in config.init_params)
            {
                if (initParamsDict.ContainsKey(item.Key))
                    continue;

                initParamsDict[item.Key] = item.Value;
            }
        }

        foreach (var item in initParamsDict)
        {
            paramDefDict[item.Key] = item.Value;
        }
    }

    public void ApplyInitParamsToModel(MyGOLive2DEx myGOLive2DEx)
    {
        foreach (var item in initParamsDict)
        {
            myGOLive2DEx.Live2DModel.setParamFloat(item.Key, item.Value);
            myGOLive2DEx.Live2DModel.saveParam();
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
            else
            {
                continue;
            }

            string calcType = inSet ? GetCalcType(item.Key) : MygoExp.CALC_TYPE_SET;
            MygoExp.Apply(model, item.Key, calcType, value, 1.0f);
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

public class SpeakTween
{
    public const string PARAM = "PARAM_MOUTH_OPEN_Y";
    public const float tInTime = 0.33f;
    public const float tOutTime = 0.33f;
    public bool IsSpeaking => Time.time < expiredTime;
    public float t;
    public float expiredTime;

    public void Update(ALive2DModel model)
    {
        if (IsSpeaking)
        {
            t += Time.deltaTime / tInTime;
            t = Mathf.Clamp01(t);
        }
        else
        {
            t -= Time.deltaTime / tOutTime;
            t = Mathf.Clamp01(t);
        }

        
        var value = model.getParamFloat(PARAM);
        // linear ping-pong between 0-1
        var time = Time.time;
        var tarValue = Mathf.PingPong(time * 4, 1);
        var finalValue = Mathf.Lerp(value, tarValue, t);
        model.setParamFloat(PARAM, finalValue);
    }
}