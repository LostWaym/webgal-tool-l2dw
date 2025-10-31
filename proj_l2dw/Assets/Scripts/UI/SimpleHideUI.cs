

using System;
using UnityEngine;

// 父物体不能有布局组件
// 父物体的父物体必须是viewport
// 父物体的锚点必须是middle top
// 父物体的中心点y必须是top
// 子物体的锚点必须是middle top
// 子物体的中心点必须是middle top
public class SimpleHideUI : MonoBehaviour
{
    private RectTransform rect;
    public int itemWidth;
    public int itemHeight;
    public int itemSpacing;
    public int itemCount;
    public int capacity;
    public Vector2 verticalPadding;
    public Action<int, bool, Vector2> OnWillingSetItemStatus;
    public Action<int> OnWillingRenderItem;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void CalcSize()
    {
        // vertical layout
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, itemHeight * itemCount + (itemCount - 1) * itemSpacing + verticalPadding.x + verticalPadding.y);
    }

    public Vector2 GetItemPosition(int index)
    {
        return new Vector2(rect.sizeDelta.x / 2, -index * (itemHeight + itemSpacing) - verticalPadding.x);
    }

    void LateUpdate()
    {
        UpdateVisible();
    }

    private int lastTopIndex = 0, lastBottomIndex = 0;
    public void UpdateVisible(bool force = false, bool allSet = false)
    {
        if (itemCount == 0)
            return;

        int topIndex;
        int bottomIndex;

        var ViewportHeight = rect.parent.GetComponent<RectTransform>().rect.height;

        topIndex = Mathf.Max(0, Mathf.FloorToInt((rect.anchoredPosition.y + verticalPadding.x) / (itemHeight + itemSpacing)));
        bottomIndex = Mathf.Min(itemCount - 1, Mathf.FloorToInt((rect.anchoredPosition.y + ViewportHeight + verticalPadding.y) / (itemHeight + itemSpacing)));

        if (lastTopIndex != topIndex || lastBottomIndex != bottomIndex || force)
        {
            int startIndex = Mathf.Min(lastTopIndex, topIndex);
            int endIndex = Mathf.Max(lastBottomIndex, bottomIndex);
            startIndex = Mathf.Max(0, startIndex);
            endIndex = Mathf.Min(itemCount, endIndex + 1);

            if (allSet)
            {
                startIndex = 0;
                endIndex = capacity;
            }

            for (int i = startIndex; i < endIndex; i++)
            {
                bool visible = i >= topIndex && i <= bottomIndex;
                OnWillingSetItemStatus?.Invoke(i, visible, GetItemPosition(i));
                if (visible)
                    OnWillingRenderItem?.Invoke(i);
            }
        }
        lastTopIndex = topIndex;
        lastBottomIndex = bottomIndex;
    }

    public void GetTopBottomIndex(out int topIndex, out int bottomIndex)
    {
        var ViewportHeight = rect.parent.GetComponent<RectTransform>().rect.height;
        topIndex = Mathf.Max(0, Mathf.FloorToInt((rect.anchoredPosition.y + verticalPadding.x) / (itemHeight + itemSpacing)));
        bottomIndex = Mathf.Min(itemCount - 1, Mathf.FloorToInt((rect.anchoredPosition.y + ViewportHeight + verticalPadding.y) / (itemHeight + itemSpacing)));
    }
}