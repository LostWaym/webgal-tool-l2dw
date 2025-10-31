

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
    public List<string> textures = new();
    public Dictionary<string, List<MygoJsonFileItem>> motions = new();
    public List<MygoJsonExpItem> expressions = new();
    public List<InitParamEntry> init_params = new();
    public List<InitOpacity> init_opacities = new();
}

[Serializable]
public class InitParamEntry
{
    public string id;
    public float value;
}

[Serializable]
public class InitOpacity
{
    public string id;
    public float value;
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
    public Dictionary<string, float> init_params;
    public Dictionary<string, float> init_opacities;

    public MygoJson json;

    public void Load(MygoJson json)
    {
        this.json = json;
        string basePath = Path.GetDirectoryName(json.filename);
        if (!TryReadBytes(Path.Combine(basePath, json.model), out model))
        {
            Debug.LogError("模型文件不存在: " + Path.Combine(basePath, json.model));
        }
        if (!string.IsNullOrEmpty(json.physics))
        {
            if (!TryReadBytes(Path.Combine(basePath, json.physics), out physics))
            {
                Debug.LogError("物理文件不存在: " + Path.Combine(basePath, json.physics));
            }
        }
        textures = new List<Texture2D>();
        for (int i = 0; i < json.textures.Count; i++)
        {
            var path = json.textures[i];
            if (!TryReadBytes(Path.Combine(basePath, path), out byte[] bytes))
            {
                Debug.LogError("贴图文件不存在: " + Path.Combine(basePath, path));
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
                Debug.LogError("动画文件不存在: " + Path.Combine(basePath, item.Value[0].file));
                continue;
            }
            motions[item.Key] = bytes;
        }
        expressions = new Dictionary<string, MygoExpJson>();
        foreach (var item in json.expressions)
        {
            if (!TryReadAllText(Path.Combine(basePath, item.file), out string text))
            {
                Debug.LogError("表情文件不存在: " + Path.Combine(basePath, item.file));
                continue;
            }
            var expJson = JsonConvert.DeserializeObject<MygoExpJson>(text);
            expressions[item.name] = expJson;
        }
        init_params = new Dictionary<string, float>();
        foreach (var item in json.init_params)
        {
            init_params[item.id] = item.value;
        }
        init_opacities = new Dictionary<string, float>();
        foreach (var item in json.init_opacities)
        {
            init_opacities[item.id] = item.value;
        }
    }

    private long m_textureFileTimeHash = 0;
    public void ReloadTextures()
    {
        m_textureFileTimeHash = 0;
        string basePath = Path.GetDirectoryName(json.filename);
        for (int i = 0; i < json.textures.Count; i++)
        {
            var texture = textures[i];
            var path = json.textures[i];
            if (!TryReadBytes(Path.Combine(basePath, path), out byte[] bytes))
            {
                Debug.LogError("贴图文件不存在: " + Path.Combine(basePath, path));
                continue;
            }
            texture.LoadImage(bytes);
            m_textureFileTimeHash += GetFileTimeHash(Path.Combine(basePath, path));
        }
    }

    public bool IsFileTimeHashDirty()
    {
        long nowHash = 0;
        string basePath = Path.GetDirectoryName(json.filename);
        for (int i = 0; i < json.textures.Count; i++)
        {
            var path = json.textures[i];
            nowHash += GetFileTimeHash(Path.Combine(basePath, path));
        }

        return nowHash != m_textureFileTimeHash;
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

    public long GetFileTimeHash(string path)
    {
        if (File.Exists(path))
            return new FileInfo(path).LastWriteTime.Ticks;
        if (File.Exists(path + ".bytes"))
            return new FileInfo(path + ".bytes").LastWriteTime.Ticks;
        return 0;
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

    public void Reset()
    {
        elapsedTime = 0;
        t = 0;
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
    public const string CALC_TYPE_DEFAULT = CALC_TYPE_ADD;

    public void Apply(ALive2DModel model)
    {
        foreach (var item in data.keyDatas)
        {
            Apply(model, item.id, item.calc, item.val, t);
        }
    }

    public static void Apply(ALive2DModel model, string key, string calc, float value, float t)
    {
        var modelValue = model.getParamFloat(key);
        
        calc = MainControl.WebGalExpressionSupport ? CALC_TYPE_SET : calc;
        switch (calc)
        {
            case CALC_TYPE_MULT:
            {
                model.multParamFloat(key, Mathf.Lerp(1, value, t));
                break;
            }
            case CALC_TYPE_SET:
            {
                model.setParamFloat(key, Mathf.Lerp(modelValue, value, t));
                break;
            }
            default:
            {
                model.addToParamFloat(key, Mathf.Lerp(0, value, t));
                break;
            }
        }
    }

    public static string GetNextCalcType(string type)
    {
        switch (type)
        {
            case CALC_TYPE_ADD:
                return CALC_TYPE_MULT;
            case CALC_TYPE_MULT:
                return CALC_TYPE_SET;
            default:
                return CALC_TYPE_ADD;
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