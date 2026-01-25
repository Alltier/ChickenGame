using UnityEngine;

public class GoldWheatCollectible : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float increaseMovementSpeed;
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
        Destroy(gameObject);
    }


}
