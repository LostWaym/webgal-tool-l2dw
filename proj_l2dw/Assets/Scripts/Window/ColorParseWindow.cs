using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorParseWindow : BaseWindow<ColorParseWindow>
{
    #region auto generated members
    private Button m_btnClose;
    private Text m_lblTitle;
    private InputField m_iptField;
    private Text m_lblHolder;
    private Button m_btnSubmit;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_btnClose = transform.Find("Background/Popup/Window/Top/m_btnClose").GetComponent<Button>();
        m_lblTitle = transform.Find("Background/Popup/Window/Top/m_lblTitle").GetComponent<Text>();
        m_iptField = transform.Find("Background/Popup/Window/Pages/InputField/m_iptField").GetComponent<InputField>();
        m_lblHolder = transform.Find("Background/Popup/Window/Pages/InputField/m_iptField/m_lblHolder").GetComponent<Text>();
        m_btnSubmit = transform.Find("Background/Popup/Window/Pages/m_btnSubmit").GetComponent<Button>();

        m_btnClose.onClick.AddListener(OnButtonCloseClick);
        m_iptField.onValueChanged.AddListener(OnInputFieldFieldChange);
        m_iptField.onEndEdit.AddListener(OnInputFieldFieldEndEdit);
        m_btnSubmit.onClick.AddListener(OnButtonSubmitClick);
    }
    #endregion


    #region auto generated events
    private void OnButtonCloseClick()
    {
        Close();
    }
    private void OnInputFieldFieldChange(string value)
    {
        Debug.Log("OnInputFieldFieldChange");
    }
    private void OnInputFieldFieldEndEdit(string value)
    {
        Debug.Log("OnInputFieldFieldEndEdit");
    }
    private void OnButtonSubmitClick()
    {
        if (m_onSubmit(m_iptField.text))
        {
            Close();
        }
    }
    #endregion

    private Func<string, bool> m_onSubmit;

    public void SetData(string title, string placeholder, Func<string, bool> onSubmit)
    {
        Show();
        m_lblTitle.text = title;
        m_lblHolder.text = placeholder;
        m_onSubmit = onSubmit;
    }

    public static void SetColorParser(Func<string, bool> onSubmit)
    {
        Instance.SetData("请输入颜色代码", "例：红色:255,0,0 或者 #ff0000", onSubmit);
    }
}
