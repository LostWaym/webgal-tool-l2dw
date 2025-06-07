using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotionDataSettingUI : BaseWindow<MotionDataSettingUI>
{
    #region auto generated members
    private InputField m_iptName;
    private Button m_btnSave;
    private Button m_btnCancel;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_iptName = transform.Find("bg/GameObject/Scroll View/Viewport/Content/view/角色名/m_iptName").GetComponent<InputField>();
        m_btnSave = transform.Find("bg/GameObject/m_btnSave").GetComponent<Button>();
        m_btnCancel = transform.Find("bg/GameObject/m_btnCancel").GetComponent<Button>();

        m_iptName.onValueChanged.AddListener(OnInputFieldNameChange);
        m_iptName.onEndEdit.AddListener(OnInputFieldNameEndEdit);
        m_btnSave.onClick.AddListener(OnButtonSaveClick);
        m_btnCancel.onClick.AddListener(OnButtonCancelClick);
    }
    #endregion


    #region auto generated events
    private void OnInputFieldNameChange(string value)
    {
        Debug.Log("OnInputFieldNameChange");
    }
    private void OnInputFieldNameEndEdit(string value)
    {
        Debug.Log("OnInputFieldNameEndEdit");
    }
    private void OnButtonSaveClick()
    {
        DoSave();
        Close();
    }
    private void OnButtonCancelClick()
    {
        Close();
    }
    #endregion

    private Live2dMotionData m_motionData;
    private Action<Live2dMotionData> m_onSave;
    public void SetData(Live2dMotionData motionData, Action<Live2dMotionData> onSave = null)
    {
        Show();
        m_motionData = motionData;
        m_iptName.text = motionData.motionDataName;
        m_onSave = onSave;
    }

    private void DoSave()
    {
        m_motionData.motionDataName = m_iptName.text.Trim();
        m_onSave?.Invoke(m_motionData);
    }
}
