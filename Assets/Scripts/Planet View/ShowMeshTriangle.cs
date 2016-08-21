using UnityEngine;
using System.Net;

public class ShowMeshTriangle : MonoBehaviour {

    //public GameObject myObject;

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

       /* LineRenderer renderer = myObject.GetComponent<LineRenderer>();
        renderer.SetPosition(0, p0);
        renderer.SetPosition(1, p1);
        renderer.SetPosition(2, p2);
        renderer.SetPosition(3, p0);*/
       Debug.DrawLine(p0, p1, Color.red, 2, false);
       Debug.DrawLine(p1, p2, Color.red, 2, false);
       Debug.DrawLine(p2, p0, Color.red, 2, false);

	}

   /* void OnPostRender() {
        if (!material) {
            var shader = Shader.Find("Hidden/Internal-Colored");
            material = new Material(shader);
            material.hideFlags = HideFlags.HideAndDontSave;
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusDstColor);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);

            material.SetInt("_cull", (int)UnityEngine.Rendering.CullMode.Off);
            material.SetInt("_ZWrite",0);
            material.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
        }
        GL.PushMatrix();
        material.SetPass(0);
        GL.Color(Color.red);
        GL.LoadOrtho();
        GL.Begin(GL.LINES);
        GL.Vertex(p0);
        GL.Vertex(p1);
        GL.Vertex(p2);
        GL.Vertex(p0);
        GL.End();
        GL.PopMatrix();
    }*/
}
