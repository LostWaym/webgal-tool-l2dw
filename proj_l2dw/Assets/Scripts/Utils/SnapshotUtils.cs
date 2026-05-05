


using System.Collections.Generic;
using UnityEngine;
public static class SnapshotUtils
{
    public static List<SnapshotData> snapshotDataList = new();

    public static bool TakeSnapshot()
    {
        var snapshotData = new SnapshotData();
        snapshotData.TakeSnapshot();
        if (snapshotData.HasSameModelName())
        {
            MainControl.Instance.ShowMessageTip("提示", "快照中存在相同模型名称，请先删除相同模型名称的快照");
            return false;
        }
        snapshotDataList.Add(snapshotData);
        return true;
    }

    public static bool DeleteSnapshot(SnapshotData snapshotData)
    {
        if (snapshotData == null)
            return false;
        
        snapshotDataList.Remove(snapshotData);
        return true;
    }

    public static bool ApplySnapshot(SnapshotData snapshotData)
    {
        if (snapshotData == null)
            return false;
        
        snapshotData.ApplySnapshot();
        return true;
    }
}

public class SnapshotData
{
    private static int snapshotId = 0;

    public string snapshotName;
    public Dictionary<string, SnapshotModelData> modelData = new();
    public void TakeSnapshot()
    {
        modelData.Clear();
        var models = MainControl.Instance.models;
        snapshotId++;
        snapshotName = $"快照{snapshotId}";
        foreach (var model in models)
        {
            var modelDataInstance = new SnapshotModelData();
            modelDataInstance.InitTransformData(model);
            this.modelData.Add(model.IdName, modelDataInstance);
        }
    }

    public bool HasSameModelName()
    {
        HashSet<string> modelNameSet = new();
        foreach (var modelData in modelData)
        {
            if (modelNameSet.Contains(modelData.Key))
                return true;
            modelNameSet.Add(modelData.Key);
        }
        return false;
    }

    public void ApplySnapshot()
    {
        foreach (var modelData in modelData)
        {
            var model = MainControl.Instance.FindTarget(modelData.Key);
            if (model == null)
                continue;

            MainControl.Instance.ExternalLoadTransform(model, modelData.Value.transformData);
            model.gameObject.SetActive(modelData.Value.visible);
        }
    }
}

public class SnapshotModelData : IJSonSerializable
{
    public string modelName;
    public bool visible;
    public MainControl.TransformData transformData;

    public void InitTransformData(ModelAdjusterBase model)
    {
        transformData = new MainControl.TransformData();
        transformData.position = model.MainPos.position;
        transformData.scale = model.RootScaleValue;
        transformData.rotation = model.RootRotation;
        transformData.reverseX = model.ReverseXScale;
        transformData.blendMode = model.blendMode;
        transformData.filterSetData = model.filterSetData.Clone();
        visible = model.gameObject.activeSelf;

        if (model is ImageModel imgModel)
        {
            modelName = imgModel.imgMeta.name;
        }
        else
        {
            modelName = model.Name;
        }
    }

    public void DeserializeFromJson(JSONObject json)
    {
        modelName = json.GetField(nameof(modelName))?.str ?? "";
        visible = json.GetField(nameof(visible))?.boolean ?? false;
        transformData = new MainControl.TransformData();
        transformData.DeserializeFromJson(json.GetField(nameof(transformData)));
    }

    public void SerializeToJson(JSONObject json)
    {
        json.AddField(nameof(modelName), modelName);
        json.AddField(nameof(visible), visible);
        json.AddField(nameof(transformData), transformData);
    }
}