using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasRenderer))]
[RequireComponent(typeof(RectTransform))]
public class UILineRenderer : MaskableGraphic
{
    [SerializeField] private List<Vector2> points = new List<Vector2>();
    [SerializeField] private float lineWidth = 10f;
    [SerializeField] private bool loop = false;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (points.Count < 2)
            return;

        for (int i = 0; i < points.Count - 1; i++)
        {
            AddLineSegment(vh, points[i], points[i + 1]);
        }

        if (loop && points.Count > 2)
        {
            AddLineSegment(vh, points[points.Count - 1], points[0]);
        }
    }

    private void AddLineSegment(VertexHelper vh, Vector2 start, Vector2 end)
    {
        Vector2 dir = (end - start).normalized;
        Vector2 perp = new Vector2(-dir.y, dir.x) * (lineWidth * 0.5f);

        Vector2 corner1 = start + perp;
        Vector2 corner2 = start - perp;
        Vector2 corner3 = end + perp;
        Vector2 corner4 = end - perp;

        UIVertex[] vertices = new UIVertex[4];
        vertices[0] = CreateVertex(corner1, color, new Vector2(0, 0));
        vertices[1] = CreateVertex(corner2, color, new Vector2(0, 1));
        vertices[2] = CreateVertex(corner4, color, new Vector2(1, 1));
        vertices[3] = CreateVertex(corner3, color, new Vector2(1, 0));

        vh.AddUIVertexQuad(vertices);
    }

    private UIVertex CreateVertex(Vector2 pos, Color32 color, Vector2 uv)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.position = pos;
        vertex.color = color;
        vertex.uv0 = uv;
        return vertex;
    }

    public void SetPoints(List<Vector2> newPoints)
    {
        points = newPoints;
        SetVerticesDirty();
    }

    public void AddPoint(Vector2 point)
    {
        points.Add(point);
        SetVerticesDirty();
    }

    public void ClearPoints()
    {
        points.Clear();
        SetVerticesDirty();
    }
}    