

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
    
    public static string SaveFileDialog(string title, string pathKey, string defaultName, string extension)
    {
        var path = PlayerPrefs.GetString(pathKey, "");
        if(string.IsNullOrEmpty(path))
        {
            path = Application.dataPath;
        }
        var result = SFB.StandaloneFileBrowser.SaveFilePanel(title, path, defaultName, extension);
        if(!string.IsNullOrEmpty(result))
        {
            PlayerPrefs.SetString(pathKey, Path.GetDirectoryName(result));
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
    
    public static string GetTemplatePath(string absolutePath, bool isBackground)
    {
        var pathResult = absolutePath;
        
        if (isBackground)
        {
            if (IsSubFolderOf(absolutePath, Global.BGPath))
            {
                pathResult = GetRelativePath(absolutePath, Global.BGPath);
            }
            else
            {
                // 如果不在全局设置的 background 目录下,则尝试截取 background 目录后面的路径
                absolutePath = absolutePath.Replace('\\', '/');
                const string backgroundDir = "background/";
                var backgroundDirIndex = absolutePath.IndexOf(backgroundDir);
                if (backgroundDirIndex >= 0)
                {
                    pathResult = absolutePath.Substring(backgroundDirIndex + backgroundDir.Length);
                }
            }
        }
        else
        {
            if (IsSubFolderOf(absolutePath, Global.ModelPath))
            {
                pathResult = GetRelativePath(absolutePath, Global.ModelPath);
            }
            else
            {
                // 如果不在全局设置的 figure 目录下,则尝试截取 figure 目录后面的路径
                absolutePath = absolutePath.Replace('\\', '/');
                const string figureDir = "figure/";
                var figureDirIndex = absolutePath.IndexOf(figureDir);
                if (figureDirIndex >= 0)
                {
                    pathResult = absolutePath.Substring(figureDirIndex + figureDir.Length);
                }
            }
        }
        
        return pathResult;
    }

    /// <summary>
    /// 尝试生成 changeFigure 模板
    /// <param name="name">模型名称</param>
    /// <param name="modelPath">模型路径</param>
    /// <param name="modelIndex">模型索引</param>
    /// </summary>
    public static string GenerateFormatText(string name, string modelPath, int modelIndex)
    {
        return $"changeFigure:{modelPath} -id={name}_{modelIndex} -zIndex={modelIndex} %me_{modelIndex}%;";
    }

    /// <summary>
    /// 尝试生成 changeFigure 模板
    /// <param name="name">模型名称</param>
    /// <param name="modelCount">模型数量</param>
    /// </summary>
    public static string GenerateFormatText(string name, int modelCount)
    {
        var outputTextLines = new List<string>();

        for (int i = 0; i < modelCount; i++)
        {
            var line = GenerateFormatText(name, $"%path_{i}%", i);
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
        return GenerateFormatText(meta.name, meta.subModels.Count() + 1);
    }
    
    /// <summary>
    /// 尝试生成 changeFigure 模板
    /// </summary>
    public static string GenerateFormatText(ImageModelMeta meta)
    {
        return GenerateFormatText(meta.name, 1);
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

    /// <summary>
    /// 获取当前模型的所以 json 文件路径，保存主模型和子模型
    /// </summary>
    public static List<string> GetModelJsonPaths()
    {
        var result = new List<string>();
        
        var curTarget = MainControl.Instance.curTarget;
        if (curTarget && curTarget is ModelAdjuster adjuster && adjuster)
        {
            for (int i = 0; i < adjuster.ModelCount; i++)
            {
                result.Add(adjuster.meta.GetValidModelFilePath(i));
            }
        }
        
        return result;
    }
    
    /// <summary>
    /// 为模型 JSON 添加动作
    /// </summary>
    /// <param name="modelJsonObj">模型 JSON 对象</param>
    /// <param name="modelJsonPath">模型 JSOn 路径</param>
    /// <param name="motionPaths">动作绝对路径列表</param>
    /// <param name="motionNames">动作名称列表</param>
    /// <returns>错误信息，空字符串为无错误</returns>
    public static string AddMotionsToModelJson(
        ref JSONObject modelJsonObj,
        string modelJsonPath,
        List<string> motionPaths,
        List<string> motionNames
    ) {
        if (modelJsonObj == null)
        {
            return "模型 JSON 对象为空";
        }
        if (motionPaths == null || motionPaths.Count == 0)
        {
            return "没有可添加的动作文件";
        }
        if (motionPaths == null || motionNames.Count == 0)
        {
            return "没有指定动作名称";
        }
        if (motionPaths.Count != motionNames.Count)
        {
            return "动作文件数量与动作名称数量不匹配";
        }

        var motions_obj = modelJsonObj.GetField("motions");
        if (motions_obj == null)
        {
            motions_obj = new JSONObject(JSONObject.Type.OBJECT);
        }

        for (int i = 0; i < motionPaths.Count; i++)
        {
            var motionPath = motionPaths[i];
            var motionName = motionNames[i];
            
            var motionRelativePath = PathHelper.GetRelativePath(modelJsonPath, motionPath);

            var newMotionObjArr = new JSONObject(JSONObject.Type.ARRAY);
            var newMotionObj = new JSONObject(JSONObject.Type.OBJECT);
            newMotionObj.SetField("file", JSONObject.StringObject(motionRelativePath));
            newMotionObjArr.Add(newMotionObj);
            motions_obj.SetField(motionName, newMotionObjArr);
        }

        modelJsonObj.SetField("motions", motions_obj);

        return "";
    }
    
    /// <summary>
    /// 为模型 JSON 添加表情
    /// </summary>
    /// <param name="modelJsonObj">模型 JSON 对象</param>
    /// <param name="modelJsonPath">模型 JSOn 路径</param>
    /// <param name="expressionPaths">表情绝对路径列表</param>
    /// <param name="expressionNames">表情名称列表</param>
    /// <returns>错误信息，空字符串为无错误</returns>
    public static string AddExpressionsToModelJson(
        ref JSONObject modelJsonObj,
        string modelJsonPath,
        List<string> expressionPaths,
        List<string> expressionNames
    ) {
        if (modelJsonObj == null)
        {
            return "模型 JSON 对象为空";
        }
        if (expressionPaths == null || expressionPaths.Count == 0)
        {
            return "没有可添加的表情文件";
        }
        if (expressionPaths == null || expressionNames.Count == 0)
        {
            return "没有指定表情名称";
        }
        if (expressionPaths.Count != expressionNames.Count)
        {
            return "表情文件数量与表情名称数量不匹配";
        }

        var expressions_obj = modelJsonObj.GetField("expressions");
        if (expressions_obj == null)
        {
            expressions_obj = new JSONObject(JSONObject.Type.ARRAY);
        }

        for (int i = 0; i < expressionPaths.Count; i++)
        {
            var expressionPath = expressionPaths[i];
            var expressionName = expressionNames[i];
            
            var expressionRelativePath = PathHelper.GetRelativePath(modelJsonPath, expressionPath);
            
            var newExpObj = new JSONObject(JSONObject.Type.OBJECT);
            newExpObj.SetField("name", JSONObject.StringObject(expressionName));
            newExpObj.SetField("file", JSONObject.StringObject(expressionRelativePath));
            
            expressions_obj.Add(newExpObj);
        }

        modelJsonObj.SetField("expressions", expressions_obj);

        return "";
    }

    /// <summary>
    /// 判断两个路径是否在同一个盘符
    /// </summary>
    /// <param name="path1">路径1</param>
    /// <param name="path2">路径2</param>
    /// <returns>是否在同一个盘符</returns>
    public static bool IsSameDrive(string path1, string path2)
    {
        if (string.IsNullOrEmpty(path1) || string.IsNullOrEmpty(path2))
            return false;
        string GetDrive(string path)
        {
            try
            {
                var root = Path.GetPathRoot(path);
                if (string.IsNullOrEmpty(root))
                    return null;
                return root.TrimEnd('\\', '/').ToUpperInvariant();
            }
            catch
            {
                return null;
            }
        }
        var drive1 = GetDrive(path1);
        var drive2 = GetDrive(path2);
        return !string.IsNullOrEmpty(drive1) && drive1 == drive2;
    }

    public static void ResetScrollViewTop(ScrollRect scroll)
    {
        scroll.normalizedPosition = new Vector2(0, 1);
        scroll.velocity = Vector2.zero;
    }
}
