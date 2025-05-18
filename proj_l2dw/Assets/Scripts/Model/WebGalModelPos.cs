

using UnityEngine;

public class WebGalModelPos : MonoBehaviour
{
    public Transform pivot;
    public Transform anchor;
    public MyGOLive2DEx model;

    public void Adjust(float offsetX, float offsetY)
    {
        if (Global.__PIVOT_2_4)
        {
            Adjust2_4(offsetX, offsetY);

        }
        else
        {
            Adjust2_1___2_3(offsetX, offsetY);
        }
    }

    private void Adjust2_1___2_3(float offsetX, float offsetY)
    {
        anchor.localScale = Vector3.one * 0.5f;
        float modelWidth = model.Live2DModel.getCanvasWidth();
        float modelHeight = model.Live2DModel.getCanvasHeight();

        float scaleX = Constants.WebGalWidth / modelWidth;
        float scaleY = Constants.WebGalHeight / modelHeight;

        float scale = Mathf.Min(scaleX, scaleY);
        scale *= 1.5f;

        transform.localScale = Vector3.one;
        transform.localEulerAngles = Vector3.zero;
        pivot.localScale = new Vector3(scale, scale, scale);
        transform.localPosition = new Vector3(Constants.WebGalWidth / 2 + offsetX, -Constants.WebGalHeight / 1.2f - offsetY, 0);
        // 需要根据除以缩放比例来让偏移正确
        model.left = offsetX / scale;
        model.up = offsetY / scale;
    }

    private void Adjust2_4(float offsetX, float offsetY)
    {
        float modelWidth = model.Live2DModel.getCanvasWidth();
        float modelHeight = model.Live2DModel.getCanvasHeight();
        
        anchor.localScale = Vector3.one * 0.5f;
        anchor.transform.localPosition = new Vector3(-modelWidth * 0.5f, modelHeight * 0.5f);

        float scaleX = Constants.WebGalWidth / modelWidth;
        float scaleY = Constants.WebGalHeight / modelHeight;

        float scale = Mathf.Min(scaleX, scaleY);

        float targetWidth = modelWidth * scale;
        float targetHeight = modelHeight * scale;

        float localY = Constants.WebGalHeight / 2;
        float localX = Constants.WebGalWidth / 2;
        // if (targetHeight < Constants.WebGalHeight)
        // {
        //     localY = Constants.WebGalHeight / 2 + Constants.WebGalHeight - targetHeight / 2;
        // }

        transform.localScale = Vector3.one;
        transform.localEulerAngles = Vector3.zero;
        pivot.localScale = new Vector3(scale, scale, scale);
        // pivot.transform.localPosition = new Vector3(0, Constants.WebGalHeight / 2, 0);
        transform.localPosition = new Vector3(localX + offsetX, -localY - offsetY, 0);
        // 需要根据除以缩放比例来让偏移正确
        model.left = offsetX / scale;
        model.up = offsetY / scale;
    }
}
