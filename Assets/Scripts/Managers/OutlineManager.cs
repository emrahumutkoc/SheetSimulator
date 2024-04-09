using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineManager : MonoBehaviour {
    [SerializeField] private float outlineDistance = 5f;
    [SerializeField] private LayerMask outlineLayers;
    [SerializeField] private Color outlineColor = Color.red;
    [SerializeField] private Transform cameraTransform;
    [SerializeField, Range(0f, 10f)] private float outlineWidth = 2f;
    private GameObject lastHoveredObject;


    private void FixedUpdate() {
        // Ray ray = mainCamera.ScreenPointToRay(mainCamera.transform.forward);

        if (Physics.Raycast(cameraTransform.transform.position, cameraTransform.transform.forward, out RaycastHit hit, outlineDistance, outlineLayers)) {
            GameObject hoveredObject = hit.collider.gameObject;

            if (lastHoveredObject != hoveredObject) {
                if (lastHoveredObject != null) {
                    // Eski nesneyi devre dýþý býrak
                    lastHoveredObject.GetComponent<Outline>().enabled = false;
                }

                if (hoveredObject.TryGetComponent(out Outline outline)) {
                    outline.enabled = true;
                } else {
                    Outline newOutline = hoveredObject.gameObject.AddComponent<Outline>();
                    newOutline.enabled = true;
                    newOutline.OutlineColor = outlineColor;
                    newOutline.OutlineWidth = outlineWidth;
                }

                lastHoveredObject = hoveredObject;
            }
        } else if (lastHoveredObject != null) {
            // Hiçbir nesne üzerinde deðilse, son nesneyi devre dýþý býrak
            lastHoveredObject.GetComponent<Outline>().enabled = false;
            lastHoveredObject = null;
        }
        /*if (Physics.Raycast(cameraPos.position, cameraPos.forward, out RaycastHit hit, outlineDistance, outlineLayers)) {
            GameObject hoveredObject = hit.collider.gameObject;
            if (lastOutline != hoveredObject) {

            }
                if (hit.transform.gameObject.TryGetComponent(out Outline outline)) {
                lastOutline = outline;
            } else {
                lastOutline = gameObject.AddComponent<Outline>();
                lastOutline.OutlineColor = hoverColor;
                lastOutline.OutlineWidth = outlineWidth;
                lastOutline.enabled = false;
            }

            lastOutline.enabled = !lastOutline.enabled;
        }*/
    }
}
