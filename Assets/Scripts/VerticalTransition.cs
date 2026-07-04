using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class VerticalTransition : MonoBehaviour
{
    [Header("Configuración de Destinos")]
    [Tooltip("Asigna aquí los GameObjects vacíos que servirán de punto de aparición.")]
    [SerializeField] private Transform bottomSpawn;
    [SerializeField] private Transform topLeftSpawn;
    [SerializeField] private Transform topRightSpawn;

    [Header("Tipo de Transición")]
    [Tooltip("Actívalo si este trigger es para caer. Desactívalo si es para subir.")]
    [SerializeField] private bool isGoingDown = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos que sea el jugador
        if (collision.CompareTag("Player"))
        {
            TeleportPlayer(collision.transform);
        }
    }

    private void TeleportPlayer(Transform playerTransform)
    {
        if (isGoingDown)
        {
            if (bottomSpawn != null)
            {
                playerTransform.position = bottomSpawn.position;
            }
        }
        else
        {
            if (topLeftSpawn != null && topRightSpawn != null)
            {
                if (playerTransform.position.x < transform.position.x)
                {
                    playerTransform.position = topLeftSpawn.position;
                }
                else
                {
                    playerTransform.position = topRightSpawn.position;
                }
            }
        }
    }
}