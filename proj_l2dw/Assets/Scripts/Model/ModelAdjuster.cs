using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class ModelAdjuster : ModelAdjusterBase
{
    public override bool SupportAnimationMode => true;
    public override bool SupportExpressionMode => true;
    public override bool HasMotions => true;
    public override string Name => meta.name;
    public override string MotionTemplate => meta.formatText;
    public override string TransformTemplate => meta.transformFormatText;

    private float rootScale = 1;
    private float rootRotation = 0;

    [SerializeField] private Transform root;

    public override Vector3 RootPosition
    {
        get => root.localPosition;
    }

    public override Vector3 RootScale
    {
        get => pivot.localScale;
    }

    public override float RootRotation
    {
        get => rootRotation;
    }

    public override float RootScaleValue
    {
        get => rootScale;
    }

    public override Transform MainPos => webgalPoses[0].transform;
    public override int ModelCount => webgalPoses.Count;

    public override ModelDisplayMode DisplayMode => MainModel.displayMode;

    public override string curExpName => MainModel.curExpName;
    public override string curMotionName => MainModel.curMotionName;

    public override List<ExpPair> ExpPairs => MainModel.expPairs;
    public override List<MotionPair> MotionPairs => MainModel.motionPairs;

    public override MygoExp CurExp => MainModel.expMgr.curExp;
    public override MygoConfig MyGOConfig => MainModel.myGOConfig;
    [SerializeField] private Transform pivot;
    [SerializeField] private WebGalModelPos webgalPosPrefab;
    public MyGOLive2DEx MainModel => webgalPoses.Count > 0 ? webgalPoses[0].model : null;
    private List<WebGalModelPos> webgalPoses = new List<WebGalModelPos>();

    private RenderTexture rt;
    private float canvasResolutionScale = 1.0f;
    private Matrix4x4 modelMatrix;
    private FieldInfo canvasHackField;

    public EmotionEditor emotionEditor = new();

    private void Start()
    {
        this.canvasHackField =
            typeof(Canvas).GetField("willRenderCanvases", BindingFlags.NonPublic | BindingFlags.Static);
    }

    public override Live2DParamInfoList GetEmotionEditorList()
    {
        return emotionEditor.list;
    }

    public override void InitTransform(Vector3 pos, float scale, float rotation, bool reverseXScale)
    {
        this.reverseXScale = reverseXScale;
        SetScale(scale);
        SetRotation(rotation);
        SetPosition(pos.x, pos.y);
    }

    public override void PlayMotion(string name)
    {
        foreach (var pos in webgalPoses)
        {
            var model = pos.model;
            model.curMotionName = name;
            model.PlayMotion(name);
        }
    }

    public override void PlayExp(string name)
    {
        foreach (var pos in webgalPoses)
        {
            var model = pos.model;
            model.curExpName = name;
            model.PlayExp(name);
        }
    }

    public override bool IsMotionParamSetContains(string name)
    {
        return emotionEditor.paramSet.Contains(name);
    }

    public override float GetMotionParamValue(string name)
    {
        return emotionEditor.paramApplyDict[name];
    }

    public override void AddMotionParamControl(string name)
    {
        emotionEditor.AddControl(name);
    }

    public override void RemoveMotionParamControl(string name)
    {
        emotionEditor.RemoveControl(name);
    }

    public override void SetMotionParamValue(string name, float value)
    {
        emotionEditor.SetParam(name, value);
    }

    public override void ApplyMotionParamValue()
    {
        foreach (var pos in webgalPoses)
        {
            var model = pos.model;
            emotionEditor.ApplyValue(model.Live2DModel);
        }
    }

    public void ApplyParamDefaultValues()
    {
        foreach (var pos in webgalPoses)
        {
            var model = pos.model;
            emotionEditor.ApplyModelDefaultValues(model.Live2DModel);
        }
    }

    public override void CopyFromExp(MygoExp exp)
    {
        emotionEditor.CopyFromExp(exp);
        ApplyMotionParamValue();
    }

    public override string GetMotionEditorExpJson()
    {
        return emotionEditor.ToMygoExpJson().PrintJson();
    }

    public override void Sample(string paramName, float value)
    {
        foreach (var pos in webgalPoses)
        {
            pos.model.Live2DModel.setParamFloat(paramName, value);
        }
    }

    public override void SampleDefaultParam(string paramName)
    {
        var list = emotionEditor.list;
        foreach (var pos in webgalPoses)
        {
            if (list.paramDefDict.TryGetValue(paramName, out var value))
            {
                pos.model.Live2DModel.setParamFloat(paramName, value);
            }
        }
    }

    private void SetModelDisplayMode(ModelDisplayMode mode)
    {
        foreach (var pos in webgalPoses)
        {
            pos.model.displayMode = mode;
        }
    }

    public override void SetDisplayMode(ModelDisplayMode mode, bool force = false)
    {
        if (MainModel.displayMode == mode && !force)
        {
            return;
        }

        var curMotionName = MainModel.curMotionName;
        var curExpName = MainModel.curExpName;

        SetModelDisplayMode(mode);

        if (mode == ModelDisplayMode.Normal)
        {
            emotionEditor.Reset();
            foreach (var pos in webgalPoses)
            {
                var model = pos.model;
                emotionEditor.ApplyValue(model.Live2DModel);
                if (!string.IsNullOrEmpty(curMotionName))
                {
                    model.PlayMotion(curMotionName);
                }

                if (!string.IsNullOrEmpty(curExpName))
                {
                    model.PlayExp(curExpName);
                }
            }
        }
        else if (mode == ModelDisplayMode.EmotionEditor)
        {
            emotionEditor.Reset();
            ApplyParamDefaultValues();
        }
        else if (mode == ModelDisplayMode.MotionEditor)
        {
            emotionEditor.Reset();
            ApplyParamDefaultValues();
        }
    }

    public void ApplyModelInitOpacities()
    {
        foreach (var pos in webgalPoses)
        {
            var model = pos.model;
            foreach (var part in model.m_partsDataList)
            {
                var partKey = part.getPartsDataID().ToString();
                if (model.myGOConfig.init_opacities.TryGetValue(partKey, out var opacity))
                {
                    model.Live2DModel.setPartsOpacity(partKey, opacity);
                }
                else
                {
                    model.Live2DModel.setPartsOpacity(partKey, 1);
                }
            }
        }
    }

    public override void ReloadTextures()
    {
        foreach (var pos in webgalPoses)
        {
            var model = pos.model;
            model.ReloadTextures();
        }
    }

    public override void ReloadModels()
    {
        var expName = curExpName;
        var motionName = curMotionName;
        CreateModel();
        Adjust();
        InitParamInfoList();
        PlayExp(expName);
        PlayMotion(motionName);
    }

    private void InitParamInfoList()
    {
        emotionEditor.list.CombineParamInfoList(webgalPoses.Select(pos => pos.model.Live2DModel));
        emotionEditor.list.CombineInitParams(webgalPoses.Select(pos => pos.model.myGOConfig));

        foreach (var pos in webgalPoses)
        {
            emotionEditor.list.ApplyInitParamsToModel(pos.model);
        }
    }

    public override string GetMotionExpressionParamsText()
    {
        return MainModel.GetOutputText();
    }
    
    public override string GetBoundsText()
    {
        if (meta == null)
        {
            Debug.LogError("Meta 为空，无法获取 Bounds 文本");
            return "";
        }
        // 检查 live2dBounds 是否全为 0
        if (
            meta.live2dBounds[0] == 0
            && meta.live2dBounds[1] == 0
            && meta.live2dBounds[2] == 0
            && meta.live2dBounds[3] == 0
            )
        {
            return "";
        }
        return $"{meta.live2dBounds[0]},{meta.live2dBounds[1]},{meta.live2dBounds[2]},{meta.live2dBounds[3]}";
    }

    public override void CreateModel()
    {
        foreach (var pos in webgalPoses)
        {
            Destroy(pos.gameObject);
        }

        webgalPoses.Clear();

        WebGalModelPos CreateWebGalModelPos(string modelFilePath)
        {
            if (!File.Exists(modelFilePath))
            {
                Debug.LogError($"模型文件不存在: {modelFilePath}");
                return null;
            }

            var config = Live2dLoadUtils.LoadConfig(modelFilePath);
            if (config == null)
            {
                return null;
            }

            var pos = Instantiate(webgalPosPrefab);
            var model = pos.model;
            model.live2dBounds = this.meta.live2dBounds; // 必须立即设置 bounds, 因为 LoadConfig 要用
            model.LoadConfig(config);
            model.transform.localScale = Vector3.one;
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.Euler(0, 0, 0);
            model.meshRenderer.gameObject.SetActive(false);

            pos.transform.SetParent(pivot);
            pos.gameObject.SetActive(true);
            return pos;
        }

        var mainModel = CreateWebGalModelPos(meta.GetValidModelFilePath(0));
        if (mainModel == null)
        {
            return;
        }

        webgalPoses.Add(mainModel);
        for (int i = 0; i < meta.modelFilePaths.Count; i++)
        {
            webgalPoses.Add(CreateWebGalModelPos(meta.GetValidModelFilePath(i + 1)));
        }

        webgalPoses.RemoveAll(pos => pos == null);
        InitParamInfoList();

        if (this.rt)
        {
            rt.Release();
        }

        this.rt = new RenderTexture(
            (int)(MainModel.getModifiedWidth() * canvasResolutionScale),
            (int)(MainModel.getModifiedHeight() * canvasResolutionScale),
            0
        );
        MainModel.meshRenderer.material.mainTexture = this.rt;
        MainModel.meshRenderer.gameObject.SetActive(true);
        this.modelMatrix = Matrix4x4.Ortho(
            meta.live2dBounds[0],
            MainModel.Live2DModel.getCanvasWidth() + meta.live2dBounds[2],
            MainModel.Live2DModel.getCanvasHeight() + meta.live2dBounds[3],
            meta.live2dBounds[1],
            -500.0f,
            500.0f
        );
        UpdateAllFilter();
    }

    public override void Adjust()
    {
        if (Global.PivotMode == Global.FigurePivotMode.W_4_5_12)
        {
            pivot.transform.localPosition = Vector3.zero;
        }

        for (int i = 0; i < webgalPoses.Count; i++)
        {
            WebGalModelPos pos = webgalPoses[i];
            meta.GetModelOffset(i, out var offsetX, out var offsetY);
            pos.Adjust(offsetX, offsetY);
        }

        if (Global.PivotMode != Global.FigurePivotMode.W_4_5_12)
        {
            var mainPos = MainPos.localPosition;
            for (int i = 0; i < webgalPoses.Count; i++)
            {
                WebGalModelPos pos = webgalPoses[i];
                pos.transform.localPosition -= mainPos;
            }

            pivot.transform.localPosition = mainPos;
        }

        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        transform.position = new Vector3(Constants.WebGalWidth * 0.01f * 0.5f * -1f,
            Constants.WebGalHeight * 0.01f * 0.5f * 1f, 0);
    }

    public override void SetPosition(float x, float y)
    {
        root.localPosition = new Vector3(x, y, 0);
    }

    public override void SetScale(float scale)
    {
        rootScale = scale;
        pivot.localScale = new Vector3(reverseXScale ? -rootScale : rootScale, rootScale, 1);
        UpdateBlurFilter();
    }

    public override void SetReverseXScale(bool reverse)
    {
        if (reverseXScale == reverse)
            return;

        var oldPos = MainPos.position;

        reverseXScale = reverse;
        pivot.localScale = new Vector3(reverseXScale ? -rootScale : rootScale, rootScale, 1);

        SetCharacterWorldPosition(oldPos.x, oldPos.y);
    }

    public override void SetRotation(float rotation)
    {
        rootRotation = rotation;
        pivot.localRotation = Quaternion.Euler(0, 0, rotation);
    }

    private Vector3 GetCharacterWorldPosition(float worldX, float worldY, Transform child)
    {
        // 计算 b 当前的世界坐标相对于父物体 a 的偏移量
        float offsetX = child.position.x - root.position.x;
        float offsetY = child.position.y - root.position.y;

        // 计算新的父物体 a 的位置
        float newAPositionX = worldX - offsetX;
        float newAPositionY = worldY - offsetY;

        // 返回新的世界坐标
        return new Vector3(newAPositionX, newAPositionY, root.position.z);
    }

    public override void SetCharacterWorldPosition(float worldX, float worldY)
    {
        root.position = GetCharacterWorldPosition(worldX, worldY, MainPos);
    }

    public override Vector3 GetCharacterSpecWorldPosition(int modelIndex)
    {
        var trans = webgalPoses[modelIndex].transform;
        var worldPos = trans.position;
        return root.parent.InverseTransformPoint(GetCharacterWorldPosition(worldPos.x, worldPos.y, MainPos.transform));
    }

    public override float GetWebGalRotation()
    {
        return -rootRotation * Mathf.PI / 180;
    }

    public override void CopyRotationFromRoot()
    {
        rootRotation = pivot.localRotation.eulerAngles.z;
    }

    public override void CopyScaleFromRoot()
    {
        pivot.localScale = Vector3.Scale(pivot.localScale, root.localScale);
        root.localScale = Vector3.one;
        rootScale = pivot.localScale.y;
    }

    public override void BeforeGroupTransform(Transform parent)
    {
        root.parent = parent;
    }

    public override void AfterGroupTransform(float rotationDelta)
    {
        var oldPos = MainPos.position;
        root.parent = transform;
        root.eulerAngles = Vector3.zero;
        SetRotation(rootRotation + rotationDelta);
        CopyScaleFromRoot();
        SetCharacterWorldPosition(oldPos.x, oldPos.y);
    }

    public override void DrawLive2D()
    {
        var camera = Camera.main;
        if (camera == null)
        {
            Debug.LogError("Camera.main is null");
            return;
        }

        if (MainModel == null)
        {
            return;
        }

        foreach (var pos in webgalPoses)
        {
            pos.model.UpdateLive2D();
            pos.model.isMainRenderLoop = false;
        }

        var camPreLayer = camera.cullingMask;
        var goPreLayer = gameObject.layer;
        camera.cullingMask = LayerMask.NameToLayer("Isolate");
        gameObject.layer = LayerMask.NameToLayer("Isolate");
        camera.targetTexture = rt;
        camera.projectionMatrix = this.modelMatrix;

        var canvasHackObject = canvasHackField.GetValue(null);
        canvasHackField.SetValue(null, null);
        camera.Render();
        canvasHackField.SetValue(null, canvasHackObject);

        camera.cullingMask = camPreLayer;
        gameObject.layer = goPreLayer;
        camera.targetTexture = null;
        camera.ResetProjectionMatrix();

        foreach (var pos in webgalPoses)
        {
            pos.model.isMainRenderLoop = true;
        }
    }

    #region 滤镜

    public override void OnFilterSetDataChanged(FilterProperty property)
    {
        switch (property)
        {
            case FilterProperty.Alpha:
            {
                UpdateAlphaFilter();
                break;
            }
            case FilterProperty.Blur:
            {
                UpdateBlurFilter();
                break;
            }
            case FilterProperty.Adjustment:
            {
                UpdateAdjustmentFilter();
                break;
            }
            case FilterProperty.Bloom:
            {
                UpdateBloomFilter();
                break;
            }
            case FilterProperty.Bevel:
            {
                UpdateBevelFilter();
                break;
            }
        }
    }

    // 更新屏幕尺寸相关参数
    private void UpdateScreenParams()
    {
        var mat = MainModel.meshRenderer.material;
        var modelAspect = MainModel.getModifiedWidth() / MainModel.getModifiedHeight();
        var pivotScale = 1.0f;
        switch (Global.PivotMode)
        {
            case Global.FigurePivotMode.W_4_5_12:
            {
                pivotScale = 1f / 1.5f;
                break;
            }
            case Global.FigurePivotMode.W_4_5_13:
            {
                pivotScale = 1.0f;
                break;
            }
            case Global.FigurePivotMode.BC_1_0:
            {
                pivotScale = 1.0f / 1.25f;
                break;
            }
        }

        FilterUtils.UpdateScreenParams(mat, modelAspect, RootScaleValue, pivotScale, false);
    }

    private void UpdateAlphaFilter()
    {
        var mat = MainModel.meshRenderer.material;
        FilterUtils.UpdateAlphaFilter(mat, filterSetData);
    }

    private void UpdateBlurFilter()
    {
        var mat = MainModel.meshRenderer.material;
        FilterUtils.UpdateBlurFilter(mat, filterSetData);
    }

    private void UpdateAdjustmentFilter()
    {
        var mat = MainModel.meshRenderer.material;
        FilterUtils.UpdateAdjustmentFilter(mat, filterSetData);
    }

    private void UpdateBloomFilter()
    {
        var mat = MainModel.meshRenderer.material;
        FilterUtils.UpdateBloomFilter(mat, filterSetData);
    }

    private void UpdateBevelFilter()
    {
        var mat = MainModel.meshRenderer.material;
        FilterUtils.UpdateBevelFilter(mat, filterSetData);
    }

    public override void UpdateAllFilter()
    {
        UpdateScreenParams();
        UpdateAlphaFilter();
        UpdateBlurFilter();
        UpdateAdjustmentFilter();
        UpdateBloomFilter();
        UpdateBevelFilter();
    }

    #endregion

    public void SaveImage()
    {
        var tex2d = new Texture2D(rt.width, rt.height);
        RenderTexture.active = rt;
        tex2d.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex2d.Apply();
        RenderTexture.active = null;
        var bytes = tex2d.EncodeToPNG();
        string fileName = $"{meta.name}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.png";
        var path = Path.Combine(Application.dataPath, "..", "Snapshots", fileName);
        //确保目录存在
        var dir = Path.GetDirectoryName(path);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        File.WriteAllBytes(path, bytes);
        Debug.Log($"Save image to {path}");
    }

    public override Texture GetCharaTexture()
    {
        return rt;
    }
}
