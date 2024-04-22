using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FOVManager : MonoBehaviour {
    [SerializeField] private Slider fovSlider;
    [SerializeField] private Text textValue;

    private void Awake() {
        float val = PlayerPrefs.GetFloat("FOV", 80f);
        fovSlider.value = val;
        textValue.text = ((int) val).ToString(); 
    }
    public void SetFOV(float value) {
        
        GameObject camObject = GameObject.Find("PlayerCamera");
        if (camObject != null && camObject.TryGetComponent(out PlayerCam playerCam)) {
            playerCam.DoFov((int) value);
        }
        textValue.text = ((int) value).ToString();
        PlayerPrefs.SetFloat("FOV", value);
    }
}
