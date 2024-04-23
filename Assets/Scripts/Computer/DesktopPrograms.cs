using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopPrograms : MonoBehaviour {
    
    [SerializeField] private GameObject marketUI;
    [SerializeField] private GameObject managementUI;
    [SerializeField] private GameObject bankUI;

    public void OpenMarketMenu() {
        marketUI.SetActive(!marketUI.activeSelf);
    }

    public void OpenManagementMenu() {
        managementUI.SetActive(!managementUI.activeSelf);
    }

    public void OpenBankMenu() {
        bankUI.SetActive(!bankUI.activeSelf);
    }
}
