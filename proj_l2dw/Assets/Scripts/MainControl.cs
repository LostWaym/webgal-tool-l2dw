using DG.Tweening;
using live2d;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainControl : MonoBehaviour
{
    public static MainControl Instance;
    public static event Action OnLoadConf;
    public static event Action OnMetaChanged;
    public static bool WebGalExpressionSupport = true;
    public static bool CloseGreenLine = false;
    public static bool AllowBlink = true;
    public static string ModelPath = "";
    public static string CurrentBGPath = "";

    public Camera mainCamera;
    public ModelAdjuster prefab;
    public ImageModel imgPrefab;
    public List<ModelAdjusterBase> models = new List<ModelAdjusterBase>();
    public ModelAdjusterBase curTarget;
    public MyGOLive2DExMeta curMeta => curTarget.meta;
    
    public Text debugText;

    public bool LockX, LockY;

    public bool showCanvases = true;
    public GameObject BG;
    public BGContainer bgContainer;
    public GameObject greenLine;

    public string filterMotion, filterExp;
    public List<string> filteredMotions = new List<string>();
    public List<string> filteredExps = new List<string>();

    public ModelGroup modelGroupPrefab;
    public ModelGroup curGroup;
    public List<ModelGroup> modelGroups = new List<ModelGroup>();
    public EditType editType = EditType.Model;

    // public bool isHighResolution;
    public int resolutionIndex = 0;
    public Vector2Int[] resolutions = new Vector2Int[] {
        new Vector2Int(1280, 720),
        new Vector2Int(1920, 1080),
        new Vector2Int(2560, 1440),
    };

    public GameObject canvasRoot;

    public Action UpdateBeat;

    void Awake()
    {
        Instance = this;
        resolutionIndex = PlayerPrefs.GetInt("resolutionIndex", 0);
        ModelPath = PlayerPrefs.GetString("ModelPath", "");
        AllowBlink = PlayerPrefs.GetInt("AllowBlink", 1) == 1;
        WebGalExpressionSupport = PlayerPrefs.GetInt("WebGalExpressionSupport", 1) == 1;
        CloseGreenLine = PlayerPrefs.GetInt("CloseGreenLine", 0) == 1;
        Global.Load();
        AddDefaultGroup();
        UpdateResolution();
        ProfileSettingWindow.onSaveAction += OnSaveSetting;
        ImageProfileSettingWindow.onSaveAction += OnSaveImageSetting;
        Live2D.init();
        LoadMainCamTransform();
        SetCloseGreenLine(CloseGreenLine);

        UIEventBus.AddListener(UIEventType.OnModelChanged, RenderBatch);
        UIEventBus.AddListener(UIEventType.OnModelDeleted, RenderBatch);

        FilterUtils.LoadFilterSetPreset();
    }

    private void OnSaveImageSetting(ImageModelMeta meta)
    {
        ShowDebugText("保存成功！");
        OnMetaChanged?.Invoke();
    }

    private void LoadMainCamTransform()
    {
        var mainCam = Camera.main;
        if (PlayerPrefs.HasKey("MainCam.X"))
        {
            var pos = new Vector3(
                PlayerPrefs.GetFloat("MainCam.X"),
                PlayerPrefs.GetFloat("MainCam.Y"),
                PlayerPrefs.GetFloat("MainCam.Z"));
            mainCam.transform.position = pos;
            mainCam.orthographicSize = PlayerPrefs.GetFloat("MainCam.Size", 5);
        }
    }

    private void SaveMainCamTransform()
    {
        var mainCam = Camera.main;
        var pos = mainCam.transform.position;
        PlayerPrefs.SetFloat("MainCam.X", pos.x);
        PlayerPrefs.SetFloat("MainCam.Y", pos.y); 
        PlayerPrefs.SetFloat("MainCam.Z", pos.z);
        PlayerPrefs.SetFloat("MainCam.Size", mainCam.orthographicSize);
    }

    void OnApplicationQuit()
    {
        if (!Global.IsLoaded)
            return;

        PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
        PlayerPrefs.SetString("ModelPath", ModelPath);
        PlayerPrefs.SetInt("AllowBlink", AllowBlink ? 1 : 0);
        PlayerPrefs.SetInt("WebGalExpressionSupport", WebGalExpressionSupport ? 1 : 0);
        PlayerPrefs.SetInt("CloseGreenLine", CloseGreenLine ? 1 : 0);
        Global.Save();
        SaveMainCamTransform();
        FilterUtils.SaveFilterSetPreset();
    }

    private void OnSaveSetting(MyGOLive2DExMeta meta)
    {
        if(curTarget == null)
            return;

        ShowDebugText("保存成功！");
        OnMetaChanged?.Invoke();
    }

    public void SetCloseGreenLine(bool close)
    {
        CloseGreenLine = close;
        greenLine.SetActive(!close);
    }

    public void SetResolution(int index)
    {
        resolutionIndex = Mathf.Clamp(index, 0, resolutions.Length - 1);
        UpdateResolution();
    }

    public void NextResolution()
    {
        resolutionIndex = (resolutionIndex + 1) % resolutions.Length;
        SetResolution(resolutionIndex);
    }

    public void PrevResolution()
    {
        resolutionIndex = (resolutionIndex - 1 + resolutions.Length) % resolutions.Length;
        SetResolution(resolutionIndex);
    }

    public void UpdateResolution()
    {
        var resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.x, resolution.y, FullScreenMode.Windowed);
    }

    private void AddDefaultGroup()
    {
        //添加一个默认的group
        ModelGroup defaultGroup = Instantiate(modelGroupPrefab);
        defaultGroup.groupName = "默认组";
        defaultGroup.gameObject.SetActive(true);
        modelGroups.Add(defaultGroup);
        SetGroup(defaultGroup);
    }

    public void SetCharacter(ModelAdjusterBase model)
    {
        if (model == null)
        {
            curTarget = null;
            UIEventBus.SendEvent(UIEventType.OnModelChanged);
            return;
        }
        curTarget = model;
        UpdateFilterContent();
        UIEventBus.SendEvent(UIEventType.OnModelChanged);
    }

    public void SetGroup(ModelGroup group)
    {
        if (group == null)
        {
            curGroup = null;
            return;
        }
        curGroup = group;
        group.RemoveInvalidModel();
    }

    public void ReloadModel(ModelAdjusterBase target = null)
    {
        if (target == null)
        {
            target = curTarget;
        }

        if (target == null)
        {
            ShowErrorDebugText("请先选择一个模型");
            return;
        }

        target.ReloadModels();
        ShowDebugText("重载模型成功！");
    }
    public bool AnyTopViewActive => settingWindow.IsShown || valueInputWindow.IsShown || settingUIWindow.IsShown || messageTipWindow.IsShown;

    public ProfileSettingWindow settingWindow;
    public ImageProfileSettingWindow imageSettingWindow;
    public SettingUIWindow settingUIWindow;
    public ValueInputWindow valueInputWindow;
    public MessageTipWindow messageTipWindow;
    public void ShowConfigEditor()
    {
        if (curTarget is ImageModel imageModel)
        {
            imageSettingWindow.gameObject.SetActive(true);
            imageSettingWindow.Load(imageModel.imgMeta);
        }
        else if (curTarget is ModelAdjuster modelAdjuster)
        {
            settingWindow.gameObject.SetActive(true);
            settingWindow.Load(modelAdjuster.meta);
        }
    }

    public void ShowSettingUIWindow()
    {
        settingUIWindow.gameObject.SetActive(true);
    }

    private Vector3 lastMousePos;

    public int freezeProcessInputFrame = 0;
    private void OnApplicationFocus(bool hasFocus)
    {
        freezeProcessInputFrame = 2;
    }
    

    public void LoadBackground(string path)
    {
        if(string.IsNullOrEmpty(path) || !File.Exists(path))
            return;

        if(!L2DWUtils.IsSubFolderOf(path, Global.BGPath))
        {
            messageTipWindow.Show("错误", "背景路径错误！\n你应该选择背景文件夹下的图片文件");
            return;
        }

        var texture = new Texture2D(2, 2);
        texture.LoadImage(File.ReadAllBytes(path));
        CurrentBGPath = path;
        bgContainer.LoadTexture(texture);

        Resources.UnloadUnusedAssets();
    }

    #region 快捷键，更新相关
    private void Update()
    {
        freezeProcessInputFrame--;
        canvasRoot.SetActive(showCanvases);

        if (Input.GetKeyDown(KeyCode.F2))
        {
            BG.SetActive(!BG.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            showCanvases = !showCanvases;
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            NextResolution();
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            ResetCamera();
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            TakeSnapshot();
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            TakeCharacterSnapShot();
        }

        UpdateBeat?.Invoke();
    }

    private void RenderBatch()
    {
        for (int i = models.Count - 1; i >= 0; i--)
        {
            ModelAdjusterBase model = models[i];
            model.ZValue = i * 0.25f;
        }
    }

    void LateUpdate()
    {
        for (int i = models.Count - 1; i >= 0; i--)
        {
            ModelAdjusterBase model = models[i];
            if (model.gameObject.activeInHierarchy)
                model.DrawLive2D();
        }
    }

    #endregion

    #region 指令输出相关

    #region 立绘

    public void CopyAllSpilt(bool motion = true, bool transform = true)
    {
        if (curTarget == null)
            return;

        var shouldMotion = motion && curTarget.HasMotions;

        if (shouldMotion)
        {
            if(string.IsNullOrEmpty(curTarget.curExpName) || string.IsNullOrEmpty(curTarget.curMotionName))
            {
                ShowErrorDebugText("请先设置表情和动作");
                return;
            }
        }

        StringBuilder commands = new StringBuilder();
        if (motion)
        {
            var format = curTarget.MotionTemplate;
            var output = format.Replace("%me%", curTarget.GetMotionExpressionParamsText());
            for (int i = 0; i < curTarget.ModelCount; i++)
            {
                output = output.Replace($"%me_{i}%", curTarget.GetMotionExpressionParamsText());
            }
            commands.AppendLine(output);
        }
        if (transform)
        {
            var format = curTarget.TransformTemplate;
            var output = format.Replace("%me%", GetTransformText());
            for (int i = 0; i < curTarget.ModelCount; i++)
            {
                output = output.Replace($"%me_{i}%", GetTransformTextTarget(curTarget, i));
            }
            commands.AppendLine(output);
        }
        L2DWUtils.CopyInstructionToCopyBoard(commands.ToString());
        ShowDebugText("复制成功！");
    }

    public void CopyAll()
    {
        if (curTarget == null)
            return;

        var shouldMotion = curTarget.HasMotions;
        if(shouldMotion && (string.IsNullOrEmpty(curTarget.curExpName) || string.IsNullOrEmpty(curTarget.curMotionName)))
        {
            ShowErrorDebugText("请先设置表情和动作");
            return;
        }

        var format = curTarget.MotionTemplate;
        var output = format.Replace("%me%", curTarget.GetMotionExpressionParamsText() + $" -transform={GetTransformTextTarget(curTarget, 0)}");
        for (int i = 0; i < curTarget.ModelCount; i++)
        {
            output = output.Replace($"%me_{i}%", curTarget.GetMotionExpressionParamsText() + $" -transform={GetTransformTextTarget(curTarget, i)}");
        }
        L2DWUtils.CopyInstructionToCopyBoard(output);
        ShowDebugText("复制成功！");
    }
    

    public void CopyMotion()
    {
        CopyAllSpilt(true, false);
    }

    public void CopyTransform()
    {
        CopyAllSpilt(false, true);
    }

    private string GetTransformText()
    {
        return GetTransformTextTarget(curTarget, 0);
    }
    
    private string GetTransformTextTarget(ModelAdjusterBase target, int modelIndex = 0)
    {
        var localPos = target.GetCharacterSpecWorldPosition(modelIndex);
        var jsonObject = new JSONObject();
        var posObject = new JSONObject();
        posObject.AddField("x", localPos.x);
        posObject.AddField("y", -localPos.y);
        jsonObject.AddField("position", posObject);
        var scaleObject = new JSONObject();
        scaleObject.AddField("x", target.RootScale.x);
        scaleObject.AddField("y", target.RootScale.y);
        jsonObject.AddField("scale", scaleObject);
        jsonObject.AddField("rotation", target.GetWebGalRotation());
        target.filterSetData.ApplyToJson(jsonObject);
        return jsonObject.ToString(false);

        //留着做备份，这个注释是给cursor看的
        // return $"{{\"position\":{{\"x\":{localPos.x:F3},\"y\":{-localPos.y:F3} }},\"scale\":{{\"x\":{target.RootScale.x:F3},\"y\":{target.RootScale.y:F3} }},\"rotation\":{target.GetWebGalRotation():F3} }}";
    }

    #endregion

    #region 背景

    public string GetTransformTextBG()
    {
        var jsonObject = new JSONObject();
        var posObject = new JSONObject();
        posObject.AddField("x", bgContainer.rootPosition.x);
        posObject.AddField("y", -bgContainer.rootPosition.y);
        jsonObject.AddField("position", posObject);
        var scaleObject = new JSONObject();
        scaleObject.AddField("x", bgContainer.rootScale);
        scaleObject.AddField("y", bgContainer.rootScale);
        jsonObject.AddField("scale", scaleObject);
        jsonObject.AddField("rotation", bgContainer.GetWebGalRotation());
        bgContainer.filterSetData.ApplyToJson(jsonObject);
        return jsonObject.ToString(false);

        // return $"{{\"position\":{{\"x\":{bgContainer.rootPosition.x:F3},\"y\":{-bgContainer.rootPosition.y:F3} }},\"scale\":{{\"x\":{bgContainer.rootScale:F3},\"y\":{bgContainer.rootScale:F3} }},\"rotation\":{bgContainer.GetWebGalRotation():F3} }}";

    }

    private bool CheckBackGroundValid()
    {
        if (string.IsNullOrEmpty(Global.BGPath))
        {
            MessageTipWindow.Instance.Show("错误", "请先设置背景文件夹");
            return false;
        }
        
        if (string.IsNullOrEmpty(MainControl.CurrentBGPath))
        {
            MessageTipWindow.Instance.Show("错误", "请先加载一张背景\n默认背景只是拿来看的，不会被导出");
            return false;
        }

        return true;
    }

    public void CopyBackgroundChange()
    {
        if (!CheckBackGroundValid())
            return;

        var relativePath = L2DWUtils.GetRelativePath(MainControl.CurrentBGPath, Global.BGPath);
        var template = Global.BGChangeTemplate;
        var result = template.Replace("%me%", relativePath);
        Debug.Log(result);
        L2DWUtils.CopyInstructionToCopyBoard(result);
        ShowDebugText("复制成功！");
    }

    public void CopyBackgroundTransform()
    {
        if (!CheckBackGroundValid())
            return;

        var template = Global.BGTransformTemplate;
        var result = template.Replace("%me%", MainControl.Instance.GetTransformTextBG());
        Debug.Log(result);
        L2DWUtils.CopyInstructionToCopyBoard(result);
        ShowDebugText("复制成功！");
    }

    public void CopyBackgroundAll()
    {
        if (!CheckBackGroundValid())
            return;

        var relativePath = L2DWUtils.GetRelativePath(MainControl.CurrentBGPath, Global.BGPath);
        var template = Global.BGChangeTemplate;
        var result = template.Replace("%me%", $"{relativePath} -transform={MainControl.Instance.GetTransformTextBG()}");
        Debug.Log(result);
        L2DWUtils.CopyInstructionToCopyBoard(result);
        ShowDebugText("复制成功！");
    }

    #endregion

    #region 立绘组

    public void CopyAllGroupSpilt(bool motion = true, bool transform = true)
    {
        if (curGroup == null)
            return;

        curGroup.RemoveInvalidModel();

        if (motion)
        {
            foreach (var model in curGroup.modelAdjusters)
            {
                var shouldMotion = model.HasMotions;
                if (shouldMotion && (string.IsNullOrEmpty(model.curExpName) || string.IsNullOrEmpty(model.curMotionName)))
                {
                    ShowErrorDebugText($"请先给 【{model.Name}】 设置表情和动作！");
                    return;
                }
            }
        }

        StringBuilder commands = new StringBuilder();
        foreach (var model in curGroup.modelAdjusters)
        {
            if (motion)
            {
                //动作表情
                var format2 = model.MotionTemplate;
                var output2 = format2.Replace("%me%", model.GetMotionExpressionParamsText());
                for (int i = 0; i < model.ModelCount; i++)
                {
                    output2 = output2.Replace($"%me_{i}%", model.GetMotionExpressionParamsText());
                }
                commands.AppendLine(output2);
            }

            if (transform)
            {
                //变换
                var format = model.TransformTemplate;
                var output = format.Replace("%me%", GetTransformTextTarget(model));
                for (int i = 0; i < model.ModelCount; i++)
                {
                    output = output.Replace($"%me_{i}%", GetTransformTextTarget(model, i));
                }
                commands.AppendLine(output);
            }
        }

        if(curGroup.containBackgroundCopy)
        {
            if (CheckBackGroundValid())
            {
                var format = Global.BGTransformTemplate;
                var bgText = format.Replace("%me%", GetTransformTextBG());
                commands.AppendLine(bgText);
            }
        }

        L2DWUtils.CopyInstructionToCopyBoard(commands.ToString());
        ShowDebugText("复制成功！");
    }
    public void CopyAllGroup()
    {
        if (curGroup == null)
            return;

        StringBuilder commands = new StringBuilder();
        curGroup.RemoveInvalidModel();
        foreach (var model in curGroup.modelAdjusters)
        {
            var shouldMotion = model.HasMotions;
            if (shouldMotion && (string.IsNullOrEmpty(model.curExpName) || string.IsNullOrEmpty(model.curMotionName)))
            {
                ShowErrorDebugText($"请先给 【{model.Name}】 设置表情和动作！");
                return;
            }
            
            var format = model.MotionTemplate;
            var output = format.Replace("%me%", model.GetMotionExpressionParamsText() + $" -transform={GetTransformTextTarget(model)}");
            for (int i = 0; i < model.ModelCount; i++)
            {
                output = output.Replace($"%me_{i}%", model.GetMotionExpressionParamsText() + $" -transform={GetTransformTextTarget(model, i)}");
            }
            commands.AppendLine(output);
        }

        if(curGroup.containBackgroundCopy)
        {
            if (CheckBackGroundValid())
            {
                var format = Global.BGTransformTemplate;
                var bgText = format.Replace("%me%", GetTransformTextBG());
                commands.AppendLine(bgText);   
            }
        }

        L2DWUtils.CopyInstructionToCopyBoard(commands.ToString());
        ShowDebugText("复制成功！");
    }

    public void CopyTransformGroup()
    {
        CopyAllGroupSpilt(false, true);
    }

    public void CopyMotionGroup()
    {
        CopyAllGroupSpilt(true, false);
    }

    private int screenshotCounter = 0;
    public void TakeSnapshot()
    {
        var now = System.DateTime.Now;
        var dateStr = now.ToString("yyyyMMdd");
        var timeStr = now.ToString("HHmm");
        var fileName = $"{dateStr}-{timeStr}-{screenshotCounter}.png";
        var folderPath = System.IO.Path.Combine(Application.dataPath, "..", "Snapshots");
        if (!System.IO.Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
        }
        var path = System.IO.Path.Combine(folderPath, fileName);

        ScreenCapture.CaptureScreenshot(path);
        screenshotCounter++;
        // messageTipWindow.Show("提示", $"截图成功！已保存为\n{path}");
    }

    public void TakeCharacterSnapShot()
    {
        if (curTarget == null)
            return;

        if (curTarget is ModelAdjuster modelAdjuster)
        {
            modelAdjuster.SaveImage();
        }
    }

    #endregion

    public void CopyMotionEditor()
    {
        GUIUtility.systemCopyBuffer = curTarget.GetMotionEditorExpJson();
        ShowDebugText("保存成功！");
    }

    public void ShowDebugText(string text)
    {
        DOTween.Kill(debugText);
        debugText.color = Color.green;
        debugText.DOColor(new Color(1 ,1, 1, 0), 2).SetEase(Ease.OutQuad);
        debugText.text = text;
    }

    public void ShowErrorDebugText(string text)
    {
        DOTween.Kill(debugText);
        debugText.color = Color.red;
        debugText.DOColor(new Color(1, 1, 1, 0), 4).SetEase(Ease.OutQuad);
        debugText.text = text;
    }

    #endregion

    #region  配置加载相关

    private SFB.ExtensionFilter[] modelFilters = new SFB.ExtensionFilter[] {
         new SFB.ExtensionFilter("模型文件", "json", "conf"),
    };
    public void LoadModelConfig()
    {
        var paths = SFB.StandaloneFileBrowser.OpenFilePanel("选择模型文件, json只能选一个", PlayerPrefs.GetString("live2d_path", "."), modelFilters, true);
        if (paths == null || paths.Length == 0)
            return;

        var path = paths[0];
        if (string.IsNullOrEmpty(path))
            return;

        PlayerPrefs.SetString("live2d_path", Path.GetDirectoryName(path));
        Debug.Log($"选择live2d文件: {path}");
        if (path.EndsWith(".json"))
        {
            LoadJsonModel(path);
        }
        else if (path.EndsWith(".conf"))
        {
            LoadConfModel(paths);
        }
    }

    private void LoadConfModel(string[] paths)
    {
        bool success = false;
        string validPath = null;

        ModelAdjusterBase finalTarget = null;
        foreach (var path in paths)
        {
            if (string.IsNullOrEmpty(path))
                continue;

            var meta = MyGOLive2DExMeta.Load(path);
            var target = CreateModelAdjuster();
            target.meta = meta;
            target.CreateModel();
            if (target.MainModel == null)
            {
                ShowErrorDebugText($"无法加载主模型，不加载了: {path}");
                DeleteModel(target);
                continue;
            }
            success = true;
            validPath = path;
            target.Adjust();
            if (meta.hasTransform)
            {
                target.InitTransform(new Vector3(meta.x, meta.y, 0), meta.scale, meta.rotation, meta.reverseX);
            }
            TryPlayDefaultMotion(target);
            finalTarget = target;
        }

        if (success)
        {
            SetCharacter(finalTarget);
            PlayerPrefs.SetString("conf_path", Path.GetDirectoryName(validPath));
            ShowDebugText("加载成功！");
            OnLoadConf?.Invoke();
        }
    }

    private void LoadJsonModel(string path)
    {
        var target = CreateModelAdjuster();
        var meta = new MyGOLive2DExMeta();
        target.meta = meta;
        meta.modelFilePath = L2DWUtils.TryParseModelRelativePath(path);
        meta.name = Path.GetFileNameWithoutExtension(path);
        meta.formatText = "";
        if (L2DWUtils.IsSubFolderOf(path, Global.ModelPath))
        {
            meta.formatText = $"changeFigure:{meta.modelFilePath} -id={meta.name} %me%;";
        }
        meta.transformFormatText = $"setTransform:%me% -target={meta.name} -duration=750;";
        
        target.CreateModel();
        if (target.MainModel == null)
        {
            ShowErrorDebugText($"无法加载主模型，不加载了: {path}");
            DeleteModel(target);
            return;
        }
        target.Adjust();
        SetCharacter(target);
        TryPlayDefaultMotion(target);



        Resources.UnloadUnusedAssets();

        if (!Global.DisableJsonModelProfileInit)
        {
            settingWindow.gameObject.SetActive(true);
            settingWindow.Load(curMeta);
        }
    }

    private void LoadImgModel(string path)
    {
        ImageModel CreateImageAdjuster()
        {
            var ins = Instantiate(imgPrefab);
            ins.gameObject.SetActive(true);
            models.Add(ins);
            return ins;
        }
        var ins = CreateImageAdjuster();
        ins.Init(path);
        ins.CreateModel();
        ins.Adjust();
        SetCharacter(ins);
        Resources.UnloadUnusedAssets();

        imageSettingWindow.Show();
        imageSettingWindow.Load(ins.imgMeta);
    }

    public void LoadConfig()
    {
        var paths = SFB.StandaloneFileBrowser.OpenFilePanel("选择live2d文件", PlayerPrefs.GetString("live2d_path", "."), "json", false);
        if (paths == null || paths.Length == 0)
            return;

        var path = paths[0];
        if (string.IsNullOrEmpty(path))
            return;

        PlayerPrefs.SetString("live2d_path", Path.GetDirectoryName(path));
        
        LoadJsonModel(path);
    }

    public void LoadImg()
    {
        if (string.IsNullOrEmpty(Global.ModelPath))
        {
            messageTipWindow.Show("错误", "请先设置模型文件夹");
            return;
        }

        var paths = L2DWUtils.OpenFileDialog("选择图片文件", "img_path", "png|jpg");
        if (paths == null || paths.Length == 0)
            return;

        var path = paths[0];
        if (string.IsNullOrEmpty(path))
            return;

        if (!L2DWUtils.TryGetRelativePath(path, Global.ModelPath, out var relativePath))
        {
            messageTipWindow.Show("错误", "图片文件路径错误！\n你应该选择模型文件夹下的图片文件");
            return;
        }
        
        LoadImgModel(path);
    }

    public void TryPlayDefaultMotion(ModelAdjusterBase model)
    {
        if (model == null || model.meta == null)
            return;

        var idleMotion = model.MotionPairs.FirstOrDefault(pair => pair.name.Contains("idle"));
        if (idleMotion != null)
        {
            model.PlayMotion(idleMotion.name);
        }

        var defaultExp = model.ExpPairs.FirstOrDefault(pair => pair.name.Contains("default"));
        if (defaultExp != null)
        {
            model.PlayExp(defaultExp.name);
        }
    }

    public void LoadConf()
    {
        var paths = SFB.StandaloneFileBrowser.OpenFilePanel("选择配置", PlayerPrefs.GetString("conf_path", "."), "conf", true);
        if (paths == null || paths.Length == 0)
            return;
            
        LoadConfModel(paths);
    }

    private ModelAdjuster CreateModelAdjuster()
    {
        var newtarget = Instantiate(prefab);
        newtarget.gameObject.SetActive(true);
        models.Add(newtarget);
        return newtarget;
    }

    public void SaveConf()
    {
        if (curTarget == null)
            return;

        var path = SFB.StandaloneFileBrowser.SaveFilePanel("保存配置", PlayerPrefs.GetString("conf_path", "."), $"{curMeta.name}.conf", "conf");
        if (string.IsNullOrEmpty(path))
            return;
        
        PlayerPrefs.SetString("conf_path", Path.GetDirectoryName(path));

        curMeta.SetTransform(curTarget.RootPosition.x, curTarget.RootPosition.y, curTarget.RootScaleValue, curTarget.RootRotation);
        curMeta.reverseX = curTarget.ReverseXScale;
        curMeta.Save(path);
        ShowDebugText("保存成功！");
    }

    #endregion

    #region Group相关

    public void AddGroup()
    {
        var group = Instantiate(modelGroupPrefab);
        group.groupName = "新组";
        modelGroups.Add(group);
        SetGroup(group);
        group.gameObject.SetActive(true);
    }

    private void RemoveModelFromAllGroups(ModelAdjusterBase model)
    {
        foreach (var group in modelGroups)
        {
            group.modelAdjusters.Remove(model);
        }
    }

    public void AddModelToGroup(ModelGroup group, ModelAdjusterBase model)
    {
        //现在支持角色在多个组中
        // RemoveModelFromAllGroups(model);
        group.modelAdjusters.Add(model);
    }

    public void RemoveGroup(ModelGroup group)
    {
        modelGroups.Remove(group);
        if (curGroup == group)
        {
            SetGroup(null);
        }
        Destroy(group.gameObject);
    }

    #endregion

    #region Character绘制相关

    public void SetModelVisible(ModelAdjusterBase model, bool visible)
    {
        model.gameObject.SetActive(visible);
    }

    public void DeleteModel(ModelAdjusterBase model)
    {
        models.Remove(model);
        if (curTarget == model)
        {
            SetCharacter(null);
        }
        UIEventBus.SendEvent(UIEventType.OnModelDeleted);
        Destroy(model.gameObject);
    }
    #endregion

    #region 表情动作相关
    public void PlayMotion(string motionName)
    {
        curTarget.PlayMotion(motionName);
    }

    public void PlayExp(string expName)
    {
        curTarget.PlayExp(expName);
    }

    
    public void UpdateFilterContent()
    {
        UpdateMotionFilterContent();
        UpdateExpFilterContent();
    }

    public void UpdateMotionFilterContent()
    {
        filteredMotions.Clear();

        if (!curTarget.HasMotions)
            return;

        string[] filters = (filterMotion + " " + curMeta.m_filterMotion).Split(' ').Where(m => !string.IsNullOrEmpty(m)).ToArray();
        if (filters.Length == 0)
        {
            filteredMotions = curTarget.MotionPairs.Select(m => m.name).ToList();
            return;
        }
        
        filteredMotions = curTarget.MotionPairs.Where(m => filters.All(f => m.name.Contains(f))).Select(m => m.name).ToList();
    }

    public void UpdateExpFilterContent()
    {
        filteredExps.Clear();

        if (!curTarget.HasMotions)
            return;

        string[] filters = (filterExp + " " + curMeta.m_filterExp).Split(' ').Where(m => !string.IsNullOrEmpty(m)).ToArray();
        if (filters.Length == 0)
        {
            filteredExps = curTarget.ExpPairs.Select(m => m.name).ToList();
            return;
        }

        filteredExps = curTarget.ExpPairs.Where(m => filters.All(f => m.name.Contains(f))).Select(m => m.name).ToList();
    }

    #endregion

    public void ResetCamera()
    {
        mainCamera.transform.position = new Vector3(0, 0, -10);
        mainCamera.orthographicSize = 7.2f;
    }
}

public enum EditType
{
    Model,
    Group,
    Background,
}

