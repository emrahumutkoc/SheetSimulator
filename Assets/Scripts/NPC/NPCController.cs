using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour {
    private NavMeshAgent agent;

    private bool isAgentNavigating = false;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        /*if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                agent.SetDestination(hit.point);
            }
        }*/
    }


    public void SetDestination(Vector3 position) {

        if (Physics.Raycast(position,Vector3.down, out RaycastHit hit)) {
            agent.SetDestination(hit.point);
        }
    }
}
