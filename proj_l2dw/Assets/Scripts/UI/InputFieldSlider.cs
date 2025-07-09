using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputFieldSlider : MonoBehaviour
{
    public InputField inputField;
    public TouchArea touchArea;
    
    public TouchArea.PointerButtonMask pointerMask = TouchArea.PointerButtonMask.Right;
    public bool isInt = false;
    public float increaseFactor = 0.1f;
    
    private Vector3 mouseStartPosition = Vector2.zero;
    private float startFloat = 0.0f;
    private bool canSlide = false;
    
    // Start is called before the first frame update
    void Start()
    {
        touchArea._OnPointerDown += OnPointerDown;
        touchArea._OnPointerUp += OnPointerUp;
        touchArea._OnPointerMove += OnPointerMove;
        touchArea._OnScroll += OnScroll;
        touchArea._OnDrag += OnDrag;
    }

    private void OnPointerDown(PointerEventData eventData)
    {
        bool checkMask = (
            eventData.button == PointerEventData.InputButton.Left
            && pointerMask.HasFlag(TouchArea.PointerButtonMask.Left)
        ) || (
            eventData.button == PointerEventData.InputButton.Right
            && pointerMask.HasFlag(TouchArea.PointerButtonMask.Right)
        ) || (
            eventData.button == PointerEventData.InputButton.Middle
            && pointerMask.HasFlag(TouchArea.PointerButtonMask.Middle)
        );

        if (!checkMask)
        {
            PassEventToText(eventData, ExecuteEvents.pointerDownHandler);
            return;
        }
        
        if (float.TryParse(inputField.text, out startFloat))
        {
            mouseStartPosition = Input.mousePosition;
            canSlide = true;
        }
        else
        {
            Debug.LogWarning($"无法正确地将 {inputField.text} 转换为数字");
        }
    }
    
    private void OnPointerUp(PointerEventData eventData)
    {
        bool checkMask = (
            eventData.button == PointerEventData.InputButton.Left
            && pointerMask.HasFlag(TouchArea.PointerButtonMask.Left)
        ) || (
            eventData.button == PointerEventData.InputButton.Right
            && pointerMask.HasFlag(TouchArea.PointerButtonMask.Right)
        ) || (
            eventData.button == PointerEventData.InputButton.Middle
            && pointerMask.HasFlag(TouchArea.PointerButtonMask.Middle)
        );

        if (!checkMask)
        {
            PassEventToText(eventData, ExecuteEvents.pointerUpHandler);
            return;
        }
        
        canSlide = false;
    }
    
    private void OnPointerMove(Vector2 vector2)
    {
        if (!canSlide)
            return;

        bool checkMask = (
            Input.GetMouseButton(0)
            && pointerMask.HasFlag(TouchArea.PointerButtonMask.Left)
        ) || (
            Input.GetMouseButton(1)
            && pointerMask.HasFlag(TouchArea.PointerButtonMask.Right)
        ) || (
            Input.GetMouseButton(2)
            && pointerMask.HasFlag(TouchArea.PointerButtonMask.Middle)
        );
        
        if (!checkMask)
            return;
        
        var posDelta = Input.mousePosition - mouseStartPosition;
        var valueDelta = posDelta.x * increaseFactor;
        var calculatedValue = startFloat + valueDelta;
        if (isInt)
            calculatedValue = math.round(calculatedValue);
        var valueString = calculatedValue.ToString();

        if (inputField.text != valueString)
        {
            inputField.DeactivateInputField();
            inputField.text = valueString;
            inputField.onEndEdit.Invoke(valueString);
        }
    }
    
    private void OnScroll(PointerEventData eventData)
    {
        PassEventToText(eventData, ExecuteEvents.scrollHandler);
    }
    
    private void OnDrag(PointerEventData eventData)
    {
        PassEventToText(eventData, ExecuteEvents.dragHandler);
    }

    private void PassEventToText<T>(BaseEventData eventData, ExecuteEvents.EventFunction<T> callback) where T : IEventSystemHandler
    {
        ExecuteEvents.ExecuteHierarchy(
            inputField.textComponent.gameObject,
            eventData,
            callback
        );
    }
}
