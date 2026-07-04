using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public bool IsPlayerDetected { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerDetected = false;
        }
    }
}