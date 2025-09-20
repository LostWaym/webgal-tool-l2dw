using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MotionSelectUI : BaseWindow<MotionSelectUI>
{
    #region auto generated members
    private InputField m_iptFieldW;
    private InputField m_iptFieldH;
    private RawImage m_rawChara;
    private InputField m_iptFilterMotion;
    private ScrollRect m_scrollMotion;
    private Transform m_tfMotionItems;
    private Transform m_itemMotion;
    private InputField m_iptFilterExpression;
    private ScrollRect m_scrollExpression;
    private Transform m_tfExpressionItems;
    private Transform m_itemExpression;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_iptFieldW = transform.Find("mask/项宽/Value/InputField/m_iptFieldW").GetComponent<InputField>();
        m_iptFieldH = transform.Find("mask/项高/Value/InputField/m_iptFieldH").GetComponent<InputField>();
        m_rawChara = transform.Find("mask/m_rawChara").GetComponent<RawImage>();
        m_iptFilterMotion = transform.Find("mask/GameObject/Motion/Title/InputField/m_iptFilterMotion").GetComponent<InputField>();
        m_scrollMotion = transform.Find("mask/GameObject/Motion/Container/m_scrollMotion").GetComponent<ScrollRect>();
        m_tfMotionItems = transform.Find("mask/GameObject/Motion/Container/m_scrollMotion/Viewport/m_tfMotionItems").GetComponent<Transform>();
        m_itemMotion = transform.Find("mask/GameObject/Motion/Container/m_scrollMotion/Viewport/m_tfMotionItems/m_itemMotion").GetComponent<Transform>();
        m_iptFilterExpression = transform.Find("mask/GameObject/Expression/Title/InputField/m_iptFilterExpression").GetComponent<InputField>();
        m_scrollExpression = transform.Find("mask/GameObject/Expression/Container/m_scrollExpression").GetComponent<ScrollRect>();
        m_tfExpressionItems = transform.Find("mask/GameObject/Expression/Container/m_scrollExpression/Viewport/m_tfExpressionItems").GetComponent<Transform>();
        m_itemExpression = transform.Find("mask/GameObject/Expression/Container/m_scrollExpression/Viewport/m_tfExpressionItems/m_itemExpression").GetComponent<Transform>();

        m_iptFieldW.onValueChanged.AddListener(OnInputFieldFieldWChange);
        m_iptFieldW.onEndEdit.AddListener(OnInputFieldFieldWEndEdit);
        m_iptFieldH.onValueChanged.AddListener(OnInputFieldFieldHChange);
        m_iptFieldH.onEndEdit.AddListener(OnInputFieldFieldHEndEdit);
        m_iptFilterMotion.onValueChanged.AddListener(OnInputFieldFilterMotionChange);
        m_iptFilterMotion.onEndEdit.AddListener(OnInputFieldFilterMotionEndEdit);
        m_iptFilterExpression.onValueChanged.AddListener(OnInputFieldFilterExpressionChange);
        m_iptFilterExpression.onEndEdit.AddListener(OnInputFieldFilterExpressionEndEdit);
    }
    #endregion

    #region auto generated events
    private void OnInputFieldFieldWChange(string value)
    {
    }
    private void OnInputFieldFieldWEndEdit(string value)
    {
        RefreshItemSize();
    }
    private void OnInputFieldFieldHChange(string value)
    {
    }
    private void OnInputFieldFieldHEndEdit(string value)
    {
        RefreshItemSize();
    }
    private void OnInputFieldFilterMotionChange(string value)
    {
        var model = MainControl.Instance.curTarget;
        if (model == null || !model.HasMotions)
        {
            return;
        }
        model.meta.m_filterMotion = value;
        RefreshAll();
    }
    private void OnInputFieldFilterMotionEndEdit(string value)
    {
    }
    private void OnInputFieldFilterExpressionChange(string value)
    {
        var model = MainControl.Instance.curTarget;
        if (model == null || !model.HasMotions)
        {
            return;
        }
        model.meta.m_filterExp = value;
        RefreshAll();
    }
    private void OnInputFieldFilterExpressionEndEdit(string value)
    {
    }
    #endregion

    protected override void OnShow()
    {
        base.OnShow();
        var model = MainControl.Instance.curTarget;
        if (model == null || !model.HasMotions)
        {
            Close();
            return;
        }

        Vector2 size = m_tfExpressionItems.GetComponent<GridLayoutGroup>().cellSize;
        if (PlayerPrefs.HasKey("MotionSelectUI_ExpressionSize"))
        {
            size = JsonUtility.FromJson<Vector2>(PlayerPrefs.GetString("MotionSelectUI_ExpressionSize"));
        }

        m_iptFieldW.SetTextWithoutNotify(size.x.ToString());
        m_iptFieldH.SetTextWithoutNotify(size.y.ToString());
        RefreshItemSize();

        m_rawChara.texture = model.GetCharaTexture();
        // L2DWUtils.AutoSizeRawImage(m_rawChara, model.GetCharaTexture());
        RefreshAll();
    }

    protected override void OnClose()
    {
        base.OnClose();
        PlayerPrefs.SetString("MotionSelectUI_ExpressionSize", JsonUtility.ToJson(m_tfExpressionItems.GetComponent<GridLayoutGroup>().cellSize));
    }

    private void UpdateFilterText()
    {
        var model = MainControl.Instance.curTarget;
        if (model == null || !model.HasMotions)
        {
            return;
        }
        m_iptFilterMotion.SetTextWithoutNotify(model.meta.m_filterMotion);
        m_iptFilterExpression.SetTextWithoutNotify(model.meta.m_filterExp);
    }

    private void RefreshItemSize()
    {
        m_tfExpressionItems.GetComponent<GridLayoutGroup>().cellSize = new Vector2(float.Parse(m_iptFieldW.text), float.Parse(m_iptFieldH.text));
        m_tfMotionItems.GetComponent<GridLayoutGroup>().cellSize = new Vector2(float.Parse(m_iptFieldW.text), float.Parse(m_iptFieldH.text));
    }

    private void RefreshAll()
    {
        UpdateFilterText();
        RefreshExpressionList();
        RefreshMotionList();
    }

    private List<MotionEntryWidget> m_listExpression = new List<MotionEntryWidget>();
    private List<MotionEntryWidget> m_listMotion = new List<MotionEntryWidget>();
    public void RefreshExpressionList()
    {
        var model = MainControl.Instance.curTarget;
        if (model == null || !model.HasMotions)
        {
            SetListItem(m_listExpression, m_itemExpression.gameObject, m_tfExpressionItems, 0, OnExpressionItemCreate);
            return;
        }

        var pairs = model.ExpPairs;

        if (!string.IsNullOrEmpty(model.meta.m_filterExp))
        {
            var filters = model.meta.m_filterExp.Split(' ').Where(s => !string.IsNullOrEmpty(s)).Select(s => s.ToLower());
            pairs = pairs.Where(x => filters.All(f => x.name.ToLower().Contains(f))).ToList();
        }

        SetListItem(m_listExpression, m_itemExpression.gameObject, m_tfExpressionItems, pairs.Count, OnExpressionItemCreate);
        var selectedExpression = model.curExpName;
        for (int i = 0; i < pairs.Count; i++)
        {
            var item = m_listExpression[i];
            var pair = pairs[i];
            item.SetData(pair.name);
            if (pair.name == selectedExpression)
            {
                item.SetStateStyle(UIStateStyle.UIState.Selected);
            }
            else
            {
                item.SetStateStyle(UIStateStyle.UIState.Normal);
            }
        }
    }

    public void RefreshMotionList()
    {
        var model = MainControl.Instance.curTarget;
        if (model == null || !model.HasMotions)
        {
            SetListItem(m_listMotion, m_itemMotion.gameObject, m_tfMotionItems, 0, OnMotionItemCreate);
            return;
        }

        var pairs = model.MotionPairs;

        if (!string.IsNullOrEmpty(model.meta.m_filterMotion))
        {
            var filters = model.meta.m_filterMotion.Split(' ').Where(s => !string.IsNullOrEmpty(s)).Select(s => s.ToLower());
            pairs = pairs.Where(x => filters.All(f => x.name.ToLower().Contains(f))).ToList();
        }

        SetListItem(m_listMotion, m_itemMotion.gameObject, m_tfMotionItems, pairs.Count, OnMotionItemCreate);
        var selectedMotion = model.curMotionName;
        for (int i = 0; i < pairs.Count; i++)
        {
            var item = m_listMotion[i];
            var pair = pairs[i];
            item.SetData(pair.name);
            if (pair.name == selectedMotion)
            {
                item.SetStateStyle(UIStateStyle.UIState.Selected);
            }
            else
            {
                item.SetStateStyle(UIStateStyle.UIState.Normal);
            }
        }
    }

    private void OnExpressionItemCreate(MotionEntryWidget widget)
    {
        widget.AddClickEvent(() => OnExpressionClicked(widget));
    }

    private void OnExpressionClicked(MotionEntryWidget widget)
    {
        MainControl.Instance.PlayExp(widget.name);
        RefreshExpressionList();
    }

    private void OnMotionItemCreate(MotionEntryWidget widget)
    {
        widget.AddClickEvent(() => OnMotionClicked(widget));
    }

    private void OnMotionClicked(MotionEntryWidget widget)
    {
        MainControl.Instance.PlayMotion(widget.name);
        RefreshMotionList();
    }
}
