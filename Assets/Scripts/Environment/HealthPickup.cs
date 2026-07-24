using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HealthPickup : MonoBehaviour
{
    [Tooltip("Cuánta vida recupera este orbe")]
    [SerializeField] private float vidaARecuperar = 20f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController jugador = collision.GetComponent<PlayerController>();
            
            // Solo nos curamos si el jugador realmente necesita vida
            if (jugador != null && jugador.GetCurrentHealth() < 100f) // Asumiendo que 100 es maxHealth
            {
                jugador.Heal(vidaARecuperar);
                
                Destroy(gameObject); 
            }
        }
    }
}