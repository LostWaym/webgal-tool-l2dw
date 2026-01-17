using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MEEditUI : BaseWindow<MEEditUI>
{
    public enum MEEditType
    {
        Motion,
        Expression,
    }

    #region auto generated members
    private Button m_btnClose;
    private Text m_lblTitle;
    private InputField m_iptFilter;
    private ScrollRect m_scrollContent;
    private Transform m_tfContent;
    private Transform m_itemMEWidget;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_btnClose = transform.Find("Window/Top/m_btnClose").GetComponent<Button>();
        m_lblTitle = transform.Find("Window/Top/m_lblTitle").GetComponent<Text>();
        m_iptFilter = transform.Find("Window/Pages/SearchBarInputField/m_iptFilter").GetComponent<InputField>();
        m_scrollContent = transform.Find("Window/Pages/m_scrollContent").GetComponent<ScrollRect>();
        m_tfContent = transform.Find("Window/Pages/m_scrollContent/Viewport/m_tfContent").GetComponent<Transform>();
        m_itemMEWidget = transform.Find("Window/Pages/m_scrollContent/Viewport/m_tfContent/m_itemMEWidget").GetComponent<Transform>();

        m_btnClose.onClick.AddListener(OnButtonCloseClick);
        m_iptFilter.onValueChanged.AddListener(OnInputFieldFilterChange);
        m_iptFilter.onEndEdit.AddListener(OnInputFieldFilterEndEdit);
    }
    #endregion


    #region auto generated events
    private void OnButtonCloseClick()
    {
        Close();
    }
    private void OnInputFieldFilterChange(string value)
    {
        L2DWUtils.ResetScrollViewTop(m_scrollContent);
        RefreshAll();
    }
    private void OnInputFieldFilterEndEdit(string value)
    {
    }
    #endregion

    private MEEditType m_type;
    private List<JSONObject> m_jsons = new ();
    private List<string> m_jsonPaths;
    private List<string> m_items = new ();
    public void SetData(MEEditType type, List<string> jsonPaths)
    {
        Show();
        m_type = type;
        m_jsonPaths = jsonPaths;
        m_lblTitle.text = type == MEEditType.Motion ? "动作编辑" : "表情编辑";
        m_iptFilter.SetTextWithoutNotify("");

        m_jsons.Clear();
        foreach (var jsonPath in jsonPaths)
        {
            m_jsons.Add(GetJsonObject(jsonPath));
        }

        RebuildItems();
        RefreshAll();
    }

    private JSONObject GetJsonObject(string filePath)
    {
        var file = System.IO.File.ReadAllText(filePath);
        return new JSONObject(file);
    }

    private void RebuildItems()
    {
        m_items.Clear();

        HashSet<string> names = new ();

        foreach (var json in m_jsons)
        {
            if (m_type == MEEditType.Motion)
            {
                var motions = json.GetField("motions");
                if (motions != null)
                {
                    names.UnionWith(motions.keys);
                }
            }
            else if (m_type == MEEditType.Expression)
            {
                var expressions = json.GetField("expressions");
                if (expressions != null)
                {
                    foreach (var expression in expressions.list)
                    {
                        var name = expression.GetField("name");
                        if (name == null)
                            continue;

                        names.Add(name.str);
                    }
                }
            }
        }

        m_items.AddRange(names);
    }

    private List<MEEditItemWidget> m_widgets = new ();
    private void RefreshAll()
    {
        var filteredItems = L2DWUtils.FilterItems(m_items, x => x, m_iptFilter.text);
        filteredItems.Sort();
        SetListItem(m_widgets, m_itemMEWidget.gameObject, m_tfContent, filteredItems.Count, null);
        for (int i = 0; i < filteredItems.Count; i++)
        {
            var widget = m_widgets[i];
            widget.SetData(filteredItems[i]);
        }
    }

    public void AskDeleteItem(string name)
    {
        ConfirmUI.Instance.SetData("删除动画/表情", $"确定要删除 {name} 吗？\n删除后将无法恢复", () =>
        {
            foreach (var json in m_jsons)
            {
                if (m_type == MEEditType.Motion)
                {
                    json.GetField("motions").RemoveField(name);
                }
                else if (m_type == MEEditType.Expression)
                {
                    var expressions = json.GetField("expressions");
                    if (expressions != null)
                    {
                        foreach (var expression in expressions.list)
                        {
                            var nameObj = expression.GetField("name");
                            if (nameObj == null)
                                continue;

                            if (nameObj.str == name)
                            {
                                expressions.list.Remove(expression);
                                break;
                            }
                        }
                    }
                }
            }

            ApplyJsons();
            RebuildItems();
            RefreshAll();
        }, null);
    }

    public void AskChangeItemName(string oldName, string newName)
    {
        ConfirmUI.Instance.SetData("修改动画/表情名称", $"确定要修改 {oldName} 的名称为 {newName} 吗？\n修改后将无法恢复", () =>
        {
            if (m_items.Contains(newName))
            {
                MessageTipWindow.Instance.Show("提示", "名称已存在");
                return;
            }

            foreach (var json in m_jsons)
            {
                if (m_type == MEEditType.Motion)
                {
                    var motions = json.GetField("motions");
                    if (motions != null)
                    {
                        var ind = motions.keys.IndexOf(oldName);
                        if (ind >= 0)
                        {
                            motions.keys[ind] = newName;
                        }
                    }
                }
                else if (m_type == MEEditType.Expression)
                {
                    var expressions = json.GetField("expressions");
                    if (expressions != null)
                    {
                        for (int i = 0; i < expressions.list.Count; i++)
                        {
                            JSONObject expression = expressions.list[i];
                            var nameObj = expression.GetField("name");
                            if (nameObj == null)
                                continue;

                            if (nameObj.str == oldName)
                            {
                                nameObj.str = newName;
                                break;
                            }
                        }
                    }
                }
            }

            ApplyJsons();
            RebuildItems();
            RefreshAll();
        }, RefreshAll);
    }

    private void ApplyJsons()
    {
        for (int i = 0; i < m_jsons.Count; i++)
        {
            JSONObject json = m_jsons[i];
            System.IO.File.WriteAllText(m_jsonPaths[i], json.ToString(true));
        }
    }
}

public class MEEditItemWidget : UIItemWidget<MEEditItemWidget>
{
    #region auto generated members
    private InputField m_iptFilter;
    private Button m_btnRemove;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_iptFilter = transform.Find("m_iptFilter").GetComponent<InputField>();
        m_btnRemove = transform.Find("m_btnRemove").GetComponent<Button>();

        m_iptFilter.onValueChanged.AddListener(OnInputFieldFilterChange);
        m_iptFilter.onEndEdit.AddListener(OnInputFieldFilterEndEdit);
        m_btnRemove.onClick.AddListener(OnButtonRemoveClick);
    }
    #endregion

    #region auto generated events
    private void OnInputFieldFilterChange(string value)
    {
        Debug.Log("OnInputFieldFilterChange");
    }
    private void OnInputFieldFilterEndEdit(string value)
    {
        if (m_name == m_iptFilter.text)
            return;
        MEEditUI.Instance.AskChangeItemName(m_name, m_iptFilter.text);
    }
    private void OnButtonRemoveClick()
    {
        MEEditUI.Instance.AskDeleteItem(m_name);
    }
    #endregion

    private string m_name;
    public void SetData(string name)
    {
        m_name = name;
        m_iptFilter.SetTextWithoutNotify(name);
    }
}