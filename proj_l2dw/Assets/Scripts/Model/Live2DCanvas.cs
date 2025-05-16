using System.Collections;
using System.Collections.Generic;
using live2d;
using UnityEngine;

public class Live2DCanvas
{
    private Live2DModelUnity model;
    public RenderTexture rt { get; private set; }
    private Matrix4x4 matrix;
    
    public Live2DCanvas(Live2DModelUnity model)
    {
        this.model = model;
        this.matrix = Matrix4x4.Ortho(
            0,
            model.getCanvasWidth(),
            model.getCanvasHeight(),
            0,
            -50.0f,
            50.0f
        );
        model.setMatrix(this.matrix);
        this.rt = new RenderTexture(
            (int)model.getCanvasWidth(),
            (int)model.getCanvasHeight(),
            0
        );
    }

    public void Draw()
    {
        var camera = Camera.main;
        if (camera == null)
        {
            Debug.LogError("Camera.main is null");
            return;
        }
        
        camera.targetTexture = this.rt;
        camera.projectionMatrix = this.matrix;
        model.setMatrix(Matrix4x4.identity);
        model.update();
        camera.Render();
        camera.targetTexture = null;
        camera.ResetProjectionMatrix();
    }
}
