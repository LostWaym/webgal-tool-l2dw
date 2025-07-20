using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGContainer : MonoBehaviour
{
    public FilterSetData filterSetData = new FilterSetData();
    public Transform root;
    public SpriteRenderer bg;

    public float rootRotation;
    public float rootScale = 1;
    public Vector3 rootPosition => root.localPosition;
    public Vector3 rootWorldPosition => root.position;

    public float width;
    public float height;

    public Transform tfLeft, tfRight, tfTop, tfBottom;

    void Awake()
    {
        Adjust();
    }

    public void Adjust()
    {
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        var width = Constants.WebGalWidth;
        var height = Constants.WebGalHeight;
        var offset = 2;
        tfLeft.localPosition = new Vector3(-width / 2 - offset, 0, 0);
        tfRight.localPosition = new Vector3(width / 2 + offset, 0, 0);
        tfTop.localPosition = new Vector3(0, height / 2 + offset, 0);
        tfBottom.localPosition = new Vector3(0, -height / 2 - offset, 0);

        LoadTexture(bg.sprite.texture);
    }

    public void LoadTexture(Texture2D texture)
    {
        var aspect = (float)Constants.WebGalWidth / Constants.WebGalHeight;
        var texAspect = (float)texture.width / texture.height;
        float scale;
        width = texture.width;
        height = texture.height;
        if(texAspect > aspect)
        {
            scale = (float)Constants.WebGalHeight / texture.height;
        }
        else
        {
            scale = (float)Constants.WebGalWidth / texture.width;
        }
        bg.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        bg.transform.localScale = new Vector3(scale * 100, scale * 100, 1);

        UpdateAllFilter();
    }

    public void SetPosition(float x, float y)
    {
        root.localPosition = new Vector3(x, y, 0);
        UIEventBus.SendEvent(UIEventType.BGTransformChanged);
    }

    public void SetWorldPosition(float x, float y)
    {
        root.position = new Vector3(x, y, root.position.z);
        UIEventBus.SendEvent(UIEventType.BGTransformChanged);
    }

    public void SetScale(float scale)
    {
        rootScale = scale;
        root.localScale = new Vector3(scale, scale, 1);
        UIEventBus.SendEvent(UIEventType.BGTransformChanged);
    }

    public void SetRotation(float rotation)
    {
        rootRotation = rotation;
        root.localRotation = Quaternion.Euler(0, 0, rotation);
        UIEventBus.SendEvent(UIEventType.BGTransformChanged);
    }

    public float GetWebGalRotation()
    {
        return -rootRotation * Mathf.PI / 180;
    }

    internal void CopyScaleFromRoot()
    {
        rootScale = root.localScale.x;
    }

    #region 滤镜

    public void OnFilterSetDataChanged(ModelAdjusterBase.FilterProperty property)
    {
        switch (property)
        {
            case ModelAdjusterBase.FilterProperty.Alpha:
            {
                UpdateAlphaFilter();
                break;
            }
            case ModelAdjusterBase.FilterProperty.Blur:
            {
                UpdateBlurFilter();
                break;
            }
            case ModelAdjusterBase.FilterProperty.Adjustment:
            {
                UpdateAdjustmentFilter();
                break;
            }
            case ModelAdjusterBase.FilterProperty.Bloom:
            {
                UpdateBloomFilter();
                break;
            }
            case ModelAdjusterBase.FilterProperty.Bevel:
            {
                UpdateBevelFilter();
                break;
            }
        }
    }
    
    // 更新屏幕尺寸相关参数
    private void UpdateScreenParams()
    {
        var mat = bg.material;
        var modelAspect = bg.sprite.bounds.size.x / bg.sprite.bounds.size.y;
        FilterUtils.UpdateScreenParams(mat, modelAspect, rootScale, 1.0f, true);
    }

    private void UpdateAlphaFilter()
    {
        var mat = bg.material;
        FilterUtils.UpdateAlphaFilter(mat, filterSetData);
    }
    
    private void UpdateBlurFilter()
    {
        var mat = bg.material;
        FilterUtils.UpdateBlurFilter(mat, filterSetData);
    }

    private void UpdateAdjustmentFilter()
    {
        var mat = bg.material;
        FilterUtils.UpdateAdjustmentFilter(mat, filterSetData);
    }
    
    private void UpdateBloomFilter()
    {
        var mat = bg.material;
        FilterUtils.UpdateBloomFilter(mat, filterSetData);
    }
    
    private void UpdateBevelFilter()
    {
        var mat = bg.material;
        FilterUtils.UpdateBevelFilter(mat, filterSetData);
    }

    public void UpdateAllFilter()
    {
        UpdateScreenParams();
        UpdateAlphaFilter();
        UpdateBlurFilter();
        UpdateAdjustmentFilter();
        UpdateBloomFilter();
        UpdateBevelFilter();
    }

    #endregion
}
