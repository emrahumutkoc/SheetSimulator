using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void Play() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        DebugConsole.instance.Log("testtt");
    }

    private void Start() {

        int selectedIndex = PlayerPrefs.GetInt("SelectedLanguage");
        DebugConsole.instance.Log("seleected index " + selectedIndex);
    }

    public void LoadOptionScene() {
        OptionManager optionManager = GameObject.Find("OptionsManager").GetComponent<OptionManager>();

        optionManager.ChangeOptionMenuVisibility(true);

    }

    public void Quit() {
        Application.Quit();
    }
}
