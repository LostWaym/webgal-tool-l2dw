using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGContainer : MonoBehaviour
{
    public Transform root;
    public SpriteRenderer bg;

    public float rootRotation;
    public float rootScale = 1;
    public Vector3 rootPosition => root.localPosition;
    public Vector3 rootWorldPosition => root.position;

    public float width;
    public float height;

    public void Adjust()
    {
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }

    public void LoadTexture(Texture2D texture)
    {
        var aspect = (float)Constants.WebGalWidth / Constants.WebGalHeight;
        var texAspect = (float)texture.width / texture.height;
        float scale;
        width = texture.width;
        height = texture.height;
        if(texAspect > aspect)
        {
            scale = (float)Constants.WebGalHeight / texture.height;
        }
        else
        {
            scale = (float)Constants.WebGalWidth / texture.width;
        }
        bg.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        bg.transform.localScale = new Vector3(scale * 100, scale * 100, 1);
    }

    public void SetPosition(float x, float y)
    {
        root.localPosition = new Vector3(x, y, 0);
        UIEventBus.SendEvent(UIEventType.BGTransformChanged);
    }

    public void SetWorldPosition(float x, float y)
    {
        root.position = new Vector3(x, y, 0);
        UIEventBus.SendEvent(UIEventType.BGTransformChanged);
    }

    public void SetScale(float scale)
    {
        rootScale = scale;
        root.localScale = new Vector3(scale, scale, 1);
        UIEventBus.SendEvent(UIEventType.BGTransformChanged);
    }

    public void SetRotation(float rotation)
    {
        rootRotation = rotation;
        root.localRotation = Quaternion.Euler(0, 0, rotation);
        UIEventBus.SendEvent(UIEventType.BGTransformChanged);
    }

    public float GetWebGalRotation()
    {
        return -rootRotation * Mathf.PI / 180;
    }

    internal void CopyScaleFromRoot()
    {
        rootScale = root.localScale.x;
    }
}
