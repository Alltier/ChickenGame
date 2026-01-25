using UnityEngine;

public class HolyWheatCollectible : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float increaseMovementSpeed;
    [SerializeField] private float increaseJumpForce;
    [SerializeField] private float duration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerController = other.gameObject?.GetComponent<PlayerController>();
        }
    }

    public void Collect()
    {
        playerController.SetMovementSpeed(increaseMovementSpeed, duration);
        playerController.SetJumpForece(increaseJumpForce, duration);
        Destroy(gameObject);
    }
}
