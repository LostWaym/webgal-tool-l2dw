


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
            var modelData = new SnapshotModelData();
            modelData.modelName = model.meta.name;
            modelData.visible = model.gameObject.activeSelf;
            modelData.transformData = new MainControl.TransformData();
            modelData.transformData.position = model.MainPos.position;
            modelData.transformData.scale = model.RootScaleValue;
            modelData.transformData.rotation = model.RootRotation;
            modelData.transformData.reverseX = model.ReverseXScale;
            modelData.transformData.blendMode = model.blendMode;
            modelData.transformData.filterSetData = model.filterSetData.Clone();
            this.modelData.Add(model.meta.name, modelData);
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

public class SnapshotModelData
{
    public string modelName;
    public bool visible;
    public MainControl.TransformData transformData;
}