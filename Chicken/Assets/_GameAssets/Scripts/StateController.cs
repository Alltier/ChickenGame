using UnityEngine;

public class StateController : MonoBehaviour
{

    private PlayerState currentPlayerState = PlayerState.Idle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeState(PlayerState.Idle);
    }

    private void Update()
    {
        Debug.Log(currentPlayerState.ToString());
    }

    public void ChangeState(PlayerState newPlayerState)
    {
        if (currentPlayerState == newPlayerState) return;

        currentPlayerState = newPlayerState;
    
    }

    public PlayerState GetCurrentState()
    {
        return currentPlayerState; 
    }
}
