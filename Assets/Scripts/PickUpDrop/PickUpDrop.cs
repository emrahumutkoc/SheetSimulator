using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpDrop : MonoBehaviour {
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickUpLayerMask;

    private ObjectGrabbable lastGrabbingObject = null;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            float pickUpDistance = 5f;
            if(Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask)) {
                if(raycastHit.transform.TryGetComponent(out ObjectGrabbable objectGrabbable)) {
                    if (!objectGrabbable.isGrabbing) {
                        objectGrabbable.Grab();
                        lastGrabbingObject = objectGrabbable;
                    } else {
                        if (objectGrabbable.isGrabbing && objectGrabbable.canPlaceObject) {
                            lastGrabbingObject.Drop();
                            lastGrabbingObject = null;
                        }
                    }
                }
            }
        }
    }
}
