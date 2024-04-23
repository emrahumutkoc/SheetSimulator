using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagementManager : MonoBehaviour {
    [SerializeField] private GameObject managementUI;

    public void CloseManagementUI() {
        managementUI.SetActive(false);
    }
}
