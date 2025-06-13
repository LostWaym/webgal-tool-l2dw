

using live2d;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class MygoJson
{
    public string filename;
    public string model;
    public string physics;
    public List<string> textures;
    public Dictionary<string, List<MygoJsonFileItem>> motions;
    public List<MygoJsonExpItem> expressions;



    //已经在别处做了容错，这里不再移除
    public void ClearEmptyMotions()
    {
        // var basePath = Path.GetDirectoryName(filename);
        // List<string> removeKeys = new List<string>();
        // foreach (var item in motions)
        // {
        //     if (File.Exists(Path.Combine(basePath, item.Value[0].file)) || File.Exists(Path.Combine(basePath, item.Value[0].file + ".bytes")))
        //         continue;

        //     removeKeys.Add(item.Key);
        // }

        // foreach (var key in removeKeys)
        // {
        //     motions.Remove(key);
        // }
    }

    public void ClearEmptyExpressions()
    {
        // var basePath = Path.GetDirectoryName(filename);
        // for (int i = expressions.Count - 1; i >= 0; i--)
        // {
        //     MygoJsonExpItem item = expressions[i];
        //     if (File.Exists(Path.Combine(basePath, item.file)) || File.Exists(Path.Combine(basePath, item.file + ".bytes")))
        //         continue;

        //     expressions.RemoveAt(i);
        // }
    }
}

[Serializable]
public class MygoJsonFileItem
{
    public string file;
}

[Serializable]
public class MygoJsonExpItem
{
    public string name;
    public string file;
}

public class MygoConfig
{
    public byte[] model;
    public byte[] physics;
    public List<Texture2D> textures;
    public Dictionary<string, byte[]> motions;
    public Dictionary<string, MygoExpJson> expressions;

    public MygoJson json;

    public void Load(MygoJson json)
    {
        this.json = json;
        string basePath = Path.GetDirectoryName(json.filename);
        if (!TryReadBytes(Path.Combine(basePath, json.model), out model))
        {
            Debug.LogError("无法加载这个地址的模型，路径可能不存在: " + Path.Combine(basePath, json.model));
        }
        if (!string.IsNullOrEmpty(json.physics))
        {
            if (!TryReadBytes(Path.Combine(basePath, json.physics), out physics))
            {
                Debug.LogError("无法加载这个地址的物理文件，路径可能不存在: " + Path.Combine(basePath, json.physics));
            }
        }
        textures = new List<Texture2D>();
        for (int i = 0; i < json.textures.Count; i++)
        {
            var path = json.textures[i];
            if (!TryReadBytes(Path.Combine(basePath, path), out byte[] bytes))
            {
                Debug.LogError("无法加载这个地址的贴图，路径可能不存在: " + Path.Combine(basePath, path));
                continue;
            }
            Texture2D tex2d = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            tex2d.LoadImage(bytes);
            textures.Add(tex2d);
        }
        motions = new Dictionary<string, byte[]>();
        foreach (var item in json.motions)
        {
            if (!TryReadBytes(Path.Combine(basePath, item.Value[0].file), out byte[] bytes))
            {
                Debug.LogError("无法加载这个地址的动画，路径可能不存在: " + Path.Combine(basePath, item.Value[0].file));
                continue;
            }
            motions[item.Key] = bytes;
        }
        expressions = new Dictionary<string, MygoExpJson>();
        foreach (var item in json.expressions)
        {
            if (!TryReadAllText(Path.Combine(basePath, item.file), out string text))
            {
                Debug.LogError("无法加载这个地址的表情，路径可能不存在: " + Path.Combine(basePath, item.file));
                continue;
            }
            var expJson = JsonConvert.DeserializeObject<MygoExpJson>(text);
            expressions[item.name] = expJson;
        }
    }

    public void ReloadTextures()
    {
        string basePath = Path.GetDirectoryName(json.filename);
        for (int i = 0; i < json.textures.Count; i++)
        {
            var texture = textures[i];
            var path = json.textures[i];
            if (!TryReadBytes(Path.Combine(basePath, path), out byte[] bytes))
            {
                Debug.LogError("无法加载这个地址的贴图，路径可能不存在: " + Path.Combine(basePath, path));
                continue;
            }
            texture.LoadImage(bytes);
        }
    }

    public byte[] ReadBytes(string path)
    {
        if (File.Exists(path))
            return File.ReadAllBytes(path);

        return File.ReadAllBytes(path + ".bytes");
    }

    public bool TryReadBytes(string path, out byte[] bytes)
    {
        if (File.Exists(path))
        {
            bytes = File.ReadAllBytes(path);
            return true;
        }
        if (File.Exists(path + ".bytes"))
        {
            bytes = File.ReadAllBytes(path + ".bytes");
            return true;
        }
        bytes = null;
        return false;
    }

    public string ReadAllText(string path)
    {
        if (File.Exists(path))
            return File.ReadAllText(path);

        return File.ReadAllText(path + ".bytes");
    }

    public bool TryReadAllText(string path, out string text)
    {
        if (File.Exists(path))
        {
            text = File.ReadAllText(path);
            return true;
        }
        if (File.Exists(path + ".bytes"))
        {
            text = File.ReadAllText(path + ".bytes");
            return true;
        }
        text = null;
        return false;
    }
}

public class MygoExp
{
    public MygoExpJson data;
    public float elapsedTime;
    public float t;

    public Dictionary<string, float> cacheValues = new Dictionary<string, float>();
    public void Reset()
    {
        elapsedTime = 0;
        t = 0;
        cacheValues.Clear();
    }

    public void Update(float delta, bool reverse = false)
    {
        elapsedTime += (reverse ? -delta : delta) * 1000 / (reverse ? data.fade_out : data.fade_in);
        elapsedTime = Mathf.Clamp01(elapsedTime);
        t = elapsedTime;
    }

    public const string CALC_TYPE_MULT = "mult";
    public const string CALC_TYPE_SET = "set";
    public const string CALC_TYPE_ADD = "add";

    public void Apply(ALive2DModel model)
    {
        cacheValues.Clear();
        foreach (var item in data.keyDatas)
        {
            var modelValue = model.getParamFloat(item.id);
            cacheValues[item.id] = modelValue;
            float expValue = item.val;
            string calc = item.calc;
            if (MainControl.WebGalExpressionSupport)
            {
                calc = CALC_TYPE_SET;
            }

            switch (calc)
            {
                case CALC_TYPE_MULT:
                {
                    model.multParamFloat(item.id, Mathf.Lerp(1, expValue, t));
                    break;
                }
                case CALC_TYPE_SET:
                {
                    model.setParamFloat(item.id, Mathf.Lerp(modelValue, expValue, t));
                    break;
                }
                default:
                {
                    model.addToParamFloat(item.id, Mathf.Lerp(0, expValue, t));
                    break;
                }
            }
        }
    }

    public void Revert(ALive2DModel model)
    {
        foreach (var item in cacheValues)
        {
            model.setParamFloat(item.Key, item.Value);
        }
    }
}

[Serializable]
public class MygoExpJson
{
    public string type;
    public int fade_in;
    public int fade_out;
    [JsonProperty("params")]
    public List<MygoExpKeyData> keyDatas = new List<MygoExpKeyData>();

    private static JsonSerializerSettings settings = new JsonSerializerSettings()
    {
        Formatting = Formatting.Indented,
    };

    static MygoExpJson()
    {
        settings.NullValueHandling = NullValueHandling.Ignore;
        settings.MissingMemberHandling = MissingMemberHandling.Ignore;
    }

    public string PrintJson()
    {
        return JsonConvert.SerializeObject(this, settings);
    }
}

[Serializable]
public class MygoExpKeyData
{
    public string id;
    public float val;
    public string calc;
}

[Serializable]
public class MotionPair
{
    public string name;
    public AMotion motion;
}

[Serializable]
public class ExpPair
{
    public string name;
    public MygoExp exp;
}