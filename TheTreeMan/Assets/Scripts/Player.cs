using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player Instance { get; private set; }

    private Vector2 inputDir;

    private CharacterController characterController;

    private float speed = 5f;

    public event EventHandler OnPlough;
    //public event EventHandler OnSow;
    //public event EventHandler OnWater;

    private bool canMove = true;
    private bool canPlough = false;
    //private bool canSow = false;
    //private bool canWater = false;

    [SerializeField] private GameObject treeBasePrefab;
    [SerializeField] private Transform treeBaseSpawnPoint;
    [SerializeField] private Transform treeBasesContainer;

    [SerializeField] private Transform hoeShoulderPoint;
    [SerializeField] private Transform hoeGripPoint;
    [SerializeField] private Transform hoe;

    private float overlapRadius = 5f;
    private bool foundBase = false;
    private bool midInteraction = false;

    List<TreeBase> treeBases = new List<TreeBase>();
    TreeBase closestTreeBase;

    private void Awake()
    {
        Instance = this;
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        GameInput.Instance.OnInteract += GameInput_OnInteract;

        ShoulderHoe();
    }

    private void GameInput_OnInteract(object sender, System.EventArgs e)
    {
        if (!midInteraction)
        {
            DecideOnInteraction();
        }

        if (inputDir == Vector2.zero)
        {
            if (canPlough)
            {
                OnPlough?.Invoke(this, EventArgs.Empty);

                canPlough = false;
                canMove = false;
                midInteraction = true;

                GripHoe();
            }
        }
    }

    private void Update()
    {
        if (canMove)
        {
            Move();
        }
    }

    private void Move()
    {
        inputDir = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputDir.x, 0f, inputDir.y);

        float cameraAngle = Camera.main.transform.eulerAngles.y;
        
        if (inputDir != Vector2.zero)
        {
            transform.rotation = Quaternion.Euler(0f, cameraAngle, 0f);
        }

        moveDir = Quaternion.Euler(0f, cameraAngle, 0f) * moveDir;

        Vector3 gravity = Vector3.down * 10f;

        Vector3 moveDirWithGravity = moveDir + gravity;

        characterController.Move(moveDirWithGravity * Time.deltaTime * speed);

        PlayerAnimator.Instance.AnimateMovement(inputDir);
    }

    public void InstantiateTreeBase()
    {
        GameObject treeBase = Instantiate(treeBasePrefab, treeBasesContainer);
        treeBase.transform.position = treeBaseSpawnPoint.position - new Vector3(0f, transform.position.y, 0f);
    }

    public Vector2 GetInputDirNormalized()
    {
        return GameInput.Instance.GetMovementVectorNormalized();
    }
    
    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    public void SetMidInteraction(bool midInteraction)
    {
        this.midInteraction = midInteraction;
    }

    public void GripHoe()
    {
        hoe.transform.parent = hoeGripPoint;
        hoe.transform.localPosition = Vector3.zero;
        hoe.transform.localRotation = quaternion.Euler(0f, 0f, 0f);
    }

    public void ShoulderHoe()
    {
        hoe.transform.parent = hoeShoulderPoint;
        hoe.transform.localPosition = Vector3.zero;
        hoe.transform.localRotation = quaternion.Euler(0f, 0f, 0f);
    }

    private void DecideOnInteraction()
    {
        GetCloseTreeBases();

        if (foundBase)
        {
            closestTreeBase = treeBases[0];
            foreach (TreeBase treeBase in treeBases)
            {
                if (Vector3.Distance(transform.position, treeBase.transform.position) < Vector3.Distance(transform.position, closestTreeBase.transform.position))
                {
                    closestTreeBase = treeBase;
                }
            }
            if (closestTreeBase.GetHasTree())
            {
                //canWater = true;
            }
            else
            {
                //canSow = true;
            }
        }
        else
        {
            canPlough = true;
        }

        treeBases.Clear();
    }

    private void GetCloseTreeBases()
    {
        Collider[] colliders = Physics.OverlapSphere(treeBaseSpawnPoint.position, overlapRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.transform.parent != null)
            {
                if (collider.transform.parent.TryGetComponent(out TreeBase treeBase))
                {
                    treeBases.Add(treeBase);
                }
            }
        }

        if (treeBases.Count > 0)
        {
            foundBase = true;
        }
        else
        {
            foundBase = false;
        }
    }
}
