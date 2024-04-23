using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerManager : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerPlacementPosition;
    [SerializeField] private Transform screenLookAtPosition;
    [SerializeField] private Transform camPos;
    [SerializeField] private Transform orientation;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerCam playerCam;
    [SerializeField] private GameObject escMenuObject;
    private bool isZooming = false;
    private bool isZoomed = false;
    private Coroutine currentZoomCoroutine;

    private void Update() {
        if (isZoomed && Input.GetMouseButtonDown(1)) {
            playerCam.SetTempFov();
            if (!isZooming && currentZoomCoroutine != null) {
                StopCoroutine(currentZoomCoroutine);
                isZooming = false; // Zoom durumunu güncelle
                currentZoomCoroutine = StartCoroutine(ZoomOutToInitial()); 
            }
        }
    }

    private void OnMouseDown() {
        if (!isZoomed && !escMenuObject.activeSelf) {
            SetPlayerPosition();
            playerCam.SetDefaultFOV();

            if (!isZooming) {
                mainCamera.transform.rotation = Quaternion.LookRotation(screenLookAtPosition.forward * -1f);
                currentZoomCoroutine = StartCoroutine(ZoomToTarget());

            }
        }
    }

    IEnumerator ZoomOutToInitial() {
        isZooming = true;
        // Vector3 endpos = screenLookAtPosition.position - (screenLookAtPosition.position - mainCamera.transform.position).normalized * 1f; // Hedef pozisyon
        yield return currentZoomCoroutine = StartCoroutine(MoveToPosition(camPos.transform.position, 0.5f)); // Hedefe zoom yap
        mainCamera.transform.rotation = Quaternion.LookRotation(screenLookAtPosition.forward * -1f);
        player.GetComponent<PlayerMovement>().SetLockModeOff();
        mainCamera.GetComponent<PlayerCam>().SetCamFreeWithoutRotation();
        isZooming = false;
        isZoomed = false;
        SetPlayerPosition();
    }

    IEnumerator ZoomToTarget() {
        isZooming = true;
        // Vector3 endpos = screenLookAtPosition.position - (screenLookAtPosition.position - mainCamera.transform.position).normalized * 1f; // Hedef pozisyon
        mainCamera.GetComponent<PlayerCam>().SetCamLocked();
        player.GetComponent<PlayerMovement>().SetLockModeOn();

        SetPlayerPosition();
        yield return currentZoomCoroutine = StartCoroutine(MoveToPosition(screenLookAtPosition.transform.position, 0.5f));
        isZooming = false;
        isZoomed = true;
    }

    IEnumerator MoveToPosition(Vector3 targetPosition, float journeyTime) {
        float elapsedTime = 0f;
        Vector3 startingPos = mainCamera.transform.position;

        while (elapsedTime < journeyTime) {
            mainCamera.transform.position = Vector3.Lerp(startingPos, targetPosition, (elapsedTime / journeyTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = targetPosition;
    }

    private void SetPlayerPosition() {
        player.position = new Vector3(playerPlacementPosition.position.x, 1, playerPlacementPosition.position.z);
    }

    public bool GetIsZoomed() {
        return isZoomed;
    }
}
