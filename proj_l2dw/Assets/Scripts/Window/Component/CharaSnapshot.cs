using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaSnapshot : UIItemWidget<CharaSnapshot>
{
    #region auto generated members
    private Button m_btnClose;
    private Transform m_itemSnapshotItemWidget;
    private InputField m_iptSnapshotName;
    private Button m_btnApplySelected;
    private Button m_btnSaveNew;
    private Button m_btnSaveSelected;
    private Button m_btnDeleteSelected;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_btnClose = transform.Find("m_btnClose").GetComponent<Button>();
        m_itemSnapshotItemWidget = transform.Find("GameObject/ScrollRectV/Viewport/Content/m_itemSnapshotItemWidget").GetComponent<Transform>();
        m_iptSnapshotName = transform.Find("GameObject/BottomLayout/LabelValueH/Value/InputField/m_iptSnapshotName").GetComponent<InputField>();
        m_btnApplySelected = transform.Find("GameObject/BottomLayout/m_btnApplySelected").GetComponent<Button>();
        m_btnSaveNew = transform.Find("GameObject/BottomLayout/GameObject/m_btnSaveNew").GetComponent<Button>();
        m_btnSaveSelected = transform.Find("GameObject/BottomLayout/GameObject/m_btnSaveSelected").GetComponent<Button>();
        m_btnDeleteSelected = transform.Find("GameObject/BottomLayout/m_btnDeleteSelected").GetComponent<Button>();

        m_btnClose.onClick.AddListener(OnButtonCloseClick);
        m_iptSnapshotName.onValueChanged.AddListener(OnInputFieldSnapshotNameChange);
        m_iptSnapshotName.onEndEdit.AddListener(OnInputFieldSnapshotNameEndEdit);
        m_btnApplySelected.onClick.AddListener(OnButtonApplySelectedClick);
        m_btnSaveNew.onClick.AddListener(OnButtonSaveNewClick);
        m_btnSaveSelected.onClick.AddListener(OnButtonSaveSelectedClick);
        m_btnDeleteSelected.onClick.AddListener(OnButtonDeleteSelectedClick);
    }
    #endregion

    #region auto generated events
    private void OnButtonCloseClick()
    {
        gameObject.SetActive(false);
    }
    private void OnInputFieldSnapshotNameChange(string value)
    {
    }
    private void OnInputFieldSnapshotNameEndEdit(string value)
    {
        if (m_selectedSnapshotData == null)
            return;

        m_selectedSnapshotData.snapshotName = value;
        RefreshAll();
    }
    private void OnButtonApplySelectedClick()
    {
        if (m_selectedSnapshotData == null)
            return;

        if (SnapshotUtils.ApplySnapshot(m_selectedSnapshotData))
        {
            MainControl.Instance.ShowDebugText("快照应用成功");
        }
        else
        {
            MainControl.Instance.ShowDebugText("快照应用失败");
        }
    }
    private void OnButtonSaveNewClick()
    {
        if (SnapshotUtils.TakeSnapshot())
        {
            MainControl.Instance.ShowDebugText("快照保存成功");
        }
        else
        {
            MainControl.Instance.ShowDebugText("快照保存失败");
        }
        RefreshAll();
    }
    private void OnButtonSaveSelectedClick()
    {
        if (m_selectedSnapshotData == null)
        {
            return;
        }

        SnapshotData tempSnapshotData = new SnapshotData();
        tempSnapshotData.TakeSnapshot();
        if (tempSnapshotData.HasSameModelName())
        {
            MainControl.Instance.ShowMessageTip("提示", "快照中存在相同模型名称，请先删除相同模型名称的快照");
            return;
        }
        
        MainControl.Instance.ShowDebugText("保存到选中快照成功！");
        m_selectedSnapshotData.modelData = tempSnapshotData.modelData;
        RefreshAll();
    }
    private void OnButtonDeleteSelectedClick()
    {
        if (m_selectedSnapshotData == null)
            return;

        if (SnapshotUtils.DeleteSnapshot(m_selectedSnapshotData))
        {
            MainControl.Instance.ShowDebugText("快照删除成功");
        }
        else
        {
            MainControl.Instance.ShowDebugText("快照删除失败");
        }
        m_selectedSnapshotData = null;
        RefreshAll();
    }
    #endregion

    private SnapshotData m_selectedSnapshotData;
    private List<SnapshotItemWidget> m_widgets = new List<SnapshotItemWidget>();

    public void RefreshAll()
    {
        SetListItem(m_widgets, m_itemSnapshotItemWidget.gameObject, m_itemSnapshotItemWidget.parent, SnapshotUtils.snapshotDataList.Count, OnSnapshotItemCreate);
        for (int i = 0; i < SnapshotUtils.snapshotDataList.Count; i++)
        {
            var widget = m_widgets[i];
            var snapshotData = SnapshotUtils.snapshotDataList[i];
            widget.SetData(snapshotData, m_selectedSnapshotData == snapshotData);
        }

        if (m_selectedSnapshotData == null)
        {
            m_iptSnapshotName.SetTextWithoutNotify("");
        }
        else
        {
            m_iptSnapshotName.SetTextWithoutNotify(m_selectedSnapshotData.snapshotName);
        }
    }

    private void OnSnapshotItemCreate(SnapshotItemWidget widget)
    {
        widget._onSelected += OnSnapshotItemSelected;
    }

    private void OnSnapshotItemSelected(SnapshotItemWidget widget)
    {
        m_selectedSnapshotData = widget.snapshotData;
        RefreshAll();
    }
}

public class SnapshotItemWidget : UIItemWidget<SnapshotItemWidget>
{
    public Action<SnapshotItemWidget> _onSelected;


    #region auto generated members
    private GameObject m_goSelected;
    private Text m_lblName;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_goSelected = transform.Find("m_goSelected").gameObject;
        m_lblName = transform.Find("m_lblName").GetComponent<Text>();

    }
    #endregion

    #region auto generated events
    #endregion

    protected override void OnInit()
    {
        AddClickEvent(() => {
            _onSelected?.Invoke(this);
        });
    }

    public SnapshotData snapshotData;
    public void SetData(SnapshotData snapshotData, bool selected)
    {
        this.snapshotData = snapshotData;
        m_lblName.text = snapshotData.snapshotName;
        m_goSelected.SetActive(selected);
    }
}