using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class QualityManager : MonoBehaviour {
    [SerializeField] private Dropdown qualityDropdown;
    private void Awake() {
        int savedQuality = PlayerPrefs.GetInt("qualityLevel", 4);
        qualityDropdown.value = savedQuality;
        SetQuality(savedQuality);
    }
    public void SetQuality(int selectedIndex) {
        PlayerPrefs.SetInt("qualityLevel", selectedIndex);
        QualitySettings.SetQualityLevel(selectedIndex);
    }
}
