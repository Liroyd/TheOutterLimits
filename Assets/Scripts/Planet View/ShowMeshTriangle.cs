using UnityEngine;
using System.Net;

public class ShowMeshTriangle : MonoBehaviour {

    Material material;

    Vector3 p0;
    Vector3 p1;
    Vector3 p2;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Vector3 touchPosition;
        #if UNITY_EDITOR
        touchPosition = Input.mousePosition;
        #else
        if (Input.touchCount != 1) {
            return;
        }
        touchPosition =  Input.touches[0].position;
        #endif

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(touchPosition), out hit)) {
            return;
        }
        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (meshCollider == null || meshCollider.sharedMesh == null) {
            return;
        }
        Mesh mesh = meshCollider.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
        p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
        p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];

        Transform hitTransform = hit.collider.transform;
        p0 = hitTransform.TransformPoint(p0);
        p1 = hitTransform.TransformPoint(p1);
        p2 = hitTransform.TransformPoint(p2);

        Debug.DrawLine(p0, p1, Color.blue, 2, false);
        Debug.DrawLine(p1, p2, Color.blue, 2, false);
        Debug.DrawLine(p2, p0, Color.blue, 2, false);
	}

 /*   void OnPostRender() {
        if (!material) {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }
        GL.PushMatrix();
        material.SetPass(0);
        GL.Color(Color.red);
        GL.LoadOrtho();
        GL.Begin(GL.TRIANGLES);
        GL.Vertex(p0);
        GL.Vertex(p1);
        GL.Vertex(p2);
        GL.End();
        GL.PopMatrix();
    }*/
}
