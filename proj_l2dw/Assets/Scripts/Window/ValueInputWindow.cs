using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueInputWindow : BaseWindow<ValueInputWindow>
{
    private Action<ValueInputPanelStatus> m_onSubmit;
    private Action m_onCancel;

    #region auto-generated code
    private Text m_lblTitle;
    private InputField m_iptValue;
    private Button m_btnSubmit;
    private Button m_btnCancel;
    private Toggle m_toggleCheck;
    private Text m_lblCheck;

    protected override void CodeGenBindMembers()
    {
        m_lblTitle = transform.Find("Image/m_lblTitle").GetComponent<Text>();
        m_iptValue = transform.Find("Image/m_iptValue").GetComponent<InputField>();
        m_btnSubmit = transform.Find("Image/m_btnSubmit").GetComponent<Button>();
        m_btnCancel = transform.Find("Image/m_btnCancel").GetComponent<Button>();
        m_toggleCheck = transform.Find("Image/m_toggleCheck").GetComponent<Toggle>();
        m_lblCheck = transform.Find("Image/m_toggleCheck/m_lblCheck").GetComponent<Text>();
        m_btnSubmit.onClick.AddListener(OnSubmitClick);
        m_btnCancel.onClick.AddListener(OnCancelClick);
    }
    #endregion

    #region auto-generated code event
        public void OnSubmitClick()
        {
            ValueInputPanelStatus status = new ValueInputPanelStatus();
            status.result = m_iptValue.text;
            status.isCheck = m_toggleCheck.isOn;
            try
            {
                m_onSubmit?.Invoke(status);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            Close();
        }
        public void OnCancelClick()
        {
            try
            {
                m_onCancel?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            Close();
        }
    #endregion

    protected override void OnInit()
    {
        base.OnInit();
        m_iptValue.onSubmit.AddListener(OnSubmitClick);
    }

    private void OnSubmitClick(string arg0)
    {
        OnSubmitClick();
    }

    public void Show(string title, string value, Action<ValueInputPanelStatus> onSubmit, Action onCancel)
    {
        canvas.enabled = true;
        m_lblTitle.text = title;
        m_iptValue.text = value;
        m_onSubmit = onSubmit;
        m_onCancel = onCancel;
        m_toggleCheck.gameObject.SetActive(false);

        m_iptValue.ActivateInputField();
    }

    public void SetToggle(bool isCheck, string text)
    {
        m_toggleCheck.gameObject.SetActive(true);
        m_toggleCheck.isOn = isCheck;
        m_lblCheck.text = text;
    }

    public void Close()
    {
        m_onSubmit = null;
        m_onCancel = null;
        canvas.enabled = false;
    }
}

public class ValueInputPanelStatus
{
    public string result;
    public bool isCheck;
}