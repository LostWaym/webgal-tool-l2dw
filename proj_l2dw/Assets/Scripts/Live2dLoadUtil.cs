

using live2d;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class Live2dLoadUtils
{
    public static MygoConfig LoadConfig(string path)
    {
        string content = File.ReadAllText(path);
        var mygo = JsonConvert.DeserializeObject<MygoJson>(content);
        mygo.filename = path;
        if (string.IsNullOrEmpty(mygo.model))
            return null;

        mygo.ClearEmptyMotions();
        mygo.ClearEmptyExpressions();
        var config = new MygoConfig();
        config.Load(mygo);
        
        return config;
    }
}