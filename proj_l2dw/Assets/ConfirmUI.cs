using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmUI : BaseWindow<ConfirmUI>
{
    #region auto generated members
    private Text m_lblTitle;
    private Text m_lblContent;
    private Button m_btnClose;
    private Button m_btnSubmit;
    private Button m_btnCancel;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_lblTitle = transform.Find("Background/Popup/Window/Top/m_lblTitle").GetComponent<Text>();
        m_lblContent = transform.Find("Background/Popup/Window/Content/m_lblContent").GetComponent<Text>();
        m_btnClose = transform.Find("Background/Popup/Window/Top/m_btnClose").GetComponent<Button>();
        m_btnSubmit = transform.Find("Background/Popup/Window/Bottom/m_btnSubmit").GetComponent<Button>();
        m_btnCancel = transform.Find("Background/Popup/Window/Bottom/m_btnCancel").GetComponent<Button>();

        m_btnClose.onClick.AddListener(OnButtonCloseClick);
        m_btnSubmit.onClick.AddListener(OnButtonSubmitClick);
        m_btnCancel.onClick.AddListener(OnButtonCancelClick);
    }
    #endregion

    #region auto generated events
    private void OnButtonCloseClick()
    {
        Close();
        _OnCancel?.Invoke();
    }
    private void OnButtonSubmitClick()
    {
        Close();
        _OnSubmit?.Invoke();
    }
    private void OnButtonCancelClick()
    {
        Close();
        _OnCancel?.Invoke();
    }
    #endregion

    public Action _OnSubmit;
    public Action _OnCancel;
    public void SetData(string title, string content, Action onSubmit, Action onCancel)
    {
        Show();
        
        m_lblTitle.text = title;
        m_lblContent.text = content;
        _OnSubmit = onSubmit;
        _OnCancel = onCancel;
    }
}
