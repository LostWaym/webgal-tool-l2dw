


using System.IO;
using UnityEngine;

public class ImageModel : ModelAdjusterBase
{
    public string relativePath;
    public Sprite sprite;
    public Transform root;
    public Transform pivot;
    public SpriteRenderer spriteRenderer;

    [SerializeField]
    private string transformTemplate;
    [SerializeField]
    public string motiontemplate;


    public override bool SupportAnimationMode => false;
    public override bool SupportExpressionMode => false;
    public override bool HasMotions => false;
    public override string Name => IdName + "[图]";
    private string IdName => imgMeta.name;

    public override string TransformTemplate => imgMeta.transformTemplate;
    public override string MotionTemplate => imgMeta.motionTemplate;


    public override float RootScaleValue => pivot.localScale.y;
    public override Vector3 RootScale => pivot.localScale;
    public override float RootRotation => rootRotation;
    private float rootRotation = 0;
    public override Vector3 RootPosition => root.transform.localPosition;

    public override Transform MainPos => root;

    public ImageModelMeta imgMeta = new ImageModelMeta();

    public override void InitTransform(Vector3 pos, float scale, float rotation, bool reverseXScale)
    {
        this.reverseXScale = reverseXScale;
        SetScale(scale);
        SetRotation(rotation);
        SetPosition(pos.x, pos.y);
    }

    public void Init(string path)
    {
        relativePath = L2DWUtils.GetRelativePath(path, Global.ModelPath);
        imgMeta.imgPath = path;
        imgMeta.CalculateRelativePath();
        imgMeta.name = Path.GetFileNameWithoutExtension(imgMeta.imgPath ?? "img");
        imgMeta.motionTemplate = $"changeFigure:{relativePath} -id={imgMeta.name} -l2dw %me%;";
        imgMeta.transformTemplate = $"setTransform:%me% -target={imgMeta.name} -duration=750;";
    }

    public override void CreateModel()
    {
        LoadTexture();
        UpdateAllFilter();
    }

    private void LoadTexture()
    {
        sprite = L2DWUtils.LoadSprite(imgMeta.imgPath);
        spriteRenderer.sprite = sprite;
    }

    public override void ReloadTextures()
    {
        LoadTexture();
        Adjust();
    }

    public override void ReloadModels()
    {
        ReloadTextures();
    }

    public override void Adjust()
    {
        var size = sprite.bounds.size;
        var scaleX = Constants.WebGalWidth / size.x;
        var scaleY = Constants.WebGalHeight / size.y;
        // var webgalAspect = Constants.WebGalWidth / Constants.WebGalHeight;
        // var aspect = size.x / size.y;
        var targetScale = Mathf.Min(scaleX, scaleY);
        transform.localScale = Vector3.one * 0.01f;
        spriteRenderer.transform.localScale = targetScale * Vector3.one;

        var targetHeight = size.y * targetScale;
        if (targetHeight < Constants.WebGalHeight)
        {
            pivot.transform.localPosition = new Vector3(0, -(Constants.WebGalHeight - targetHeight) / 2, 0);
        }
    }

    public override void SetScale(float scale)
    {
        pivot.transform.localScale = new Vector3((reverseXScale ? -1:1) * scale, scale, 1);
    }

    public override void SetRotation(float deg)
    {
        rootRotation = deg;
        pivot.transform.eulerAngles = new Vector3(0, 0, deg);
    }

    public override void SetPosition(float x, float y)
    {
        root.transform.localPosition = new Vector3(x, y, 0);
    }

    public override void SetReverseXScale(bool reverse)
    {
        reverseXScale = reverse;
        SetScale(RootScaleValue);
    }

    public override void SetCharacterWorldPosition(float worldX, float worldY)
    {
        root.transform.position = new Vector3(worldX, worldY, root.transform.position.z);
    }

    public override Vector3 GetCharacterSpecWorldPosition(int modelIndex)
    {
        return RootPosition;
    }

    public override float GetWebGalRotation()
    {
        return -RootRotation * Mathf.PI / 180;
    }

    public override void CopyScaleFromRoot()
    {
        pivot.localScale = Vector3.Scale(pivot.localScale, root.localScale);
        root.localScale = Vector3.one;
    }

    public override void BeforeGroupTransform(Transform parent)
    {
        root.parent = parent;
    }

    public override void AfterGroupTransform(float rotationDelta)
    {
        var oldPos = MainPos.position;
        root.parent = transform;
        root.eulerAngles = Vector3.zero;
        SetRotation(RootRotation + rotationDelta);
        CopyScaleFromRoot();
        SetCharacterWorldPosition(oldPos.x, oldPos.y);
    }

    #region 滤镜

    public override void OnFilterSetDataChanged(FilterProperty property)
    {
        switch (property)
        {
            case FilterProperty.Alpha:
            {
                UpdateAlphaFilter();
                break;
            }
            case FilterProperty.Blur:
            {
                UpdateBlurFilter();
                break;
            }
            case FilterProperty.Adjustment:
            {
                UpdateAdjustmentFilter();
                break;
            }
            case FilterProperty.Bevel:
            {
                UpdateBevelFilter();
                break;
            }
        }
    }
    
    // 更新屏幕尺寸相关参数
    private void UpdateScreenParams()
    {
        var mat = spriteRenderer.material;
        var modelAspect = sprite.bounds.size.x / sprite.bounds.size.y;
        FilterUtils.UpdateScreenParams(mat, modelAspect, RootScaleValue, 1f);
    }

    private void UpdateAlphaFilter()
    {
        var mat = spriteRenderer.material;
        FilterUtils.UpdateAlphaFilter(mat, filterSetData);
    }
    
    private void UpdateBlurFilter()
    {
        var mat = spriteRenderer.material;
        FilterUtils.UpdateBlurFilter(mat, filterSetData);
    }

    private void UpdateAdjustmentFilter()
    {
        var mat = spriteRenderer.material;
        FilterUtils.UpdateAdjustmentFilter(mat, filterSetData);
    }
    
    private void UpdateBevelFilter()
    {
        var mat = spriteRenderer.material;
        FilterUtils.UpdateBevelFilter(mat, filterSetData);
    }

    private void UpdateAllFilter()
    {
        UpdateScreenParams();
        UpdateAlphaFilter();
        UpdateBlurFilter();
        UpdateAdjustmentFilter();
        UpdateBevelFilter();
    }

    #endregion
}

public class ImageModelMeta
{
    public string name;
    public string imgPath;
    public string motionTemplate;
    public string transformTemplate;

    public string relativePath;

    public bool CalculateRelativePath()
    {
        if (L2DWUtils.TryGetRelativePath(imgPath, Global.ModelPath, out var relativePath))
        {
            this.relativePath = relativePath;
            return true;
        }
        return false;
    }
}