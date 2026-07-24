using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AbyssTrigger : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Arrastra aquí el objeto vacío donde reaparecerá el jugador")]
    [SerializeField] private Transform puntoDeRespawn;
    
    [Tooltip("Cantidad de daño que recibe el jugador al caer")]
    [SerializeField] private float cantidadDeDano = 10f;

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController jugador = collision.GetComponent<PlayerController>();
            if (jugador != null)
            {
                jugador.TakeDamage(cantidadDeDano);
            }

            if (puntoDeRespawn != null)
            {
                collision.transform.position = puntoDeRespawn.position;
                
                Rigidbody2D rbJugador = collision.GetComponent<Rigidbody2D>();
                if (rbJugador != null)
                {
                    rbJugador.linearVelocity = Vector2.zero; 
                }
            }
            else
            {
                Debug.LogWarning("¡Cuidado! Te olvidaste de asignar el Punto de Respawn en el Inspector del Abismo.");
            }
        }
    }
}