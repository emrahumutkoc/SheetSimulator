using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {

    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] GameObject spawnPointsBody;
    [SerializeField] private Transform cameraPositionTransform;

    private List<CartItem> cart = new List<CartItem>();
    

    public void StartDeliveryProcess(List<CartItem> newCart) {
        cart = newCart;
        List<SpawnPoint> availablePoints = GetAvailableSlots();
        for(int i = 0; i < newCart.Count; i++) {
            CartItem item = newCart[i];
            for (int j = 0; j < item.quantity; j++) {
                SpawnPoint spawnPoint = availablePoints[0];
                Transform point = spawnPoint.GetSpawnPointTransform();
                availablePoints.RemoveAt(0);
                GameObject newObject = Instantiate(item.product.productPrefab, point.position, point.rotation);
                VerticalGrabbable vg = newObject.GetComponent<VerticalGrabbable>();
                if (vg != null) {
                    vg.SetCameraTransform(cameraPositionTransform);
                }
            }
            
        }
        
    }

    private List<SpawnPoint> GetAvailableSlots() {
        List<SpawnPoint> availablePoints = new List<SpawnPoint>();
        foreach (Transform child in spawnPointsBody.transform) {
            SpawnPoint point = child.GetComponentInChildren<SpawnPoint>();

            if (point.isPointAvailable()) {
                availablePoints.Add(point);
            }
        }
        return availablePoints;
    }

    public int GetCountAvailableSlots() {
        return GetAvailableSlots().Count;
    }
}
