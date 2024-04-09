using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHover : MonoBehaviour {

    [SerializeField] private Color hoverColor = Color.red;
    [SerializeField, Range(0f, 10f)]
    private float outlineWidth = 2f;
    private Outline outline;

    private void Awake() {
        outline = gameObject.GetComponent<Outline>();
        if (outline == null) {
            outline = gameObject.AddComponent<Outline>();
        }
        outline.OutlineColor = hoverColor;
        outline.OutlineWidth = outlineWidth;
        outline.enabled = false;
    }
}