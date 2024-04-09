using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimiter : MonoBehaviour {
    public enum FPSLimitEnum {
        noLimit = 0,
        limit30 = 30,
        limit60 = 60,
        limit120 = 120,
        limit240 = 240,
        limit360 = 360
    }

    [SerializeField] private FPSLimitEnum limit = FPSLimitEnum.limit240;


    private void Start() {
        QualitySettings.vSyncCount = 0;
    }
    private void Awake() {
        Application.targetFrameRate = (int)limit;
    }
}
