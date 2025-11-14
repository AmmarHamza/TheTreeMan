using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    public static PlayerAnimator Instance { get; private set; }

    private Animator animator;

    private const string ZMOVE = "zMove";
    private const string XMOVE = "xMove";
    private const string PLOUGHING_STARTED = "ploughingStarted";
    private const string SOWING_STARTED = "sowingStarted";

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Player.Instance.OnPlough += Player_OnPlough;
        Player.Instance.OnSow += Player_OnSow;
    }

    private void Player_OnSow(object sender, System.EventArgs e)
    {
        animator.SetTrigger(SOWING_STARTED);
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

    public void EndPloughing()
    {
        Player.Instance.SetCanMove(true);
        Player.Instance.SetMidInteraction(false);
        Player.Instance.InstantiateTreeBase();
        Player.Instance.ShoulderHoe();
    }
    public void EndSowing()
    {
        Player.Instance.SetCanMove(true);
        Player.Instance.SetMidInteraction(false);
    }
}
