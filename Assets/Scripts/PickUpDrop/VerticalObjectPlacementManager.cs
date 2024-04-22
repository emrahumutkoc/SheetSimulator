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
            // lastVerticalObject.transform.Rotate(0, 90f, 0f, Space.World);
            float currentYRotation = lastVerticalObject.transform.eulerAngles.y;
            float newYRotation = currentYRotation + 90f;
            newYRotation = Mathf.Round(newYRotation / 90f) * 90f;
            lastVerticalObject.transform.eulerAngles = new Vector3(0, newYRotation, 0);
        }

        if (lastVerticalObject != null && scroll != 0f) {
            float yRotation = scroll * rotationSensivity;
            lastVerticalObject.transform.Rotate(0f, yRotation, 0f, Space.World);
        }
    }

    private float NormalizeAndRoundRotation(float rotation) {
        // Normalize the rotation to be within 0-360 degrees
        rotation = rotation % 360;
        if (rotation < 0) rotation += 360;

        // Determine the closest of the four target angles (0, 90, 270, 360)
        float[] targetAngles = { 0, 90, 270, 360 };
        float closestAngle = targetAngles[0];
        float smallestDifference = Mathf.Abs(rotation - closestAngle);

        foreach (float targetAngle in targetAngles) {
            float difference = Mathf.Abs(rotation - targetAngle);
            if (difference < smallestDifference) {
                closestAngle = targetAngle;
                smallestDifference = difference;
            }
        }

        return closestAngle;
    }

}
