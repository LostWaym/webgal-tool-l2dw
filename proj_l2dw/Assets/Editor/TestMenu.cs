

using live2d;
using Newtonsoft.Json;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class TestMenu
{
    [MenuItem("Tools/Load Json")]
    public static void LoadJson()
    {
        string path = EditorUtility.OpenFilePanel("模型json文件", EditorPrefs.GetString("模型json文件", "."), "json");
        if (string.IsNullOrEmpty(path) )
        {
            return;
        }

        EditorPrefs.SetString("模型json文件", Path.GetDirectoryName(path));

        string text = File.ReadAllText(path);
        var mygo = JsonConvert.DeserializeObject<MygoJson>(text);
        Debug.Log(JsonConvert.SerializeObject(mygo));

        
    }
}