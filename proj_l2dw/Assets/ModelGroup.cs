


using System.Collections.Generic;
using UnityEngine;

public class ModelGroup : MonoBehaviour
{
    public string groupName;
    public Transform root;
    public List<ModelAdjusterBase> modelAdjusters;
    public Vector3 WorldPosition => root.position;
    public Vector3 LocalPosition => root.localPosition;

    public bool containBackground = false;
    public bool containBackgroundCopy = false;

    public void RemoveInvalidModel()
    {
        modelAdjusters.RemoveAll(model => model == null);
    }

    private void MoveModelRootToRoot()
    {
        RemoveInvalidModel();
        foreach (var model in modelAdjusters)
        {
            model.BeforeGroupTransform(root);
        }

        if (containBackground)
        {
            MainControl.Instance.bgContainer.root.parent = root;
        }
    }

    private void MoveModelRootToModel(float rotationDelta = 0)
    {
        RemoveInvalidModel();
        foreach (var model in modelAdjusters)
        {
            model.AfterGroupTransform(rotationDelta);
        }

        if (containBackground)
        {
            var bgContainer = MainControl.Instance.bgContainer;
            bgContainer.root.parent = bgContainer.transform;
            bgContainer.SetRotation(bgContainer.rootRotation + rotationDelta);
            bgContainer.CopyScaleFromRoot();
        }
    }

    public void SetPivotPositon(Vector3 pivotPosition)
    {
        root.transform.position = pivotPosition;
        UIEventBus.SendEvent(UIEventType.GroupTransformChanged);
    }

    public void SetLocalPivotPosition(Vector3 localPosition)
    {
        root.localPosition = localPosition;
        UIEventBus.SendEvent(UIEventType.GroupTransformChanged);
    }

    public void SetPosition(Vector3 position)
    {
        RemoveInvalidModel();
        MoveModelRootToRoot();

        root.transform.position = position;
        UIEventBus.SendEvent(UIEventType.GroupTransformChanged);
        MoveModelRootToModel();
    }

    public void SetLocalPosition(Vector3 localPosition)
    {
        RemoveInvalidModel();
        MoveModelRootToRoot();

        root.localPosition = localPosition;
        UIEventBus.SendEvent(UIEventType.GroupTransformChanged);

        MoveModelRootToModel();
    }
    

    public void SetRotation(float rotation)
    {
        RemoveInvalidModel();
        MoveModelRootToRoot();

        root.transform.rotation = Quaternion.Euler(0, 0, rotation);

        MoveModelRootToModel(rotation);

        root.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void SetScale(float scale)
    {
        RemoveInvalidModel();
        MoveModelRootToRoot();
        scale = scale + 1;
        root.transform.localScale = new Vector3(scale, scale, scale);

        MoveModelRootToModel();

        root.transform.localScale = new Vector3(1, 1, 1);
    }
}