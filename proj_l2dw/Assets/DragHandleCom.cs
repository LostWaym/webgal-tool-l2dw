using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandleCom : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Action<DragHandleCom, Vector2> _OnDrag;
    public Action<DragHandleCom, Vector2> _OnPointerDown;
    public Action<DragHandleCom, Vector2> _OnPointerUp;

    public void OnDrag(PointerEventData eventData)
    {
        _OnDrag?.Invoke(this, eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _OnPointerDown?.Invoke(this, eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _OnPointerUp?.Invoke(this, eventData.position);
    }
}
