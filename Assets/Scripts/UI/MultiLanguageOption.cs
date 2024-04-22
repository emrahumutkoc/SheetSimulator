using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SimpleLocalization.Scripts;

public class MultiLanguageOption : MonoBehaviour {
    [SerializeField] private TMPro.TMP_Dropdown languageOptionDropdown;

    private void Awake() {        
        LocalizationManager.Read();

       /* switch (Application.systemLanguage) {
            case SystemLanguage.English:
                LocalizationManager.Language = "English";
                languageOptionDropdown.value = 0;
                break;
            case SystemLanguage.Turkish:
                LocalizationManager.Language = "Turkish";
                languageOptionDropdown.value = 1;
                break;
            default:
                LocalizationManager.Language = "English";
                languageOptionDropdown.value = 0;
                break;
        }*/

        int selectedIndex = PlayerPrefs.GetInt("SelectedLanguage");
        // DebugConsole.instance.Log("seleected index ");
        languageOptionDropdown.value = selectedIndex;
        SetLanguage(selectedIndex);
    }

    public void SetLanguage(int selectedLanguageIndex) {
        PlayerPrefs.SetInt("SelectedLanguage", selectedLanguageIndex);
        switch (selectedLanguageIndex) {
            case 0:
                LocalizationManager.Language = "English";
                break;
            case 1:
                LocalizationManager.Language = "Turkish";
                break;
            default:
                LocalizationManager.Language = "English";
                break;
        }
    }

}
