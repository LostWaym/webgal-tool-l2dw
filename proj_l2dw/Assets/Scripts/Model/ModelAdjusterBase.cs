

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModelAdjusterBase : MonoBehaviour
{
    public enum FilterProperty
    {
        Alpha,
        Blur,
        Adjustment,
        Bloom,
        Bevel,
        OldFilm,
        DotFilm,
        ReflectionFilm,
        GlitchFilm,
        RgbFilm,
        GodrayFilm,
    }
    
    public virtual bool SupportAnimationMode => false;
    public virtual bool SupportExpressionMode => false;
    public virtual bool HasMotions => false;
    public virtual string Name => "";

    protected float zValue = 0;
    public float ZValue
    {
        get => zValue;
        set
        {
            zValue = value;
            var pos = transform.position;
            pos.z = zValue;
            transform.position = pos;
        }
    }

    public virtual Vector3 RootPosition => default;
    public virtual Vector3 RootScale => default;
    public virtual float RootRotation => default;
    public virtual float RootScaleValue => default;
    public bool ReverseXScale => reverseXScale;

    public virtual Transform MainPos => null;
    public virtual int ModelCount => 1;

    public virtual string TransformTemplate => "";
    public virtual string MotionTemplate => "";

    #region live2d
    public virtual ModelDisplayMode DisplayMode => ModelDisplayMode.Normal;
    public virtual string curExpName => "";
    public virtual string curMotionName => "";
    public virtual List<ExpPair> ExpPairs => null;
    public virtual List<MotionPair> MotionPairs => null;

    public virtual MygoExp CurExp => null;
    public MyGOLive2DExMeta meta;
    public virtual MygoConfig MyGOConfig => null;
    public List<Live2dMotionData> motionDataList = new List<Live2dMotionData>();
    #endregion


    [SerializeField]
    protected bool reverseXScale = false;

    public virtual void InitTransform(Vector3 pos, float scale, float rotation, bool reverseXScale)
    {

    }

    public virtual void Adjust()
    {
    }

    #region live2d

    public virtual Texture GetCharaTexture()
    {
        return null;
    }

    public virtual Live2DParamInfoList GetEmotionEditorList()
    {
        return null;
    }

    public virtual void PlayMotion(string name)
    {
    }

    public virtual void PlayExp(string name)
    {

    }

    public virtual bool IsMotionParamSetContains(string name)
    {
        return false;
    }

    public virtual float GetMotionParamValue(string name)
    {
        return 0;
    }

    public virtual void AddMotionParamControl(string name)
    {

    }

    public virtual void RemoveMotionParamControl(string name)
    {

    }

    public virtual void SetMotionParamValue(string name, float value)
    {

    }

    public virtual void ApplyMotionParamValue()
    {

    }

    public virtual void CopyFromExp(MygoExp exp)
    {

    }

    public virtual string GetMotionEditorExpJson()
    {
        return "";
    }

    public virtual void Sample(string paramName, float value)
    {

    }

    public virtual void SampleDefaultParam(string paramName)
    {
        
    }

    public virtual void SetDisplayMode(ModelDisplayMode mode, bool force = false)
    {

    }

    #endregion

    public virtual void ReloadTextures()
    {

    }

    public virtual void ReloadModels()
    {

    }

    public virtual string GetMotionExpressionParamsText()
    {
        return "";
    }
    
    public virtual string GetBoundsText()
    {
        return "";
    }

    public virtual void CreateModel()
    {

    }
    
    public virtual void SetPosition(float x, float y)
    {

    }

    public virtual void SetScale(float scale)
    {

    }

    public virtual void SetReverseXScale(bool reverse)
    {

    }

    public virtual void SetRotation(float rotation)
    {

    }

    public virtual void SetCharacterWorldPosition(float worldX, float worldY)
    {

    }

    public virtual Vector3 GetCharacterSpecWorldPosition(int modelIndex)
    {
        return default;
    }

    public virtual float GetWebGalRotation()
    {
        return 0;
    }

    public virtual void CopyRotationFromRoot()
    {
    }

    public virtual void CopyScaleFromRoot()
    {

    }

    public virtual void DrawLive2D()
    {

    }

    #region group-operation

    public virtual void BeforeGroupTransform(Transform parent)
    {

    }

    public virtual void AfterGroupTransform(float rotationDelta)
    {

    }

    #endregion

    #region Filter
    public FilterSetData filterSetData = new FilterSetData();

    public virtual void OnFilterSetDataChanged(FilterProperty property)
    {

    }

    public virtual void UpdateAllFilter()
    {

    }

    #endregion
}

public class FilterSetData
{
    private float alpha = 1;
    public float Alpha
    {
        get => alpha;
        set
        {
            value = Mathf.Clamp01(value);
            alpha = value;
        }
    }

    private int blur = 0;
    public int Blur
    {
        get => blur;
        set
        {
            value = Mathf.Max(value, 0);
            blur = value;
        }
    }
    
    private float brightness = 1.0f;
    public float Brightness
    {
        get => brightness;
        set
        {
            value = Mathf.Max(value, 0);
            brightness = value;
        }
    }
    
    private float contrast = 1.0f;
    public float Contrast
    {
        get => contrast;
        set
        {
            value = Mathf.Max(value, 0);
            contrast = value;
        }
    }
    
    private float saturation = 1.0f;
    public float Saturation
    {
        get => saturation;
        set
        {
            value = Mathf.Max(value, 0);
            saturation = value;
        }
    }
    
    private float gamma = 1.0f;
    public float Gamma
    {
        get => gamma;
        set
        {
            value = Mathf.Max(value, 0);
            gamma = value;
        }
    }
    
    private float colorRed = 255.0f;
    public float ColorRed
    {
        get => colorRed;
        set
        {
            value = Mathf.Clamp(value, 0.0f, 255.0f);
            colorRed = value;
        }
    }
    
    private float colorGreen = 255.0f;
    public float ColorGreen
    {
        get => colorGreen;
        set
        {
            value = Mathf.Clamp(value, 0.0f, 255.0f);
            colorGreen = value;
        }
    }
    
    private float colorBlue = 255.0f;
    public float ColorBlue
    {
        get => colorBlue;
        set
        {
            value = Mathf.Clamp(value, 0.0f, 255.0f);
            colorBlue = value;
        }
    }

    private float bloom = 0.0f;
    public float Bloom
    {
        get => bloom;
        set
        {
            value = Mathf.Clamp01(value);
            bloom = value;
        }
    }
    
    private float bloomBrightness = 1.0f;
    public float BloomBrightness
    {
        get => bloomBrightness;
        set
        {
            value = Mathf.Max(value, 0);
            bloomBrightness = value;
        }
    }
    
    private float bloomBlur = 0.0f;
    public float BloomBlur
    {
        get => bloomBlur;
        set
        {
            value = Mathf.Max(value, 0);
            bloomBlur = value;
        }
    }
    
    private float bloomThreshold = 0.0f;
    public float BloomThreshold
    {
        get => bloomThreshold;
        set
        {
            value = Mathf.Clamp01(value);
            bloomThreshold = value;
        }
    }
    
    private bool bevelLegacy;
    public bool BevelLegacy
    {
        get => bevelLegacy;
        set => bevelLegacy = value;
    }
    
    private float bevel = 0.0f;
    public float Bevel
    {
        get => bevel;
        set
        {
            value = Mathf.Clamp01(value);
            bevel = value;
        }
    }
    
    private float bevelThickness = 0.0f;
    public float BevelThickness
    {
        get => bevelThickness;
        set
        {
            value = Mathf.Max(value, 0);
            bevelThickness = value;
        }
    }
    
    private float bevelRotation = 0.0f;
    public float BevelRotation
    {
        get => bevelRotation;
        set
        {
            value = Mathf.Max(value, 0);
            bevelRotation = value;
        }
    }
    
    private float bevelSoftness = 0.0f;
    public float BevelSoftness
    {
        get => bevelSoftness;
        set
        {
            value = Mathf.Clamp01(value);
            bevelSoftness = value;
        }
    }
    
    private float bevelRed = 255.0f;
    public float BevelRed
    {
        get => bevelRed;
        set
        {
            value = Mathf.Clamp(value, 0.0f, 255.0f);
            bevelRed = value;
        }
    }
    
    private float bevelGreen = 255.0f;
    public float BevelGreen
    {
        get => bevelGreen;
        set
        {
            value = Mathf.Clamp(value, 0.0f, 255.0f);
            bevelGreen = value;
        }
    }
    
    private float bevelBlue = 255.0f;
    public float BevelBlue
    {
        get => bevelBlue;
        set
        {
            value = Mathf.Clamp(value, 0.0f, 255.0f);
            bevelBlue = value;
        }
    }

    private bool oldFilm;
    public bool OldFilm
    {
        get => oldFilm;
        set => oldFilm = value;
    }

    private bool dotFilm;
    public bool DotFilm
    {
        get => dotFilm;
        set => dotFilm = value;
    }

    private bool reflectionFilm;
    public bool ReflectionFilm
    {
        get => reflectionFilm;
        set => reflectionFilm = value;
    }

    private bool glitchFilm;
    public bool GlitchFilm
    {
        get => glitchFilm;
        set => glitchFilm = value;
    }

    private bool rgbFilm;
    public bool RgbFilm
    {
        get => rgbFilm;
        set => rgbFilm = value;
    }

    private bool godrayFilm;
    public bool GodrayFilm
    {
        get => godrayFilm;
        set => godrayFilm = value;
    }

    public void ApplyToJson(JSONObject json)
    {
        ApplyToJson_21_24(json);
    }

    private void ApplyToJson_21_24(JSONObject json)
    {
        ApplyIfNotApproximately(json, "alpha", alpha, 1);
        ApplyIfNot(json, "blur", blur, 0);
        
        ApplyIfNotApproximately(json, "brightness", brightness, 1.0f);
        ApplyIfNotApproximately(json, "contrast", contrast, 1.0f);
        ApplyIfNotApproximately(json, "saturation", saturation, 1.0f);
        ApplyIfNotApproximately(json, "gamma", gamma, 1.0f);
        ApplyIfNotApproximately(json, "colorRed", colorRed, 255.0f);
        ApplyIfNotApproximately(json, "colorGreen", colorGreen, 255.0f);
        ApplyIfNotApproximately(json, "colorBlue", colorBlue, 255.0f);
        
        ApplyIfNotApproximately(json, "bloom", bloom, 0.0f);
        ApplyIfNotApproximately(json, "bloomBrightness", bloomBrightness, 1.0f);
        ApplyIfNotApproximately(json, "bloomBlur", bloomBlur, 0.0f);
        ApplyIfNotApproximately(json, "bloomThreshold", bloomThreshold, 0.0f);
        
        ApplyIfNotApproximately(json, "bevel", bevel, 0.0f);
        ApplyIfNotApproximately(json, "bevelThickness", bevelThickness, 0.0f);
        ApplyIfNotApproximately(json, "bevelRotation", bevelRotation, 0.0f);
        ApplyIfNotApproximately(json, "bevelSoftness", bevelSoftness, 0.0f);
        ApplyIfNotApproximately(json, "bevelRed", bevelRed, 255.0f);
        ApplyIfNotApproximately(json, "bevelGreen", bevelGreen, 255.0f);
        ApplyIfNotApproximately(json, "bevelBlue", bevelBlue, 255.0f);
        
        ApplyIfTrue(json, "oldFilm", oldFilm);
        ApplyIfTrue(json, "dotFilm", dotFilm);
        ApplyIfTrue(json, "reflectionFilm", reflectionFilm);
        ApplyIfTrue(json, "glitchFilm", glitchFilm);
        ApplyIfTrue(json, "rgbFilm", rgbFilm);
        ApplyIfTrue(json, "godrayFilm", godrayFilm);
    }

    public void ReadFromJson(JSONObject json)
    {
        if (json == null)
            return;
            
        ReadFromJson_21_24(json);
    }

    private void ReadFromJson_21_24(JSONObject json)
    {
        alpha = GetFloatField(json, "alpha", 1);
        blur = GetIntField(json, "blur", 0);

        
        brightness = GetFloatField(json, "brightness", 1.0f);
        contrast = GetFloatField(json, "contrast", 1.0f);
        saturation = GetFloatField(json, "saturation", 1.0f);
        gamma = GetFloatField(json, "gamma", 1.0f);
        colorRed = GetFloatField(json, "colorRed", 255.0f);
        colorGreen = GetFloatField(json, "colorGreen", 255.0f);
        colorBlue = GetFloatField(json, "colorBlue", 255.0f);


        bloom = GetFloatField(json, "bloom", 0.0f);
        bloomBrightness = GetFloatField(json, "bloomBrightness", 1.0f);
        bloomBlur = GetFloatField(json, "bloomBlur", 0.0f);
        bloomThreshold = GetFloatField(json, "bloomThreshold", 0.0f);


        bevel = GetFloatField(json, "bevel", 0.0f);
        bevelThickness = GetFloatField(json, "bevelThickness", 0.0f);
        bevelRotation = GetFloatField(json, "bevelRotation", 0.0f);
        bevelSoftness = GetFloatField(json, "bevelSoftness", 0.0f);
        bevelRed = GetFloatField(json, "bevelRed", 255.0f);
        bevelGreen = GetFloatField(json, "bevelGreen", 255.0f);
        bevelBlue = GetFloatField(json, "bevelBlue", 255.0f);
        

        oldFilm = GetIntBoolField(json, "oldFilm", false);
        dotFilm = GetIntBoolField(json, "dotFilm", false);
        reflectionFilm = GetIntBoolField(json, "reflectionFilm", false);
        glitchFilm = GetIntBoolField(json, "glitchFilm", false);
        rgbFilm = GetIntBoolField(json, "rgbFilm", false);
        godrayFilm = GetIntBoolField(json, "godrayFilm", false);
    }

    private int GetIntField(JSONObject json, string key, int defValue = 0)
    {
        var field = json.GetField(key);
        if (field == null)
        {
            return defValue;
        }
        else
        {
            return (int)field.number;
        }
    }

    private float GetFloatField(JSONObject json, string key, float defValue = 0)
    {
        var field = json.GetField(key);
        if (field == null)
        {
            return defValue;
        }
        else
        {
            return (float)field.number;
        }
    }

    private bool GetIntBoolField(JSONObject json, string key, bool defValue = false)
    {
        return GetIntField(json, key, defValue ? 1 : 0) == 1;
    }

    private void ApplyIfNot(JSONObject json, string key, int value, int notValue)
    {
        if (value != notValue)
        {
            json.AddField(key, value);
        }
    }

    private void ApplyIfNotApproximately(JSONObject json, string key, float value, float notValue)
    {
        if (!Mathf.Approximately(value, notValue))
        {
            json.AddField(key, value);
        }
    }

    private void ApplyIfTrue(JSONObject json, string key, bool value)
    {
        if (value)
        {
            json.AddField(key, 1);
        }
    }

    public FilterSetData Clone()
    {
        var data = new FilterSetData();
        var json = new JSONObject();
        ApplyToJson(json);
        data.ReadFromJson(json);
        return data;
    }
}