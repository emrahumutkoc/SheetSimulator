using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscMenuManager : MonoBehaviour {

    [SerializeField] private GameObject escMenuGameObject;
    [SerializeField] private PlayerCam playerCam;
    [SerializeField] private ComputerManager computerManager; 
    private void Awake() {
        if (escMenuGameObject.activeSelf) {
            escMenuGameObject.SetActive(false);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            escMenuGameObject.SetActive(!escMenuGameObject.activeSelf);
            if (escMenuGameObject.activeSelf) {
                playerCam.SetCamLocked();
                Time.timeScale = 0;
            } else {
                if (!computerManager.GetIsZoomed()) {
                    playerCam.SetCamFree();
                }
                Time.timeScale = 1;
            }
        }        
    }

    public void Continue() {
        escMenuGameObject.SetActive(false);
        playerCam.SetCamFree();
        Time.timeScale = 1;
    }

    public void Options() {
        OptionManager optionManager = GameObject.Find("OptionsManager").GetComponent<OptionManager>();

        optionManager.ChangeOptionMenuVisibility(true);
    }

    public void GoToMainMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Quit() {
        Application.Quit();
    }
}
