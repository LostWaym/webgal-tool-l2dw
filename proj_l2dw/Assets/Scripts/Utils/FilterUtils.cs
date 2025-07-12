


using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FilterUtils
{
    public static List<FilterSetPresetData> filterSetPresets = new List<FilterSetPresetData>();
    
    // 更新屏幕尺寸相关参数
    public static void UpdateScreenParams(Material mat, float modelAspect, float rootScaleValue, float pivotScale = 1f/1.5f)
    {
        var stageAspect = (float)Constants.WebGalWidth / (float)Constants.WebGalHeight;
        var aspectRatio = stageAspect / modelAspect;
        var factor = 1.0f;
        if (!Global.__PIVOT_2_4)
            factor = pivotScale;
        
        mat.SetFloat(
            "_SampleScaleX",
            factor * Mathf.Max(aspectRatio, 1.0f) / (float)Constants.WebGalWidth / rootScaleValue
        );
        mat.SetFloat(
            "_SampleScaleY",
            factor * Mathf.Min(aspectRatio, 1.0f) / (float)Constants.WebGalHeight / rootScaleValue
        );
    }

    public static void UpdateAlphaFilter(Material mat, FilterSetData filterSetData)
    {
        mat.SetFloat("_Alpha", filterSetData.Alpha);
    }
    
    public static void UpdateBlurFilter(Material mat, FilterSetData filterSetData)
    {
        mat.SetFloat("_Blur", filterSetData.Blur);
        if (filterSetData.Blur > 0)
            mat.EnableKeyword("_BLUR_FILTER");
        else
            mat.DisableKeyword("_BLUR_FILTER");
    }

    public static void UpdateAdjustmentFilter(Material mat, FilterSetData filterSetData)
    {
        mat.SetFloat("_Brightness", filterSetData.Brightness);
        mat.SetFloat("_Contrast", filterSetData.Contrast);
        mat.SetFloat("_Saturation", filterSetData.Saturation);
        mat.SetFloat("_Gamma", filterSetData.Gamma);
        mat.SetFloat("_ColorRed", filterSetData.ColorRed);
        mat.SetFloat("_ColorGreen", filterSetData.ColorGreen);
        mat.SetFloat("_ColorBlue", filterSetData.ColorBlue);
    }
    
    public static void UpdateBloomFilter(Material mat, FilterSetData filterSetData)
    {
        mat.SetFloat("_Bloom", filterSetData.Bloom);
        mat.SetFloat("_BloomBrightness", filterSetData.BloomBrightness);
        mat.SetFloat("_BloomBlur", filterSetData.BloomBlur);
        mat.SetFloat("_BloomThreshold", filterSetData.BloomThreshold);

        if (filterSetData.BloomBlur > 0.0f)
            mat.EnableKeyword("_BLOOM_FILTER_BLUR");
        else
            mat.DisableKeyword("_BLOOM_FILTER_BLUR");
    }
    
    public static void UpdateBevelFilter(Material mat, FilterSetData filterSetData)
    {
        mat.SetFloat("_Bevel", filterSetData.Bevel);
        mat.SetFloat("_BevelThickness", filterSetData.BevelThickness);
        mat.SetFloat("_BevelRotation", filterSetData.BevelRotation);
        mat.SetFloat("_BevelSoftness", filterSetData.BevelSoftness);
        mat.SetFloat("_BevelRed", filterSetData.BevelRed);
        mat.SetFloat("_BevelGreen", filterSetData.BevelGreen);
        mat.SetFloat("_BevelBlue", filterSetData.BevelBlue);

        if (filterSetData.BevelSoftness > 0.0f)
            mat.EnableKeyword("_BEVEL_FILTER_SOFTNESS");
        else
            mat.DisableKeyword("_BEVEL_FILTER_SOFTNESS");
    }

    public static void AddFilterSetPreset(FilterSetPresetData preset)
    {
        filterSetPresets.Add(preset);
        SaveFilterSetPreset();
    }

    public static void RemoveFilterSetPreset(FilterSetPresetData preset)
    {
        if (filterSetPresets.Remove(preset))
            SaveFilterSetPreset();
    }

    public static void SaveFilterSetPreset()
    {
        var arrayObj = new JSONObject(JSONObject.Type.ARRAY);
        for (int i = 0; i < filterSetPresets.Count; i++)
        {
            var preset = filterSetPresets[i];
            var presetObj = new JSONObject();
            presetObj.AddField("name", preset.name);
            presetObj.AddField("jsonObject", preset.jsonObject);
            arrayObj.Add(presetObj);
        }
        var folderPath = System.IO.Path.Combine(Application.dataPath, "..");
        var path = System.IO.Path.Combine(folderPath, "FilterSetPreset.json");
        File.WriteAllText(path, arrayObj.ToString(true));
    }

    public static void LoadFilterSetPreset()
    {
        var folderPath = System.IO.Path.Combine(Application.dataPath, "..");
        var path = System.IO.Path.Combine(folderPath, "FilterSetPreset.json");
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            var arrayObj = new JSONObject(json);
            for (int i = 0; i < arrayObj.Count; i++)
            {
                var presetObj = arrayObj[i];
                var preset = new FilterSetPresetData();
                preset.name = presetObj["name"].str;
                preset.jsonObject = presetObj["jsonObject"];
                filterSetPresets.Add(preset);
            }
        }
    }
}

public class FilterSetPresetData
{
    public string name;
    public JSONObject jsonObject;

    public void CopyFrom(FilterSetData filterSetData)
    {
        jsonObject = new JSONObject();
        filterSetData.ApplyToJson(jsonObject);
    }
}