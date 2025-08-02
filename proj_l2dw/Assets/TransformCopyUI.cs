using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformCopyUI : BaseWindow<TransformCopyUI>
{
    #region auto generated members
    private Button m_btnClose;
    private Text m_lblTitle;
    private Toggle m_toggleCopyPosition;
    private Toggle m_toggleCopyScale;
    private Toggle m_toggleCopyRotate;
    private Toggle m_toggleCopyFilter;
    private Button m_btnApply;
    private Button m_btnCopyAll;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_btnClose = transform.Find("Window/Top/m_btnClose").GetComponent<Button>();
        m_lblTitle = transform.Find("Window/Top/m_lblTitle").GetComponent<Text>();
        m_toggleCopyPosition = transform.Find("Window/Pages/m_toggleCopyPosition").GetComponent<Toggle>();
        m_toggleCopyScale = transform.Find("Window/Pages/m_toggleCopyScale").GetComponent<Toggle>();
        m_toggleCopyRotate = transform.Find("Window/Pages/m_toggleCopyRotate").GetComponent<Toggle>();
        m_toggleCopyFilter = transform.Find("Window/Pages/m_toggleCopyFilter").GetComponent<Toggle>();
        m_btnApply = transform.Find("Window/Pages/m_btnApply").GetComponent<Button>();
        m_btnCopyAll = transform.Find("Window/Pages/m_btnCopyAll").GetComponent<Button>();

        m_btnClose.onClick.AddListener(OnButtonCloseClick);
        m_toggleCopyPosition.onValueChanged.AddListener(OnToggleCopyPositionChange);
        m_toggleCopyScale.onValueChanged.AddListener(OnToggleCopyScaleChange);
        m_toggleCopyRotate.onValueChanged.AddListener(OnToggleCopyRotateChange);
        m_toggleCopyFilter.onValueChanged.AddListener(OnToggleCopyFilterChange);
        m_btnApply.onClick.AddListener(OnButtonApplyClick);
        m_btnCopyAll.onClick.AddListener(OnButtonCopyAllClick);
    }
    #endregion

    #region auto generated events
    private void OnButtonCloseClick()
    {
        Close();
    }
    private void OnToggleCopyPositionChange(bool value)
    {
        Debug.Log("OnToggleCopyPositionChange");
    }
    private void OnToggleCopyScaleChange(bool value)
    {
        Debug.Log("OnToggleCopyScaleChange");
    }
    private void OnToggleCopyRotateChange(bool value)
    {
        Debug.Log("OnToggleCopyRotateChange");
    }
    private void OnToggleCopyFilterChange(bool value)
    {
        Debug.Log("OnToggleCopyFilterChange");
    }
    private void OnButtonApplyClick()
    {
        MainControl.Instance.LoadTransform(m_toggleCopyPosition.isOn, m_toggleCopyScale.isOn, m_toggleCopyRotate.isOn, m_toggleCopyFilter.isOn);
        Close();
    }
    private void OnButtonCopyAllClick()
    {
        MainControl.Instance.LoadTransform(true, true, true, true);
        Close();
    }
    #endregion

}
