

using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class GroupSnapshotHelper
{
    public static List<GroupSnapshotData> groupSnapshotDataList = new List<GroupSnapshotData>();

    public static void RecordGroupSnapshot(ModelGroup group)
    {
        var groupSnapshotData = new GroupSnapshotData();
        groupSnapshotData.groupName = group.groupName;
        groupSnapshotData.modelDataList.Clear();
        foreach (var model in group.TempModelIncludingSubGroup)
        {
            var groupSnapshotModelData = new GroupSnapshotModelData();
            groupSnapshotModelData.modelName = model.IdName;
            groupSnapshotModelData.visible = model.gameObject.activeSelf;
            groupSnapshotModelData.position = model.MainPos.position;
            groupSnapshotModelData.scale = model.RootScaleValue;
            groupSnapshotModelData.rotation = model.RootRotation;
            groupSnapshotModelData.reverseX = model.ReverseXScale;
            groupSnapshotModelData.motionName = model.curMotionName;
            groupSnapshotModelData.expressionName = model.curExpName;
            groupSnapshotData.modelDataList.Add(groupSnapshotModelData);
        }
        groupSnapshotDataList.Add(groupSnapshotData);
    }

    public static void ApplyGroupSnapshot(GroupSnapshotData groupSnapshotData)
    {
        foreach (var modelData in groupSnapshotData.modelDataList)
        {
            var model = MainControl.Instance.FindTarget(modelData.modelName);
            if (model == null)
                continue;
            model.gameObject.SetActive(modelData.visible);
            model.SetRTS(modelData.position, modelData.scale, modelData.rotation, modelData.reverseX);
            model.PlayMotion(modelData.motionName);
            model.PlayExp(modelData.expressionName);
        }
    }

    public static void RemoveAllSnapshot()
    {
        groupSnapshotDataList.Clear();
    }

    public static string PrintCommand()
    {
        const string COMMAND_TEMPLATE = "setTempAnimation:{0} -target={1} -keep -next;";

        List<GroupSnapshotModelData> allData = groupSnapshotDataList.SelectMany(item => item.modelDataList).ToList();
        var groupBy = allData.GroupBy(item => item.modelName);
        StringBuilder sb = new StringBuilder();
        foreach (var group in groupBy)
        {
            var modelIdName = group.Key;
            var jobj = PrintSingleCharacterCommand(group.ToList());
            var command = string.Format(COMMAND_TEMPLATE, jobj.ToString(false), modelIdName);
            sb.AppendLine(command);
        }

        // TODO
        // sb.AppendLine();
        // sb.AppendLine();

        // foreach(var item in groupSnapshotDataList)
        // {
        //     foreach(var modelData in item.modelDataList)
        //     {
        //         var model = MainControl.Instance.FindTarget(modelData.modelName);
        //         if (model == null)
        //             continue;
        //         var command = new CommandInfo();
        //         command.SetParameter("transform", modelData.position, modelData.scale, modelData.rotation, modelData.reverseX);
        //         command.SetParameter("duration", modelData.duration);
        //         command.SetParameter("ease", modelData.ease);
        //         sb.AppendLine(command.ToString());
        //     }
        // }

        return sb.ToString();
    }

    private static JSONObject PrintSingleCharacterCommand(List<GroupSnapshotModelData> modelDataList)
    {
        var root = JSONObject.arr;
        foreach (var modelData in modelDataList)
        {
            var obj = JSONObject.obj;
            var model = MainControl.Instance.FindTarget(modelData.modelName);
            var webgalPos = model.GetWebgalPosition(modelData.position);
            obj.AddVector2Field("position", new Vector2(webgalPos.x, -webgalPos.y));
            obj.AddVector2Field("scale", new Vector2(modelData.scale * (modelData.reverseX ? -1 : 1), modelData.scale));
            obj.AddField("rotation", GetWebGalRotation(modelData.rotation));
            obj.AddField("duration", modelData.duration);
            obj.AddField("ease", modelData.ease);
            root.Add(obj);
        }
        return root;
    }

    private static float GetWebGalRotation(float rotation)
    {
        return -rotation * Mathf.PI / 180;
    }
}

public class GroupSnapshotData
{
    public string groupName;
    public List<GroupSnapshotModelData> modelDataList = new List<GroupSnapshotModelData>();
}

public class GroupSnapshotModelData
{
    public string modelName;
    public bool visible;
    public Vector3 position;
    public float scale;
    public float rotation;
    public bool reverseX;
    public string motionName;
    public string expressionName;
    public int duration = 1000;
    public string ease = "easeIn";
}