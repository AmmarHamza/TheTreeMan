using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class NPCManager : MonoBehaviour
{


    [SerializeField] private NPCSO npcSO;

    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<Transform> stopPoints;

    private int npcPrefabIndex = 0;

    private float spawnTimer = 0f;
    private float spawnTimerMax = 2f;

    public static int npcCounter = 0;
    private const int totalNPCs = 50;
    private GameObject[] npcs = new GameObject[totalNPCs];


    private void Awake()
    {
        CreateNPCPool();
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnTimerMax)
        {
            spawnTimer = 0f;
            SpawnNPC();
        }
    }

    private void SpawnNPC()
    {
        if(npcCounter < totalNPCs)
        {
            for (int i = 0; i < totalNPCs; i++)
            {
                if (!npcs[i].activeInHierarchy)
                {
                    int spawnPointIndex = Random.Range(0, spawnPoints.Count);
                    Transform spawnPoint = spawnPoints[spawnPointIndex];

                    npcs[i].transform.position = spawnPoint.position;

                    npcs[i].SetActive(true);

                    int destinationIndex = Random.Range(0, spawnPoints.Count);
                    while (destinationIndex == spawnPointIndex)
                    {
                        destinationIndex = Random.Range(0, spawnPoints.Count);
                    }
                    Vector3 destinationPosition = spawnPoints[destinationIndex].position;

                    npcs[i].GetComponent<NPC>().SetDestination(destinationPosition);

                    Vector3[] singleAgentStopPoints = new Vector3[NPC.GetNumberOfStopPoints()];

                    for (int j = 0; j < 3; j++)
                    {
                        int stopPointIndex = Random.Range(0, stopPoints.Count);
                        singleAgentStopPoints[j] = stopPoints[stopPointIndex].position;
                    }

                    npcs[i].GetComponent<NPC>().SetStopPoints(singleAgentStopPoints);

                    npcCounter++;
                    break;
                }
            }
        }
    }

    private void CreateNPCPool()
    {
        for (int i = 0; i < totalNPCs; i++)
        {
            GameObject npcPrefab = npcSO.NPCList[npcPrefabIndex];

            npcs[i] = Instantiate(npcPrefab);

            npcs[i].SetActive(false);

            if (npcPrefabIndex < npcSO.NPCList.Count - 1)
            {
                npcPrefabIndex++;
            }
            else
            {
                npcPrefabIndex = 0;
            }
        }
    }
}
