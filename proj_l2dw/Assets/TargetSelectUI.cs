using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectUI : BaseWindow<TargetSelectUI>
{
    #region auto generated members
    private Button m_btnClose;
    private RectTransform m_rectRoot;
    private InputField m_iptSearch;
    private Button m_btnClearFilter;
    private Transform m_tfItemRoot;
    private Transform m_itemSelectedItem;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_btnClose = transform.Find("m_btnClose").GetComponent<Button>();
        m_rectRoot = transform.Find("m_rectRoot").GetComponent<RectTransform>();
        m_iptSearch = transform.Find("m_rectRoot/Search/m_iptSearch").GetComponent<InputField>();
        m_btnClearFilter = transform.Find("m_rectRoot/Search/m_btnClearFilter").GetComponent<Button>();
        m_tfItemRoot = transform.Find("m_rectRoot/Scroll View/Viewport/m_tfItemRoot").GetComponent<Transform>();
        m_itemSelectedItem = transform.Find("m_rectRoot/Scroll View/Viewport/m_tfItemRoot/m_itemSelectedItem").GetComponent<Transform>();

        m_btnClose.onClick.AddListener(OnButtonCloseClick);
        m_iptSearch.onValueChanged.AddListener(OnInputFieldSearchChange);
        m_iptSearch.onEndEdit.AddListener(OnInputFieldSearchEndEdit);
        m_btnClearFilter.onClick.AddListener(OnButtonClearFilterClick);
    }
    #endregion

    #region auto generated events
    private void OnButtonCloseClick()
    {
        Close();
    }
    private void OnInputFieldSearchChange(string value)
    {
        UpdateFilter();
    }
    private void OnInputFieldSearchEndEdit(string value)
    {
    }
    private void OnButtonClearFilterClick()
    {
        m_iptSearch.SetTextWithoutNotify("");
        UpdateFilter();
    }
    #endregion


    public Action<string> _OnTargetItemClick;
    private List<TargetItemWidget> m_listTargetItem = new List<TargetItemWidget>();
    private List<string> m_listTargetName = new List<string>();
    private List<string> m_listTargetNameFiltered = new List<string>();
    private Dictionary<string, string> m_dictTargetMark = new Dictionary<string, string>();
    private string m_markKey;
    public void SetData(RectTransform headerArea, RectTransform contentArea, List<string> targetList, string markKey = null)
    {
        Show();

        // 将rectRoot的世界坐标高度设置为与contentArea相同,宽度与headerArea相同
        var contentAreaWorldPos = contentArea.transform.position;
        var headerAreaWorldPos = headerArea.transform.position;
        var targetWorldPos = new Vector3(headerAreaWorldPos.x, contentAreaWorldPos.y, contentAreaWorldPos.z);
        m_rectRoot.position = targetWorldPos;

        // 计算rectRoot的大小,使其在世界空间中与totalArea高度相同,与searchArea宽度相同
        var contentAreaWorldRect = RectTransformUtility.PixelAdjustRect(contentArea, null);
        var headerAreaWorldRect = RectTransformUtility.PixelAdjustRect(headerArea, null);
        m_rectRoot.sizeDelta = new Vector2(headerAreaWorldRect.width, contentAreaWorldRect.height);

        // 设置iptSearch的大小与searchArea在世界空间中的大小相同
        var headerAreaWorldSize = headerArea.rect.size;
        m_iptSearch.GetComponent<RectTransform>().sizeDelta = headerAreaWorldSize;

        m_listTargetName = targetList;
        UpdateFilter();

        m_markKey = markKey;
        if (string.IsNullOrEmpty(markKey))
        {
            m_iptSearch.text = "";
        }
        else
        {
            m_iptSearch.text = m_dictTargetMark.TryGetValue(markKey, out var mark) ? mark : "";
        }
    }

    /// <summary>
    /// 直接传入宽高以及item高度的版本，需要headerArea来获得初始展示位置
    /// </summary>
    /// <param name="headerArea">用于确定初始展示位置的header区域</param>
    /// <param name="width">窗口宽度</param>
    /// <param name="height">窗口高度</param>
    /// <param name="itemHeight">每个 item 的高度</param>
    /// <param name="targetList">目标列表</param>
    /// <param name="markKey">标记 key</param>
    public void SetData(RectTransform headerArea, float width, float height, float itemHeight, List<string> targetList, string markKey = null)
    {
        Show();

        // 根据headerArea确定初始展示位置
        if (headerArea != null)
        {
            var headerAreaWorldPos = headerArea.transform.position;
            // rectRoot在世界坐标下与headerArea的x/y/z保持一致
            m_rectRoot.position = headerAreaWorldPos;
        }

        m_rectRoot.sizeDelta = new Vector2(width, height);

        // 设置iptSearch的宽度和高度
        var searchRect = m_iptSearch.GetComponent<RectTransform>();
        if (searchRect != null)
        {
            searchRect.sizeDelta = new Vector2(width, itemHeight > 0 ? itemHeight : searchRect.sizeDelta.y);
        }

        m_listTargetName = targetList;
        UpdateFilter();

        m_markKey = markKey;
        if (string.IsNullOrEmpty(markKey))
        {
            m_iptSearch.text = "";
        }
        else
        {
            m_iptSearch.text = m_dictTargetMark.TryGetValue(markKey, out var mark) ? mark : "";
        }
    }

    /// <summary>
    /// 使用默认宽高和item高度的重载方法
    /// </summary>
    /// <param name="headerArea">用于确定初始展示位置的header区域</param>
    /// <param name="targetList">目标列表</param>
    /// <param name="markKey">标记 key</param>
    public void SetData(RectTransform headerArea, List<string> targetList, string markKey = null)
    {
        float defaultWidth = 400f;
        float defaultHeight = 600f;
        float defaultItemHeight = 40f;
        SetData(headerArea, defaultWidth, defaultHeight, defaultItemHeight, targetList, markKey);
    }

    public override void Close()
    {
        if (!string.IsNullOrEmpty(m_markKey))
        {
            m_dictTargetMark[m_markKey] = m_iptSearch.text;
        }
        base.Close();
    }

    private void OnTargetItemCreate(TargetItemWidget widget)
    {
        widget._OnButtonClick += OnTargetItemButtonClick;
    }

    private void OnTargetItemButtonClick(string targetName)
    {
        _OnTargetItemClick?.Invoke(targetName);
    }

    public void UpdateFilter()
    {
        string filter = m_iptSearch.text;
        m_listTargetNameFiltered.Clear();
        m_listTargetNameFiltered.AddRange(m_listTargetName);

        if (!string.IsNullOrEmpty(filter))
        {
            var filters = filter.Split(' ').Where(f => !string.IsNullOrEmpty(f)).ToList();
            m_listTargetNameFiltered = m_listTargetName.Where(name => filters.All(filter => name.Contains(filter))).ToList();
        }

        SetListItem(m_listTargetItem, m_itemSelectedItem.gameObject, m_tfItemRoot, m_listTargetNameFiltered.Count, OnTargetItemCreate);
        for (int i = 0; i < m_listTargetNameFiltered.Count; i++)
        {
            m_listTargetItem[i].SetData(i, m_listTargetNameFiltered[i]);
        }
    }
}

public class TargetItemWidget : UIItemWidget<TargetItemWidget>
{
    #region auto generated members
    private Button m_btnButton;
    private Text m_lblTitle;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_btnButton = transform.Find("m_btnButton").GetComponent<Button>();
        m_lblTitle = transform.Find("m_btnButton/m_lblTitle").GetComponent<Text>();

        m_btnButton.onClick.AddListener(OnButtonButtonClick);
    }
    #endregion

    #region auto generated events
    private void OnButtonButtonClick()
    {
        _OnButtonClick?.Invoke(targetName);
    }
    #endregion

    public int index;
    public string targetName;
    public Action<string> _OnButtonClick;
    public void SetData(int index, string targetName)
    {
        this.index = index;
        this.targetName = targetName;
        m_lblTitle.text = targetName;
    }
}

