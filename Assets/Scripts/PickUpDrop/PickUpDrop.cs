using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpDrop : MonoBehaviour {
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private float rotationSensivity = 10f;

    private ObjectGrabbable lastGrabbingObject = null;
    private bool isGrabbing = false;
    

    private void Update() {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1))) {
            float pickUpDistance = 5f;
            if(Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask)) {
                if(raycastHit.transform.TryGetComponent(out ObjectGrabbable objectGrabbable)) {
                    if (!objectGrabbable.isGrabbing && lastGrabbingObject == null) {
                        objectGrabbable.Grab();
                        isGrabbing = true;
                        lastGrabbingObject = objectGrabbable;
                    } else {
                        if (objectGrabbable.isGrabbing && objectGrabbable.canPlaceObject) {
                            lastGrabbingObject.Drop();
                            lastGrabbingObject = null;
                            isGrabbing = false;
                        }
                    }
                } else if (raycastHit.transform.TryGetComponent(out HorizantalGrabbable orizantalGrabbable)) {
                    if (!orizantalGrabbable.isGrabbing && lastGrabbingObject == null) {
                        orizantalGrabbable.Grab();
                        isGrabbing = true;
                        lastGrabbingObject = objectGrabbable;
                    } else {
                        if (orizantalGrabbable.isGrabbing && objectGrabbable.canPlaceObject) {
                            lastGrabbingObject.Drop();
                            lastGrabbingObject = null;
                            isGrabbing = false;
                        }
                    }
                }
            } else {
                if (lastGrabbingObject != null && lastGrabbingObject.isGrabbing && lastGrabbingObject.canPlaceObject) {
                    lastGrabbingObject.Drop();
                    lastGrabbingObject = null;
                    isGrabbing = false;
                }
            }
        }

/*        if (Input.mouseScrollDelta.y != 0 && lastGrabbingObject != null) {
            lastGrabbingObject.transform.Rotate(Vector3.right * Input.mouseScrollDelta.y * 5f);
        }
*/
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKeyDown(KeyCode.R) && lastGrabbingObject != null) {
            lastGrabbingObject.transform.Rotate(0f, 90f, 0f, Space.World);

        }
        // Objeyi Y ekseni etrafýnda döndür
        // Scroll deðeri pozitifse veya negatifse, objenin Y ekseni etrafýnda dönmesini saðla
        if (lastGrabbingObject != null && scroll != 0f) {
            // Y ekseninde dönme miktarýný hesapla
            float yRotation = scroll * rotationSensivity;

            // Mevcut rotasyona yeni dönme miktarýný ekle
            lastGrabbingObject.transform.Rotate(0f, yRotation, 0f, Space.World);
        }
    }
}
