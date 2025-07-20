

using Unity.Mathematics;
using UnityEngine;

public static class Global
{
    public enum FigurePivotMode
    {
        W_4_5_12 = 0,
        W_4_5_13 = 1,
        BC_1_0 = 2,
    }
    
    public static string ModelPath = "";
    public static string BGPath = "";
    public static string BGChangeTemplate = "";
    public static string BGTransformTemplate = "";
    public static InstDealOperation InstNextMode = 0;
    private static float _CameraZoomFactor = 1.1f;
    public static float CameraZoomFactor
    {
        get { return _CameraZoomFactor; }
        set { _CameraZoomFactor = math.max(value, 1.0f); }
    }
    private static float _CameraZoomBoostFactor = 1.5f;
    public static float CameraZoomBoostFactor
    {
        get { return _CameraZoomBoostFactor; }
        set { _CameraZoomBoostFactor = math.max(value, 1.0f); }
    }

    public static FigurePivotMode PivotMode = FigurePivotMode.W_4_5_12;
    public static bool DisableJsonModelProfileInit = false;

    public static bool IsLoaded = false;

    public static bool IsSetResolution = false;
    public static int NewResolutionWidth = 2560;
    public static int NewResolutionHeight = 1440;
    

    public static void Save()
    {
        if (!IsLoaded)
            return;

        PlayerPrefs.SetString("Global.ModelPath", ModelPath);
        PlayerPrefs.SetString("Global.BGPath", BGPath);
        PlayerPrefs.SetString("Global.BGChangeTemplate", BGChangeTemplate);
        PlayerPrefs.SetString("Global.BGTransformTemplate", BGTransformTemplate);
        PlayerPrefs.SetInt("Global.PivotMode", (int)PivotMode);
        PlayerPrefs.SetInt("Global.InstNextMode", (int)InstNextMode);
        PlayerPrefs.SetFloat("Global.CameraZoomFactor", CameraZoomFactor);
        PlayerPrefs.SetFloat("Global.CameraZoomBoostFactor", CameraZoomBoostFactor);
        PlayerPrefs.SetInt("Global.DisableJsonModelProfileInit", DisableJsonModelProfileInit ? 1 : 0);
        PlayerPrefs.SetInt("Global.IsSetResolution", IsSetResolution ? 1 : 0);
        PlayerPrefs.SetInt("Global.NewResolutionWidth", NewResolutionWidth);
        PlayerPrefs.SetInt("Global.NewResolutionHeight", NewResolutionHeight);
    }

    public static void Load()
    {
        IsLoaded = true;
        ModelPath = PlayerPrefs.GetString("Global.ModelPath", "");
        BGPath = PlayerPrefs.GetString("Global.BGPath", "");
        BGChangeTemplate = PlayerPrefs.GetString("Global.BGChangeTemplate", "");
        BGTransformTemplate = PlayerPrefs.GetString("Global.BGTransformTemplate", "");
        PivotMode = (FigurePivotMode)PlayerPrefs.GetInt("Global.PivotMode", 0);
        InstNextMode = (InstDealOperation)PlayerPrefs.GetInt("Global.InstNextMode", 0);
        CameraZoomFactor = PlayerPrefs.GetFloat("Global.CameraZoomFactor", 1.1f);
        CameraZoomBoostFactor = PlayerPrefs.GetFloat("Global.CameraZoomBoostFactor", 1.5f);
        DisableJsonModelProfileInit = PlayerPrefs.GetInt("Global.DisableJsonModelProfileInit", 0) == 1;
        IsSetResolution = PlayerPrefs.GetInt("Global.IsSetResolution", 0) == 1;
        NewResolutionWidth = PlayerPrefs.GetInt("Global.NewResolutionWidth", 2560);
        NewResolutionHeight = PlayerPrefs.GetInt("Global.NewResolutionHeight", 1440);

        if (string.IsNullOrEmpty(BGChangeTemplate))
        {
            BGChangeTemplate = "changeBg:%me%;";
        }
        if (string.IsNullOrEmpty(BGTransformTemplate))
        {
            BGTransformTemplate = "setTransform:%me% -target=bg-main -duration=750;";
        }
    }
}

public enum InstDealOperation
{
    No,
    All,
    ExceptLast,
}