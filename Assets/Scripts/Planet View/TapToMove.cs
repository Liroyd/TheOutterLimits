using UnityEngine;
using System.Collections;

public class TapToMove : MonoBehaviour {
    private float dist;
    private bool dragging = false;
    private Vector3 offset;
    private Transform toDrag;

    void Update() {
        UpdateMouse();
        //UpdateTouch();
        //TODO: add logic based on editor
    }


    void UpdateTouch() {
        if (Input.touchCount != 1) {
            dragging = false;
            return;
        }

        Vector3 v3;
        Touch touch = Input.touches[0];

        if (touch.phase == TouchPhase.Began) {
            RaycastHit hit;
            Vector3 pos = touch.position;
            Ray ray = Camera.main.ScreenPointToRay(pos);
            if (Physics.Raycast(ray, out hit) && (hit.collider.tag == "Draggable")) {
                toDrag = hit.transform;
                dist = hit.transform.position.z - Camera.main.transform.position.z;
                v3 = new Vector3(pos.x, pos.y, dist);
                v3 = Camera.main.ScreenToWorldPoint(v3);
                offset = toDrag.position - v3;
                dragging = true;
            }
        }
        if (dragging && touch.phase == TouchPhase.Moved) {
            v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
            v3 = Camera.main.ScreenToWorldPoint(v3);
            toDrag.position = v3 + offset;
        }
        if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)) {
            dragging = false;
        }
    }

    void UpdateMouse() {
        Vector3 v3;
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit  hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && (hit.collider.tag == "Draggable")) {
                toDrag = hit.transform;
                dist = hit.transform.position.z - Camera.main.transform.position.z;
                v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
                v3 = Camera.main.ScreenToWorldPoint(v3);
                offset = toDrag.position - v3;
                dragging = true;
            }
        }
        if (Input.GetMouseButton(0) && dragging) {
            v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
            v3 = Camera.main.ScreenToWorldPoint(v3);
            toDrag.position = v3 + offset;
            toDrag.position -= new Vector3(0, 0, toDrag.transform.position.z + 382f);
            //TODO: refactor it : drag&drop
        }
        if (Input.GetMouseButtonUp(0)) {
            dragging = false;
            test();
        }
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