using UnityEngine;
using System.Net;

public class ShowMeshTriangle : MonoBehaviour {

    LineRenderer lineRenderer;

	// Use this for initialization
	void Start () {
        lineRenderer = createLineRenderer();
	}

	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Vector3 touchPosition;
        #if UNITY_EDITOR
        touchPosition = Input.mousePosition;
        #else
        if (Input.touchCount != 1) {
            clearTriangle(lineRenderer);
            return;
        }
        touchPosition =  Input.touches[0].position;
        #endif

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(touchPosition), out hit)) {
            clearTriangle(lineRenderer);
            return;
        }
        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (meshCollider == null || meshCollider.sharedMesh == null) {
            clearTriangle(lineRenderer);
            return;
        }
        Mesh mesh = meshCollider.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];

        Transform hitTransform = hit.collider.transform;
        p0 = hitTransform.TransformPoint(p0);
        p1 = hitTransform.TransformPoint(p1);
        p2 = hitTransform.TransformPoint(p2);

        lineRenderer.SetPosition(0, p0);
        lineRenderer.SetPosition(1, p1);
        lineRenderer.SetPosition(2, p2);
        lineRenderer.SetPosition(3, p0);
	}

    private LineRenderer createLineRenderer() {
        GameObject triangle = new GameObject();
        triangle.transform.parent = this.transform;
        triangle.name = "Triangle";
        LineRenderer lineRenderer = triangle.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 3;
        lineRenderer.endWidth = 3;
        Material material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material = material;

        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        lineRenderer.positionCount = 4;
        clearTriangle(lineRenderer);
        return lineRenderer;
    }

    private void clearTriangle(LineRenderer lineRenderer) {
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);
        lineRenderer.SetPosition(2, Vector3.zero);
        lineRenderer.SetPosition(3, Vector3.zero);
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
