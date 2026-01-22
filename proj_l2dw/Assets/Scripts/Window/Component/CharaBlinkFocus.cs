using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharaBlinkFocus : UIItemWidget<CharaBlinkFocus>
{
    #region auto generated members
    private Button m_btnClose;
    private Toggle m_toggleBlink;
    private InputField m_iptFieldBlinkInterval;
    private InputField m_iptFieldBlinkIntervalRandom;
    private InputField m_iptFieldClosingDuration;
    private InputField m_iptFieldClosedDuration;
    private InputField m_iptFieldOpeningDuration;
    private Toggle m_toggleFocus;
    private Image m_imgFocusBG;
    private Image m_imgFocusPoint;
    private TouchArea m_touchFocus;
    private InputField m_iptFieldFocusX;
    private InputField m_iptFieldFocusY;
    private Toggle m_toggleFocusInstant;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_btnClose = transform.Find("m_btnClose").GetComponent<Button>();
        m_toggleBlink = transform.Find("ScrollRectV/Viewport/Content/Blink/Container/m_toggleBlink").GetComponent<Toggle>();
        m_iptFieldBlinkInterval = transform.Find("ScrollRectV/Viewport/Content/Blink/Container/LabelValueH/Value/InputField/m_iptFieldBlinkInterval").GetComponent<InputField>();
        m_iptFieldBlinkIntervalRandom = transform.Find("ScrollRectV/Viewport/Content/Blink/Container/LabelValueH (1)/Value/InputField/m_iptFieldBlinkIntervalRandom").GetComponent<InputField>();
        m_iptFieldClosingDuration = transform.Find("ScrollRectV/Viewport/Content/Blink/Container/LabelValueH (2)/Value/InputField/m_iptFieldClosingDuration").GetComponent<InputField>();
        m_iptFieldClosedDuration = transform.Find("ScrollRectV/Viewport/Content/Blink/Container/LabelValueH (3)/Value/InputField/m_iptFieldClosedDuration").GetComponent<InputField>();
        m_iptFieldOpeningDuration = transform.Find("ScrollRectV/Viewport/Content/Blink/Container/LabelValueH (4)/Value/InputField/m_iptFieldOpeningDuration").GetComponent<InputField>();
        m_toggleFocus = transform.Find("ScrollRectV/Viewport/Content/Focus/Container/m_toggleFocus").GetComponent<Toggle>();
        m_imgFocusBG = transform.Find("ScrollRectV/Viewport/Content/Focus/Container/GameObject/m_imgFocusBG").GetComponent<Image>();
        m_imgFocusPoint = transform.Find("ScrollRectV/Viewport/Content/Focus/Container/GameObject/m_imgFocusPoint").GetComponent<Image>();
        m_touchFocus = transform.Find("ScrollRectV/Viewport/Content/Focus/Container/GameObject/m_touchFocus").GetComponent<TouchArea>();
        m_iptFieldFocusX = transform.Find("ScrollRectV/Viewport/Content/Focus/Container/LabelValueH/Value/InputField/m_iptFieldFocusX").GetComponent<InputField>();
        m_iptFieldFocusY = transform.Find("ScrollRectV/Viewport/Content/Focus/Container/LabelValueH (1)/Value/InputField/m_iptFieldFocusY").GetComponent<InputField>();
        m_toggleFocusInstant = transform.Find("ScrollRectV/Viewport/Content/Focus/Container/m_toggleFocusInstant").GetComponent<Toggle>();

        m_btnClose.onClick.AddListener(OnButtonCloseClick);
        m_toggleBlink.onValueChanged.AddListener(OnToggleBlinkChange);
        m_iptFieldBlinkInterval.onValueChanged.AddListener(OnInputFieldFieldBlinkIntervalChange);
        m_iptFieldBlinkInterval.onEndEdit.AddListener(OnInputFieldFieldBlinkIntervalEndEdit);
        m_iptFieldBlinkIntervalRandom.onValueChanged.AddListener(OnInputFieldFieldBlinkIntervalRandomChange);
        m_iptFieldBlinkIntervalRandom.onEndEdit.AddListener(OnInputFieldFieldBlinkIntervalRandomEndEdit);
        m_iptFieldClosingDuration.onValueChanged.AddListener(OnInputFieldFieldClosingDurationChange);
        m_iptFieldClosingDuration.onEndEdit.AddListener(OnInputFieldFieldClosingDurationEndEdit);
        m_iptFieldClosedDuration.onValueChanged.AddListener(OnInputFieldFieldClosedDurationChange);
        m_iptFieldClosedDuration.onEndEdit.AddListener(OnInputFieldFieldClosedDurationEndEdit);
        m_iptFieldOpeningDuration.onValueChanged.AddListener(OnInputFieldFieldOpeningDurationChange);
        m_iptFieldOpeningDuration.onEndEdit.AddListener(OnInputFieldFieldOpeningDurationEndEdit);
        m_toggleFocus.onValueChanged.AddListener(OnToggleFocusChange);
        m_iptFieldFocusX.onValueChanged.AddListener(OnInputFieldFieldFocusXChange);
        m_iptFieldFocusX.onEndEdit.AddListener(OnInputFieldFieldFocusXEndEdit);
        m_iptFieldFocusY.onValueChanged.AddListener(OnInputFieldFieldFocusYChange);
        m_iptFieldFocusY.onEndEdit.AddListener(OnInputFieldFieldFocusYEndEdit);
        m_toggleFocusInstant.onValueChanged.AddListener(OnToggleFocusInstantChange);
    }
    #endregion

    #region auto generated events
    private void OnToggleBlinkChange(bool value)
    {
        modelAdjuster.extraData.enableBlink = value;
        OnSetFieldSuccess();
    }
    private void OnToggleFocusChange(bool value)
    {
        modelAdjuster.extraData.enableFocus = value;
        OnSetFieldSuccess();
    }
    private void OnButtonCloseClick()
    {
        gameObject.SetActive(false);
    }
    private void OnInputFieldFieldBlinkIntervalChange(string value)
    {
        Debug.Log("OnInputFieldFieldBlinkIntervalChange");
    }
    private void OnInputFieldFieldBlinkIntervalEndEdit(string value)
    {
        TrySetField(value, ref modelAdjuster.extraData.blinkData.blinkInterval);
    }
    private void OnInputFieldFieldBlinkIntervalRandomChange(string value)
    {
        Debug.Log("OnInputFieldFieldBlinkIntervalRandomChange");
    }
    private void OnInputFieldFieldBlinkIntervalRandomEndEdit(string value)
    {
        TrySetField(value, ref modelAdjuster.extraData.blinkData.blinkIntervalRandom);
    }
    private void OnInputFieldFieldClosingDurationChange(string value)
    {
        Debug.Log("OnInputFieldFieldClosingDurationChange");
    }
    private void OnInputFieldFieldClosingDurationEndEdit(string value)
    {
        TrySetField(value, ref modelAdjuster.extraData.blinkData.closingDuration);
    }
    private void OnInputFieldFieldClosedDurationChange(string value)
    {
        Debug.Log("OnInputFieldFieldClosedDurationChange");
    }
    private void OnInputFieldFieldClosedDurationEndEdit(string value)
    {
        TrySetField(value, ref modelAdjuster.extraData.blinkData.closedDuration);
    }
    private void OnInputFieldFieldOpeningDurationChange(string value)
    {
        Debug.Log("OnInputFieldFieldOpeningDurationChange");
    }
    private void OnInputFieldFieldOpeningDurationEndEdit(string value)
    {
        TrySetField(value, ref modelAdjuster.extraData.blinkData.openingDuration);
    }
    private void OnInputFieldFieldFocusXChange(string value)
    {
        Debug.Log("OnInputFieldFieldFocusXChange");
    }
    private void OnInputFieldFieldFocusXEndEdit(string value)
    {
        TrySetField(value, ref modelAdjuster.extraData.focusData.x);
    }
    private void OnInputFieldFieldFocusYChange(string value)
    {
        Debug.Log("OnInputFieldFieldFocusYChange");
    }
    private void OnInputFieldFieldFocusYEndEdit(string value)
    {
        TrySetField(value, ref modelAdjuster.extraData.focusData.y);
    }
    #endregion
    private void OnToggleFocusInstantChange(bool value)
    {
        modelAdjuster.extraData.focusData.instant = value;
        OnSetFieldSuccess();
    }

    protected override void OnInit()
    {
        m_touchFocus._OnPointerDown += OnTouchFocusPointerDown;
        m_touchFocus._OnDrag += OnTouchFocusDrag;
    }

    private void OnTouchFocusPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_imgFocusBG.rectTransform, eventData.position, eventData.pressEventCamera, out var localPos);
        var rect = m_imgFocusBG.rectTransform.rect;
        var x = Mathf.Clamp(localPos.x, rect.xMin, rect.xMax);
        var y = Mathf.Clamp(localPos.y, rect.yMin, rect.yMax);
        x /= rect.width * 0.5f;
        y /= rect.height * 0.5f;
        modelAdjuster.extraData.focusData.x = x;
        modelAdjuster.extraData.focusData.y = y;
        OnSetFieldSuccess();
    }
    private void OnTouchFocusDrag(PointerEventData eventData)
    {
        OnTouchFocusPointerDown(eventData);
    }

    private ModelAdjuster modelAdjuster;
    public void SetTargetModel(ModelAdjuster modelAdjuster)
    {
        this.modelAdjuster = modelAdjuster;

        RefreshAll();
    }

    private void RefreshAll()
    {
        var focusData = modelAdjuster.extraData.focusData;
        var blinkData = modelAdjuster.extraData.blinkData;
        SetFocusData(focusData);
        SetBlinkData(blinkData);
    }

    private void SetFocusData(ModelExtraData.FocusData focusData)
    {
        m_iptFieldFocusX.SetTextWithoutNotify(focusData.x.ToString());
        m_iptFieldFocusY.SetTextWithoutNotify(focusData.y.ToString());
        m_toggleFocusInstant.SetIsOnWithoutNotify(focusData.instant);

        var x = L2DWUtils.Remap(focusData.x, -1, 1, -m_imgFocusBG.rectTransform.rect.width / 2, m_imgFocusBG.rectTransform.rect.width / 2);
        var y = L2DWUtils.Remap(focusData.y, -1, 1, -m_imgFocusBG.rectTransform.rect.height / 2, m_imgFocusBG.rectTransform.rect.height / 2);
        m_imgFocusPoint.rectTransform.localPosition = new Vector2(x, y);
    }

    private void SetBlinkData(ModelExtraData.BlinkData blinkData)
    {
        m_iptFieldBlinkInterval.SetTextWithoutNotify(blinkData.blinkInterval.ToString());
        m_iptFieldBlinkIntervalRandom.SetTextWithoutNotify(blinkData.blinkIntervalRandom.ToString());
        m_iptFieldClosingDuration.SetTextWithoutNotify(blinkData.closingDuration.ToString());
        m_iptFieldClosedDuration.SetTextWithoutNotify(blinkData.closedDuration.ToString());
        m_iptFieldOpeningDuration.SetTextWithoutNotify(blinkData.openingDuration.ToString());
    }

    private void OnSetFieldSuccess()
    {
        modelAdjuster.ApplyExtraData();
        RefreshAll();
    }
    
    private bool TrySetField(string value, ref int field)
    {
        if (int.TryParse(value, out int result))
        {
            field = result;
            OnSetFieldSuccess();
            return true;
        }
        RefreshAll();
        return false;
    }

    private bool TrySetField(string value, ref float field)
    {
        if (float.TryParse(value, out float result))
        {
            field = result;
            OnSetFieldSuccess();
            return true;
        }
        RefreshAll();
        return false;
    }

}