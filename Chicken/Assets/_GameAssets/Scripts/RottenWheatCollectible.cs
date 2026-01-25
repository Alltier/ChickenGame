using UnityEngine;

public class RottenWheatCollectible : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float decreaseMovementSpeed;
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
        playerController.SetMovementSpeed(decreaseMovementSpeed, duration);
        Destroy(gameObject);
    }
}
