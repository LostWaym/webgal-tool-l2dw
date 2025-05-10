

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

    public void ClearEmptyMotions()
    {
        var basePath = Path.GetDirectoryName(filename);
        List<string> removeKeys = new List<string>();
        foreach (var item in motions)
        {
            if (File.Exists(Path.Combine(basePath, item.Value[0].file)) || File.Exists(Path.Combine(basePath, item.Value[0].file + ".bytes")))
                continue;

            removeKeys.Add(item.Key);
        }

        foreach (var key in removeKeys)
        {
            motions.Remove(key);
        }
    }

    public void ClearEmptyExpressions()
    {
        var basePath = Path.GetDirectoryName(filename);
        for (int i = expressions.Count - 1; i >= 0; i--)
        {
            MygoJsonExpItem item = expressions[i];
            if (File.Exists(Path.Combine(basePath, item.file)) || File.Exists(Path.Combine(basePath, item.file + ".bytes")))
                continue;

            expressions.RemoveAt(i);
        }
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
        model = ReadBytes(Path.Combine(basePath, json.model));
        if (!string.IsNullOrEmpty(json.physics))
        {
            physics = ReadBytes(Path.Combine(basePath, json.physics));
        }
        textures = new List<Texture2D>();
        for (int i = 0; i < json.textures.Count; i++)
        {
            var path = json.textures[i];
            byte[] bytes = ReadBytes(Path.Combine(basePath, path));
            Texture2D tex2d = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            tex2d.LoadImage(bytes);
            textures.Add(tex2d);
        }
        motions = new Dictionary<string, byte[]>();
        foreach (var item in json.motions)
        {
            motions.Add(item.Key, ReadBytes(Path.Combine(basePath, item.Value[0].file)));
        }
        expressions = new Dictionary<string, MygoExpJson>();
        foreach (var item in json.expressions)
        {
            var expJson = JsonConvert.DeserializeObject<MygoExpJson>(ReadAllText(Path.Combine(basePath, item.file)));
            expressions.Add(item.name, expJson);
        }
    }

    public void ReloadTextures()
    {
        string basePath = Path.GetDirectoryName(json.filename);
        for (int i = 0; i < json.textures.Count; i++)
        {
            var texture = textures[i];
            var path = json.textures[i];
            byte[] bytes = ReadBytes(Path.Combine(basePath, path));
            texture.LoadImage(bytes);
        }
    }

    public byte[] ReadBytes(string path)
    {
        if (File.Exists(path))
            return File.ReadAllBytes(path);

        return File.ReadAllBytes(path + ".bytes");
    }

    public string ReadAllText(string path)
    {
        if (File.Exists(path))
            return File.ReadAllText(path);

        return File.ReadAllText(path + ".bytes");
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

    public void Apply(ALive2DModel model)
    {
        cacheValues.Clear();
        foreach (var item in data.keyDatas)
        {
            var modelValue = model.getParamFloat(item.id);
            cacheValues[item.id] = modelValue;
            float expValue = item.val;
            if (item.calc == "mult" && !MainControl.WebGalExpressionSupport)
            {
                expValue = Mathf.Lerp(modelValue, modelValue * expValue, t);
            }
            else
            {
                expValue = Mathf.Lerp(modelValue, expValue, t);
            }
            model.setParamFloat(item.id, expValue);
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