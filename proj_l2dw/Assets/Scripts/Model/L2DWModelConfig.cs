

using System.Collections.Generic;

public class L2DWModelConfig : IJSonSerializable
{
    public const string EXTENSION = ".wmdl";
    public string name;
    public string modelRelativePath;
    public List<SubModelData> subModels = new List<SubModelData>();
    public string figureTemplate;
    public string transformTemplate;
    public float x, y, scale, rotation;
    public bool reverseX;
    public float[] live2dBounds = new float[] { 0, 0, 0, 0 }; // left, top, right, bottom

    #region IJSonSerializable
    public void DeserializeFromJson(JSONObject json)
    {
        name = json.GetField(nameof(name))?.str ?? "";
        modelRelativePath = json.GetField(nameof(modelRelativePath))?.str ?? "";
        figureTemplate = json.GetField(nameof(figureTemplate))?.str ?? "";
        transformTemplate = json.GetField(nameof(transformTemplate))?.str ?? "";
        x = json.GetField(nameof(x))?.f ?? 0;
        y = json.GetField(nameof(y))?.f ?? 0;
        scale = json.GetField(nameof(scale))?.f ?? 0;
        rotation = json.GetField(nameof(rotation))?.f ?? 0;
        reverseX = json.GetField(nameof(reverseX))?.boolean ?? false;
        live2dBounds[0] = json.GetField(nameof(live2dBounds))?[0].f ?? 0;
        live2dBounds[1] = json.GetField(nameof(live2dBounds))?[1].f ?? 0;
        live2dBounds[2] = json.GetField(nameof(live2dBounds))?[2].f ?? 0;
        live2dBounds[3] = json.GetField(nameof(live2dBounds))?[3].f ?? 0;
        JsonSerializeUtils.DeserializeFromJsonArray(subModels, json.GetField(nameof(subModels)));
    }

    public void SerializeToJson(JSONObject json)
    {
        json.AddField(nameof(name), name);
        json.AddField(nameof(modelRelativePath), modelRelativePath);
        json.AddField(nameof(figureTemplate), figureTemplate);
        json.AddField(nameof(transformTemplate), transformTemplate);
        JsonSerializeUtils.SerializeToJsonFieldArray(subModels, json, nameof(subModels));
        json.AddField(nameof(x), x);
        json.AddField(nameof(y), y);
        json.AddField(nameof(scale), scale);
        json.AddField(nameof(rotation), rotation);
        json.AddField(nameof(reverseX), reverseX);
        var live2dBoundsArray = new JSONObject(JSONObject.Type.ARRAY);
        live2dBoundsArray.Add(live2dBounds[0]);
        live2dBoundsArray.Add(live2dBounds[1]);
        live2dBoundsArray.Add(live2dBounds[2]);
        live2dBoundsArray.Add(live2dBounds[3]);
        json.AddField(nameof(live2dBounds), live2dBoundsArray);
    }
    #endregion
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
        x = subModels.Count > index ? subModels[index].offsetX : 0;
        y = subModels.Count > index ? subModels[index].offsetY : 0;
    }

    public void SetTransform(float x, float y, float scale, float rotation)
    {
        this.x = x;
        this.y = y;
        this.scale = scale;
        this.rotation = rotation;
    }

    
    private string InternalGetOriginalModelFilePath(int modelIndex)
    {
        if (modelIndex == 0)
        {
            return modelRelativePath;
        }
        modelIndex -= 1;
        if (modelIndex < 0 || modelIndex >= subModels.Count)
        {
            return null;
        }
        return subModels[modelIndex].modelRelativePath;
    }

    public string GetValidModelFilePath(int modelIndex)
    {
        var originalFilePath = InternalGetOriginalModelFilePath(modelIndex);
        var path = L2DWUtils.TryParseModelAbsolutePath(originalFilePath);
        return path;
    }
}

public class SubModelData : IJSonSerializable
{
    public string modelRelativePath;
    public float offsetX;
    public float offsetY;

    public void DeserializeFromJson(JSONObject json)
    {
        modelRelativePath = json.GetField(nameof(modelRelativePath))?.str ?? "";
        offsetX = json.GetField(nameof(offsetX))?.f ?? 0;
        offsetY = json.GetField(nameof(offsetY))?.f ?? 0;
    }

    public void SerializeToJson(JSONObject json)
    {
        json.AddField(nameof(modelRelativePath), modelRelativePath);
        json.AddField(nameof(offsetX), offsetX);
        json.AddField(nameof(offsetY), offsetY);
    }
}

public interface IJSonSerializable
{
    void SerializeToJson(JSONObject json);
    void DeserializeFromJson(JSONObject json);
}

public static class JsonSerializeUtils
{
    // 序列化到json数组
    public static void SerializeToJsonArray<T>(List<T> list, JSONObject jsonArray) where T : IJSonSerializable, new()
    {
        foreach (var item in list)
        {
            var itemJson = new JSONObject();
            item.SerializeToJson(itemJson);
            jsonArray.Add(itemJson);
        }
    }

    // 序列化到json对象的数组字段
    public static void SerializeToJsonFieldArray<T>(List<T> list, JSONObject jsonObject, string fieldName) where T : IJSonSerializable, new()
    {
        var jsonArray = new JSONObject(JSONObject.Type.ARRAY);
        SerializeToJsonArray(list, jsonArray);
        jsonObject.AddField(fieldName, jsonArray);
    }

    // 从json数组反序列化
    public static void DeserializeFromJsonArray<T>(List<T> list, JSONObject jsonArray) where T : IJSonSerializable, new()
    {
        list.Clear();
        if (jsonArray == null || jsonArray.type != JSONObject.Type.ARRAY)
        {
            return;
        }
        foreach (var item in jsonArray.list)
        {
            var itemData = new T();
            itemData.DeserializeFromJson(item);
            list.Add(itemData);
        }
    }
}