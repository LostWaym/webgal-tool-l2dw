

using UnityEngine;

public static class Global
{
    public static string ModelPath = "";
    public static string BGPath = "";
    public static string BGChangeTemplate = "";
    public static string BGTransformTemplate = "";
    public static InstDealOperation InstNextMode = 0;

    public static bool __PIVOT_2_4 = false;

    public static bool IsLoaded = false;

    public static void Save()
    {
        if (!IsLoaded)
            return;

        PlayerPrefs.SetString("Global.ModelPath", ModelPath);
        PlayerPrefs.SetString("Global.BGPath", BGPath);
        PlayerPrefs.SetString("Global.BGChangeTemplate", BGChangeTemplate);
        PlayerPrefs.SetString("Global.BGTransformTemplate", BGTransformTemplate);
        PlayerPrefs.SetInt("Global.__PIVOT_2_4", __PIVOT_2_4 ? 1 : 0);
        PlayerPrefs.SetInt("Global.InstNextMode", (int)InstNextMode);
    }

    public static void Load()
    {
        IsLoaded = true;
        ModelPath = PlayerPrefs.GetString("Global.ModelPath", "");
        BGPath = PlayerPrefs.GetString("Global.BGPath", "");
        BGChangeTemplate = PlayerPrefs.GetString("Global.BGChangeTemplate", "");
        BGTransformTemplate = PlayerPrefs.GetString("Global.BGTransformTemplate", "");
        __PIVOT_2_4 = PlayerPrefs.GetInt("Global.__PIVOT_2_4", 0) == 1;
        InstNextMode = (InstDealOperation)PlayerPrefs.GetInt("Global.InstNextMode", 0);

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