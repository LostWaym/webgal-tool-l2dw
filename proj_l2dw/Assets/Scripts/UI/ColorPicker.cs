using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class ColorPicker : MonoBehaviour
{
    public Action<Color> _OnColorChanged;
    private Color _color = Color.white;
    // 颜色
    public Color color
    {
        get { return _color; }
        set
        {
            var oldValue = _color;
            _color = value;
            if (_color != oldValue)
                _OnColorChanged?.Invoke(_color);
        }
    }
    
    public Action<float> _OnHueChanged;
    private float _hue = 0.0f;
    // 色调
    public float hue
    {
        get { return _hue; }
        set
        {
            var oldValue = _hue;
            _hue = math.clamp(value, 0.0f, 1.0f);
            if (_hue != oldValue)
                _OnHueChanged?.Invoke(_hue);
        }
    }
    
    public Action<float> _OnSaturationChanged;
    private float _saturation = 0.0f;
    // 饱和度
    public float saturation
    {
        get { return _saturation; }
        set
        {
            var oldValue = _saturation;
            _saturation = math.clamp(value, 0.0f, 1.0f);
            if (_saturation != oldValue)
                _OnSaturationChanged?.Invoke(_saturation);
        }
    }
    
    public Action<float> _OnValueChanged;
    private float _value = 1.0f;
    // 亮度
    public float value
    {
        get { return _value; }
        set
        {
            var oldValue = _value;
            _value = math.clamp(value, 0.0f, 1.0f);
            if (_value != oldValue)
                _OnValueChanged?.Invoke(_value);
        }
    }
    
    [Header("Touch")]
    [SerializeField] private TouchArea svRectTouch;
    [SerializeField] private Image svRectImage;
    [SerializeField] private Image svRectDragger;
    [SerializeField] private TouchArea hueBarTouch;
    [SerializeField] private Image hueBarImage;
    [SerializeField] private Image hueBarDragger;
    
    [Header("Input")]
    [SerializeField] private InputField inputFieldRed;
    [SerializeField] private InputField inputFieldGreen;
    [SerializeField] private InputField inputFieldBlue;
    [SerializeField] private InputField inputFieldHue;
    [SerializeField] private InputField inputFieldSaturation;
    [SerializeField] private InputField inputFieldValue;
    [Header("Button")]
    [SerializeField] private Button m_btnInput;

    private Material svRectImageMaterial;
    private Material svRectDraggerMaterial;
    private Material hueBarImageMaterial;
    private Material hueBarDraggerMaterial;

    private bool hsvToColor = false;
    
    // Start is called before the first frame update
    void Start()
    {
        svRectImageMaterial = Instantiate(svRectImage.material);
        svRectImage.material = svRectImageMaterial;
        svRectDraggerMaterial = Instantiate(svRectDragger.material);
        svRectDragger.material = svRectDraggerMaterial;
        hueBarImageMaterial = Instantiate(hueBarImage.material);
        hueBarImage.material = hueBarImageMaterial;
        hueBarDraggerMaterial = Instantiate(hueBarDragger.material);
        hueBarDragger.material = hueBarDraggerMaterial;

        _OnColorChanged += OnColorChanged;
        _OnHueChanged += OnHueChanged;
        _OnSaturationChanged += OnSaturationChanged;
        _OnValueChanged += OnValueChanged;
        
        svRectTouch._OnPointerDown += OnSvRectTouchDown;
        svRectTouch._OnPointerUp += OnSvRectTouchUp;
        svRectTouch._OnPointerMove += OnSvRectTouchMove;
        svRectTouch._OnScroll += OnSvRectTouchScroll;
        svRectTouch._OnDrag += OnSvRectTouchDrag;
        hueBarTouch._OnPointerDown += OnHueBarTouchDown;
        hueBarTouch._OnPointerUp += OnHueBarTouchUp;
        hueBarTouch._OnPointerMove += OnHueBarTouchMove;
        hueBarTouch._OnScroll += OnHueBarTouchScroll;
        hueBarTouch._OnDrag += OnHueBarTouchDrag;
        inputFieldRed.onEndEdit.AddListener(OnInputFieldRedEndEdit);
        inputFieldGreen.onEndEdit.AddListener(OnInputFieldGreenEndEdit);
        inputFieldBlue.onEndEdit.AddListener(OnInputFieldBlueEndEdit);
        inputFieldHue.onEndEdit.AddListener(OnInputFieldHueEndEdit);
        inputFieldSaturation.onEndEdit.AddListener(OnInputFieldSaturationEndEdit);
        inputFieldValue.onEndEdit.AddListener(OnInputFieldValueEndEdit);

        m_btnInput.onClick.AddListener(OnColorInputClicked);

        UpdateSvRectDragger();
        UpdateSvRectImage();
        UpdateHueBarDragger();
    }

    private void OnColorInputClicked()
    {
        bool Submit(string content)
        {
            try
            {
                var result = content.Split(',').Select(c => int.Parse(c)).ToArray();
                int r = result[0], g = result[1], b = result[2];
                color = new Color(r / 255.0f, g / 255.0f, b / 255.0f);
                return true;
            }
            catch{}

            try
            {
                Assert.IsTrue(content.StartsWith("#"));
                // parse hex color code to rgb, such as #ffffff
                string hex = content.TrimStart('#');
                if (hex.Length == 6)
                {
                    int r = Convert.ToInt32(hex.Substring(0, 2), 16);
                    int g = Convert.ToInt32(hex.Substring(2, 2), 16);
                    int b = Convert.ToInt32(hex.Substring(4, 2), 16);
                    color = new Color(r / 255.0f, g / 255.0f, b / 255.0f);
                    return true;
                }
                else if (hex.Length == 3)
                {
                    // handle shorthand hex #fff (expand to #ffffff)
                    int r = Convert.ToInt32(new string(hex[0], 2), 16);
                    int g = Convert.ToInt32(new string(hex[1], 2), 16);
                    int b = Convert.ToInt32(new string(hex[2], 2), 16);
                    color = new Color(r / 255.0f, g / 255.0f, b / 255.0f);
                    return true;
                }
            }
            catch{}

            MessageTipWindow.Instance.Show("提示", "格式错误！");
            return false;
        }

        ColorParseWindow.SetColorParser(Submit);
    }

    private void ConvertSvRectTouch(Vector2 vector)
    {
        var parentRectTransform = svRectDragger.rectTransform.parent as RectTransform;
        var localPos = parentRectTransform.InverseTransformPoint(vector);
        
        var leftBound = parentRectTransform.rect.width * -1.0f / 2.0f;
        var rightBound = parentRectTransform.rect.width * 1.0f / 2.0f;
        var topBound = parentRectTransform.rect.height * 1.0f / 2.0f;
        var bottomBound = parentRectTransform.rect.height * -1.0f / 2.0f;
        
        localPos.x = math.clamp(localPos.x, leftBound, rightBound);
        localPos.y = math.clamp(localPos.y, bottomBound, topBound);
        
        saturation = (localPos.x - leftBound) / (rightBound - leftBound);
        value = (localPos.y - bottomBound) / (topBound - bottomBound);
    }

    private void UpdateSvRectImage()
    {
        svRectImageMaterial.SetFloat("_Hue", hue);
    }
    
    private void UpdateSvRectDragger()
    {
        var localPos = svRectDragger.rectTransform.localPosition;
        var parentRectTransform = svRectDragger.rectTransform.parent as RectTransform;
        var leftBound = parentRectTransform.rect.width * -1.0f / 2.0f;
        var rightBound = parentRectTransform.rect.width * 1.0f / 2.0f;
        var topBound = parentRectTransform.rect.height * 1.0f / 2.0f;
        var bottomBound = parentRectTransform.rect.height * -1.0f / 2.0f;
        
        localPos.x = math.lerp(leftBound, rightBound, saturation);
        localPos.y = math.lerp(bottomBound, topBound, value);
        svRectDragger.rectTransform.localPosition = localPos;

        svRectDraggerMaterial.SetFloat("_Hue", hue);
        svRectDraggerMaterial.SetFloat("_Saturation", saturation);
        svRectDraggerMaterial.SetFloat("_Value", value);
    }
    
    private void ConvertHueBarTouch(Vector2 vector)
    {
        var parentRectTransform = hueBarDragger.rectTransform.parent as RectTransform;
        var localPos = parentRectTransform.InverseTransformPoint(vector);
        
        var leftBound = parentRectTransform.rect.width * -1.0f / 2.0f;
        var rightBound = parentRectTransform.rect.width * 1.0f / 2.0f;
        
        localPos.x = math.clamp(localPos.x, leftBound, rightBound);
        
        hue = (localPos.x - leftBound) / (rightBound - leftBound);
    }
    
    private void UpdateHueBarDragger()
    {
        var localPos = hueBarDragger.rectTransform.localPosition;
        var parentRectTransform = hueBarDragger.rectTransform.parent as RectTransform;
        var leftBound = parentRectTransform.rect.width * -1.0f / 2.0f;
        var rightBound = parentRectTransform.rect.width * 1.0f / 2.0f;
        
        localPos.x = math.lerp(leftBound, rightBound, hue);
        hueBarDragger.rectTransform.localPosition = localPos;

        hueBarDraggerMaterial.SetFloat("_Hue", hue);
        hueBarDraggerMaterial.SetFloat("_Saturation", 1.0f);
        hueBarDraggerMaterial.SetFloat("_Value", 1.0f);
    }

    private void OnColorChanged(Color color)
    {
        Color.RGBToHSV(this.color, out var hue, out var saturation, out var value);
        if (!hsvToColor)
        {
            this.hue = hue;
            this.saturation = saturation;
            this.value = value;
        }
        
        inputFieldRed.text = (color.r * 255.0f).ToString();
        inputFieldGreen.text = (color.g * 255.0f).ToString();
        inputFieldBlue.text = (color.b * 255.0f).ToString();
    }

    private void OnHueChanged(float hue)
    {
        hsvToColor = true;
        color = Color.HSVToRGB(this.hue, this.saturation, this.value);
        hsvToColor = false;
        UpdateSvRectImage();
        UpdateSvRectDragger();
        UpdateHueBarDragger();
        
        inputFieldHue.SetTextWithoutNotify((hue * 360.0f).ToString());
    }

    private void OnSaturationChanged(float saturation)
    {
        hsvToColor = true;
        color = Color.HSVToRGB(this.hue, this.saturation, this.value);
        hsvToColor = false;
        UpdateSvRectDragger();
        
        inputFieldSaturation.SetTextWithoutNotify((saturation * 100.0f).ToString());
    }

    private void OnValueChanged(float value)
    {
        hsvToColor = true;
        color = Color.HSVToRGB(this.hue, this.saturation, this.value);
        hsvToColor = false;
        UpdateSvRectDragger();
        
        inputFieldValue.SetTextWithoutNotify((value * 100.0f).ToString());
    }
    
    private void OnSvRectTouchDown(PointerEventData eventData)
    {
    }

    private void OnSvRectTouchUp(PointerEventData eventData)
    {
    }
    
    private void OnSvRectTouchMove(Vector2 vector)
    {
        ConvertSvRectTouch(vector);
    }

    private void OnSvRectTouchScroll(PointerEventData eventData)
    {
        ExecuteEvents.ExecuteHierarchy<IScrollHandler>(
            transform.parent.gameObject,
            eventData,
            ExecuteEvents.scrollHandler
        );
    }

    private void OnSvRectTouchDrag(PointerEventData eventData)
    {
        ConvertSvRectTouch(Input.mousePosition);
    }

    private void OnHueBarTouchDown(PointerEventData eventData)
    {
    }

    private void OnHueBarTouchUp(PointerEventData eventData)
    {
    }
    
    private void OnHueBarTouchMove(Vector2 vector)
    {
        ConvertHueBarTouch(vector);
    }
    
    private void OnHueBarTouchScroll(PointerEventData eventData)
    {
        ExecuteEvents.ExecuteHierarchy<IScrollHandler>(
            transform.parent.gameObject,
            eventData,
            ExecuteEvents.scrollHandler
        );
    }

    private void OnHueBarTouchDrag(PointerEventData eventData)
    {
        ConvertHueBarTouch(Input.mousePosition);
    }

    private void OnInputFieldRedEndEdit(string text)
    {
        if (float.TryParse(text, out var number))
        {
            var newColor = new Color(
                number / 255.0f,
                color.g,
                color.b
            );
            color = newColor;
        }
    }
    
    private void OnInputFieldGreenEndEdit(string text)
    {
        if (float.TryParse(text, out var number))
        {
            var newColor = new Color(
                color.r,
                number / 255.0f,
                color.b
            );
            color = newColor;
        }
    }
    
    private void OnInputFieldBlueEndEdit(string text)
    {
        if (float.TryParse(text, out var number))
        {
            var newColor = new Color(
                color.r,
                color.g,
                number / 255.0f
            );
            color = newColor;
        }
    }
    
    private void OnInputFieldHueEndEdit(string text)
    {
        if (float.TryParse(text, out var number))
        {
            hue = number / 360.0f;
        }
    }
    
    private void OnInputFieldSaturationEndEdit(string text)
    {
        if (float.TryParse(text, out var number))
        {
            saturation = number / 100.0f;
        }
    }
    
    private void OnInputFieldValueEndEdit(string text)
    {
        if (float.TryParse(text, out var number))
        {
            value = number / 100.0f;
        }
    }
}
