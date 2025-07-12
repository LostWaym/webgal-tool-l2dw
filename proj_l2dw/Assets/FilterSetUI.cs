using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterSetUI : BaseWindow<FilterSetUI>
{
    #region auto generated members
    private Button m_btnClose;
    private Text m_lblTitle;
    private InputField m_iptField;
    private Transform m_tfContent;
    private Transform m_itemFilterItem;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_btnClose = transform.Find("Window/Top/m_btnClose").GetComponent<Button>();
        m_lblTitle = transform.Find("Window/Top/m_lblTitle").GetComponent<Text>();
        m_iptField = transform.Find("Window/Pages/LabelValueH/Value/InputField/m_iptField").GetComponent<InputField>();
        m_tfContent = transform.Find("Window/Pages/ScrollRectV/Viewport/m_tfContent").GetComponent<Transform>();
        m_itemFilterItem = transform.Find("Window/Pages/ScrollRectV/Viewport/m_tfContent/m_itemFilterItem").GetComponent<Transform>();

        m_btnClose.onClick.AddListener(OnButtonCloseClick);
        m_iptField.onValueChanged.AddListener(OnInputFieldFieldChange);
        m_iptField.onEndEdit.AddListener(OnInputFieldFieldEndEdit);
    }
    #endregion

    #region auto generated events
    private void OnButtonCloseClick()
    {
        Close();
    }
    private void OnInputFieldFieldChange(string value)
    {
        if (selectedData == null)
            return;

        selectedData.name = value;
        nameModified = true;
        RefreshAll();
    }
    private void OnInputFieldFieldEndEdit(string value)
    {
        //
    }
    #endregion

    protected override void OnInit()
    {
        base.OnInit();
    }

    protected override void OnClose()
    {
        base.OnClose();
        if (nameModified)
        {
            FilterUtils.SaveFilterSetPreset();
        }
    }

    public FilterSetPresetData selectedData;
    private Action<FilterSetPresetData> onApply;

    public bool nameModified = false;

    public List<FilterSetPresetWidget> m_widgets = new();
    public void SetData(FilterSetPresetData data, Action<FilterSetPresetData> onApply)
    {
        Show();
        selectedData = data;
        this.onApply = onApply;
        
        RefreshAll();
    }

    private void RefreshAll()
    {
        if (selectedData != null)
        {
            m_iptField.SetTextWithoutNotify(selectedData.name);
        }

        var count = FilterUtils.filterSetPresets.Count;
        SetListItem(m_widgets, m_itemFilterItem.gameObject, m_tfContent, count, (widget) => {
            widget._onApply += OnFilterSetPresetWidgetApply;
            widget._onDelete += OnFilterSetPresetWidgetDelete;
            widget.AddClickEvent(()=>{
                OnWidgetSelected(widget);
            });
        });
        for (int i = 0; i < count; i++)
        {
            var widget = m_widgets[i];
            var data = FilterUtils.filterSetPresets[i];
            widget.SetData(data, data == selectedData);
        }
    }

    private void OnWidgetSelected(FilterSetPresetWidget widget)
    {
        selectedData = widget.data;
        RefreshAll();
    }

    private void OnFilterSetPresetWidgetApply(FilterSetPresetWidget widget)
    {
        onApply?.Invoke(widget.data);
        Close();
    }

    private void OnFilterSetPresetWidgetDelete(FilterSetPresetWidget widget)
    {
        FilterUtils.RemoveFilterSetPreset(widget.data);
        if (widget.data == selectedData)
        {
            selectedData = null;
        }
        RefreshAll();
    }
}

public class FilterSetPresetWidget : UIItemWidget<FilterSetPresetWidget>
{
    #region auto generated members
    private Text m_lblTitle;
    private Button m_btnApply;
    private Button m_btnDelete;
    private GameObject m_goUnSelected;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_lblTitle = transform.Find("m_lblTitle").GetComponent<Text>();
        m_btnApply = transform.Find("m_btnApply").GetComponent<Button>();
        m_btnDelete = transform.Find("m_btnDelete").GetComponent<Button>();
        m_goUnSelected = transform.Find("m_goUnSelected").gameObject;

        m_btnApply.onClick.AddListener(OnButtonApplyClick);
        m_btnDelete.onClick.AddListener(OnButtonDeleteClick);
    }
    #endregion

    #region auto generated events
    private void OnButtonApplyClick()
    {
        _onApply?.Invoke(this);
    }
    private void OnButtonDeleteClick()
    {
        _onDelete?.Invoke(this);
    }
    #endregion

    public Action<FilterSetPresetWidget> _onApply;
    public Action<FilterSetPresetWidget> _onDelete;
    public FilterSetPresetData data;
    public void SetData(FilterSetPresetData data, bool selected)
    {
        this.data = data;
        m_lblTitle.text = data.name;
        m_goUnSelected.SetActive(!selected);
    }
}