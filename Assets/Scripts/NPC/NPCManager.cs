using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour {
    [SerializeField] private List<Transform> npcSpawnPoints;
    [SerializeField] private List<GameObject> npcPrefabs;
    [SerializeField] private Transform pointToGo;

    [SerializeField] private int spawnNpcTime = 5;

    private void Start() {
        StartCoroutine(SpawnNPC());
    }

    private IEnumerator SpawnNPC() {

        while (true) {
            WaitForSeconds wait = new WaitForSeconds(spawnNpcTime);

            int randomNpcIndex = Random.Range(0, npcPrefabs.Count);
            GameObject npcToSpawn = npcPrefabs[randomNpcIndex];

            int randomSpawnPointIndex = Random.Range(0, npcSpawnPoints.Count);
            Transform pointToSpawn = npcSpawnPoints[randomSpawnPointIndex];

            GameObject npc = Instantiate(npcToSpawn, pointToSpawn.position, Quaternion.identity);
            if(npc.TryGetComponent(out NPCController npcController)) {
                npcController.SetDestination(pointToGo.position);
            }
            yield return wait;
        }
    }
}
