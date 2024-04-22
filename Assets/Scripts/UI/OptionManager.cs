using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class OptionManager : MonoBehaviour {
    [SerializeField] private GameObject optionMenuCanvas;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Back();
        }
    }

    public void SetSensitivity() {

    }

    public void SaveOption() {
        Debug.Log("Save Options");
    }

    public void ChangeOptionMenuVisibility(bool active) {
        optionMenuCanvas.SetActive(active);
    }

    public void Back() {
        // Unload option scene
        /*        Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("1_OptionsMenu");

                if (scene.isLoaded && optionMenuCanvas.activeSelf) {
                    optionMenuCanvas.SetActive(false);
                }*/

        ChangeOptionMenuVisibility(false);

    }
}
