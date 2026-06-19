using UnityEngine;

public class Bench : MonoBehaviour
{
    [Header("Referencias")]
    public Transform sitPoint;
    public GameObject interactText;

    private void Start()
    {
        // Asegurarse de que el texto empiece oculto
        if (interactText != null)
        {
            interactText.SetActive(false);
        }
    }

    public void ActivateBench(GameObject player)
    {
        // Mover al jugador al punto de descanso
        player.transform.position = sitPoint.position;

        // Ocultar el mensaje de interacción
        if (interactText != null)
        {
            interactText.SetActive(false);
        }

        Debug.Log("Banco activado");

        // FUTURO:
        // Restaurar vida
        // Guardar partida
        // Reproducir animación
        // Reproducir sonido
    }
}