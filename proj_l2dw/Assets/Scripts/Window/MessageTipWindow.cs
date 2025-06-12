using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageTipWindow : BaseWindow<MessageTipWindow>
{
    #region auto generated members
    private Text m_lblTitle;
    private Text m_lblContent;
    private Button m_btnClose;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_lblTitle = transform.Find("Background/Popup/Window/Top/m_lblTitle").GetComponent<Text>();
        m_lblContent = transform.Find("Background/Popup/Window/Content/m_lblContent").GetComponent<Text>();
        m_btnClose = transform.Find("Background/Popup/Window/Top/m_btnClose").GetComponent<Button>();

        m_btnClose.onClick.AddListener(OnButtonCloseClick);
    }
    #endregion

    #region auto generated events
    private void OnButtonCloseClick()
    {
        Close();
    }
    #endregion

    public void Show(string title, string content)
    {
        Show();
        m_lblTitle.text = title;
        m_lblContent.text = content;
    }
}
