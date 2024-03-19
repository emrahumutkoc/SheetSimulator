using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour {
    public GameObject player;
    public List<GameObject> moveToOtherSceneObjects = new List<GameObject>();

    public void TeleportPlayerToNextScene() {
        Debug.Log("Teleport fired");
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Map1") {


            // StartCoroutine(LoadSceneAsync("Map2"));
            Scene nextScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("Map2");

            /*if (nextScene.IsValid()) {
                foreach (GameObject obj in moveToOtherSceneObjects) {
                    UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(obj, nextScene);
                }
            }*/
            // Time.timeScale = 0;
            UnityEngine.SceneManagement.SceneManager.LoadScene(1, LoadSceneMode.Additive);
            // Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

            // Debug.Log("active scene" + scene.name);
            player.transform.position = new Vector3(76.8560028f, 1.41199994f, 7.96000004f);
            //UnityEngine.SceneManagement.SceneManager.SetActiveScene(nextScene);
        }
    }

    IEnumerator LoadSceneAsync(string sceneName) {
        // Asenkron sahne yüklemesini baþlat
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Yükleme tamamlanana kadar bekle
        while (!asyncLoad.isDone) {
            yield return null;
        }

        // Yükleme tamamlandýktan sonra sahneyi al
        Scene loadedScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);

        // Hedef sahneye objeleri taþý
        if (loadedScene.IsValid()) {
            foreach (GameObject obj in moveToOtherSceneObjects) {
                UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(obj, loadedScene);
            }
            // Yüklenen sahneyi aktif sahne olarak ayarla
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(loadedScene);

            // Player'ýn pozisyonunu ayarla
            player.transform.position = new Vector3(76.8560028f, 1.41199994f, 7.96000004f);
        } else {
            Debug.LogError("Yüklenen sahne geçerli deðil: " + sceneName);
        }
    }
}
