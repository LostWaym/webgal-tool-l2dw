

using UnityEngine;

public class WebGalModelPos : MonoBehaviour
{
    public Transform pivot;
    public Transform anchor;
    public MyGOLive2DEx model;

    public void Adjust(float offsetX, float offsetY)
    {
        // if (Global.__PIVOT_2_4)
        // {
        //     Adjust2_4(offsetX, offsetY);
        // }
        // else
        // {
        //     Adjust2_1___2_3(offsetX, offsetY);
        // }
        switch (Global.PivotMode)
        {
            case Global.FigurePivotMode.W_4_5_12:
            {
                Adjust2_1___2_3(offsetX, offsetY);
                break;
            }
            case Global.FigurePivotMode.W_4_5_13:
            {
                Adjust2_4(offsetX, offsetY);
                break;
            }
            case Global.FigurePivotMode.BC_1_0:
            {
                AdjustBC_1_0(offsetX, offsetY);
                break;
            }
        }
    }

    private void Adjust2_1___2_3(float offsetX, float offsetY)
    {
        anchor.localScale = Vector3.one * 0.5f;
        float modelWidth = model.getModifiedWidth();
        float modelHeight = model.getModifiedHeight();

        float scaleX = Constants.WebGalWidth / modelWidth;
        float scaleY = Constants.WebGalHeight / modelHeight;

        float scale = Mathf.Min(scaleX, scaleY);
        scale *= 1.5f;
        
        float snapBottomOffset = 0.0f;
        float modelAspect = modelWidth / modelHeight;
        float screenAspect = (float)Constants.WebGalWidth / (float)Constants.WebGalHeight;
        if (modelAspect > screenAspect)
        {
            snapBottomOffset = (Constants.WebGalHeight * screenAspect / modelAspect - Constants.WebGalHeight) / 2.0f;
        }

        transform.localScale = Vector3.one;
        transform.localEulerAngles = Vector3.zero;
        pivot.localScale = new Vector3(scale, scale, scale);
        transform.localPosition = new Vector3(Constants.WebGalWidth / 2.0f + offsetX, -Constants.WebGalHeight / 1.2f - offsetY + snapBottomOffset, 0);
        // 需要根据除以缩放比例来让偏移正确
        model.left = offsetX / scale;
        model.up = offsetY / scale;
    }

    private void Adjust2_4(float offsetX, float offsetY)
    {
        float modelWidth = model.getModifiedWidth();
        float modelHeight = model.getModifiedHeight();
        
        anchor.localScale = Vector3.one * 0.5f;
        anchor.transform.localPosition = new Vector3(-modelWidth * 0.5f, modelHeight * 0.5f);

        float scaleX = Constants.WebGalWidth / modelWidth;
        float scaleY = Constants.WebGalHeight / modelHeight;

        float scale = Mathf.Min(scaleX, scaleY);

        float targetWidth = modelWidth * scale;
        float targetHeight = modelHeight * scale;

        float localY = Constants.WebGalHeight / 2.0f;
        float localX = Constants.WebGalWidth / 2.0f;
        // if (targetHeight < Constants.WebGalHeight)
        // {
        //     localY = Constants.WebGalHeight / 2 + Constants.WebGalHeight - targetHeight / 2;
        // }

        float snapBottomOffset = 0.0f;
        float modelAspect = modelWidth / modelHeight;
        float screenAspect = (float)Constants.WebGalWidth / (float)Constants.WebGalHeight;
        if (modelAspect > screenAspect)
        {
            snapBottomOffset = (Constants.WebGalHeight * screenAspect / modelAspect - Constants.WebGalHeight) / 2.0f;
        }

        transform.localScale = Vector3.one;
        transform.localEulerAngles = Vector3.zero;
        pivot.localScale = new Vector3(scale, scale, scale);
        // pivot.transform.localPosition = new Vector3(0, Constants.WebGalHeight / 2, 0);
        transform.localPosition = new Vector3(localX + offsetX, -localY - offsetY + snapBottomOffset, 0);
        // 需要根据除以缩放比例来让偏移正确
        model.left = offsetX / scale;
        model.up = offsetY / scale;
    }
    
    private void AdjustBC_1_0(float offsetX, float offsetY)
    {
        float modelWidth = model.getModifiedWidth();
        float modelHeight = model.getModifiedHeight();
        
        anchor.localScale = Vector3.one * 0.5f;
        anchor.transform.localPosition = new Vector3(-modelWidth * 0.5f, modelHeight / 2.2f);

        float scaleX = Constants.WebGalWidth / modelWidth;
        float scaleY = Constants.WebGalHeight / modelHeight;

        float scale = Mathf.Min(scaleX, scaleY);
        scale *= 1.25f;

        float localY = Constants.WebGalHeight / 2.0f;
        float localX = Constants.WebGalWidth / 2.0f;
        
        float snapBottomOffset = 0.0f;
        float modelAspect = modelWidth / modelHeight;
        float screenAspect = (float)Constants.WebGalWidth / (float)Constants.WebGalHeight;
        if (modelAspect > screenAspect)
        {
            snapBottomOffset = (Constants.WebGalHeight * screenAspect / modelAspect - Constants.WebGalHeight) / 2.0f;
        }

        transform.localScale = Vector3.one;
        transform.localEulerAngles = Vector3.zero;
        pivot.localScale = new Vector3(scale, scale, scale);
        transform.localPosition = new Vector3(localX + offsetX, -localY - offsetY + snapBottomOffset, 0);
        // 需要根据除以缩放比例来让偏移正确
        model.left = offsetX / scale;
        model.up = offsetY / scale;
    }
}
