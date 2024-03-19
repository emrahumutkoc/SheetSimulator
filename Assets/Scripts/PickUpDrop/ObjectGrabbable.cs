using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour {

    public bool isGrabbing = false;
    [Header("Ghost Settings")]
    [SerializeField] private Material ghostMaterialValid;
    [SerializeField] private Material ghostMaterialInvalid;

    [Header("Pick Up Settings")]
    [SerializeField] private Transform cameraPositionTransform;
    [SerializeField] private LayerMask objectHoverLayers;
    [SerializeField] private LayerMask objectPlacementLayers;
    [SerializeField] private LayerMask ignoredLayers;
    [SerializeField] private float lerpSpeed = 2f;
    [SerializeField] private float pickUpDistance = 5f;

    [SerializeField] private GameObject connectionPoint;


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
            transform.position = newPlacementPosition;
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
            Collider collider = GetComponent<Collider>();
            float height = collider.bounds.size.y / 2;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, pickUpDistance, objectHoverLayers)) {
                Vector3 newPosition = new Vector3(hit.point.x, hit.point.y + height, hit.point.z);
                newPlacementPosition = newPosition;
                Vector3 newPositionLerp = Vector3.Lerp(transform.position, newPosition, Time.fixedDeltaTime * lerpSpeed);
                rb.MovePosition(newPositionLerp);
            } else {
                Vector3 forwardPosition = Camera.main.transform.position + Camera.main.transform.forward * pickUpDistance;
                int layerMask = 1 << LayerMask.NameToLayer("Grabbable");
                layerMask = ~layerMask;
                if (Physics.Raycast(forwardPosition, Vector3.down, out RaycastHit hitInfo, pickUpDistance, layerMask)) {

                    Vector3 hitPoint = new Vector3(hitInfo.point.x, hitInfo.point.y + height, hitInfo.point.z);
                    newPlacementPosition = hitPoint;
                    hitPoint.y = hitPoint.y + 0.1f;
                    Vector3 newPositionLerp = Vector3.Lerp(transform.position, hitPoint, Time.fixedDeltaTime * lerpSpeed);
                    rb.MovePosition(newPositionLerp);
                }
            }
            if (connectionPoint != null) {
                bool allHitTargetLayer = true;

                foreach (Transform child in connectionPoint.transform) {
                    RaycastHit pointHit;
                    if (Physics.Raycast(child.position, Vector3.down, out pointHit, 2f, objectPlacementLayers)) {
                        Debug.Log(child.name + " çarptýðý obje: " + pointHit.collider.gameObject.name + ", Mesafe: " + pointHit.distance);
                        Debug.DrawRay(child.position, Vector3.down * pointHit.distance, Color.green);
                    } else {
                        Physics.Raycast(child.position, Vector3.down, out pointHit, 2f);
                        Debug.Log(child.name + " hedef layer ile çarpýþmadý." + "ÇARPTIÐI NESNE: " + pointHit.transform.gameObject.name);
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
            Debug.Log("exit" + collider.gameObject.layer);
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
