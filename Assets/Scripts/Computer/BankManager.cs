using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankManager : MonoBehaviour {
    [SerializeField] private GameObject bankUI;

    public void CloseBankUI() {
        bankUI.SetActive(false);
    }
}
