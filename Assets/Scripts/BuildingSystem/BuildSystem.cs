using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSystem : MonoBehaviour {

    [SerializeField] private GameObject wallObject;
    [SerializeField] private LayerMask replacableLayers;
    [SerializeField] private float placeDistance = 10f;
    [SerializeField] private float lerpSpeed = 2f;

    private GameObject ghostBuildGameObject;

    private bool isBuilding = false;
    [SerializeField] private float connectorOverlapRadius = 1f;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.B)) {
            isBuilding = !isBuilding;

            if (isBuilding && ghostBuildGameObject == null) {
                ghostBuildGameObject = Instantiate(wallObject);
                Collider[] colliders = ghostBuildGameObject.GetComponentsInChildren<Collider>();
                foreach(Collider coll in colliders) {
                    coll.isTrigger = true;
                }
            }
             
            if (!isBuilding) {
                ghostBuildGameObject = null;
                Destroy(ghostBuildGameObject);
            }
        }

        if (isBuilding) {
            Collider collider = ghostBuildGameObject.transform.GetChild(0).GetComponent<Collider>();
            // float height = collider.bounds.size.y / 2;
            // float height = 0;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, placeDistance)) {
                Vector3 newPosition = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                Vector3 newPositionLerp = Vector3.Lerp(transform.position, newPosition, Time.fixedDeltaTime * lerpSpeed);
                ghostBuildGameObject.transform.position = hit.point;
            }
        }
    }

    private void MoveGhostPrefabToRaycast() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 5f, replacableLayers)) {
            ghostBuildGameObject.transform.position = hit.point;
        }
    }
}
