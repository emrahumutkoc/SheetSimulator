 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DebugConsole : MonoBehaviour {

    public static DebugConsole instance;
    [SerializeField] private RectTransform displayRect;
    [SerializeField] private Text displayText;

    private float initHeight;

    private void Awake() {
        if (DebugConsole.instance != null) {
            DestroyImmediate(gameObject);
        } else {
            DebugConsole.instance = this;
        }

        initHeight = displayRect.anchoredPosition.y;
    }

    public void ChangeDisplayPosition(float newPos) {
        displayRect.anchoredPosition = new Vector2(displayRect.anchoredPosition.x, initHeight + newPos);
    }

    public void Log(string newLog) {
        displayText.text = newLog + "\n" + displayText.text;
    }

}
