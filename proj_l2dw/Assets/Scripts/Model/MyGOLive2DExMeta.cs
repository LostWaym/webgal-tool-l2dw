using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class MyGOLive2DExMeta
{
    //index 0: name
    //index 1: formatText
    //index 2: modelFilePath
    //index 3: transformFormatText
    //index 4: x,y,scale,rotation
    //index 5: modelFilePaths
    //index 6: modelOffsetX,modelOffsetY|modelOffsetX,modelOffsetY...
    //index 7: reverseX
    //index 8: boundsLeft,boundsTop,boundsRight,boundsBottom
    public string name;
    public string formatText;
    public string modelFilePath;
    public string transformFormatText;
    public float[] live2dBounds = new float[] { 0, 0, 0, 0 }; // left, top, right, bottom

    public float x,y,scale,rotation;
    public List<string> modelFilePaths = new List<string>();
    public List<float> modelOffset = new List<float>();
    public bool hasTransform = false;
    public bool reverseX = false;

    public string m_filterMotion, m_filterExp;

    public static MyGOLive2DExMeta Load(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var meta = new MyGOLive2DExMeta();
        meta.name = lines.Length > 0 ? lines[0] : "";
        meta.formatText = lines.Length > 1 ? lines[1].Replace("\\n", "\n") : "";
        meta.modelFilePath = lines.Length > 2 ? lines[2] : "";
        meta.transformFormatText = lines.Length > 3 ? lines[3].Replace("\\n", "\n") : "";
        if (lines.Length > 4)
        {
            var parts = lines[4].Split('|');
            meta.x = parts.Length > 0 ? float.Parse(parts[0]) : 0;
            meta.y = parts.Length > 1 ? float.Parse(parts[1]) : 0;
            meta.scale = parts.Length > 2 ? float.Parse(parts[2]) : 1;
            meta.rotation = parts.Length > 3 ? float.Parse(parts[3]) : 0;
            meta.hasTransform = true;
        }
        else
        {
            meta.x = 0;
            meta.y = 0;
            meta.scale = 1;
            meta.rotation = 0;
            meta.hasTransform = false;
        }

        if (lines.Length > 5)
        {
            var modelPaths = lines[5].Split("\\n");
            foreach (var path in modelPaths)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    meta.modelFilePaths.Add(path);
                }
            }
        }

        if (lines.Length > 6)
        {
            var parts = lines[6].Split(',');
            foreach (var part in parts)
            {
                meta.modelOffset.Add(SafeParseFloat(part));
            }
        }

        if (lines.Length > 7)
        {
            meta.reverseX = int.TryParse(lines[7], out var result) && result == 1;
        }
        
        if (lines.Length > 8)
        {
            var boundsParts = lines[8].Split(',');
            if (boundsParts.Length == 4)
            {
                meta.live2dBounds[0] = SafeParseFloat(boundsParts[0]); // left
                meta.live2dBounds[1] = SafeParseFloat(boundsParts[1]); // top
                meta.live2dBounds[2] = SafeParseFloat(boundsParts[2]); // right
                meta.live2dBounds[3] = SafeParseFloat(boundsParts[3]); // bottom
            }
            else 
            {
                Debug.LogWarning($"无法加载 {filePath} 的 bounds 参数，需要 4 个参数, 而文件中只有 {boundsParts.Length} 个.");
            }
        }

        return meta;
    }

    public void GetModelOffset(int index, out float x, out float y)
    {
        if (index == 0)
        {
            x = 0;
            y = 0;
            return;
        }
        index -= 1;
        //考虑安全，如果index超出范围，则返回0,0
        x = modelOffset.Count > index * 2 ? modelOffset[index * 2] : 0;
        y = modelOffset.Count > index * 2 + 1 ? modelOffset[index * 2 + 1] : 0;
    }

    private static float SafeParseFloat(string value)
    {
        return float.TryParse(value, out var result) ? result : 0;
    }

    public void SetTransform(float x, float y, float scale, float rotation)
    {
        this.x = x;
        this.y = y;
        this.scale = scale;
        this.rotation = rotation;
    }

    public void Save(string filePath)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(name);
        sb.AppendLine(formatText.Replace(Environment.NewLine, "\\n").Replace("\n", "\\n"));
        sb.AppendLine(modelFilePath);
        sb.AppendLine(transformFormatText.Replace(Environment.NewLine, "\\n").Replace("\n", "\\n"));
        sb.AppendLine($"{x:F3}|{y:F3}|{scale:F3}|{rotation:F3}");
        sb.AppendLine(string.Join("\\n", modelFilePaths));
        sb.AppendLine(string.Join(",", modelOffset));
        sb.AppendLine(reverseX ? "1" : "0");
        sb.AppendLine($"{live2dBounds[0]:F3},{live2dBounds[1]:F3},{live2dBounds[2]:F3},{live2dBounds[3]:F3}");
        File.WriteAllText(filePath, sb.ToString());
    }

    private string InternalGetOriginalModelFilePath(int modelIndex)
    {
        if (modelIndex == 0)
        {
            return modelFilePath;
        }
        modelIndex -= 1;
        if (modelIndex < 0 || modelIndex >= modelFilePaths.Count)
        {
            return null;
        }
        return modelFilePaths[modelIndex];
    }

    public string GetValidModelFilePath(int modelIndex)
    {
        var originalFilePath = InternalGetOriginalModelFilePath(modelIndex);
        var path = L2DWUtils.TryParseModelAbsolutePath(originalFilePath);
        return path;
    }
}
