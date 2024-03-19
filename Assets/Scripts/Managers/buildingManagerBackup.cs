using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingManagerBackup : MonoBehaviour {
    [Header("Build Objects")]
    [SerializeField] private List<GameObject> floorObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> wallObjects = new List<GameObject>();

    [Header("Build Settings")]
    [SerializeField] private SelectedBuildType currentBuildType;
    [SerializeField] private LayerMask connectorLayer;

    [Header("Destroy Settings")]
    [SerializeField] private bool isDestroying = false;
    private Transform lastHitDestroyTransform;
    private List<Material> LastHitMaterials = new List<Material>();

    [Header("Ghost Settings")]
    [SerializeField] private Material ghostMaterialValid;
    [SerializeField] private Material ghostMaterialInvalid;
    [SerializeField] private float connectorOverlapRadius = 1;
    [SerializeField] private float maxGroundAngle = 45f;

    [Header("Internal State")]
    [SerializeField] private bool isBuilding = false;
    [SerializeField] private int currentBuildingIndex;
    private GameObject ghostBuildGameobject;
    private bool isGhostInValidPosition = false;
    private Transform ModelParent = null;

    [Header("UI")]
    [SerializeField] private GameObject buildingUI;
    [SerializeField] private TMP_Text destoryText;


    private void Update() {
        /*if (Input.GetKeyDown(KeyCode.B)) {
            isBuilding = !isBuilding;
            if(isDestroying) {
                isDestroying = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.V)) {
            isDestroying= !isDestroying;
            if (isBuilding) {
                isBuilding = false;
            }
        }*/

        if (Input.GetKeyDown(KeyCode.Tab)) {
            ToggleBuildingUI(!buildingUI.activeInHierarchy);
        }

        if (isBuilding && !isDestroying) {
            GhostBuild();

            if (Input.GetMouseButtonDown(0)) {
                PlaceBuild();
            }
        } else if(ghostBuildGameobject) {
            Destroy(ghostBuildGameobject);
            ghostBuildGameobject = null;
        }

        if (isDestroying) {
            GhostDestroy();

            if(Input.GetMouseButtonDown(0)) {
                DestroyBuild();
            }
        }
    }

    private void GhostBuild() {
        GameObject currentBuild = GetCurrentBuild();
        CreateGhostPrefab(currentBuild);

        MoveGhostPrefabToRaycast();
        CheckBuildValidity();
    }
    private void CreateGhostPrefab(GameObject currentBuild) {
        if (ghostBuildGameobject == null) {
            ghostBuildGameobject = Instantiate(currentBuild);

            ModelParent = ghostBuildGameobject.transform.GetChild(0);

            GhostifyModel(ModelParent, ghostMaterialValid);
            GhostifyModel(ghostBuildGameobject.transform);
        }
    }

    private void MoveGhostPrefabToRaycast() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            ghostBuildGameobject.transform.position = hit.point;
        }
    }

    private void CheckBuildValidity() {
        Collider[] colliders = Physics.OverlapSphere(ghostBuildGameobject.transform.position, connectorOverlapRadius, connectorLayer);
        if (colliders.Length > 0) {
            GhostConnectBuild(colliders);
        } else {
            GhostSeperateBuild();

            if (isGhostInValidPosition) {
                Collider[] overlapColliders = Physics.OverlapBox(ghostBuildGameobject.transform.position, new Vector3(2f, 2f, 2f), ghostBuildGameobject.transform.rotation);

                foreach(Collider overlapCollider in overlapColliders) {
                    if (overlapCollider.gameObject != ghostBuildGameobject && overlapCollider.transform.root.CompareTag("Buildables")) {
                        GhostifyModel(ModelParent, ghostMaterialInvalid);
                        isGhostInValidPosition = false;
                        return;
                    }
                }
            }
        }
    }

    private void GhostConnectBuild(Collider[] colliders) {
        Connector bestConnector = null;

        foreach(Collider collider in colliders) {
            Connector connector = collider.GetComponent<Connector>();
            if (connector.canConnectTo) {
                bestConnector = connector;
                break;
            }
        }

        if (bestConnector == null || currentBuildType == SelectedBuildType.floor && bestConnector.isConnectedToFloor || currentBuildType == SelectedBuildType.wall && bestConnector.isConnectedToWall) {
            GhostifyModel(ModelParent, ghostMaterialInvalid);
            isGhostInValidPosition = false;
            return;
        }

        SnapGhostPrefabToConnector(bestConnector);
    }

    private void SnapGhostPrefabToConnector(Connector connector) {
        Transform ghostConnector = FindSnapConnector(connector.transform, ghostBuildGameobject.transform.GetChild(1));
        ghostBuildGameobject.transform.position = connector.transform.position - (ghostConnector.position - ghostBuildGameobject.transform.position);

        if (currentBuildType == SelectedBuildType.wall) {
            Quaternion newRotation = ghostBuildGameobject.transform.rotation;
            newRotation.eulerAngles = new Vector3(newRotation.eulerAngles.x, connector.transform.rotation.eulerAngles.y, newRotation.eulerAngles.z);
            ghostBuildGameobject.transform.rotation = newRotation;
        }

        GhostifyModel(ModelParent, ghostMaterialValid);
        isGhostInValidPosition = true;
    }
    
    private void GhostSeperateBuild() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            if (currentBuildType == SelectedBuildType.wall) {
                GhostifyModel(ModelParent, ghostMaterialInvalid);
                isGhostInValidPosition = false;
                return;
            }

            if (Vector2.Angle(hit.normal, Vector3.up) < maxGroundAngle) {
                GhostifyModel(ModelParent, ghostMaterialValid);
                isGhostInValidPosition = true;
            } else {
                GhostifyModel(ModelParent, ghostMaterialInvalid);
                isGhostInValidPosition = false;
            }
        }
    }

    private Transform FindSnapConnector(Transform snapConnector, Transform ghostConnectorParent) {
        ConnectorPosition OppositeConnectorTag = GetOppositePosition(snapConnector.GetComponent<Connector>());

        foreach(Connector connector in ghostConnectorParent.GetComponentsInChildren<Connector>()) {
            if (connector.connectorPosition == OppositeConnectorTag) {
                return connector.transform;
            }

        }

        return null;
    }

    private ConnectorPosition GetOppositePosition(Connector connector) {
        ConnectorPosition position = connector.connectorPosition;

        if (currentBuildType == SelectedBuildType.wall && connector.connectorParentType == SelectedBuildType.floor) {
            return ConnectorPosition.bottom;
        }

        if (currentBuildType == SelectedBuildType.floor && connector.connectorParentType == SelectedBuildType.wall && connector.connectorPosition == ConnectorPosition.top) {
            if (connector.transform.root.rotation.y == 0) {
                return GetConnectorClosesToPlayer(true);
            } else {
                return GetConnectorClosesToPlayer(false);
            }
        }

        switch (position) {
            case ConnectorPosition.left:
                return ConnectorPosition.right;
            case ConnectorPosition.right:
                return ConnectorPosition.left;            
            case ConnectorPosition.bottom:
                return ConnectorPosition.top;          
            case ConnectorPosition.top:
                return ConnectorPosition.bottom;
            default:
                return ConnectorPosition.bottom;
        }
    }

    private ConnectorPosition GetConnectorClosesToPlayer(bool topBottom) {
        Transform cameraTransform = Camera.main.transform;

        if (topBottom) {
            return cameraTransform.position.z >= ghostBuildGameobject.transform.position.z ? ConnectorPosition.bottom : ConnectorPosition.top;
        } else {
            return cameraTransform.position.x >= ghostBuildGameobject.transform.position.x ? ConnectorPosition.left : ConnectorPosition.right;
        }
    }

    private void GhostifyModel(Transform modelParent, Material ghostMaterial = null) {
        if (ghostMaterial != null) {
            foreach(MeshRenderer meshRenderer in modelParent.GetComponentsInChildren<MeshRenderer>()) {
                meshRenderer.material = ghostMaterial;
            }
        } else {
            foreach(Collider modelColliders in modelParent.GetComponentsInChildren<Collider>()) {
                modelColliders.enabled = false;
            }
        }
    }

    private GameObject GetCurrentBuild() {
        switch (currentBuildType) {
            case SelectedBuildType.floor:
                return floorObjects[currentBuildingIndex];
            case SelectedBuildType.wall:
                return wallObjects[currentBuildingIndex];
        }

        return null;
    }

    private void PlaceBuild() {
        if (ghostBuildGameobject != null && isGhostInValidPosition) {
            GameObject newBuild = Instantiate(GetCurrentBuild(), ghostBuildGameobject.transform.position, ghostBuildGameobject.transform.rotation);
            Destroy(ghostBuildGameobject);
            ghostBuildGameobject = null;

            // isBuilding = false;
            foreach(Connector connector in newBuild.GetComponentsInChildren<Connector>()) {
                connector.UpdateConnectors();
            }
        }
    }

    private void GhostDestroy() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            if (hit.transform.root.CompareTag("Buildables")) {
                if (!lastHitDestroyTransform) {
                    lastHitDestroyTransform = hit.transform.root;

                    LastHitMaterials.Clear();


                    foreach(MeshRenderer lastHitMeshRenderers in lastHitDestroyTransform.GetComponentsInChildren<MeshRenderer>()) {
                        LastHitMaterials.Add(lastHitMeshRenderers.material);
                    }

                    GhostifyModel(lastHitDestroyTransform.GetChild(0), ghostMaterialInvalid);
                } else if (hit.transform.root != lastHitDestroyTransform) {
                    ResetLastHitDestroyTransform();
                }
            } else if (lastHitDestroyTransform) {
                ResetLastHitDestroyTransform();
            }
        }
    }

    private void ResetLastHitDestroyTransform() {
        int counter = 0;
        foreach (MeshRenderer lastHitMeshRenderers in lastHitDestroyTransform.GetComponentsInChildren<MeshRenderer>()) {
            lastHitMeshRenderers.material = LastHitMaterials[counter];
            counter++;
        }

        lastHitDestroyTransform = null;
    }

    private void DestroyBuild() {
        if (lastHitDestroyTransform) {
            foreach(Connector connector in lastHitDestroyTransform.GetComponentsInParent<Connector>()) {
                connector.gameObject.SetActive(false);
                connector.UpdateConnectors(true);
            }
            Destroy(lastHitDestroyTransform.gameObject);

            // DestoryBuildingToogle(true);
            lastHitDestroyTransform = null;
        }
    }

    public void ToggleBuildingUI (bool active) {
        isBuilding = false;
        buildingUI.SetActive(active);

        Cursor.visible = active;
        Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void DestoryBuildingToogle(bool fromScript) {
        if (fromScript) {
            isDestroying = false;
            destoryText.text = "Destroy Off";
            destoryText.color = Color.green;
        } else {
            isDestroying = !isDestroying;
            destoryText.text = isDestroying ? "Destroy On" : "Distory Off";
            destoryText.color = isDestroying ? Color.red : Color.green;
            ToggleBuildingUI(false);
        }
    }

    public void ChangeBuildTypeButton(string selectedBuildType)  {
        if (System.Enum.TryParse(selectedBuildType, out SelectedBuildType result)) {
            currentBuildType = result;
        } else {
            Debug.Log("Build type doesn't exist");
        }
    }

    public void StartBuildingButton(int buildIndex) {
        currentBuildingIndex = buildIndex;

        ToggleBuildingUI(false);
        isBuilding = true;
    }
}

[System.Serializable]
public enum SelectedBuildTypeBackup {
    floor,
    wall,
}