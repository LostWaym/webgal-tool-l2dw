

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SFB;
using UnityEngine;
using UnityEngine.UI;

public static class L2DWUtils
{
    public static string CopyBoard => GUIUtility.systemCopyBuffer;

    public static bool IsSubFolderOf(string subFolder, string folder)
    {
        if (string.IsNullOrEmpty(folder))
            return false;

        subFolder = subFolder.Replace('\\', '/');
        folder = folder.Replace('\\', '/');
        return subFolder.StartsWith(folder) && subFolder.Length > folder.Length && subFolder[folder.Length] == '/';
    }

    public static string GetRelativePath(string path, string basePath)
    {
        path = path.Replace('\\', '/');
        basePath = basePath.Replace('\\', '/');
        return path.Substring(basePath.Length + 1);
    }

    public static bool TryGetRelativePath(string path, string basePath, out string relativePath)
    {
        if (string.IsNullOrEmpty(basePath) || !IsSubFolderOf(path, basePath))
        {
            relativePath = null;
            return false;
        }
        relativePath = GetRelativePath(path, basePath);
        return !string.IsNullOrEmpty(relativePath);
    }

    public static string TryParseModelRelativePath(string path)
    {
        if (string.IsNullOrEmpty(Global.ModelPath))
        {
            return path;
        }

        if (IsSubFolderOf(path, Global.ModelPath))
        {
            return GetRelativePath(path, Global.ModelPath);
        }
        return path;
    }

    public static string TryParseModelAbsolutePath(string path)
    {
        if (string.IsNullOrEmpty(Global.ModelPath))
        {
            return path;
        }

        if (File.Exists(path))
        {
            return path;
        }

        path = Path.Combine(Global.ModelPath, path);
        path = path.Replace('/', '\\');
        return path;
    }

    public static string[] OpenFileDialog(string title, string pathKey, string extension, bool allowMultiple = false)
    {
        var path = PlayerPrefs.GetString(pathKey, "");
        if(string.IsNullOrEmpty(path))
        {
            path = Application.dataPath;
        }
        var result = SFB.StandaloneFileBrowser.OpenFilePanel(title, path, new ExtensionFilter[]{new("", extension.Split('|'))}, allowMultiple);
        if(result.Length > 0)
        {
            PlayerPrefs.SetString(pathKey, Path.GetDirectoryName(result[0]));
        }
        return result;
    }

    public static string[] OpenFolderDialog(string title, string pathKey, bool allowMultiple = false)
    {
        var path = PlayerPrefs.GetString(pathKey, "");
        if(string.IsNullOrEmpty(path))
        {
            path = Application.dataPath;
        }
        var result = SFB.StandaloneFileBrowser.OpenFolderPanel(title, path, allowMultiple);
        if(result.Length > 0)
        {
            PlayerPrefs.SetString(pathKey, result[0]);
        }
        return result;
    }

    public static bool AutoSizeButton(string text)
    {
        var size = GUI.skin.button.CalcSize(new GUIContent(text));
        return GUILayout.Button(text, GUILayout.Width(size.x + 12), GUILayout.Height(size.y));
    }

    public static void AutoSizeLabel(string text)
    {
        var size = GUI.skin.label.CalcSize(new GUIContent(text));
        GUILayout.Label(text, GUILayout.Width(size.x + 12), GUILayout.Height(size.y));
    }

    public static string ExecuteInstCopyOperation(string text, InstDealOperation oper)
    {
        switch (oper)
        {
            case InstDealOperation.No:
                return text;
            case InstDealOperation.ExceptLast:
            case InstDealOperation.All:
                var commands = text.Replace("\r", "").Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).Select(line => Experiment.Parse(line)).Where(exp => exp != null).ToList();
                bool exceptLast = oper == InstDealOperation.ExceptLast;
                for (int i = 0; i < commands.Count; i++)
                {
                    var item = commands[i];
                    if (i + 1 == commands.Count && exceptLast)
                    {
                        item.RemoveParameter("next");
                        continue;
                    }
                    item.SetParameter("next");
                }

                string ret = string.Join(System.Environment.NewLine, commands.Select(command => command.GetInstruction()));
                return ret;
        }

        return text;
    }

    public static void CopyInstructionToCopyBoard(string inst)
    {
        var final = ExecuteInstCopyOperation(inst, Global.InstNextMode);
        GUIUtility.systemCopyBuffer = final;
    }

    public static Texture2D LoadTexture(string path)
    {
        var texture = new Texture2D(2, 2);
        texture.LoadImage(File.ReadAllBytes(path));

        return texture;
    }

    public static Sprite LoadSprite(string path)
    {
        var texture = LoadTexture(path);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public static string GetShortNumberString(double value, int precision = 3)
    {
        string ret = ((int)value == value) ? ((int)value).ToString() : value.ToString($"F{precision}").TrimEnd('0').TrimEnd('.');
        if (ret == "" || ret == "-")
            ret = "0";
        return ret;
    }

    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static Vector2 Bezier4(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        var p01 = Vector2.Lerp(p0, p1, t);
        var p12 = Vector2.Lerp(p1, p2, t);
        var p23 = Vector2.Lerp(p2, p3, t);
        var p012 = Vector2.Lerp(p01, p12, t);
        var p123 = Vector2.Lerp(p12, p23, t);
        return Vector2.Lerp(p012, p123, t);
        // float t2 = t * t;
        // float t3 = t2 * t;
        // float oneMinusT = 1 - t;
        // float oneMinusT2 = oneMinusT * oneMinusT;
        // float oneMinusT3 = oneMinusT2 * oneMinusT;
        // return new Vector2(
        //     oneMinusT3 * p0.x + 3 * oneMinusT2 * t * p1.x + 3 * oneMinusT * t2 * p2.x + t3 * p3.x,
        //     oneMinusT3 * p0.y + 3 * oneMinusT2 * t * p1.y + 3 * oneMinusT * t2 * p2.y + t3 * p3.y
        // );
    }

    public static float GetBezierYValueAtX(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float x)
    {
        // 初始参数:
        // - t的初始值为0.5,步长为0.25
        // - 最大迭代次数为10次
        // - 精度为0.001,即x值的误差小于0.001时认为找到了对应点
        // 每次迭代步长会减半,所以最终精度约为:
        // 0.25 * (0.5^9) ≈ 0.00049,即小数点后3-4位
        float t = 0.5f;
        float step = 0.25f;
        int maxIterations = 10;
        int iteration = 0;

        while (iteration < maxIterations)
        {
            var p = Bezier4(p0, p1, p2, p3, t);
            if (Mathf.Abs(p.x - x) < 0.001f)
            {
                return p.y;
            }

            if (p.x > x)
            {
                t -= step;
            }
            else
            {
                t += step;
            }
            step *= 0.5f;
            iteration++;
        }

        // 达到最大迭代次数后返回最后一次计算的y值
        return Bezier4(p0, p1, p2, p3, t).y;
    }

    public static string[] GetLines(string text, bool removeEmpty = true)
    {
        var lines = text.Replace("\r", "").Split('\n');
        if (removeEmpty)
        {
            lines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
        }
        return lines;
    }

    public static void AutoSizeRawImage(RawImage rawImage, Texture texture)
    {
        const float previewWindowWidth = 400.0f;
        var scaleFactor = previewWindowWidth / texture.width;
        rawImage.rectTransform.sizeDelta = new Vector2(texture.width, texture.height) * scaleFactor;
    }

    /// <summary>
    /// 尝试生成单行 changeFigure 模板
    /// <param name= "name" >模型名称/id</param>
    /// <param name= "modelPath" >模型路径</param>
    /// <param name= "index" >模型索引</param>
    /// </summary>
    public static string GenerateFormatText(string name, string modelPath, int index)
    {
        if (IsSubFolderOf(modelPath, Global.ModelPath))
        {
            modelPath = GetRelativePath(modelPath, Global.ModelPath);
        }
        else
        {
            // 如果不在全局设置的 figure 目录下,则尝试截取 figure 目录后面的路径
            modelPath = modelPath.Replace('\\', '/');
            const string figureDir = "figure/";
            var figureDirIndex = modelPath.IndexOf(figureDir);
            if (figureDirIndex >= 0)
            {
                modelPath = modelPath.Substring(figureDirIndex + figureDir.Length);
            }
        }
        
        return $"changeFigure:{modelPath} -id={name}_{index} -zIndex={index} %me_{index}%;";
    }
    
    /// <summary>
    /// 尝试生成 changeFigure 模板
    /// <param name="name">模型名称</param>
    /// <param name="mainModelPath">主模型路径</param>
    /// <param name="subModelPaths">子模型路径</param>
    /// </summary>
    public static string GenerateFormatText(string name, string mainModelPath, string[] subModelPaths)
    {
        var outputTextLines = new List<string>();
        var modelCount = subModelPaths.Length + 1;
        for (int i = 0; i < modelCount; i++)
        {
            var line = GenerateFormatText(name, i == 0 ? mainModelPath : subModelPaths[i - 1], i);
            // 除了最后一个模型外,其他模型行尾添加 -next 参数
            if (i < modelCount - 1)
            {
                line = line.Insert(line.Length - 1, " -next");
            }
            outputTextLines.Add(line);
        }
        return string.Join("\n", outputTextLines);
    }
    
    /// <summary>
    /// 尝试生成 changeFigure 模板
    /// </summary>
    public static string GenerateFormatText(L2DWModelConfig meta)
    {
        var modelAbsolutePath = meta.GetValidModelFilePath(0);
        var subModelAbsolutePaths = new string[meta.subModels.Count()];
        for (int i = 0; i < meta.subModels.Count; i++)
        {
            subModelAbsolutePaths[i] = meta.GetValidModelFilePath(i + 1);
        }
        return GenerateFormatText(meta.name, modelAbsolutePath, subModelAbsolutePaths);
    }
    
    /// <summary>
    /// 尝试生成 changeFigure 模板
    /// </summary>
    public static string GenerateFormatText(ImageModelMeta meta)
    {
        return GenerateFormatText(meta.name, meta.imgPath, Array.Empty<string>());
    }

    /// <summary>
    /// 尝试生成 setTransfrom 模板
    /// </summary>
    public static string GenerateTransformFormatText(string name, int modelCount)
    {
        var outputTextLines = new List<string>();
        for (int i = 0; i < modelCount; i++)
        {
            var line = $"setTransform:%me_{i}% -target={name}_{i} -duration=750 -writeDefault;";
            // 除了最后一个模型外,其他模型行尾添加 -next 参数
            if (i < modelCount - 1)
            {
                line = line.Insert(line.Length - 1, " -next");
            }
            outputTextLines.Add(line);
        }
        return string.Join("\n", outputTextLines);
    }
    
    /// <summary>
    /// 尝试生成 setTransfrom 模板
    /// </summary>
    public static string GenerateTransformFormatText(L2DWModelConfig meta)
    {
        return GenerateTransformFormatText(meta.name, meta.subModels.Count + 1);
    }
    
    /// <summary>
    /// 尝试生成 setTransfrom 模板
    /// </summary>
    public static string GenerateTransformFormatText(ImageModelMeta meta)
    {
        return GenerateTransformFormatText(meta.name, 1);
    }
}
