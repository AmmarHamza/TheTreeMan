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

    private Vector3 lightDirection;

    [SerializeField] private HeatBarImage heatBarImage;
    [SerializeField] private Canvas canvas;

    private float arrivalRadius = 2f;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        canvas.worldCamera = Camera.main;
    }

    private void Start()
    {
        heatBarImage.OnHeatBarFilled += HeatBarImage_OnHeatBarFilled;
    }

    private void HeatBarImage_OnHeatBarFilled(object sender, System.EventArgs e)
    {
        ResetNPC();
    }

    private void Update()
    {
        HandleNPCMovement();
        if(IsShaded())
        {
            heatBarImage.DrainHeatBar();
        }
        else
        {
            heatBarImage.FillHeatBar();
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

    private void HandleNPCMovement()
    {
        if (destination != null)
        {
            if (currentStopPointIndex < numberOfStopPoints)
            {
                agent.destination = stopPoints[currentStopPointIndex];
                if (!toFinalDestination && Vector3.Distance(transform.position, stopPoints[currentStopPointIndex]) <= arrivalRadius)
                {
                    currentStopPointIndex++;
                    if (currentStopPointIndex >= numberOfStopPoints)
                    {
                        agent.destination = destination;
                        toFinalDestination = true;
                    }
                }
            }
            if (toFinalDestination && Vector3.Distance(transform.position, destination) <= arrivalRadius)
            {
                ResetNPC();
            }
        }
    }

    private bool IsShaded()
    {
        lightDirection = -RenderSettings.sun.transform.forward;
        Vector3 origin = transform.position + 1.8f * Vector3.up;
        float raycastMaxDistance = 5f;

        if (Physics.Raycast(origin, lightDirection, raycastMaxDistance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ResetNPC()
    {
        gameObject.SetActive(false);
        NPCManager.npcCounter--;
        currentStopPointIndex = 0;
        toFinalDestination = false;
        heatBarImage.ResetHeatBar();
    }
}
