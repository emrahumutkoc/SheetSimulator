using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCam : MonoBehaviour {
    [SerializeField] private GameObject inventory;

    public float sensX;
    public float sensY;
    public float sens = 1;

    public Transform orientation;
    private bool camLocked = false;

    private float xRotation;
    private float yRotation;

    private float defaultFOV = 60;
    private float tempFOV;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        float savedFov = PlayerPrefs.GetFloat("FOV", 80f);
        float savedSens = PlayerPrefs.GetFloat("sensitivity", 3);
        bool invertX = PlayerPrefs.GetInt("invertX", 0) == 1;
        bool invertY = PlayerPrefs.GetInt("invertY", 0) == 1;

        InvertXAxis(invertX);
        InvertYAxis(invertY);
        SetSensitivity(savedSens);
        tempFOV = savedFov;
        DoFov(savedFov);
    }

    private void Update() {
        // get mouse input
        if (!inventory.activeSelf && !camLocked) {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * sensX * sens;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * sensY * sens;

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
    }

    public void SetCamFreeWithoutRotation() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        camLocked = false;
        xRotation = 0;
        yRotation = 0;
    }

    public void DoFov(float endValue) {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt) {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }

    public void SetDefaultFOV() {
        DoFov(defaultFOV);
    }

    public void SetTempFov() {
        DoFov(tempFOV);
    }
    
    public void SetSensitivity(float sensitivity) {
        sens = sensitivity;
    }

    public void InvertXAxis(bool invert) {        
        sensX = Mathf.Abs(sensX) * (invert ? -1 : 1);
    }

    public void InvertYAxis(bool invert) {
        sensY = Mathf.Abs(sensY) * (invert ? -1 : 1);
    }
}
