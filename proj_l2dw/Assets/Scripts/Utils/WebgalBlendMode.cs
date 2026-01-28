using UnityEngine;

public enum WebgalBlendMode
{
    DontChange,
    Normal,
    Add,
    Multiply,
    Screen,
}

public class WebgalBlendModeUtils
{
    public static string ToString(WebgalBlendMode blendMode)
    {
        switch (blendMode)
        {
            case WebgalBlendMode.DontChange:
                return "";
            case WebgalBlendMode.Normal:
                return "normal";
            case WebgalBlendMode.Add:
                return "add";
            case WebgalBlendMode.Multiply:
                return "multiply";
            case WebgalBlendMode.Screen:
                return "screen";
            default:
                Debug.LogWarning($"未知的混合模式: {blendMode}");
                return "unknown";
        }
    }
    
    public static WebgalBlendMode FromString(string str)
    {
        switch (str.ToLower())
        {
            case "":
                return WebgalBlendMode.DontChange;
            case "normal":
                return WebgalBlendMode.Normal;
            case "add":
                return WebgalBlendMode.Add;
            case "multiply":
                return WebgalBlendMode.Multiply;
            case "screen":
                return WebgalBlendMode.Screen;
            default:
                Debug.LogWarning($"未知的混合模式字符串: {str}");
                return WebgalBlendMode.DontChange;
        }
    }
}
