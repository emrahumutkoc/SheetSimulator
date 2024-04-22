using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizantalObjectPlacementManager : MonoBehaviour {
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private float rotationSensivity = 10f;

    private HorizantalGrabbable lastHorizantalObject = null;

    private bool isGrabbing = false;
    

    private void Update() {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1))) {
            float pickUpDistance = 5f;
            if (!isGrabbing) {
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask)) {
                    if (raycastHit.transform.TryGetComponent(out HorizantalGrabbable objectGrabbable)) {
                        if (!objectGrabbable.isGrabbing && lastHorizantalObject == null) {
                            objectGrabbable.Grab();
                            isGrabbing = true;
                            lastHorizantalObject = objectGrabbable;
                        }
                    }
                }
            } else {
                if (lastHorizantalObject != null && lastHorizantalObject.isGrabbing && lastHorizantalObject.canPlaceObject) {
                    lastHorizantalObject.PlaceObject();
                    lastHorizantalObject = null;
                    isGrabbing = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.G) && lastHorizantalObject != null && lastHorizantalObject.isGrabbing) {
            lastHorizantalObject.Drop();
            lastHorizantalObject = null;
            isGrabbing = false;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKeyDown(KeyCode.R) && lastHorizantalObject != null) {
            // lastHorizantalObject.transform.Rotate(0f, 90f, 0f, Space.World);
            float currentYRotation = lastHorizantalObject.transform.eulerAngles.y;
            float newYRotation = currentYRotation + 90f;
            newYRotation = Mathf.Round(newYRotation / 90f) * 90f;
            lastHorizantalObject.transform.eulerAngles = new Vector3(0, newYRotation, 0);
        }

        if (lastHorizantalObject != null && scroll != 0f) {
            float yRotation = scroll * rotationSensivity;
            lastHorizantalObject.transform.Rotate(0f, yRotation, 0f, Space.World);
        }
    }
}
