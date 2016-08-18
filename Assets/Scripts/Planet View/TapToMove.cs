using UnityEngine;

public class TapToMove : MonoBehaviour {
    private float dist;
    private bool dragging = false;
    private Vector3 offset;
    private Transform toDrag;

    void Update() {
        #if UNITY_EDITOR
            UpdateMouse();
        #else
            UpdateTouch();
        #endif
    }

    void UpdateTouch() {
        if (Input.touchCount != 1) {
            dragging = false;
            return;
        }
        Touch touch = Input.touches[0];

        if (touch.phase == TouchPhase.Began) {
            onTouchBegin(touch.position);
        }
        if (dragging && touch.phase == TouchPhase.Moved) {
            Vector3 touchPosition = new Vector3(touch.position.x, touch.position.y, dist);
            moveObject(touchPosition);
        }
        if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)) {
            onTouchEnd();
        }
    }

    void UpdateMouse() {
        if (Input.GetMouseButtonDown(0)) {
            onTouchBegin(Input.mousePosition);
        }
        if (dragging && Input.GetMouseButton(0)) {
            Vector3 touchPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
            moveObject(touchPosition);

        }
        if (dragging && Input.GetMouseButtonUp(0)) {
            onTouchEnd();
        }
    }

     private void onTouchBegin(Vector3 pos) {
         RaycastHit hit;
         Ray ray = Camera.main.ScreenPointToRay(pos);
         if (Physics.Raycast(ray, out hit) && (hit.collider.tag == "Draggable")) {
             toDrag = hit.transform;
             dist = hit.transform.position.z - Camera.main.transform.position.z;
             Vector3 v3 = new Vector3(pos.x, pos.y, dist);
             v3 = Camera.main.ScreenToWorldPoint(v3);
             offset = toDrag.position - v3;
             dragging = true;
         }
     }

    private void moveObject(Vector3 v3) {
        v3 = Camera.main.ScreenToWorldPoint(v3);
        toDrag.position = v3 + offset;
        //TODO: refactor it : drag&drop
        toDrag.position -= new Vector3(0, 0, toDrag.transform.position.z + 382f);
    }

    private void onTouchEnd() {
        dragging = false;
        test();
    }

    void test() {

        RaycastHit  hitCurrentObject;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitCurrentObject) && (hitCurrentObject.collider.tag == "Draggable")) {
            RaycastHit  hitPlanetObject;
            if (Physics.Raycast (ray, out hitPlanetObject, 10000f, 1 << 8)) {
                hitCurrentObject.transform.position -= new Vector3 (0, 0, hitCurrentObject.transform.position.z
                + hitPlanetObject.transform.position.z + hitPlanetObject.transform.position.magnitude + 350);
                //TODO: refactor it : drag&drop
            }
        }



    }
}