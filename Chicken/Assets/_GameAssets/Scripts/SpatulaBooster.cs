using UnityEngine;

public class SpatulaBooster : MonoBehaviour, IBoostable
{

    [SerializeField] private float jumpForce;

    [SerializeField] private Animator animator;

    private bool isActivated;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Boost(PlayerController playerController)
    {
        if (isActivated) { return; }
        PlayBoostAnimation();

        Rigidbody playerRigidBody = playerController.GetPlayerRigidBody();

        playerRigidBody.linearVelocity = new Vector3 (playerRigidBody.linearVelocity.x, 0f, playerRigidBody.linearVelocity.z);
        playerRigidBody.AddForce(transform.up *  jumpForce, ForceMode.Impulse);
        isActivated = true;
        Invoke(nameof(ResetActivation), 0.2f);


    }

    private void PlayBoostAnimation () 
    {
        animator.SetTrigger(Consts.OtherAnimations.IS_SPATULA_JUMPING);
    }

    private void ResetActivation () 
    {
        isActivated = false;
    }
}
