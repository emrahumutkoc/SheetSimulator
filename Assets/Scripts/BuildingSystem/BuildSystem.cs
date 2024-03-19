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
    [SerializeField] private float connectorOverlapRadius = 1;

    private void Update() {
        // 'B' tuþuna basýldýðýnda inþa modunu aktif et
        if (Input.GetKeyDown(KeyCode.B)) {
            isBuilding = !isBuilding;

            if (isBuilding && ghostBuildGameObject == null) {
                ghostBuildGameObject = Instantiate(wallObject);
                Rigidbody rb = ghostBuildGameObject.transform.GetChild(0).GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.isKinematic = true;

            }
             
            if (!isBuilding) {
                ghostBuildGameObject = null;
                Destroy(ghostBuildGameObject);
            }
        }

        // Ýnþa modu aktifse
       
    }


    public void FixedUpdate() {
        if (isBuilding) {

            /*if (ghostBuildGameObject == null) {
                ghostBuildGameObject = Instantiate(wallObject);
                *//*if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit firstPoint, placeDistance)) {
                    ghostBuildGameObject.transform.position = firstPoint.point;
                }*//*
                // ghostBuildGameObject.GetComponent<Rigidbody>().useGravity = false;
            }*/
            Collider collider = ghostBuildGameObject.transform.GetChild(0).GetComponent<Collider>();
            // float height = collider.bounds.size.y / 2;
            float height = 0;

            // Kameranýn baktýðý noktadan 5f mesafeye bir raycast çek
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, placeDistance)) {

                Vector3 newPosition = new Vector3(hit.point.x, hit.point.y + height, hit.point.z);
                Vector3 newPositionLerp = Vector3.Lerp(transform.position, newPosition, Time.fixedDeltaTime * lerpSpeed);
                 ghostBuildGameObject.transform.position = newPosition;
                //ghostBuildGameObject.GetComponent<Rigidbody>().MovePosition(newPositionLerp);
                // Raycast bir þeyle çarparsa, çarptýðý noktada duvar objesini instantiate et

                // Instantiate(wallObject, hit.point, Quaternion.identity);
                // MoveGhostPrefabToRaycast();
                // Ýnþa iþlemi tamamlandýktan sonra isBuilding'i false yap
            }

            Collider[] colliders = Physics.OverlapSphere(ghostBuildGameObject.transform.position, connectorOverlapRadius, replacableLayers);
            if (colliders.Length > 0) {
                BuildConnector bestConnector = null;

                foreach (Collider coll in colliders) {
                    // Debug.Log("coll" + coll.gameObject.name);
                    BuildConnector connector = coll.GetComponent<BuildConnector>();
                    if (connector != null && connector.canConnectTo) {
                        Debug.Log("connector found");
                    }
                }

/*                if (bestConnector == null) {
                    GhostifyModel(ModelParent, ghostMaterialInvalid);
                    isGhostInValidPosition = false;
                    return;
                }*/
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
