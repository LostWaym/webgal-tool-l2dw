

using System.IO;
using System.Linq;
using SFB;
using UnityEngine;

public static class L2DWUtils
{
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
}
