using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public bool IsPlayerDetected { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerDetected = true;
            Debug.Log("🔵 Jugador DETECTADO en DetectionZone (Enter)");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerDetected = true;
            // Debug opcional para ver que se mantiene la detección
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerDetected = false;
            Debug.Log("🔴 Jugador PERDIDO en DetectionZone (Exit)");
        }
    }
}