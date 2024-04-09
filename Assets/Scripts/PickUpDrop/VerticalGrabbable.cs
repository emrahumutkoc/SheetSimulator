using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalGrabbable : MonoBehaviour {

    public bool isGrabbing = false;
    [Header("Ghost Settings")]
    [SerializeField] private Material ghostMaterialValid;
    [SerializeField] private Material ghostMaterialInvalid;

    [Header("Pick Up Settings")]
    [SerializeField] private Transform cameraPositionTransform;
    // [SerializeField] private Transform offsetToPlacementPoint;
    [SerializeField] private LayerMask objectHoverLayers;
    [SerializeField] private LayerMask objectPlacementLayers;
    [SerializeField] private LayerMask ignoredLayers;
    [SerializeField] private float lerpSpeed = 2f;
    [SerializeField] private float pickUpDistance = 5f;
    [SerializeField] private ReplacementDirection direction = ReplacementDirection.down;

    [SerializeField] private GameObject connectionPoints;
    [SerializeField] private Transform placementPoint;



    private List<Material> initialMaterials = new List<Material>();
    private Rigidbody rb;
    private Collider coll;

    private Vector3 newPlacementPosition;

    public bool canPlaceObject = true;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
    }
    public void Grab() {
        isGrabbing = true;
        coll.isTrigger = true;
        rb.useGravity = false;
        if (ghostMaterialValid != null) {
            initialMaterials.Clear();
            foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>()) {
                initialMaterials.Add(meshRenderer.material);
                meshRenderer.material = ghostMaterialValid;
            }
        }
    }

    public void Drop() {
        if (canPlaceObject) {
            PlaceObject();
            return;
        }

        isGrabbing = false;
        coll.isTrigger = false;
        rb.useGravity = true;
        // rb.isKinematic = false;
        canPlaceObject = false;

        foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>()) {
            foreach (Material material in initialMaterials) {
                meshRenderer.material = material;
            }
        }
    }

    public void PlaceObject() {
        if (canPlaceObject) {
            // transform.position = newPlacementPosition;
            isGrabbing = false;
            coll.isTrigger = false;
            rb.useGravity = true;

            foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>()) {
                foreach (Material material in initialMaterials) {
                    meshRenderer.material = material;
                }
            }
        }
    }

    private void FixedUpdate() {
        if (isGrabbing) {
            Vector3 offsetToPlacementPoint = transform.position - placementPoint.position;

       /*     Collider collider = GetComponent<Collider>();
            float height = 0f;*/
            if (Physics.Raycast(cameraPositionTransform.transform.position, cameraPositionTransform.transform.forward, out RaycastHit hit, pickUpDistance, objectHoverLayers)) {
                Vector3 placementPosition = hit.point + offsetToPlacementPoint;
                /*Quaternion targetRotation = Quaternion.LookRotation(hit.normal);
                targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y + 180f, 0);
                gameObject.transform.rotation = targetRotation;*/

                // Vector3 newPosition = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                // newPlacementPosition = newPosition;
                Vector3 newPositionLerp = Vector3.Lerp(transform.position, placementPosition, Time.fixedDeltaTime * lerpSpeed);
                rb.MovePosition(newPositionLerp);
            } else {
                Vector3 forwardPosition = cameraPositionTransform.transform.position + cameraPositionTransform.transform.forward * pickUpDistance;
                int layerMask = 1 << LayerMask.NameToLayer("Grabbable");
                layerMask = ~layerMask;
                if (Physics.Raycast(forwardPosition, Vector3.down, out RaycastHit hitInfo, pickUpDistance, layerMask)) {
                    Vector3 placementPosition = hitInfo.point + offsetToPlacementPoint;
                    Vector3 newPositionLerp = Vector3.Lerp(transform.position, placementPosition, Time.fixedDeltaTime * lerpSpeed);
                    rb.MovePosition(newPositionLerp);
                }
            }

            if (connectionPoints != null) {
                bool allHitTargetLayer = true;

                foreach (Transform child in connectionPoints.transform) {
                    RaycastHit pointHit;
                    if (Physics.Raycast(child.position, Vector3.down, out pointHit, 1f, objectPlacementLayers)) {
                        // Debug.Log(child.name + " çarptýðý obje: " + pointHit.collider.gameObject.name + ", Mesafe: " + pointHit.distance);
                        Debug.DrawRay(child.position, Vector3.down * pointHit.distance, Color.green);
                    } else {
                        Physics.Raycast(child.position, Vector3.down, out pointHit, 2f);
                        // Debug.Log(child.name + " hedef layer ile çarpýþmadý." + "ÇARPTIÐI NESNE: " + pointHit.transform.gameObject.name);
                        Debug.DrawRay(child.position, Vector3.down * 100, Color.red);
                        allHitTargetLayer = false;
                    }
                }
                if (allHitTargetLayer) {
                    MakeObjectPlacable();
                } else {
                    MakeObjectUnavailable();
                }
            }
        }
    }

    private void OnTriggerStay(Collider collider) {
        if (isGrabbing && ((objectPlacementLayers.value & (1 << collider.gameObject.layer)) == 0) && ((ignoredLayers.value & (1 << collider.gameObject.layer)) == 0)) {
            //MakeObjectPlacable();
            MakeObjectUnavailable();
        }
    }

    private void MakeObjectPlacable() {
        canPlaceObject = true;
        foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>()) {
            meshRenderer.material = ghostMaterialValid;
        }
    }

    private void MakeObjectUnavailable() {
        canPlaceObject = false;
        foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>()) {
            meshRenderer.material = ghostMaterialInvalid;
        }
    }
}

public enum ReplacementDirection {
    down,
    forward,
}
