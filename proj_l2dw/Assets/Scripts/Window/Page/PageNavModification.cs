using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageNavModification : UIPageWidget<PageNavModification>
{
    #region auto generated members
    private Transform m_tfCharaRoot;
    private RawImage m_rawChara;
    private TouchArea m_touchChara;
    private Button m_btnResetCharaArea;
    private Button m_btnMotion;
    private Text m_lblMotion;
    private RectTransform m_rectSelectMotionArea;
    private Button m_btnHelp;
    private Button m_btnSave;
    private GameObject m_goLeft;
    private Transform m_tfTrackHeaderRoot;
    private Toggle m_toggleParts;
    private Toggle m_toggleInitParams;
    private Toggle m_toggleMotion;
    private Toggle m_toggleExpression;
    private GameObject m_goRight;
    private Transform m_itemPageModification_Parts;
    private Transform m_itemPageModification_InitParams;
    private Transform m_itemPageModification_Motion;
    private Transform m_itemPageModification_Expression;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_tfCharaRoot = transform.Find("CharaArea/m_tfCharaRoot").GetComponent<Transform>();
        m_rawChara = transform.Find("CharaArea/m_tfCharaRoot/m_rawChara").GetComponent<RawImage>();
        m_touchChara = transform.Find("CharaArea/m_touchChara").GetComponent<TouchArea>();
        m_btnResetCharaArea = transform.Find("CharaArea/m_btnResetCharaArea").GetComponent<Button>();
        m_btnMotion = transform.Find("CharaArea/ToolBar/m_btnMotion").GetComponent<Button>();
        m_lblMotion = transform.Find("CharaArea/ToolBar/m_btnMotion/m_lblMotion").GetComponent<Text>();
        m_rectSelectMotionArea = transform.Find("CharaArea/ToolBar/m_btnMotion/m_rectSelectMotionArea").GetComponent<RectTransform>();
        m_btnHelp = transform.Find("CharaArea/ToolBar/m_btnHelp").GetComponent<Button>();
        m_btnSave = transform.Find("CharaArea/ToolBar/m_btnSave").GetComponent<Button>();
        m_goLeft = transform.Find("m_goLeft").gameObject;
        m_tfTrackHeaderRoot = transform.Find("m_goLeft/m_tfTrackHeaderRoot").GetComponent<Transform>();
        m_toggleParts = transform.Find("m_goLeft/m_tfTrackHeaderRoot/m_toggleParts").GetComponent<Toggle>();
        m_toggleInitParams = transform.Find("m_goLeft/m_tfTrackHeaderRoot/m_toggleInitParams").GetComponent<Toggle>();
        m_toggleMotion = transform.Find("m_goLeft/m_tfTrackHeaderRoot/m_toggleMotion").GetComponent<Toggle>();
        m_toggleExpression = transform.Find("m_goLeft/m_tfTrackHeaderRoot/m_toggleExpression").GetComponent<Toggle>();
        m_goRight = transform.Find("m_goRight").gameObject;
        m_itemPageModification_Parts = transform.Find("m_goRight/m_itemPageModification_Parts").GetComponent<Transform>();
        m_itemPageModification_InitParams = transform.Find("m_goRight/m_itemPageModification_InitParams").GetComponent<Transform>();
        m_itemPageModification_Motion = transform.Find("m_goRight/m_itemPageModification_Motion").GetComponent<Transform>();
        m_itemPageModification_Expression = transform.Find("m_goRight/m_itemPageModification_Expression").GetComponent<Transform>();

        m_btnResetCharaArea.onClick.AddListener(OnButtonResetCharaAreaClick);
        m_btnMotion.onClick.AddListener(OnButtonMotionClick);
        m_btnHelp.onClick.AddListener(OnButtonHelpClick);
        m_btnSave.onClick.AddListener(OnButtonSaveClick);
        m_toggleParts.onValueChanged.AddListener(OnTogglePartsChange);
        m_toggleInitParams.onValueChanged.AddListener(OnToggleInitParamsChange);
        m_toggleMotion.onValueChanged.AddListener(OnToggleMotionChange);
        m_toggleExpression.onValueChanged.AddListener(OnToggleExpressionChange);
    }
    #endregion

    #region auto generated events
    private void OnButtonResetCharaAreaClick()
    {
        m_tfCharaRoot.localScale = Vector3.one;
        m_rawChara.rectTransform.localPosition = Vector3.zero;
    }
    private void OnButtonMotionClick()
    {
    }
    private void OnButtonHelpClick()
    {
        string helpText = "目前不支持拼好模的编辑，只支持单个模型的编辑嗷…\n执意要在拼好模的编辑器里编辑的话，请先在拼好模的编辑器里导出单个模型，再在单个模型的编辑器里编辑嗷…\n否则会出现可怕的事情嗷…";
        MessageTipWindow.Instance.Show("帮助", helpText);
    }
    private void OnButtonSaveClick()
    {
        if (m_curModel == null)
            return;

        var jsonObj = new JSONObject();
        var initParamsObj = new JSONObject(JSONObject.Type.ARRAY);
        var initOpacitiesObj = new JSONObject(JSONObject.Type.ARRAY);

        foreach (var part in m_curModel.MyGOConfig.init_opacities)
        {
            var partObj = new JSONObject();
            partObj.AddField("id", part.Key);
            partObj.AddField("value", part.Value);
            initOpacitiesObj.Add(partObj);
        }

        foreach (var param in m_curModel.MyGOConfig.init_params)
        {
            var paramObj = new JSONObject();
            paramObj.AddField("id", param.Key);
            paramObj.AddField("value", param.Value);
            initParamsObj.Add(paramObj);
        }

        jsonObj.AddField("init_params", initParamsObj);
        jsonObj.AddField("init_opacities", initOpacitiesObj);

        GUIUtility.systemCopyBuffer = jsonObj.ToString(true);
        MessageTipWindow.Instance.Show("配置", "已复制到剪贴板！");
    }
    private void OnTogglePartsChange(bool value)
    {
    }
    private void OnToggleInitParamsChange(bool value)
    {
    }
    private void OnToggleMotionChange(bool value)
    {
    }
    private void OnToggleExpressionChange(bool value)
    {
    }
    #endregion

    private PageNavModificationParts m_pageNavModificationParts;
    private PageNavModificationInitParams m_pageNavModificationInitParams;
    private PageNavModificationMotion m_pageNavModificationMotion;
    private PageNavModificationExpression m_pageNavModificationExpression;

    protected override void OnInit()
    {
        base.OnInit();
        m_pageNavModificationParts = PageNavModificationParts.CreateWidget(m_itemPageModification_Parts.gameObject);
        m_pageNavModificationInitParams = PageNavModificationInitParams.CreateWidget(m_itemPageModification_InitParams.gameObject);
        m_pageNavModificationMotion = PageNavModificationMotion.CreateWidget(m_itemPageModification_Motion.gameObject);
        m_pageNavModificationExpression = PageNavModificationExpression.CreateWidget(m_itemPageModification_Expression.gameObject);

        m_pageNavModificationParts.owner = this;
        m_pageNavModificationInitParams.owner = this;
        m_pageNavModificationMotion.owner = this;
        m_pageNavModificationExpression.owner = this;

        BindChild(m_pageNavModificationParts);
        BindChild(m_pageNavModificationInitParams);
        BindChild(m_pageNavModificationMotion);
        BindChild(m_pageNavModificationExpression);

        m_pageNavModificationParts.BindToToggle(m_toggleParts);
        m_pageNavModificationInitParams.BindToToggle(m_toggleInitParams);
        m_pageNavModificationMotion.BindToToggle(m_toggleMotion);
        m_pageNavModificationExpression.BindToToggle(m_toggleExpression);

        m_touchChara._OnPointerDown += OnTouchCharaPointerDown;
        m_touchChara._OnPointerMove += OnTouchCharaPointerMove;
        m_touchChara._OnScroll += OnTouchCharaScroll;
    }

    private Vector2 m_charaStartPosition;
    private void OnTouchCharaPointerDown(PointerEventData eventData)
    {
        m_charaStartPosition = eventData.position;
    }

    private void OnTouchCharaPointerMove(Vector2 vector)
    {
        var delta = vector - m_charaStartPosition;
        m_charaStartPosition = vector;
        var pos = m_rawChara.rectTransform.position;
        pos += new Vector3(delta.x, delta.y, 0);
        m_rawChara.rectTransform.position = pos;
    }

    private void OnTouchCharaScroll(PointerEventData eventData)
    {
        var ctrlPressed = Input.GetKey(KeyCode.LeftControl);
        var scaleFactor = ctrlPressed ? Global.CameraZoomBoostFactor : Global.CameraZoomFactor;
        if (eventData.scrollDelta.y < 0)
            scaleFactor = 1.0f / scaleFactor;
        m_tfCharaRoot.localScale = new Vector3(
            m_tfCharaRoot.localScale.x * scaleFactor,
            m_tfCharaRoot.localScale.y * scaleFactor,
            m_tfCharaRoot.localScale.z
        );
    }

    protected override void OnPageShown()
    {
        UIEventBus.AddListener(UIEventType.OnModelChanged, RefreshAll);
        RefreshAll();

        base.OnPageShown();
    }

    protected override void OnPageHidden()
    {
        UIEventBus.RemoveListener(UIEventType.OnModelChanged, RefreshAll);

        base.OnPageHidden();
    }

    public ModelAdjuster m_curModel;
    public void RefreshAll()
    {
        var curTarget = MainControl.Instance.curTarget;
        if (curTarget == null || curTarget is not ModelAdjuster model)
        {
            m_rawChara.texture = null;
            return;
        }
        m_rawChara.texture = model.MainModel.meshRenderer.material.mainTexture;
        m_curModel = model;
    }
}

public class PageOfPageNavModification<T> : UIPageWidget<T> where T : PageOfPageNavModification<T>, new()
{
    public PageNavModification owner;
    protected ModelAdjuster CurModel => owner.m_curModel;

    public virtual void OnModelChanged()
    {

    }
}

public class PageNavModificationParts : PageOfPageNavModification<PageNavModificationParts>
{
    #region auto generated members
    private Transform m_itemPartsEntry;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_itemPartsEntry = transform.Find("ScrollRectV/Viewport/Content/m_itemPartsEntry").GetComponent<Transform>();

    }
    #endregion

    #region auto generated events
    #endregion

    private List<PageNavModificationPartsEntry> m_listPartsEntries = new List<PageNavModificationPartsEntry>();

    protected override void OnPageShown()
    {
        base.OnPageShown();
        RefreshAll();
    }

    public override void OnModelChanged()
    {
        base.OnModelChanged();
        RefreshAll();
    }

    private void RefreshAll()
    {
        if (CurModel == null)
        {
            SetListItem(m_listPartsEntries, m_itemPartsEntry.gameObject, m_itemPartsEntry.parent, 0, null);
            return;
        }

        CurModel.SetDisplayMode(ModelDisplayMode.EmotionEditor, true);
        var partsDataList = CurModel.MainModel.m_partsDataList;
        var partsCount = partsDataList.Count;
        SetListItem(m_listPartsEntries, m_itemPartsEntry.gameObject, m_itemPartsEntry.parent, partsCount, (entry) =>
        {
            entry._OnToggleEnableChange += OnPartsEntryToggleEnableChange;
            entry._OnSliderValueChange += OnPartsEntrySliderValueChange;
        });
        for (int i = 0; i < partsCount; i++)
        {
            var partsData = partsDataList[i];
            var entry = m_listPartsEntries[i];
            var partKey = partsData.getPartsDataID().ToString();
            var enable = CurModel.MyGOConfig.init_opacities.TryGetValue(partKey, out var opacity);
            entry.SetData(partKey, enable, enable ? opacity : 1);
        }

        CurModel.ApplyModelInitOpacities();
    }

    private void OnPartsEntryToggleEnableChange(PageNavModificationPartsEntry entry, bool value)
    {
        if (CurModel == null)
        {
            return;
        }
        var partKey = entry.m_partKey;
        if (value)
        {
            CurModel.MyGOConfig.init_opacities[partKey] = entry.SliderValue;
        }
        else
        {
            CurModel.MyGOConfig.init_opacities.Remove(partKey);
        }

        RefreshAll();
    }
    private void OnPartsEntrySliderValueChange(PageNavModificationPartsEntry entry, float value)
    {
        if (CurModel == null)
        {
            return;
        }
        var partKey = entry.m_partKey;
        CurModel.MyGOConfig.init_opacities[partKey] = value;
        RefreshAll();
    }
}

public class PageNavModificationPartsEntry : UIItemWidget<PageNavModificationPartsEntry>
{
    #region auto generated members
    private Toggle m_toggleEnable;
    private Text m_lblName;
    private Slider m_sliderValue;
    private Image m_imgSliderFill;
    private Image m_imgSliderHandle;
    private Text m_lblValue;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_toggleEnable = transform.Find("m_toggleEnable").GetComponent<Toggle>();
        m_lblName = transform.Find("m_toggleEnable/m_lblName").GetComponent<Text>();
        m_sliderValue = transform.Find("m_sliderValue").GetComponent<Slider>();
        m_imgSliderFill = transform.Find("m_sliderValue/Fill Area/m_imgSliderFill").GetComponent<Image>();
        m_imgSliderHandle = transform.Find("m_sliderValue/Handle Slide Area/m_imgSliderHandle").GetComponent<Image>();
        m_lblValue = transform.Find("m_lblValue").GetComponent<Text>();

        m_toggleEnable.onValueChanged.AddListener(OnToggleEnableChange);
        m_sliderValue.onValueChanged.AddListener(OnSliderValueChange);
    }
    #endregion

    #region auto generated events
    private void OnToggleEnableChange(bool value)
    {
        _OnToggleEnableChange?.Invoke(this, value);
    }
    private void OnSliderValueChange(float value)
    {
        _OnSliderValueChange?.Invoke(this, value);
    }
    #endregion

    public Action<PageNavModificationPartsEntry, bool> _OnToggleEnableChange;
    public Action<PageNavModificationPartsEntry, float> _OnSliderValueChange;

    public string m_partKey;
    public float SliderValue => m_sliderValue.value;

    public void SetData(string name, bool enable, float value)
    {
        m_partKey = name;


        m_lblName.text = name;
        m_sliderValue.SetValueWithoutNotify(value);
        m_lblValue.text = value.ToString("F2");
        m_toggleEnable.onValueChanged.RemoveListener(SendToggleEnableChange);
        m_toggleEnable.isOn = enable;
        m_toggleEnable.onValueChanged.AddListener(SendToggleEnableChange);
    }

    private void SendToggleEnableChange(bool value)
    {
        _OnToggleEnableChange?.Invoke(this, value);
    }
}

public class PageNavModificationInitParams : PageOfPageNavModification<PageNavModificationInitParams>
{
    #region auto generated members
    private Transform m_itemPartsEntry;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_itemPartsEntry = transform.Find("ScrollRectV/Viewport/Content/m_itemPartsEntry").GetComponent<Transform>();

    }
    #endregion

    #region auto generated events
    #endregion

    private List<PageNavModificationInitParamsEntry> m_listInitParamsEntries = new List<PageNavModificationInitParamsEntry>();


    protected override void OnPageShown()
    {
        base.OnPageShown();
        RefreshAll();

        UIEventBus.AddListener(UIEventType.OnModelChanged, RefreshAll);
    }

    protected override void OnPageHidden()
    {
        base.OnPageHidden();

        UIEventBus.RemoveListener(UIEventType.OnModelChanged, RefreshAll);
    }

    private void RefreshAll()
    {
        if (CurModel == null)
        {
            SetListItem(m_listInitParamsEntries, m_itemPartsEntry.gameObject, m_itemPartsEntry.parent, 0, null);
            return;
        }

        CurModel.SetDisplayMode(ModelDisplayMode.EmotionEditor, true);

        var initParams = CurModel.emotionEditor.list.list;
        var defValues = CurModel.emotionEditor.list.paramDefDict;
        var initParamsCount = initParams.Count;
        SetListItem(m_listInitParamsEntries, m_itemPartsEntry.gameObject, m_itemPartsEntry.parent, initParamsCount, (entry) =>
        {
            entry._OnInputFieldValueSubmit += OnInitParamsEntryInputFieldValueSubmit;
            entry._OnSliderValueChange += OnInitParamsEntrySliderValueChange;
            entry._OnToggleEnableChange += OnInitParamsEntryToggleEnableChange;
        });

        for (int i = 0; i < initParamsCount; i++)
        {
            var initParam = initParams[i];
            var entry = m_listInitParamsEntries[i];
            var enable = CurModel.MyGOConfig.init_params.TryGetValue(initParam.name, out var value);
            defValues.TryGetValue(initParam.name, out var defValue);
            entry.SetData(initParam.name, enable, enable ? value : defValue, initParam.min, initParam.max);
        }

        CurModel.ApplyParamDefaultValues();
    }

    private void OnInitParamsEntryToggleEnableChange(PageNavModificationInitParamsEntry entry, bool arg2)
    {
        if (CurModel == null)
        {
            return;
        }
        if (arg2)
        {
            var defValues = CurModel.emotionEditor.list.realParamDefDict;
            defValues.TryGetValue(entry.m_paramName, out var defValue);
            CurModel.MyGOConfig.init_params[entry.m_paramName] = defValue;

            CurModel.emotionEditor.list.SetDefParam(entry.m_paramName, defValue);
        }
        else
        {
            CurModel.MyGOConfig.init_params.Remove(entry.m_paramName);

            CurModel.emotionEditor.list.RemoveDefParam(entry.m_paramName);
        }

        RefreshAll();
    }

    private void OnInitParamsEntrySliderValueChange(PageNavModificationInitParamsEntry entry, float arg2)
    {
        if (CurModel == null)
        {
            return;
        }
        CurModel.MyGOConfig.init_params[entry.m_paramName] = arg2;
        CurModel.emotionEditor.list.SetDefParam(entry.m_paramName, arg2);
        RefreshAll();
    }

    private void OnInitParamsEntryInputFieldValueSubmit(PageNavModificationInitParamsEntry entry, string arg2)
    {
        if (CurModel == null)
        {
            return;
        }
        if (float.TryParse(arg2, out var value))
        {
            value = Mathf.Clamp(value, entry.m_min, entry.m_max);
            CurModel.MyGOConfig.init_params[entry.m_paramName] = value;
            CurModel.emotionEditor.list.SetDefParam(entry.m_paramName, value);
        }
        RefreshAll();
    }
}

public class PageNavModificationInitParamsEntry : UIItemWidget<PageNavModificationInitParamsEntry>
{
    #region auto generated members
    private GameObject m_goValue;
    private Toggle m_toggleEnable;
    private Text m_lblName;
    private Slider m_sliderValue;
    private Image m_imgSliderFill;
    private Image m_imgSliderHandle;
    private Button m_btnValue;
    private Text m_lblValue;
    private GameObject m_goInput;
    private Text m_lblMin;
    private Text m_lblMax;
    private InputField m_iptValue;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_goValue = transform.Find("m_goValue").gameObject;
        m_toggleEnable = transform.Find("m_goValue/m_toggleEnable").GetComponent<Toggle>();
        m_lblName = transform.Find("m_goValue/m_toggleEnable/m_lblName").GetComponent<Text>();
        m_sliderValue = transform.Find("m_goValue/m_sliderValue").GetComponent<Slider>();
        m_imgSliderFill = transform.Find("m_goValue/m_sliderValue/Fill Area/m_imgSliderFill").GetComponent<Image>();
        m_imgSliderHandle = transform.Find("m_goValue/m_sliderValue/Handle Slide Area/m_imgSliderHandle").GetComponent<Image>();
        m_btnValue = transform.Find("m_goValue/m_btnValue").GetComponent<Button>();
        m_lblValue = transform.Find("m_goValue/m_btnValue/m_lblValue").GetComponent<Text>();
        m_goInput = transform.Find("m_goInput").gameObject;
        m_lblMin = transform.Find("m_goInput/m_lblMin").GetComponent<Text>();
        m_lblMax = transform.Find("m_goInput/m_lblMax").GetComponent<Text>();
        m_iptValue = transform.Find("m_goInput/InputField/m_iptValue").GetComponent<InputField>();

        m_toggleEnable.onValueChanged.AddListener(OnToggleEnableChange);
        m_sliderValue.onValueChanged.AddListener(OnSliderValueChange);
        m_btnValue.onClick.AddListener(OnButtonValueClick);
        m_iptValue.onValueChanged.AddListener(OnInputFieldValueChange);
        m_iptValue.onEndEdit.AddListener(OnInputFieldValueEndEdit);
    }
    #endregion

    #region auto generated events
    private void OnToggleEnableChange(bool value)
    {
        _OnToggleEnableChange?.Invoke(this, value);
    }
    private void OnSliderValueChange(float value)
    {
    }
    private void OnButtonValueClick()
    {
        ShowEditMode(true);
    }
    private void OnInputFieldValueChange(string value)
    {
    }
    private void OnInputFieldValueEndEdit(string value)
    {
        ShowEditMode(false);
        _OnInputFieldValueSubmit?.Invoke(this, value);
    }
    #endregion

    protected override void OnInit()
    {
        base.OnInit();
    }

    public Action<PageNavModificationInitParamsEntry, bool> _OnToggleEnableChange;
    public Action<PageNavModificationInitParamsEntry, float> _OnSliderValueChange;
    public Action<PageNavModificationInitParamsEntry, string> _OnInputFieldValueSubmit;

    public string m_paramName;
    public float m_min;
    public float m_max;
    public void SetData(string name, bool enable, float value, float min, float max)
    {
        ShowEditMode(false);
        m_paramName = name;
        m_lblName.text = name;

        m_toggleEnable.onValueChanged.RemoveListener(SendToggleEnableChange);
        m_toggleEnable.isOn = enable;
        m_toggleEnable.onValueChanged.AddListener(SendToggleEnableChange);

        m_sliderValue.onValueChanged.RemoveListener(SendSliderValueChange);
        m_sliderValue.SetValueWithoutNotify(value);
        m_sliderValue.minValue = min;
        m_sliderValue.maxValue = max;
        m_min = min;
        m_max = max;
        m_sliderValue.onValueChanged.AddListener(SendSliderValueChange);

        m_iptValue.SetTextWithoutNotify(value.ToString("F2"));

        m_lblMin.text = min.ToString("F2");
        m_lblMax.text = max.ToString("F2");

        m_lblValue.text = value.ToString("F2");
    }

    private void SendSliderValueChange(float arg0)
    {
        _OnSliderValueChange?.Invoke(this, arg0);
    }

    private void SendToggleEnableChange(bool arg0)
    {
        _OnToggleEnableChange?.Invoke(this, arg0);
    }

    public void ShowEditMode(bool show)
    {
        m_goInput.SetActive(show);
        m_goValue.SetActive(!show);

        if (show)
        {
            EventSystem.current.SetSelectedGameObject(m_iptValue.gameObject);
        }
    }
}

public class PageNavModificationMotion : PageOfPageNavModification<PageNavModificationMotion>
{

}

public class PageNavModificationExpression : PageOfPageNavModification<PageNavModificationExpression>
{

}
