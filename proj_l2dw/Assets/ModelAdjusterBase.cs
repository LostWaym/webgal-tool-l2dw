

using System.Collections.Generic;
using UnityEngine;

public class ModelAdjusterBase : MonoBehaviour
{
    public virtual bool SupportAnimationMode => false;
    public virtual bool SupportExpressionMode => false;
    public virtual bool HasMotions => false;
    public virtual string Name => "";

    protected float zValue = 0;
    public float ZValue
    {
        get => zValue;
        set
        {
            zValue = value;
            var pos = transform.position;
            pos.z = zValue;
            transform.position = pos;
        }
    }

    public virtual Vector3 RootPosition => default;
    public virtual Vector3 RootScale => default;
    public virtual float RootRotation => default;
    public virtual float RootScaleValue => default;
    public bool ReverseXScale => reverseXScale;

    public virtual Transform MainPos => null;
    public virtual int ModelCount => 1;

    public virtual string TransformTemplate => "";
    public virtual string MotionTemplate => "";

    #region live2d
    public virtual ModelDisplayMode DisplayMode => ModelDisplayMode.Normal;
    public virtual string curExpName => "";
    public virtual string curMotionName => "";
    public virtual List<ExpPair> ExpPairs => null;
    public virtual List<MotionPair> MotionPairs => null;

    public virtual MygoExp CurExp => null;
    public MyGOLive2DExMeta meta;
    #endregion


    [SerializeField]
    protected bool reverseXScale = false;

    public virtual void InitTransform(Vector3 pos, float scale, float rotation, bool reverseXScale)
    {

    }

    public virtual void Adjust()
    {
    }

    #region live2d

    public virtual Live2DParamInfoList GetEmotionEditorList()
    {
        return null;
    }

    public virtual void PlayMotion(string name)
    {
    }

    public virtual void PlayExp(string name)
    {

    }

    public virtual bool IsMotionParamSetContains(string name)
    {
        return false;
    }

    public virtual float GetMotionParamValue(string name)
    {
        return 0;
    }

    public virtual void AddMotionParamControl(string name)
    {

    }

    public virtual void RemoveMotionParamControl(string name)
    {

    }

    public virtual void SetMotionParamValue(string name, float value)
    {

    }

    public virtual void ApplyMotionParamValue()
    {

    }

    public virtual void CopyFromExp(MygoExp exp)
    {

    }

    public virtual string GetMotionEditorExpJson()
    {
        return "";
    }

    public virtual void SetDisplayMode(ModelDisplayMode mode)
    {

    }

    #endregion

    public virtual void ReloadTextures()
    {

    }

    public virtual void ReloadModels()
    {

    }

    public virtual string GetMotionExpressionParamsText()
    {
        return "";
    }

    public virtual void CreateModel()
    {

    }
    
    public virtual void SetPosition(float x, float y)
    {

    }

    public virtual void SetScale(float scale)
    {

    }

    public virtual void SetReverseXScale(bool reverse)
    {

    }

    public virtual void SetRotation(float rotation)
    {

    }

    public virtual void SetCharacterWorldPosition(float worldX, float worldY)
    {

    }

    public virtual Vector3 GetCharacterSpecWorldPosition(int modelIndex)
    {
        return default;
    }

    public virtual float GetWebGalRotation()
    {
        return 0;
    }

    public virtual void CopyRotationFromRoot()
    {
    }

    public virtual void CopyScaleFromRoot()
    {

    }

    public virtual void DoRender()
    {

    }

    #region group-operation

    public virtual void BeforeGroupTransform(Transform parent)
    {

    }

    public virtual void AfterGroupTransform(float rotationDelta)
    {

    }

    #endregion
}
