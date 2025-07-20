using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SettingUIWindow : BaseWindow<SettingUIWindow>
{
    #region auto generated members
    private Toggle m_toggleGeneral;
    private Toggle m_toggleNavigation;
    private Toggle m_toggleExperiment;
    private Toggle m_toggleManual;
    private Toggle m_toggleAbout;
    private Toggle m_toggleThanks;
    private Transform m_itemSettingPageGeneral;
    private Transform m_itemSettingPageNavigation;
    private Transform m_itemSettingPageExperiment;
    private Transform m_itemSettingPageManual;
    private Transform m_itemSettingPageAbout;
    private Transform m_itemSettingPageThanks;
    private Text m_lblTitle;
    private Button m_btnClose;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_toggleGeneral = transform.Find("Background/Popup/Left/Viewport/Content/m_toggleGeneral").GetComponent<Toggle>();
        m_toggleNavigation = transform.Find("Background/Popup/Left/Viewport/Content/m_toggleNavigation").GetComponent<Toggle>();
        m_toggleExperiment = transform.Find("Background/Popup/Left/Viewport/Content/m_toggleExperiment").GetComponent<Toggle>();
        m_toggleManual = transform.Find("Background/Popup/Left/Viewport/Content/m_toggleManual").GetComponent<Toggle>();
        m_toggleAbout = transform.Find("Background/Popup/Left/Viewport/Content/m_toggleAbout").GetComponent<Toggle>();
        m_toggleThanks = transform.Find("Background/Popup/Left/Viewport/Content/m_toggleThanks").GetComponent<Toggle>();
        m_itemSettingPageGeneral = transform.Find("Background/Popup/Right/Pages/m_itemSettingPageGeneral").GetComponent<Transform>();
        m_itemSettingPageNavigation = transform.Find("Background/Popup/Right/Pages/m_itemSettingPageNavigation").GetComponent<Transform>();
        m_itemSettingPageExperiment = transform.Find("Background/Popup/Right/Pages/m_itemSettingPageExperiment").GetComponent<Transform>();
        m_itemSettingPageManual = transform.Find("Background/Popup/Right/Pages/m_itemSettingPageManual").GetComponent<Transform>();
        m_itemSettingPageAbout = transform.Find("Background/Popup/Right/Pages/m_itemSettingPageAbout").GetComponent<Transform>();
        m_itemSettingPageThanks = transform.Find("Background/Popup/Right/Pages/m_itemSettingPageThanks").GetComponent<Transform>();
        m_lblTitle = transform.Find("Background/Popup/Right/Top/m_lblTitle").GetComponent<Text>();
        m_btnClose = transform.Find("Background/Popup/Right/Top/m_btnClose").GetComponent<Button>();

        m_toggleGeneral.onValueChanged.AddListener(OnToggleGeneralChange);
        m_toggleNavigation.onValueChanged.AddListener(OnToggleNavigationChange);
        m_toggleExperiment.onValueChanged.AddListener(OnToggleExperimentChange);
        m_toggleManual.onValueChanged.AddListener(OnToggleManualChange);
        m_toggleAbout.onValueChanged.AddListener(OnToggleAboutChange);
        m_toggleThanks.onValueChanged.AddListener(OnToggleThanksChange);
        m_btnClose.onClick.AddListener(OnButtonCloseClick);
    }
    #endregion


    #region auto-generated code event
    private void OnToggleGeneralChange(bool value)
    {
    }
    private void OnToggleNavigationChange(bool value)
    {
    }
    private void OnToggleExperimentChange(bool value)
    {
    }
    private void OnToggleManualChange(bool value)
    {
    }
    private void OnToggleAboutChange(bool value)
    {
    }
    private void OnToggleThanksChange(bool value)
    {
    }
    private void OnButtonCloseClick()
    {
        gameObject.SetActive(false);
    }
    #endregion

    private SettingPageGeneral m_settingPageGeneral;
    private SettingPageAbout m_settingPageAbout;
    private SettingPageManual m_settingPageManual;
    private SettingPageExperiment m_settingPageExperiment;
    private SettingPageNavigation m_settingPageNavigation;
    private SettingPageThanks m_settingPageThanks;
    protected override void OnInit()
    {
        base.OnInit();
        m_settingPageGeneral = SettingPageGeneral.CreateWidget(m_itemSettingPageGeneral.gameObject);
        m_settingPageAbout = SettingPageAbout.CreateWidget(m_itemSettingPageAbout.gameObject);
        m_settingPageManual = SettingPageManual.CreateWidget(m_itemSettingPageManual.gameObject);
        m_settingPageExperiment = SettingPageExperiment.CreateWidget(m_itemSettingPageExperiment.gameObject);
        m_settingPageNavigation = SettingPageNavigation.CreateWidget(m_itemSettingPageNavigation.gameObject);
        m_settingPageThanks = SettingPageThanks.CreateWidget(m_itemSettingPageThanks.gameObject);

        m_settingPageGeneral.Inject(this, m_toggleGeneral);
        m_settingPageAbout.Inject(this, m_toggleAbout);
        m_settingPageManual.Inject(this, m_toggleManual);
        m_settingPageExperiment.Inject(this, m_toggleExperiment);
        m_settingPageNavigation.Inject(this, m_toggleNavigation);
        m_settingPageThanks.Inject(this, m_toggleThanks);
    }

    void Start()
    {
        m_toggleGeneral.isOn = true;
    }

    public void SetTitle(string title)
    {
        m_lblTitle.text = title;
    }

}

public class SettingPageBase<T> : UIPageWidget<T> where T : SettingPageBase<T>, new()
{
    protected SettingUIWindow owner;
    protected virtual string Title => typeof(T).Name;
    public void Inject(SettingUIWindow owner, Toggle toggle)
    {
        this.owner = owner;
        BindToToggle(toggle);
    }

    public override void OnPageShown()
    {
        base.OnPageShown();
        owner.SetTitle(Title);
    }
}

public class SettingPageGeneralWidget : UIItemWidget<SettingPageGeneralWidget>
{
    #region auto generated members
    private InputField m_iptField;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_iptField = transform.Find("Value/InputField/m_iptField").GetComponent<InputField>();

        m_iptField.onValueChanged.AddListener(OnInputFieldFieldChange);
    }
    #endregion

    #region auto generated events
    private void OnInputFieldFieldChange(string value)
    {
        _OnValueChanged?.Invoke(value);
    }
    #endregion

    public event Action<string> _OnValueChanged;


    protected override void OnInit()
    {
        base.OnInit();
    }

    public void SetValue(string value)
    {
        m_iptField.SetTextWithoutNotify(value);
    }
}

public class SettingPageGeneral : SettingPageBase<SettingPageGeneral>
{
    #region auto generated members
    private Transform m_itemModelPath;
    private Transform m_itemBGPath;
    private Transform m_itemBGChangeTemplate;
    private Transform m_itemBGTransformTemplate;
    private Toggle m_toggleCloseGreenLine;
    private Toggle m_toggleDisableJsonModelProfileInit;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_itemModelPath = transform.Find("ScrollRect/Viewport/Content/m_itemModelPath").GetComponent<Transform>();
        m_itemBGPath = transform.Find("ScrollRect/Viewport/Content/m_itemBGPath").GetComponent<Transform>();
        m_itemBGChangeTemplate = transform.Find("ScrollRect/Viewport/Content/m_itemBGChangeTemplate").GetComponent<Transform>();
        m_itemBGTransformTemplate = transform.Find("ScrollRect/Viewport/Content/m_itemBGTransformTemplate").GetComponent<Transform>();
        m_toggleCloseGreenLine = transform.Find("ScrollRect/Viewport/Content/m_toggleCloseGreenLine").GetComponent<Toggle>();
        m_toggleDisableJsonModelProfileInit = transform.Find("ScrollRect/Viewport/Content/m_toggleDisableJsonModelProfileInit").GetComponent<Toggle>();

        m_toggleCloseGreenLine.onValueChanged.AddListener(OnToggleCloseGreenLineChange);
        m_toggleDisableJsonModelProfileInit.onValueChanged.AddListener(OnToggleDisableJsonModelProfileInitChange);
    }
    #endregion

    #region auto generated events
    private void OnToggleCloseGreenLineChange(bool value)
    {
        MainControl.Instance.SetCloseGreenLine(value);
    }

    private void OnToggleDisableJsonModelProfileInitChange(bool value)
    {
        Global.DisableJsonModelProfileInit = value;
    }
    #endregion


    protected override string Title => "常规";

    private SettingPageGeneralWidget m_itemModelPathWidget;
    private SettingPageGeneralWidget m_itemBGPathWidget;
    private SettingPageGeneralWidget m_itemBGChangeTemplateWidget;
    private SettingPageGeneralWidget m_itemBGTransformTemplateWidget;
    protected override void OnInit()
    {
        base.OnInit();
        m_itemModelPathWidget = SettingPageGeneralWidget.CreateWidget(m_itemModelPath.gameObject);
        m_itemBGPathWidget = SettingPageGeneralWidget.CreateWidget(m_itemBGPath.gameObject);
        m_itemBGChangeTemplateWidget = SettingPageGeneralWidget.CreateWidget(m_itemBGChangeTemplate.gameObject);
        m_itemBGTransformTemplateWidget = SettingPageGeneralWidget.CreateWidget(m_itemBGTransformTemplate.gameObject);

        m_itemModelPathWidget._OnValueChanged += OnModelPathChanged;
        m_itemBGPathWidget._OnValueChanged += OnBGPathChanged;
        m_itemBGChangeTemplateWidget._OnValueChanged += OnBGChangeTemplateChanged;
        m_itemBGTransformTemplateWidget._OnValueChanged += OnBGTransformTemplateChanged;
    }

    private void OnModelPathChanged(string value)
    {
        Global.ModelPath = value;
    }

    private void OnBGPathChanged(string value)
    {
        Global.BGPath = value;
    }

    private void OnBGChangeTemplateChanged(string value)
    {
        Global.BGChangeTemplate = value;
    }

    private void OnBGTransformTemplateChanged(string value)
    {
        Global.BGTransformTemplate = value;
    }

    public override void OnPageShown()
    {
        base.OnPageShown();

        if (Global.IsLoaded)
        {
            m_itemModelPathWidget.SetValue(Global.ModelPath);
            m_itemBGPathWidget.SetValue(Global.BGPath);
            m_itemBGChangeTemplateWidget.SetValue(Global.BGChangeTemplate);
            m_itemBGTransformTemplateWidget.SetValue(Global.BGTransformTemplate);
        }

        m_toggleCloseGreenLine.SetIsOnWithoutNotify(MainControl.CloseGreenLine);
        m_toggleDisableJsonModelProfileInit.SetIsOnWithoutNotify(Global.DisableJsonModelProfileInit);
    }
}

public class SettingPageAbout : SettingPageBase<SettingPageAbout>
{
    protected override string Title => "关于";
    protected override void OnInit()
    {
        base.OnInit();
    }
}

public class SettingPageManual : SettingPageBase<SettingPageManual>
{
    protected override string Title => "使用说明";
    protected override void OnInit()
    {
        base.OnInit();
    }
}

public class SettingPageExperiment : SettingPageBase<SettingPageExperiment>
{
    #region auto generated members
    private Toggle m_toggleBlink;
    private Toggle m_toggleWebgalExpSupport;
    private Dropdown m_dropdownPivotMode;
    private Toggle m_toggleUseCustomResolution;
    private InputField m_iptValueResolutionWidth;
    private InputField m_iptValueResolutionHeight;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_toggleBlink = transform.Find("ScrollRect/Viewport/Content/m_toggleBlink").GetComponent<Toggle>();
        m_toggleWebgalExpSupport = transform.Find("ScrollRect/Viewport/Content/m_toggleWebgalExpSupport").GetComponent<Toggle>();
        m_dropdownPivotMode = transform.Find("ScrollRect/Viewport/Content/PivotMode/Value/m_dropdownPivotMode").GetComponent<Dropdown>();
        m_toggleUseCustomResolution = transform.Find("ScrollRect/Viewport/Content/m_toggleUseCustomResolution").GetComponent<Toggle>();
        m_iptValueResolutionWidth = transform.Find("ScrollRect/Viewport/Content/LabelInputField/Value/m_iptValueResolutionWidth").GetComponent<InputField>();
        m_iptValueResolutionHeight = transform.Find("ScrollRect/Viewport/Content/LabelInputField/Value/m_iptValueResolutionHeight").GetComponent<InputField>();

        m_toggleBlink.onValueChanged.AddListener(OnToggleBlinkChange);
        m_toggleWebgalExpSupport.onValueChanged.AddListener(OnToggleWebgalExpSupportChange);
        m_dropdownPivotMode.onValueChanged.AddListener(OnDropdownPivotModeChange);
        m_toggleUseCustomResolution.onValueChanged.AddListener(OnToggleUseCustomResolutionChange);
        m_iptValueResolutionWidth.onValueChanged.AddListener(OnInputFieldValueResolutionWidthChange);
        m_iptValueResolutionWidth.onEndEdit.AddListener(OnInputFieldValueResolutionWidthEndEdit);
        m_iptValueResolutionHeight.onValueChanged.AddListener(OnInputFieldValueResolutionHeightChange);
        m_iptValueResolutionHeight.onEndEdit.AddListener(OnInputFieldValueResolutionHeightEndEdit);

        m_dropdownPivotMode.value = (int)Global.PivotMode;
    }
    #endregion


    #region auto generated events
    private void OnToggleBlinkChange(bool value)
    {
        if (value == MainControl.AllowBlink)
            return;
        MainControl.AllowBlink = value;
    }
    private void OnToggleWebgalExpSupportChange(bool value)
    {
        if (value == MainControl.WebGalExpressionSupport)
            return;
        MainControl.WebGalExpressionSupport = value;
    }
    private void OnDropdownPivotModeChange(int value)
    {
        Global.PivotMode = (Global.FigurePivotMode)value;
    }
    private void OnToggleUseCustomResolutionChange(bool value)
    {
        Global.IsSetResolution = value;
    }
    private void OnInputFieldValueResolutionWidthChange(string value)
    {
        if (int.TryParse(value, out var width))
        {
            Global.NewResolutionWidth = width;
        }
    }
    private void OnInputFieldValueResolutionWidthEndEdit(string value)
    {
    }
    private void OnInputFieldValueResolutionHeightChange(string value)
    {
        if (int.TryParse(value, out var height))
        {
            Global.NewResolutionHeight = height;
        }
    }
    private void OnInputFieldValueResolutionHeightEndEdit(string value)
    {
    }

    #endregion

    protected override string Title => "实验性";
    protected override void OnInit()
    {
        base.OnInit();
    }

    public override void OnPageShown()
    {
        base.OnPageShown();
        m_toggleBlink.SetIsOnWithoutNotify(MainControl.AllowBlink);
        m_toggleWebgalExpSupport.SetIsOnWithoutNotify(MainControl.WebGalExpressionSupport);
        // m_toggle_2_4Support.SetIsOnWithoutNotify(Global.__PIVOT_2_4);
        m_toggleUseCustomResolution.SetIsOnWithoutNotify(Global.IsSetResolution);
        m_iptValueResolutionWidth.SetTextWithoutNotify(Global.NewResolutionWidth.ToString());
        m_iptValueResolutionHeight.SetTextWithoutNotify(Global.NewResolutionHeight.ToString());
    }
}

public class SettingPageNavigationWidget : UIItemWidget<SettingPageNavigationWidget>
{
    #region auto generated members
    private InputField m_iptField;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_iptField = transform.Find("Value/InputField/m_iptField").GetComponent<InputField>();

        m_iptField.onValueChanged.AddListener(OnInputFieldFieldChange);
        m_iptField.onEndEdit.AddListener(OnInputFieldEndEdit);
    }
    #endregion

    #region auto generated events
    private void OnInputFieldFieldChange(string value)
    {
        _OnValueChanged?.Invoke(value);
    }
    private void OnInputFieldEndEdit(string value)
    {
        _OnEndEdit?.Invoke(value);
    }
    #endregion

    public event Action<string> _OnValueChanged;
    public event Action<string> _OnEndEdit;


    protected override void OnInit()
    {
        base.OnInit();
    }

    public void SetValue(string value)
    {
        m_iptField.SetTextWithoutNotify(value);
    }
}

public class SettingPageNavigation : SettingPageBase<SettingPageNavigation>
{
    #region auto generated members
    private Transform m_itemCameraZoomFactor;
    private Transform m_itemCameraZoomBoostFactor;
    #endregion
    
    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_itemCameraZoomFactor = transform.Find("ScrollRect/Viewport/Content/m_itemCameraZoomFactor").GetComponent<Transform>();
        m_itemCameraZoomBoostFactor = transform.Find("ScrollRect/Viewport/Content/m_itemCameraZoomBoostFactor").GetComponent<Transform>();
    }
    #endregion
    
    protected override string Title => "导航";

    private SettingPageNavigationWidget m_itemCameraZoomFactorWidget;
    private SettingPageNavigationWidget m_itemCameraZoomBoostFactorWidget;
    protected override void OnInit()
    {
        base.OnInit();
        m_itemCameraZoomFactorWidget = SettingPageNavigationWidget.CreateWidget(m_itemCameraZoomFactor.gameObject);
        m_itemCameraZoomBoostFactorWidget = SettingPageNavigationWidget.CreateWidget(m_itemCameraZoomBoostFactor.gameObject);

        m_itemCameraZoomFactorWidget._OnEndEdit += OnCameraZoomFactorEndEdit;
        m_itemCameraZoomBoostFactorWidget._OnEndEdit += OnCameraZoomFactorBoostEndEdit;
    }

    private void OnCameraZoomFactorEndEdit(string value)
    {
        try
        {
            var num = float.Parse(value);
            Global.CameraZoomFactor = math.max(num, 1.0f);
        }
        catch
        {
            Global.CameraZoomFactor = 1.0f;
        }
        m_itemCameraZoomFactorWidget.SetValue(Global.CameraZoomFactor.ToString());
    }
    
    private void OnCameraZoomFactorBoostEndEdit(string value)
    {
        try
        {
            var num = float.Parse(value);
            Global.CameraZoomBoostFactor = num;
        }
        catch
        {
            Global.CameraZoomBoostFactor = 1.0f;
        }
        m_itemCameraZoomBoostFactorWidget.SetValue(Global.CameraZoomBoostFactor.ToString());
    }

    public override void OnPageShown()
    {
        base.OnPageShown();

        if (Global.IsLoaded)
        {
            m_itemCameraZoomFactorWidget.SetValue(Global.CameraZoomFactor.ToString());
            m_itemCameraZoomBoostFactorWidget.SetValue(Global.CameraZoomBoostFactor.ToString());
        }
    }
}

public class SettingPageThanks : SettingPageBase<SettingPageThanks>
{
    protected override string Title => "鸣谢";
    protected override void OnInit()
    {
        base.OnInit();
    }
}
