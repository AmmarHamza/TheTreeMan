using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    public static PlayerAnimator Instance { get; private set; }

    private Animator animator;

    private const string ZMOVE = "zMove";
    private const string XMOVE = "xMove";
    private const string PLOUGHING_STARTED = "ploughingStarted";

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Player.Instance.OnPlough += Player_OnPlough;
    }

    private void Player_OnPlough(object sender, System.EventArgs e)
    {
        animator.SetTrigger(PLOUGHING_STARTED);
    }

    public void AnimateMovement(Vector2 inputDir)
    {
        animator.SetFloat(ZMOVE, inputDir.y, 0.125f, Time.deltaTime);
        animator.SetFloat(XMOVE, inputDir.x, 0.125f, Time.deltaTime);
    }

    public void PloughingEnded()
    {
        Player.Instance.SetCanMove(true);
        Player.Instance.SetMidInteraction(false);
        Player.Instance.InstantiateTreeBase();
        Player.Instance.ShoulderHoe();
    }
}
