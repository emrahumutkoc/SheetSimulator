using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizantalGrabbable: MonoBehaviour {

    public bool isGrabbing = false;
    [Header("Ghost Settings")]
    [SerializeField] private Material ghostMaterialValid;
    [SerializeField] private Material ghostMaterialInvalid;

    [Header("Pick Up Settings")]
    [SerializeField] private Transform cameraPositionTransform;
    [SerializeField] private Transform offsetToPlacementPoint;
    [SerializeField] private LayerMask objectHoverIgnoredLayers;
    [SerializeField] private LayerMask objectPlacementLayers;
    [SerializeField] private LayerMask ignoredLayers;
    [SerializeField] private float lerpSpeed = 2f;
    [SerializeField] private float pickUpDistance = 5f;

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
            Collider collider = GetComponent<Collider>();
            float height = collider.bounds.size.y / 2;
            Vector3 offsetToPlacementPoint = transform.position - placementPoint.position;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 10f, objectPlacementLayers)) {
                //Debug.Log("tam olarak ordayým");
                Vector3 placementPosition = hit.point + offsetToPlacementPoint;
                // Vector3 currentEulerAngles = gameObject.transform.rotation.eulerAngles;
                // Vector3 newEulerAngles = new Vector3(currentEulerAngles.x, placementPoint.transform.rotation.eulerAngles.y, currentEulerAngles.z);
                // Quaternion newRotation = Quaternion.Euler(newEulerAngles);
                // gameObject.transform.rotation = newRotation;
                Vector3 newPositionLerp = Vector3.Lerp(transform.position, placementPosition, Time.fixedDeltaTime * lerpSpeed);
                rb.MovePosition(newPositionLerp);
            } else {
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, pickUpDistance, ~objectHoverIgnoredLayers)) {
                    Vector3 placementPosition = hitInfo.point + offsetToPlacementPoint;
                    Vector3 newPositionLerp = Vector3.Lerp(transform.position, placementPosition, Time.fixedDeltaTime * lerpSpeed);
                    rb.MovePosition(newPositionLerp);
                } else {
                    Vector3 forwardPosition = Camera.main.transform.position + Camera.main.transform.forward * pickUpDistance;

                    Vector3 placementPosition = forwardPosition + offsetToPlacementPoint;

                    Vector3 newPositionLerp = Vector3.Lerp(transform.position, placementPosition, Time.fixedDeltaTime * lerpSpeed);
                    rb.MovePosition(newPositionLerp);
                }
            }

            Debug.Log("Camera.main.transform.rotation" + Camera.main.transform.rotation);

            // Vector3 directionOfRay = direction == ReplacementDirection.down ? Vector3.down : gameObject.transform.forward;


            /*Quaternion targetRotation = Quaternion.LookRotation(newPlacementPosition);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, targetRotation, Time.fixedDeltaTime * lerpSpeed);
*/
            if (connectionPoints != null) {
                bool allHitTargetLayer = true;

                foreach (Transform child in connectionPoints.transform) {
                    RaycastHit pointHit;
                    if (Physics.Raycast(child.position, child.transform.forward, out pointHit, 0.2f, objectPlacementLayers)) {
                        //Debug.Log(child.name + " çarptýðý obje: " + pointHit.collider.gameObject.name + ", Mesafe: " + pointHit.distance + "layer" + pointHit.transform.gameObject.layer);
                        Debug.DrawRay(child.position, child.transform.forward * pointHit.distance, Color.green);
                    } else {
                        Physics.Raycast(child.position, child.transform.forward, out pointHit, 2f);
                        // Debug.Log(child.name + " hedef layer ile çarpýþmadý." + "ÇARPTIÐI NESNE: " + pointHit.transform.gameObject.name);
                        Debug.DrawRay(child.position, child.transform.forward * 100, Color.red);
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
            //   Debug.Log("exit" + collider.gameObject.layer);
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