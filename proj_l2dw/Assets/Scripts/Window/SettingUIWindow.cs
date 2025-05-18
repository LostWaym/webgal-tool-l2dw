using System;
using System.Collections;
using System.Collections.Generic;
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
        m_toggleGeneral = transform.Find("Image/Panel/Left/Viewport/Content/m_toggleGeneral").GetComponent<Toggle>();
        m_toggleNavigation = transform.Find("Image/Panel/Left/Viewport/Content/m_toggleNavigation").GetComponent<Toggle>();
        m_toggleExperiment = transform.Find("Image/Panel/Left/Viewport/Content/m_toggleExperiment").GetComponent<Toggle>();
        m_toggleManual = transform.Find("Image/Panel/Left/Viewport/Content/m_toggleManual").GetComponent<Toggle>();
        m_toggleAbout = transform.Find("Image/Panel/Left/Viewport/Content/m_toggleAbout").GetComponent<Toggle>();
        m_toggleThanks = transform.Find("Image/Panel/Left/Viewport/Content/m_toggleThanks").GetComponent<Toggle>();
        m_itemSettingPageGeneral = transform.Find("Image/Panel/Right/Pages/m_itemSettingPageGeneral").GetComponent<Transform>();
        m_itemSettingPageNavigation = transform.Find("Image/Panel/Right/Pages/m_itemSettingPageNavigation").GetComponent<Transform>();
        m_itemSettingPageExperiment = transform.Find("Image/Panel/Right/Pages/m_itemSettingPageExperiment").GetComponent<Transform>();
        m_itemSettingPageManual = transform.Find("Image/Panel/Right/Pages/m_itemSettingPageManual").GetComponent<Transform>();
        m_itemSettingPageAbout = transform.Find("Image/Panel/Right/Pages/m_itemSettingPageAbout").GetComponent<Transform>();
        m_itemSettingPageThanks = transform.Find("Image/Panel/Right/Pages/m_itemSettingPageThanks").GetComponent<Transform>();
        m_lblTitle = transform.Find("Image/Panel/Right/Top/m_lblTitle").GetComponent<Text>();
        m_btnClose = transform.Find("Image/Panel/Right/Top/m_btnClose").GetComponent<Button>();

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
        m_iptField = transform.Find("Value/m_iptField").GetComponent<InputField>();

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
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_itemModelPath = transform.Find("Viewport/Content/m_itemModelPath").GetComponent<Transform>();
        m_itemBGPath = transform.Find("Viewport/Content/m_itemBGPath").GetComponent<Transform>();
        m_itemBGChangeTemplate = transform.Find("Viewport/Content/m_itemBGChangeTemplate").GetComponent<Transform>();
        m_itemBGTransformTemplate = transform.Find("Viewport/Content/m_itemBGTransformTemplate").GetComponent<Transform>();
        m_toggleCloseGreenLine = transform.Find("Viewport/Content/m_toggleCloseGreenLine").GetComponent<Toggle>();

        m_toggleCloseGreenLine.onValueChanged.AddListener(OnToggleCloseGreenLineChange);
    }
    #endregion

    #region auto generated events
    private void OnToggleCloseGreenLineChange(bool value)
    {
        MainControl.Instance.SetCloseGreenLine(value);
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
    private Toggle m_toggle_2_4Support;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_toggleBlink = transform.Find("Viewport/Content/m_toggleBlink").GetComponent<Toggle>();
        m_toggleWebgalExpSupport = transform.Find("Viewport/Content/m_toggleWebgalExpSupport").GetComponent<Toggle>();
        m_toggle_2_4Support = transform.Find("Viewport/Content/m_toggle_2_4Support").GetComponent<Toggle>();

        m_toggleBlink.onValueChanged.AddListener(OnToggleBlinkChange);
        m_toggleWebgalExpSupport.onValueChanged.AddListener(OnToggleWebgalExpSupportChange);
        m_toggle_2_4Support.onValueChanged.AddListener(OnToggle_2_4SupportChange);
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
    private void OnToggle_2_4SupportChange(bool value)
    {
        if (value == Global.__PIVOT_2_4)
            return;

        Global.__PIVOT_2_4 = value;
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
        m_toggleBlink.isOn = MainControl.AllowBlink;
        m_toggleWebgalExpSupport.isOn = MainControl.WebGalExpressionSupport;
        m_toggle_2_4Support.isOn = Global.__PIVOT_2_4;
    }
}

public class SettingPageNavigation : SettingPageBase<SettingPageNavigation>
{
    protected override string Title => "导航";
    protected override void OnInit()
    {
        base.OnInit();
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
