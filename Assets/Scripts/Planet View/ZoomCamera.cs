using UnityEngine;

public class ZoomCamera : MonoBehaviour
{
    private float perspectiveZoomSpeed = 0.2f;        // The rate of change of the field of view in perspective mode.
    private float orthographicZoomSpeed = 0.2f;        // The rate of change of the orthographic size in orthographic mode.
    Camera currentCamera;

    void  Start() {
        currentCamera = GetComponent<Camera>();
    }

    void Update() {
        // If there are two touches on the device...
        if (Input.touchCount == 2) {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // If the camera is orthographic...
            if (currentCamera.orthographic) {
                // ... change the orthographic size based on the change in distance between the touches.
                currentCamera.orthographicSize += deltaMagnitudeDiff * orthographicZoomSpeed;

                // Clamp the field of view to make sure it's between 330 and 1150.
                currentCamera.orthographicSize = Mathf.Clamp(currentCamera.orthographicSize, 330f, 1150f);
            } else {
                // Otherwise change the field of view based on the change in distance between the touches.
                currentCamera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                // Clamp the field of view to make sure it's between 0 and 180.
                currentCamera.fieldOfView = Mathf.Clamp(currentCamera.fieldOfView, 40f, 100f);
            }
        }
    }
}