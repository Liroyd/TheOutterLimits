using UnityEngine;

public class MoveDraggableObject : MonoBehaviour {

    private bool dragging = false;
    private Transform toDrag;
    private readonly float INITIAL_POZITION_Z = -450f;
    private readonly int LAYER_PLANET = 1 << 8;

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
            moveObject(touch.position);
        }
        if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)) {
            onTouchEnd(touch.position);
        }
    }

    void UpdateMouse() {
        if (Input.GetMouseButtonDown(0)) {
            onTouchBegin(Input.mousePosition);
        }
        if (dragging && Input.GetMouseButton(0)) {
            moveObject(Input.mousePosition);

        }
        if (dragging && Input.GetMouseButtonUp(0)) {
            onTouchEnd(Input.mousePosition);
        }
    }

    private void onTouchBegin(Vector3 touchPosition) {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out hit) && (hit.collider.tag == "Draggable")) {
            toDrag = hit.transform;
            dragging = true;
        }
    }

    private void moveObject(Vector3 touchPosition) {
        touchPosition = new Vector3(touchPosition.x, touchPosition.y, INITIAL_POZITION_Z - Camera.main.transform.position.z);
        touchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        toDrag.position = touchPosition;
    }

    private void onTouchEnd(Vector3 touchPosition) {
        dragging = false;
        dropObjectOnPlanet(touchPosition);
    }

    private void dropObjectOnPlanet(Vector3 touchPosition) {
        RaycastHit  hitCurrentObject;
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out hitCurrentObject) && (hitCurrentObject.collider.tag == "Draggable")) {
            RaycastHit  hitPlanetObject;
           // Vector3 screenPositionOfCurrentObject = Camera.main.WorldToScreenPoint(hitCurrentObject.transform.position);
           // ray = Camera.main.ScreenPointToRay(screenPositionOfCurrentObject);
            if (Physics.Raycast(ray, out hitPlanetObject, 10000f, LAYER_PLANET)) {
                hitCurrentObject.transform.position = hitPlanetObject.point;
            }
        }
    }
}