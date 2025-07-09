using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IScrollHandler, IDragHandler
{
    [System.Flags]
    public enum PointerButtonMask
    {
        None = 0,
        Left = 1 << 0,
        Right = 1 << 1,
        Middle = 1 << 2,
    }
    /// <summary>
    /// 按住鼠标的哪个键, 触发 _OnPointerMove 事件
    /// </summary>
    public PointerButtonMask pointerButtonMask = PointerButtonMask.Left;
    public bool processingLeft = false;
    public bool processingRight = false;
    public bool processingMiddle = false;
    public Action<PointerEventData> _OnPointerDown;
    public Action<Vector2> _OnPointerMove;
    public Action<PointerEventData> _OnPointerUp;
    public Action<PointerEventData> _OnScroll;
    public Action<PointerEventData> _OnDrag;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CheckPointerButtonMask())
            return;
        
        if (
            eventData.button == PointerEventData.InputButton.Left
            && pointerButtonMask.HasFlag(PointerButtonMask.Left)
        ) {
            processingLeft = true;
        }

        if (
            eventData.button == PointerEventData.InputButton.Right
            && pointerButtonMask.HasFlag(PointerButtonMask.Right)
        ) {
            processingRight = true;
        }
        
        if (
            eventData.button == PointerEventData.InputButton.Middle
            && pointerButtonMask.HasFlag(PointerButtonMask.Middle)
        ) {
            processingMiddle = true;
        }

        _OnPointerDown?.Invoke(eventData);
    }

    void Update()
    {
        if (processingLeft || processingRight || processingMiddle)
        {
            _OnPointerMove?.Invoke(Input.mousePosition);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!CheckPointerButtonMask())
            return;

        if (
            eventData.button == PointerEventData.InputButton.Left
            && pointerButtonMask.HasFlag(PointerButtonMask.Left)
        ) {
            processingLeft = false;
        }

        if (
            eventData.button == PointerEventData.InputButton.Right
            && pointerButtonMask.HasFlag(PointerButtonMask.Right)
        ) {
            processingRight = false;
        }
        
        if (
            eventData.button == PointerEventData.InputButton.Middle
            && pointerButtonMask.HasFlag(PointerButtonMask.Middle)
        ) {
            processingMiddle = false;
        }

        _OnPointerUp?.Invoke(eventData);
    }

    public void OnScroll(PointerEventData eventData)
    {
        _OnScroll?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CheckPointerButtonMask())
            return;

        if (
            (
                eventData.button == PointerEventData.InputButton.Left
                && pointerButtonMask.HasFlag(PointerButtonMask.Left)
            ) || (
                eventData.button == PointerEventData.InputButton.Right
                && pointerButtonMask.HasFlag(PointerButtonMask.Right)
            ) || (
                eventData.button == PointerEventData.InputButton.Middle
                && pointerButtonMask.HasFlag(PointerButtonMask.Middle)
            )
        ) {
            _OnDrag?.Invoke(eventData);
        }
    }

    private bool CheckPointerButtonMask()
    {
        var leftButton = pointerButtonMask.HasFlag(PointerButtonMask.Left);
        var rightButton = pointerButtonMask.HasFlag(PointerButtonMask.Right);
        var middleButton = pointerButtonMask.HasFlag(PointerButtonMask.Middle);
        
        return leftButton || middleButton || rightButton;
    }
}
