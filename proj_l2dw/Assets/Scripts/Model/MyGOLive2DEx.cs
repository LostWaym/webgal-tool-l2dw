


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using live2d;
using live2d.framework;
using UnityEngine;

public class MyGOLive2DEx : MonoBehaviour
{
    public bool externalHandleRender = false;
    private Live2DModelUnity live2DModel;
    public Live2DModelUnity Live2DModel => live2DModel;
    private L2DPhysics physics;
    
    public List<MotionPair> motionPairs = new List<MotionPair>();
    public MotionQueueManager motionMgr;
    
    public List<ExpPair> expPairs = new List<ExpPair>();
    public ExpQueueManager expMgr;

    private MyGOBlink eyeBlink = new MyGOBlink();

    public string curMotionName, curExpName;

    public Live2DParamInfoList paramInfoList = new Live2DParamInfoList();
    public EmotionEditor emotionEditor = new EmotionEditor();

    public ModelDisplayMode displayMode = ModelDisplayMode.Normal;

    public bool canRender;
    
    private Matrix4x4 live2DCanvasPos;

    public float modelWidth;
    public float left, up;

    public MygoConfig myGOConfig;
    
    // 是否为主渲染循环
    private bool isMainRenderLoop = true;
    [SerializeField] private MeshRenderer meshRenderer;
    private RenderTexture rt;
    private Matrix4x4 modelMatrix;
    private float canvasResolutionScale = 1.0f;
    private FieldInfo canvasHackField;

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
        paramInfoList.ReadFrom(live2DModel);
        emotionEditor.list = paramInfoList;
        
        Debug.Log($"width: {live2DModel.getCanvasWidth()}, height:{live2DModel.getCanvasHeight()}");
        
        this.modelMatrix = Matrix4x4.Ortho(
            0,
            live2DModel.getCanvasWidth(),
            live2DModel.getCanvasHeight(),
            0,
            -50.0f,
            50.0f
        );
        if (this.rt)
        {
            rt.Release();
        }
        this.rt = new RenderTexture(
            (int)(live2DModel.getCanvasWidth() * canvasResolutionScale),
            (int)(live2DModel.getCanvasHeight() * canvasResolutionScale),
            0
        );
        meshRenderer.material.mainTexture = this.rt;
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
        canRender = true;
        this.canvasHackField = typeof(Canvas).GetField("willRenderCanvases", BindingFlags.NonPublic | BindingFlags.Static);
    }

    void Update()
    {
        if (live2DModel == null)
            return;
        
        if (!Application.isPlaying)
        {
            live2DModel.update();
            return;
        }
        
        if (displayMode == ModelDisplayMode.Normal)
            NormalUpdate();
        else if (displayMode == ModelDisplayMode.EmotionEditor)
            EmotionEditorUpdate();
        else if (displayMode == ModelDisplayMode.MotionEditor)
            MotionUpdate();
        
        DrawLive2d();
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
        if (externalHandleRender)
            return;

        DoRender();
    }

    public void DoRender()
    {
        if (!canRender)
            return;

        if (live2DModel == null)
            return;

        if (live2DModel.getRenderMode() == Live2D.L2D_RENDER_DRAW_MESH_NOW && !isMainRenderLoop)
        {
            live2DModel.draw();
        }
    }

    private void DrawLive2d()
    {
        if (!canRender)
        {
            return;
        }
        var camera = Camera.main;
        if (camera == null)
        {
            Debug.LogError("Camera.main is null");
            return;
        }
        
        this.isMainRenderLoop = false;
        var camPreLayer = camera.cullingMask;
        var goPreLayer = gameObject.layer;
        camera.cullingMask = LayerMask.NameToLayer("Isolate");
        gameObject.layer = LayerMask.NameToLayer("Isolate");
        camera.targetTexture = this.rt;
        camera.projectionMatrix = this.modelMatrix;
        live2DModel.setMatrix(Matrix4x4.identity);
        live2DModel.update();
        
        var canvasHackObject = canvasHackField.GetValue(null);
        canvasHackField.SetValue(null, null);
        camera.Render();
        canvasHackField.SetValue(null, canvasHackObject);
        
        this.isMainRenderLoop = true;
        camera.cullingMask = camPreLayer;
        gameObject.layer = goPreLayer;
        camera.targetTexture = null;
        camera.ResetProjectionMatrix();
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

    private List<ParamID> GetParamIDListReflection(ModelContext context)
    {
        var type = context.GetType();
        var field = type.GetField("floatParamIDList", BindingFlags.NonPublic | BindingFlags.Instance);
        if (field == null)
            return new List<ParamID>();

        var list = field.GetValue(context) as ParamID[];
        return list.ToList();
    }

    public string GetAllParamInfoTextOutput()
    {
        string paramList = string.Join("\n", list.Select(x => $"{x.name}: {x.value} ({x.min} ~ {x.max})"));
        return $"����������: {list.Count}\n{paramList}";
    }
}

public class AnimationEditor
{

}

public class EmotionEditor
{
    public Live2DParamInfoList list;

    public Dictionary<string, float> paramApplyDict = new Dictionary<string, float>();
    public HashSet<string> paramSet = new HashSet<string>();

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
    }

    public void SetParam(string name, float value)
    {
        paramApplyDict[name] = value;
    }

    public void AddControl(string name)
    {
        paramSet.Add(name);
    }

    public void RemoveControl(string name)
    {
        paramSet.Remove(name);
        SetParam(name, list.paramDefDict[name]);
    }

    public void CopyFromExp(MygoExp exp)
    {
        Reset();
        foreach (var item in exp.data.keyDatas)
        {
            AddControl(item.id);
            SetParam(item.id, item.val);
        }
    }

    public void ApplyValue(ALive2DModel model)
    {
        foreach (var item in paramApplyDict)
        {
            var inSet = paramSet.Contains(item.Key);
            if (inSet)
            {
                model.setParamFloat(item.Key, item.Value);
            }
            else
            {
                model.setParamFloat(item.Key, list.paramDefDict[item.Key]);
            }
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
                ret.keyDatas.Add(new MygoExpKeyData()
                {
                    id = item.Key,
                    val = item.Value,
                });
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