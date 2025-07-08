using UnityEngine;

public static class UIUtils
{
    public static bool IsPointInRect(RectTransform rect, Vector2 worldPosition)
    {
        var localPos = rect.InverseTransformPoint(worldPosition);
        return rect.rect.Contains(localPos);
    }
}