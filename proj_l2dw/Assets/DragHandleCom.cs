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

    public PointerEventData.InputButton Button => m_button;
    private PointerEventData.InputButton m_button;

    public void OnDrag(PointerEventData eventData)
    {
        m_button = eventData.button;
        _OnDrag?.Invoke(this, eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_button = eventData.button;
        _OnPointerDown?.Invoke(this, eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_button = eventData.button;
        _OnPointerUp?.Invoke(this, eventData.position);
    }
}
