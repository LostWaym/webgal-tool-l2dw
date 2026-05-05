


using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class ProjData : IJSonSerializable
{
    public const string CUR_VERSION = "1.0.0";
    public const string EXTENSION = "wproj";
    public const string PATH_KEY = "proj_path";
    public const string DEFAULT_NAME = "new_proj";

    public List<ProjCharaData> charaDataList;
    public List<ProjGroupData> groupDataList;
    public string bgPath;
    public string version = CUR_VERSION;

    private string m_basePath;

    public void SetBasePath(string basePath)
    {
        m_basePath = basePath;
    }

    public void Snapshot()
    {
        var models = MainControl.Instance.models;
        charaDataList = new List<ProjCharaData>();
        foreach (var model in models)
        {
            var charaData = new ProjCharaData();
            if (model is ImageModel imgModel)
            {
                charaData.imgMeta = imgModel.imgMeta;
                charaData.modelPath = imgModel.imgMeta.imgPath;
            }
            else
            {
                charaData.modelPath = model.meta.temp_filePath;
            }
            charaData.motion = model.curMotionName;
            charaData.expression = model.curExpName;
            charaData.snapshotModelData = new SnapshotModelData();
            charaData.snapshotModelData.InitTransformData(model);
            charaDataList.Add(charaData);
        }

        groupDataList = new List<ProjGroupData>();
        foreach (var group in MainControl.Instance.modelGroups)
        {
            if (group.isAllGroup)
                continue;

            var groupData = new ProjGroupData();
            groupData.name = group.groupName;
            groupData.modelNames = group.modelAdjusters.Select(model => model.IdName).ToList();
            groupData.containBackground = group.containBackground;
            groupData.containBackgroundCopy = group.containBackgroundCopy;
            groupData.autoApply = group.autoApply;
            groupData.rotationDeg = group.RotationDeg;
            groupData.scale = group.Scale;
            groupData.subGroups = group.subGroups.Select(subGroup => subGroup.groupName).ToList();
            groupData.worldPosition = group.WorldPosition;
            groupDataList.Add(groupData);
        }

        bgPath = MainControl.CurrentBGPath;
    }

    public void Apply(bool addtive = false)
    {
        if (!addtive)
            MainControl.Instance.DeleteAllModels();
        foreach (var charaData in charaDataList)
        {
            string modelPath;
            if (charaData.imgMeta != null)
            {
                modelPath = charaData.imgMeta.imgPath;
            }
            else
            {
                modelPath = charaData.modelPath;
            }
            var model = MainControl.Instance.AddModel(modelPath, false);
            if (model == null)
            {
                Debug.LogError($"无法加载模型: {modelPath}");
                continue;
            }
            
            model.PlayMotion(charaData.motion);
            model.PlayExp(charaData.expression);
            if (model is ImageModel imgModel)
            {
                imgModel.imgMeta = charaData.imgMeta;
            }
            else if (model is ModelAdjuster model2)
            {
                model2.meta.name = charaData.name;
            }
            model.gameObject.SetActive(charaData.snapshotModelData.visible);
            MainControl.Instance.ExternalLoadTransform(model, charaData.snapshotModelData.transformData);
        }

        if (!addtive)
            MainControl.Instance.RemoveAllGroups();
        foreach (var groupData in groupDataList)
        {
            var group = MainControl.Instance.AddGroup(groupData.name);
            group.containBackground = groupData.containBackground;
            group.containBackgroundCopy = groupData.containBackgroundCopy;
            group.autoApply = groupData.autoApply;
            group.SetRotation(groupData.rotationDeg);
            group.SetScale(groupData.scale);
            group.SetPivotPositon(groupData.worldPosition);
            foreach (var modelName in groupData.modelNames)
            {
                var model = MainControl.Instance.FindTarget(modelName);
                if (model == null)
                    continue;
                MainControl.Instance.AddModelToGroup(group, model);
            }
            foreach (var subGroupName in groupData.subGroups)
            {
                var subGroup = MainControl.Instance.FindGroup(subGroupName);
                if (subGroup == null)
                    continue;

                if (!group.subGroups.Contains(subGroup))
                    group.subGroups.Add(subGroup);
            }
        }

        MainControl.Instance.LoadBackground(bgPath);

        MainControl.Instance.InvokeLoadConf();
    }

    public void SerializeToJson(JSONObject json)
    {
        DoRelativePaths(m_basePath);

        json.AddFieldList(nameof(charaDataList), charaDataList);
        json.AddFieldList(nameof(groupDataList), groupDataList);
        json.AddField(nameof(bgPath), bgPath);
        json.AddField(nameof(version), version);

        DoAbsolutePaths(m_basePath);
    }

    public void DeserializeFromJson(JSONObject json)
    {
        json.GetFieldList(nameof(charaDataList), ref charaDataList);
        json.GetFieldList(nameof(groupDataList), ref groupDataList);
        bgPath = json.GetField(nameof(bgPath))?.str ?? "";
        version = json.GetField(nameof(version))?.str ?? CUR_VERSION;

        DoAbsolutePaths(m_basePath);
    }

    public void DoAbsolutePaths(string basePath)
    {
        if (string.IsNullOrEmpty(basePath))
            return;

        foreach (var charaData in charaDataList)
        {
            if (charaData.imgMeta != null)
            {
                charaData.imgMeta.imgPath = L2DWUtils.ToAbsolutePath(basePath, charaData.imgMeta.imgPath);
            }
            charaData.modelPath = L2DWUtils.ToAbsolutePath(basePath, charaData.modelPath);
        }

        bgPath = L2DWUtils.ToAbsolutePath(basePath, bgPath);
    }

    public void DoRelativePaths(string basePath)
    {
        foreach (var charaData in charaDataList)
        {
            if (charaData.imgMeta != null)
            {
                charaData.imgMeta.imgPath = L2DWUtils.CalculateRelativePath(basePath, charaData.imgMeta.imgPath);
            }
            charaData.modelPath = L2DWUtils.CalculateRelativePath(basePath, charaData.modelPath);
        }

        bgPath = L2DWUtils.CalculateRelativePath(basePath, bgPath);
    }
}

public class ProjCharaData : IJSonSerializable
{
    public string name => snapshotModelData.modelName;
    public string modelPath;
    public ImageModelMeta imgMeta;
    public string motion;
    public string expression;
    public SnapshotModelData snapshotModelData;

    public void DeserializeFromJson(JSONObject json)
    {
        modelPath = json.GetField(nameof(modelPath))?.str ?? "";
        motion = json.GetField(nameof(motion))?.str ?? "";
        expression = json.GetField(nameof(expression))?.str ?? "";
        json.GetField(nameof(snapshotModelData), ref snapshotModelData);
        json.GetField(nameof(imgMeta), ref imgMeta);
    }

    public void SerializeToJson(JSONObject json)
    {
        json.AddField(nameof(modelPath), modelPath);
        json.AddField(nameof(motion), motion);
        json.AddField(nameof(expression), expression);
        json.AddField(nameof(snapshotModelData), snapshotModelData);
        json.AddField(nameof(imgMeta), imgMeta);
    }
}

public class ProjGroupData : IJSonSerializable
{
    public string name;
    public List<string> modelNames;
    public bool containBackground;
    public bool containBackgroundCopy;
    public bool autoApply;
    public float rotationDeg;
    public float scale;
    public Vector3 worldPosition;
    public List<string> subGroups;

    public void DeserializeFromJson(JSONObject json)
    {
        name = json.GetField(nameof(name))?.str ?? "";
        modelNames = json.GetField(nameof(modelNames))?.list.Select(item => item.str).ToList() ?? new List<string>();
        containBackground = json.GetField(nameof(containBackground))?.boolean ?? false;
        containBackgroundCopy = json.GetField(nameof(containBackgroundCopy))?.boolean ?? false;
        autoApply = json.GetField(nameof(autoApply))?.boolean ?? false;
        rotationDeg = json.GetField(nameof(rotationDeg))?.f ?? 0;
        scale = json.GetField(nameof(scale))?.f ?? 0;
        worldPosition = json.GetVector3Field(nameof(worldPosition));
        subGroups = json.GetField(nameof(subGroups))?.list.Select(item => item.str).ToList() ?? new List<string>();
    }

    public void SerializeToJson(JSONObject json)
    {
        json.AddField(nameof(name), name);
        json.AddStringFieldList(nameof(modelNames), modelNames);
        json.AddField(nameof(containBackground), containBackground);
        json.AddField(nameof(containBackgroundCopy), containBackgroundCopy);
        json.AddField(nameof(autoApply), autoApply);
        json.AddField(nameof(rotationDeg), rotationDeg);
        json.AddField(nameof(scale), scale);
        json.AddVector3Field(nameof(worldPosition), worldPosition);
        json.AddStringFieldList(nameof(subGroups), subGroups);
    }
}