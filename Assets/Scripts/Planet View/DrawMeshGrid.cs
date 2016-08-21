using UnityEngine;
using System;

public class DrawMeshGrid : MonoBehaviour {

    int[] trianglesToDraw = {510, 511, 478, 477, 471, 472, 479, 446, 445, 444, 429, 428, 545, 544, 559, 558, 566, 567,
        574, 575, 638, 637, 635, 599, 598, 606, 607, 543, 542, 541, 540, 539, 538, 537, 536, 430, 435, 474, 475, 476,
        415, 414, 72, 80, 67};

    // Use this for initialization
    void Start () {
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider == null || meshCollider.sharedMesh == null) {
            return;
        }

        drawGrid(meshCollider);
    }

    private void drawGrid(MeshCollider meshCollider) {
        Mesh mesh = meshCollider.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        LineRenderer lineRenderer = createLineRenderer();
        int gridIndex = 0;
        foreach (int i in trianglesToDraw) {
            Vector3 p0 = vertices[triangles[i * 3 + 0]];
            Vector3 p1 = vertices[triangles[i * 3 + 1]];
            Vector3 p2 = vertices[triangles[i * 3 + 2]];

            p0 = meshCollider.transform.TransformPoint(p0);
            p1 = meshCollider.transform.TransformPoint(p1);
            p2 = meshCollider.transform.TransformPoint(p2);

            lineRenderer.SetPosition(gridIndex, p0);
            lineRenderer.SetPosition(gridIndex + 1, p1);
            lineRenderer.SetPosition(gridIndex + 2, p2);
            lineRenderer.SetPosition(gridIndex + 3, p0);
            gridIndex += 4;
        }
    }

    private LineRenderer createLineRenderer() {
        GameObject grid = new GameObject();
        grid.name = "Grid";
        LineRenderer lineRenderer = grid.AddComponent<LineRenderer>();
        lineRenderer.SetWidth(1, 1);
        Material material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material = material;

        lineRenderer.SetColors(Color.green, Color.green);
        lineRenderer.SetVertexCount(trianglesToDraw.Length * 4);
        return lineRenderer;
    }
}