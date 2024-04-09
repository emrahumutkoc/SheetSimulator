using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour {
    [SerializeField] private GameObject inventory;

    public float sensX;
    public float sensY;

    public Transform orientation;
    private bool camLocked = false;

    private float xRotation;
    private float yRotation;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        // get mouse input
        if (!inventory.activeSelf && !camLocked) {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * sensY;

            yRotation += mouseX;
            xRotation -= mouseY;
            // Debug.Log(xRotation + "     " + yRotation);
            // player cant look up or down more than 90 degrees
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            // rotate cam and orientation

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
            // Debug.Log(transform.rotation);
        }
    }

    public void SetCamLocked() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        camLocked = true;
    }

    public void SetCamFree() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        camLocked = false;
        xRotation = 0;
        yRotation = 0;
    }
}
