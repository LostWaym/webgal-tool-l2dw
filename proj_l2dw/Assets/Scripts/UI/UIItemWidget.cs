using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIItemWidget
{
    public GameObject gameObject;
    public Transform transform;
    protected MonoUIStyle style;
    public UIStateStyle StateStyle => style.style;
    public bool IsActive => gameObject && gameObject.activeInHierarchy;

    public void Create(GameObject gameObject)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
        style = gameObject.GetComponent<MonoUIStyle>();
        CodeGenBindMembers();
        OnInit();
    }

    protected virtual void CodeGenBindMembers()
    {
    }

    protected virtual void OnInit()
    {

    }

    public void SetSimpleHideVertical(Transform root, Transform prefab, int itemSpacing, Vector2 verticalPadding, Action<int, bool, Vector2> onItemStatusChanged)
    {
        var rect = prefab.GetComponent<RectTransform>();
        int itemWidth = (int)rect.sizeDelta.x;
        int itemHeight = (int)rect.sizeDelta.y;
        SetSimpleHideVertical(root, itemWidth, itemHeight, itemSpacing, verticalPadding, onItemStatusChanged);
    }

    public void SetSimpleHideVertical(Transform root, int itemWidth, int itemHeight, int itemSpacing, Vector2 verticalPadding, Action<int, bool, Vector2> onItemStatusChanged)
    {
        var comp = root.GetOrAddComponent<SimpleHideUI>();
        comp.itemWidth = itemWidth;
        comp.itemHeight = itemHeight;
        comp.itemSpacing = itemSpacing;
        comp.verticalPadding = verticalPadding;
        comp.OnWillingSetItemStatus = onItemStatusChanged;
        comp.CalcSize();
        comp.UpdateVisible(true, true);
    }

    private void UpdateSimpleHideVertical(Transform root, int count)
    {
        var comp = root.GetComponent<SimpleHideUI>();
        if (comp != null)
        {
            comp.itemCount = count;
            comp.CalcSize();
            comp.UpdateVisible(true, true);
        }
    }

    public void SetSimpleHideVerticalListItem<ListItem>(List<ListItem> list, GameObject prefab, Transform root, int count, Action<ListItem> onItemCreate, Action<int> itemRenderer) where ListItem : UIItemWidget<ListItem>, new()
    {
        prefab.gameObject.SetActive(false);

        //如果列表数量比count大，那就只显示count个
        if (list.Count < count)
        {
            for (int i = list.Count; i < count; i++)
            {
                GameObject gameObject = GameObject.Instantiate(prefab, root);
                ListItem item = new ListItem();
                item.Create(gameObject);
                list.Add(item);
                gameObject.transform.SetParent(root);
                onItemCreate?.Invoke(item);
            }
        }

        var comp = root.GetComponent<SimpleHideUI>();
        comp.itemCount = count;
        comp.capacity = list.Count;
        comp.CalcSize();
        comp.OnWillingRenderItem = itemRenderer;
        comp.UpdateVisible(true, true);
    }

    public void SetListItem<ListItem>(List<ListItem> list, GameObject prefab, Transform root, int count, Action<ListItem> onItemCreate) where ListItem : UIItemWidget<ListItem>, new()
    {
        prefab.SetActive(false);

        if (list.Count == count)
        {
            if (list.Count > 0 && !list[count - 1].gameObject.activeInHierarchy)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].gameObject.SetActive(true);
                }
            }
            return;
        }

        //如果列表数量比count大，那就只显示count个
        if (list.Count > count)
        {
            for (int i = 0; i < list.Count; i++)
            {
                ListItem item = list[i];
                item.gameObject.SetActive(i < count);
            }
        }
        else //否则创建到count个
        {
            for (int i = 0; i < list.Count; i++)
            {
                ListItem item = list[i];
                item.gameObject.SetActive(true);
            }
            for (int i = list.Count; i < count; i++)
            {
                GameObject gameObject = GameObject.Instantiate(prefab, root);
                ListItem item = new ListItem();
                item.Create(gameObject);
                list.Add(item);
                gameObject.transform.SetParent(root);
                gameObject.SetActive(true);
                // var image = gameObject.GetComponent<Image>();
                // if (image)
                //     image.color = new Color(image.color.r, image.color.g, image.color.b, (float)i % 2f * 0.5f + 0.5f);
                onItemCreate?.Invoke(item);
            }
        }
    }

    public int GetListItemIndex<ListItem>(List<ListItem> list, ListItem item) where ListItem : UIItemWidget<ListItem>, new()
    {
        return list.IndexOf(item);
    }
}

public class UIItemWidget<T> : UIItemWidget where T : UIItemWidget<T>, new()
{
    public static T CreateWidget(GameObject gameObject)
    {
        T widget = new T();
        widget.Create(gameObject);
        return widget;
    }

    public virtual void SetStateStyle(UIStateStyle.UIState state)
    {
        
    }

    public void AddClickEvent(UnityAction action)
    {
        gameObject.GetComponent<Button>().onClick.AddListener(action);
    }

    public void RemoveClickEvent(UnityAction action)
    {
        gameObject.GetComponent<Button>().onClick.RemoveListener(action);
    }
}

public interface IPageBind
{
    void ExternalShow();
    void ExternalHide();
}

public class UIPageWidget<T> : UIItemWidget<T>, IPageBind where T : UIPageWidget<T>, new()
{
    private Toggle bindedToggle;
    private List<IPageBind> childPages = new List<IPageBind>();

    private bool isActive = false;

    public void BindToToggle(Toggle toggle)
    {
        bindedToggle = toggle;
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
        OnToggleValueChanged(toggle.isOn);
    }

    public void BindChild(IPageBind page)
    {
        childPages.Add(page);
    }

    private void OnToggleValueChanged(bool value)
    {
        if (value)
        {
            if (isActive)
            {
                gameObject.SetActive(true);
                return;
            }
            OnPageShown();
        }
        else
        {
            if (!isActive)
            {
                gameObject.SetActive(false);
                return;
            }
            OnPageHidden();
        }
    }

    public void TrySwitchTo()
    {
        if (bindedToggle)
        {
            bindedToggle.isOn = true;
        }
    }

    protected virtual void OnPageShown()
    {
        // Debug.Log("OnPageShown" + gameObject.name);
        gameObject.SetActive(true);
        isActive = true;
        foreach (var page in childPages)
        {
            page.ExternalShow();
        }
    }

    protected virtual void OnPageHidden()
    {
        // Debug.Log("OnPageHidden" + gameObject.name);
        isActive = false;
        foreach (var page in childPages)
        {
            page.ExternalHide();
        }
        gameObject.SetActive(false);
    }

    public void ExternalShow()
    {
        if (bindedToggle && bindedToggle.isOn)
        {
            OnToggleValueChanged(true);
        }
    }

    public void ExternalHide()
    {
        if (bindedToggle && bindedToggle.isOn)
        {
            OnToggleValueChanged(false);
        }
    }
}
