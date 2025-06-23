using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IScrollHandler, IDragHandler
{
    public bool processing = false;
    public Action<Vector2> _OnPointerDown;
    public Action<Vector2> _OnPointerMove;
    public Action<Vector2> _OnPointerUp;
    public Action<PointerEventData> _OnScroll;
    public Action<PointerEventData> _OnDrag;

    public void OnPointerDown(PointerEventData eventData)
    {
        processing = true;
        _OnPointerDown?.Invoke(eventData.position);
    }

    void Update()
    {
        if (processing && Input.GetMouseButton(0))
        {
            _OnPointerMove?.Invoke(Input.mousePosition);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        processing = false;
        _OnPointerUp?.Invoke(eventData.position);
    }

    public void OnScroll(PointerEventData eventData)
    {
        _OnScroll?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _OnDrag?.Invoke(eventData);
    }
}
