using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour {
    private string objectID;

    private void Awake() {
        objectID = name + transform.position.ToString();
    }
    private void Start() {
        DontDestroy[] dontDestoryObjects = Object.FindObjectsOfType<DontDestroy>();

        for (int i = 0; i < dontDestoryObjects.Length; i++) {
            if (dontDestoryObjects[i] != this) {
                if (dontDestoryObjects[i].objectID == objectID) {
                    Destroy(gameObject);
                }
            }

        }
        DontDestroyOnLoad(gameObject);
    }
}
