using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private StateController stateController;
    private PlayerController playerController;

    private void Awake()
    {
        stateController = GetComponent<StateController>();
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        playerController.OnPlayerJumped += PlayerController_OnPlayerJumped;
    }

    private void PlayerController_OnPlayerJumped()
    {
        animator.SetBool(Consts.PlayerAnimations.IS_JUMPING, true);
        Invoke(nameof(ResetJumping), 0.5f);
    }

    private void ResetJumping()
    {
        animator.SetBool(Consts.PlayerAnimations.IS_JUMPING, false);

    }

    private void Update()
    {
        SetPlayerAnimations();
    }

    private void SetPlayerAnimations()
    {
        var currentState = stateController.GetCurrentState();

        switch (currentState)
        {
            case PlayerState.Idle:
                animator.SetBool(Consts.PlayerAnimations.IS_SLIDING, false);
                animator.SetBool(Consts.PlayerAnimations.IS_MOVING, false);
                break;

            case PlayerState.Move:
                animator.SetBool(Consts.PlayerAnimations.IS_SLIDING, false);
                animator.SetBool(Consts.PlayerAnimations.IS_MOVING, true);
                break;

            case PlayerState.SlideIdle:
                animator.SetBool(Consts.PlayerAnimations.IS_SLIDING, true);
                animator.SetBool(Consts.PlayerAnimations.IS_SLIDING_ACTIVE, false);
                break;

            case PlayerState.Slide:
                animator.SetBool(Consts.PlayerAnimations.IS_SLIDING, true);
                animator.SetBool(Consts.PlayerAnimations.IS_SLIDING_ACTIVE, true);
                break;




        }


    }



}
