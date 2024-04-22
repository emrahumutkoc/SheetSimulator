using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour {
    private NavMeshAgent agent;

    private bool isAgentNavigating = false;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.X)) {
            isAgentNavigating = !isAgentNavigating;
        }   

        if (isAgentNavigating) {

        }
    }
}
