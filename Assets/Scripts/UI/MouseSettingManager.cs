using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSettingManager : MonoBehaviour {
    [SerializeField] private Slider sensSlider;
    [SerializeField] private Toggle xAxisSlider;
    [SerializeField] private Toggle yAxisSlider;
    [SerializeField] private Text sensText;
    private void Awake() {
        float savedSens = PlayerPrefs.GetFloat("Sensitivity", 1f);
        sensSlider.value = savedSens;
        sensText.text = (Mathf.Round(savedSens * 10) / 10.0).ToString();
        xAxisSlider.isOn = PlayerPrefs.GetInt("invertX", 0) == 1;
        yAxisSlider.isOn = PlayerPrefs.GetInt("invertY", 0) == 1;
    }
    public void SetSensitivity(float sensitivity) {
        GetPlayerCamFromScene()?.SetSensitivity(sensitivity);
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        sensText.text = (Mathf.Round(sensitivity * 10) / 10.0).ToString();
    }

    public void InvertXAxis(bool invert) {
        GetPlayerCamFromScene()?.InvertXAxis(invert);
        PlayerPrefs.SetInt("invertX", invert ? 1 : 0);
    }

    public void InvertYAxis(bool invert) {
        GetPlayerCamFromScene()?.InvertYAxis(invert);
        PlayerPrefs.SetInt("invertY", invert ? 1 : 0);
    }

    public PlayerCam GetPlayerCamFromScene() {
        GameObject camObject = GameObject.Find("PlayerCamera");
        if (camObject != null && camObject.TryGetComponent(out PlayerCam playerCam)) {
            return playerCam;
        }
        return null;
    }
}
