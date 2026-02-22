


using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModelGroup : MonoBehaviour
{
    public string groupName;
    public Transform root;
    public List<ModelAdjusterBase> modelAdjusters;
    public List<ModelGroup> subGroups;
    public Vector3 WorldPosition => root.position;
    public Vector3 LocalPosition => root.localPosition;

    public bool isAllGroup = false;

    public bool containBackground = false;
    public bool containBackgroundCopy = false;
    public bool autoApply = true;

    public float RotationDeg => root.transform.rotation.eulerAngles.z;
    public float Scale => root.transform.localScale.x;

    public void RemoveInvalidModelAndSubGroup()
    {
        if (isAllGroup)
        {
            m_tempModelsHashSet.Clear();
            foreach (var model in MainControl.Instance.models)
            {
                m_tempModelsHashSet.Add(model);
            }
            return;
        }

        modelAdjusters.RemoveAll(model => model == null);
        subGroups.RemoveAll(group => group == null);

        m_tempModelsHashSet.Clear();
        m_tempGroupsHashSet.Clear();
        AddGroupRecursive(this);
        m_tempGroupsHashSet.RemoveWhere(group => group == null);

        foreach (var group in m_tempGroupsHashSet)
        {
            foreach (var model in group.modelAdjusters)
            {
                m_tempModelsHashSet.Add(model);
            }
        } 

        void AddGroupRecursive(ModelGroup group)
        {
            if (!m_tempGroupsHashSet.Add(group))
                return;

            foreach (var group1 in group.subGroups)
            {
                AddGroupRecursive(group1);
            }
        }
    }

    public HashSet<ModelAdjusterBase> TempModelIncludingSubGroup
    {
        get
        {
            RemoveInvalidModelAndSubGroup();
            return m_tempModelsHashSet;
        }
    }

    private static readonly HashSet<ModelAdjusterBase> m_tempModelsHashSet = new HashSet<ModelAdjusterBase>();
    private static readonly HashSet<ModelGroup> m_tempGroupsHashSet = new HashSet<ModelGroup>();
    private void MoveModelRootToRoot()
    {
        RemoveInvalidModelAndSubGroup();

        foreach (var model in m_tempModelsHashSet)
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
        RemoveInvalidModelAndSubGroup();
        foreach (var model in m_tempModelsHashSet)
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
        RemoveInvalidModelAndSubGroup();
        MoveModelRootToRoot();

        root.transform.position = position;
        UIEventBus.SendEvent(UIEventType.GroupTransformChanged);
        MoveModelRootToModel();
    }

    public void SetLocalPosition(Vector3 localPosition)
    {
        RemoveInvalidModelAndSubGroup();
        MoveModelRootToRoot();

        root.localPosition = localPosition;
        UIEventBus.SendEvent(UIEventType.GroupTransformChanged);

        MoveModelRootToModel();
    }
    

    public void SetRotation(float rotation)
    {
        RemoveInvalidModelAndSubGroup();
        MoveModelRootToRoot();

        float delta = rotation - RotationDeg;
        root.transform.rotation = Quaternion.Euler(0, 0, rotation);

        MoveModelRootToModel(delta);

        if (autoApply)
        {
            ApplyGroup();
        }

        UIEventBus.SendEvent(UIEventType.GroupTransformChanged);
    }

    public void SetScale(float scale)
    {
        RemoveInvalidModelAndSubGroup();
        MoveModelRootToRoot();
        root.transform.localScale = new Vector3(scale, scale, scale);

        MoveModelRootToModel();

        if (autoApply)
        {
            ApplyGroup();
        }

        UIEventBus.SendEvent(UIEventType.GroupTransformChanged);
    }

    public void ApplyGroup()
    {
        root.transform.rotation = Quaternion.Euler(0, 0, 0);
        root.transform.localScale = new Vector3(1, 1, 1);
    }
}