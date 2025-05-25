

using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BaseWindow<T> : BaseWindow where T : BaseWindow<T>
{
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindAnyObjectByType<T>(FindObjectsInactive.Include);
            }
            return instance;
        }
    }
    protected static T instance;
    protected Canvas canvas;
    protected CanvasScaler canvasScaler;
    public bool IsShown => gameObject.activeSelf && canvas.enabled;

    protected sealed override void Awake()
    {
        instance = this as T;
        canvas = GetComponent<Canvas>();
        canvasScaler = GetComponent<CanvasScaler>();
        canvasScaler.scaleFactor = math.max(1.0f, Screen.dpi / 100.0f);
        Debug.Log(Screen.dpi);
        base.Awake();
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
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
                GameObject gameObject = Instantiate(prefab, root);
                ListItem item = new ListItem();
                item.Create(gameObject);
                list.Add(item);
                gameObject.transform.parent = root;
                gameObject.SetActive(true);
                onItemCreate?.Invoke(item);
            }
        }
    }

    public int GetListItemIndex<ListItem>(List<ListItem> list, ListItem item) where ListItem : UIItemWidget<ListItem>, new()
    {
        return list.IndexOf(item);
    }
}