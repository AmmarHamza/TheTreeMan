using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{

    private NavMeshAgent agent;

    private Vector3 destination;

    private const int numberOfStopPoints = 3;
    private Vector3[] stopPoints = new Vector3[numberOfStopPoints];
    private int currentStopPointIndex = 0;
    private bool toFinalDestination = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (destination != null)
        {
            if(currentStopPointIndex < numberOfStopPoints)
            {
                agent.destination = stopPoints[currentStopPointIndex];
                if (!toFinalDestination && Vector3.Distance(transform.position, stopPoints[currentStopPointIndex]) <= 2f)
                {
                    currentStopPointIndex++;
                    if (currentStopPointIndex >= numberOfStopPoints)
                    {
                        agent.destination = destination;
                        toFinalDestination = true;
                    }
                }
            }
            if (toFinalDestination && Vector3.Distance(transform.position, destination) <= 2f)
            {
                gameObject.SetActive(false);
                NPCManager.npcCounter--;
                currentStopPointIndex = 0;
                toFinalDestination = false;
            }
        }
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
    }

    public void SetStopPoints(Vector3[] stopPoints)
    {
        this.stopPoints = stopPoints;
    }

    public static int GetNumberOfStopPoints()
    {
        return numberOfStopPoints;
    }

}
