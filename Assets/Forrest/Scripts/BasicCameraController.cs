using UnityEngine;

public class BasicCameraController : MonoBehaviour {

    [SerializeField]
    private Transform target;
    [SerializeField]
    private float targetDistance = 5;
    [SerializeField]
    private Vector3 lookAtOffset = Vector3.zero;
    [SerializeField]
    private Vector2 cameraSensitivity = Vector2.one;

    private Vector3 camRotation = new Vector3(25, 0, 0);

    void LateUpdate() { 
        handleInput();
        transform.position = lookAtOffset + target.transform.position + calculateTargetPosition();
        transform.LookAt(target.transform.position + lookAtOffset);
    }

    private void handleInput() {
        if (Input.GetMouseButton(1)) {
            Cursor.lockState = CursorLockMode.Locked;
            camRotation += new Vector3(-Input.GetAxisRaw("Mouse Y") * cameraSensitivity.y, Input.GetAxisRaw("Mouse X") * cameraSensitivity.x, 0) * 180 * Time.deltaTime;
        } else {
            Cursor.lockState = CursorLockMode.None;
            camRotation += Vector3.up * Input.GetAxis("Horizontal") * 180 * Time.deltaTime;
        }

        camRotation = new Vector3(Mathf.Clamp(camRotation.x, -20, 70), camRotation.y, camRotation.z);
    }

    private Vector3 getTargetRotation() {
        return camRotation;
    }

    private Vector3 calculateTargetPosition() {
        Vector3 targetPos = Vector3.back * targetDistance;

        Matrix4x4 rotation = Matrix4x4.Rotate(Quaternion.Euler(getTargetRotation()));
        targetPos = rotation * targetPos;

        return targetPos;
    }

}
