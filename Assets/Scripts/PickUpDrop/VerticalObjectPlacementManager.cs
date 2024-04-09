using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalObjectPlacementManager : MonoBehaviour {
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private float rotationSensivity = 10f;

    private VerticalGrabbable lastVerticalObject = null;

    private bool isGrabbing = false;
    

    private void Update() {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1))) {
            float pickUpDistance = 5f;

            if (!isGrabbing) {
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask)) {
                    if (raycastHit.transform.TryGetComponent(out VerticalGrabbable objectGrabbable)) {
                        if (!objectGrabbable.isGrabbing && lastVerticalObject == null) {
                            objectGrabbable.Grab();
                            isGrabbing = true;
                            lastVerticalObject = objectGrabbable;
                        }
                    }
                }
            } else {
                if (lastVerticalObject != null && lastVerticalObject.isGrabbing && lastVerticalObject.canPlaceObject) {
                    lastVerticalObject.PlaceObject();
                    lastVerticalObject = null;
                    isGrabbing = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.G) && lastVerticalObject != null && lastVerticalObject.isGrabbing) {
            lastVerticalObject.Drop();
            lastVerticalObject = null;
            isGrabbing = false;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKeyDown(KeyCode.R) && lastVerticalObject != null) {
            lastVerticalObject.transform.Rotate(0, 90f, 0f, Space.World);

        }
        if (lastVerticalObject != null && scroll != 0f) {
            float yRotation = scroll * rotationSensivity;
            lastVerticalObject.transform.Rotate(0f, yRotation, 0f, Space.World);
        }
    }
}
