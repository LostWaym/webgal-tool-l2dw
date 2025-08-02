

using live2d;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class Live2dLoadUtils
{
    private static JsonSerializerSettings settings = new JsonSerializerSettings()
    {
        MissingMemberHandling = MissingMemberHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore,
        DefaultValueHandling = DefaultValueHandling.Ignore,
    };

    public static MygoConfig LoadConfig(string path)
    {
        string content = File.ReadAllText(path);
        var mygo = JsonConvert.DeserializeObject<MygoJson>(content, settings);
        mygo.filename = path;
        if (string.IsNullOrEmpty(mygo.model))
            return null;
        
        var config = new MygoConfig();
        config.Load(mygo);

        if (config.model == null)
        {
            Debug.LogError("配置有误，无法找到模型，不加载了: " + path);
            return null;
        }
        
        return config;
    }
}