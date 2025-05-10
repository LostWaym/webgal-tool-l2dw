

using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleStyle : MonoBehaviour
{
    private Toggle toggle;
    public Text toggleText;
    public Image toggleImage;
    public UIStateStyle style;

    public bool setTextContentWhenSelected;
    public Text textContentObject;
    void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnValueChanged);
        OnValueChanged(toggle.isOn);
    }

    private void OnValueChanged(bool arg0)
    {
        if (arg0)
        {
            style.SetActiveObject(UIStateStyle.UIState.Selected);
            style.SetColor(toggleText, UIStateStyle.UIState.Selected);
            style.SetColor(toggleImage, UIStateStyle.UIState.Selected);
            if (setTextContentWhenSelected)
            {
                style.SetText(textContentObject, UIStateStyle.UIState.Selected);
            }
        }
        else
        {
            style.SetActiveObject(UIStateStyle.UIState.Normal);
            style.SetColor(toggleText, UIStateStyle.UIState.Normal);
            style.SetColor(toggleImage, UIStateStyle.UIState.Normal);
        }
    }
}

[Serializable]
public class UIStateStyle
{
    public enum UIState
    {
        Normal,
        Selected,
        Disabled
    }
    public Color normalColor = Color.white;
    public Color selectedColor = Color.white;
    public Color disabledColor = Color.white;
    public GameObject normalObject;
    public GameObject selectedObject;
    public GameObject disabledObject;
    public string normalText;
    public string selectedText;
    public string disabledText;

    public void SetText(Text text, UIState state)
    {
        if (text == null)
        {
            return;
        }
        switch (state)
        {
            case UIState.Normal:
                text.text = normalText;
                break;
            case UIState.Selected:
                text.text = selectedText;
                break;
            case UIState.Disabled:
                text.text = disabledText;
                break;
        }
    }
    public void SetColor(Image image, UIState state)
    {
        if (image == null)
        {
            return;
        }
        switch (state)
        {
            case UIState.Normal:
                image.color = normalColor;
                break;
            case UIState.Selected:
                image.color = selectedColor;
                break;
            case UIState.Disabled:
                image.color = disabledColor;
                break;
        }
    }

    public void SetColor(Text text, UIState state)
    {
        if (text == null)
        {
            return;
        }
        switch (state)
        {
            case UIState.Normal:
                text.color = normalColor;
                break;
            case UIState.Selected:
                text.color = selectedColor;
                break;
            case UIState.Disabled:
                text.color = disabledColor;
                break;
        }
    }

    public void SetActiveObject(UIState state)
    {
        GameObject activeTarget = null;
        switch (state)
        {
            case UIState.Normal:
                activeTarget = normalObject;
                break;
            case UIState.Selected:
                activeTarget = selectedObject;
                break;
            case UIState.Disabled:
                activeTarget = disabledObject;
                break;
        }
        UnactiveAllObjects();
        if (activeTarget)
        {
            activeTarget.SetActive(true);
        }
    }

    public void UnactiveAllObjects()
    {
        if (normalObject)
        {
            normalObject.SetActive(false);
        }
        if (selectedObject)
        {
            selectedObject.SetActive(false);
        }
        if (disabledObject)
        {
            disabledObject.SetActive(false);
        }
    }

    public void SetObjectsColor(UIState state)
    {
        Color color = Color.white;
        switch (state)
        {
            case UIState.Normal:
                color = normalColor;
                break;
            case UIState.Selected:
                color = selectedColor;
                break;
            case UIState.Disabled:
                color = disabledColor;
                break;
        }
        if (normalObject && normalObject.TryGetComponent(out Image image))
        {
            image.color = color;
        }
        if (selectedObject && selectedObject.TryGetComponent(out Image image2))
        {
            image2.color = color;
        }
        if (disabledObject && disabledObject.TryGetComponent(out Image image3))
        {
            image3.color = color;
        }
    }
}