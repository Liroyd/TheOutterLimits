using UnityEngine;

public class DragCamera : MonoBehaviour {

    private bool isMoveCameraAfterFastSlide;
    private bool secondTouchDetected;

    private Vector3 initialCameraPosition;
    private Vector3 initialTouchPosition;
    public float panSpeed = 1000f;

    private Vector3 deltaCameraPosition;
    private float deltaMagnitudeOfSlide;
    private int counter = 0;

    private float slideForwardSpeedReduce = 1f;
    private float slideBackwardSpeedReduce = 5f;

    public float cameraMovementLimit = 500f;
    private Vector3 minCameraPosition;
    private Vector3 maxCameraPosition;

    void Start() {
        calculateLimitCameraPosition();
    }

    private void calculateLimitCameraPosition() {
        minCameraPosition = new Vector3(transform.position.x - cameraMovementLimit, transform.position.y - cameraMovementLimit, transform.position.z);
        maxCameraPosition = new Vector3(transform.position.x + cameraMovementLimit, transform.position.y + cameraMovementLimit, transform.position.z);
    }


    void Update() {
        #if UNITY_EDITOR
        UpdateMouse();
        #else
        UpdateTouch();
        #endif
    }

    void UpdateMouse() {
        if (Input.GetMouseButtonDown(0)) {
            setInitialPosition(Input.mousePosition);
        }
        // move camera if not touch draggable
        if (Input.GetMouseButton(0) && !isHitDraggable(Input.mousePosition)) {
            moveCamera(Input.mousePosition);
            return;
        }

        // no touch = no movement; isMoveCameraAfterFastSlide allow moving camera after fast slide
        if (isMoveCameraAfterFastSlide) {
            // move camera in slide direction for fast slide
            moveCameraAfterFastSlide();
        }
    }

    void UpdateTouch() {
        var touchCount = Input.touchCount;
        // no touch = no movement; isMoveCameraAfterFastSlide allow moving camera after fast slide
        if (touchCount == 0) {
            if (isMoveCameraAfterFastSlide) {
                // move camera in slide direction for fast slide
                moveCameraAfterFastSlide();
            }
            // no touch = no movement
            return;
        }

        // second touch detected
        if (touchCount > 1) {
            secondTouchDetected = true;
            return;
        }

        Touch touch = Input.GetTouch(0);

        // if (secondTouchDetected) is needed to erase stored data and avoid jumping of camera
        if (touch.phase == TouchPhase.Began || secondTouchDetected) {
            setInitialPosition(touch.position);
        }
        // move camera if not touch draggable
        if (touch.phase == TouchPhase.Moved && !isHitDraggable(touch.position)) {
            moveCamera(touch.position);
        }
    }

    private void setInitialPosition(Vector3 touchPosition) {
        secondTouchDetected = false;
        initialCameraPosition = transform.position;
        initialTouchPosition = Camera.main.ScreenToViewportPoint(touchPosition);
        counter = 0;
    }

    private void moveCamera(Vector3 touchPosition) {
        Vector3 currentTouchPosition = Camera.main.ScreenToViewportPoint(touchPosition);
        Vector3 deltaTouchPosition = initialTouchPosition - currentTouchPosition; //Get the difference between where the touches

        Vector3 targetPosition = initialCameraPosition + deltaTouchPosition * panSpeed;
        targetPosition = getCameraPositionBasedOnLimit(targetPosition);

        // is needed for moveCameraAfterFastSlide
        deltaCameraPosition = transform.position - targetPosition;
        calculateMagnitudeOfSlide();

        transform.position = targetPosition;
    }

    private void calculateMagnitudeOfSlide() {
        deltaMagnitudeOfSlide = (deltaCameraPosition / Time.deltaTime).magnitude; // speed of swipe
        isMoveCameraAfterFastSlide = deltaMagnitudeOfSlide > 1000f;
    }

    private Vector3 getCameraPositionBasedOnLimit(Vector3 targetPosition) {
        float x = Mathf.Clamp(targetPosition.x, minCameraPosition.x, maxCameraPosition.x);
        float y = Mathf.Clamp(targetPosition.y, minCameraPosition.y, maxCameraPosition.y);
        return new Vector3(x, y, targetPosition.z);
    }

    private void moveCameraAfterFastSlide() {
        if (counter == 0) {
            calculateSpeedForEndOfSliding();
        } else if (counter < 7) {
            Vector3 targetPosition = transform.position - deltaCameraPosition / slideForwardSpeedReduce;
            transform.position = getCameraPositionBasedOnLimit(targetPosition); // move camera in slide direction
        } else if (counter < 10) {
            Vector3 targetPosition = transform.position + deltaCameraPosition / slideForwardSpeedReduce;
            //transform.position = getCameraPositionBasedOnLimit(targetPosition); // move camera backward for drama effect //TODO move came back
        } else {
            counter = 0;
            isMoveCameraAfterFastSlide = false;
            return;
        }
        counter++;
    }

    private void calculateSpeedForEndOfSliding() {
        if (deltaMagnitudeOfSlide < 2500f) {
            slideForwardSpeedReduce = 5f;
            slideBackwardSpeedReduce = 9f;
        } else if (deltaMagnitudeOfSlide < 4000f) {
            slideForwardSpeedReduce = 4f;
            slideBackwardSpeedReduce = 8f;
        } else if (deltaMagnitudeOfSlide < 8000f) {
            slideForwardSpeedReduce = 3f;
            slideBackwardSpeedReduce = 7f;
        } else if (deltaMagnitudeOfSlide < 10000f) {
            slideForwardSpeedReduce = 2f;
            slideBackwardSpeedReduce = 6f;
        } else {
            slideForwardSpeedReduce = 1f;
            slideBackwardSpeedReduce = 5f;
        }
    }

    private bool isHitDraggable (Vector3 touchPosition) {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        return Physics.Raycast(ray, out hit) && (hit.collider.tag == "Draggable");
    }
}
