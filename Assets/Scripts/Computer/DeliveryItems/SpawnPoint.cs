using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

    [SerializeField] private Transform point;
    private Collider col;
    private void Awake() {
        col = GetComponent<Collider>();
        col.enabled = false;

    }
    public bool isPointAvailable() {
        col.enabled = true;
        Collider[] colliders = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation);
        col.enabled = false;
        foreach (Collider col2 in colliders) {
            if (col != col2) {
                return false;
            }
        }
        return true;
    }

    public Transform GetSpawnPointTransform() {
        return point;
    }
}
