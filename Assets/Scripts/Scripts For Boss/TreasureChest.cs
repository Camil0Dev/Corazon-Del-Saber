using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class TreasureChest : MonoBehaviour
{
    [Header("Sprites del Cofre")]
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite halfOpenSprite;
    [SerializeField] private Sprite openSprite;

    [Header("Recompensa UI")]
    [SerializeField] private Sprite iconBoots;
    [Tooltip("El texto que aparecerá en el pop-up gigante al abrir el cofre")]
    [SerializeField] private string unlockMessage = "¡Botas de Impulso Obtenidas!";

    [Header("Configuración")]
    [SerializeField] private float animationDelay = 0.2f;
    
    [Tooltip("Arrastra aquí el objeto de texto (UI) que dice 'Presiona E'")]
    [SerializeField] private GameObject interactUI;

    private SpriteRenderer sr;
    private bool isOpened = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (closedSprite != null) sr.sprite = closedSprite; 
        
        // Nos aseguramos de que el texto esté oculto al iniciar
        if (interactUI != null) interactUI.SetActive(false); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpened && collision.CompareTag("Player") && !collision.isTrigger)
        {
            if (interactUI != null) interactUI.SetActive(true); // Mostrar texto

            // Le decimos a Eira que está frente a un cofre
            if (collision.TryGetComponent<PlayerController>(out var player))
            {
                player.SetCurrentChest(this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isOpened && collision.CompareTag("Player") && !collision.isTrigger)
        {
            if (interactUI != null) interactUI.SetActive(false); // Ocultar texto

            // Limpiamos la referencia cuando Eira se aleja
            if (collision.TryGetComponent<PlayerController>(out var player))
            {
                player.ClearCurrentChest();
            }
        }
    }

    // 🔹 Este método es llamado por el PlayerController cuando presionas 'E'
    public void InteractChest(GameObject player)
    {
        if (isOpened) return;
        
        // Ocultar texto permanentemente
        if (interactUI != null) interactUI.SetActive(false); 
        
        // Limpiar la referencia en el jugador para que no intente interactuar de nuevo
        if (player.TryGetComponent<PlayerController>(out var controller))
        {
            controller.ClearCurrentChest();
        }

        StartCoroutine(OpenChestRoutine(player));
    }

    private IEnumerator OpenChestRoutine(GameObject player)
    {
        isOpened = true;

        // Frame 2: Medio Abierto
        if (halfOpenSprite != null) sr.sprite = halfOpenSprite;
        yield return new WaitForSeconds(animationDelay);
        
        // Frame 3: Abierto
        if (openSprite != null) sr.sprite = openSprite;

        // Desbloquear el Dash y llamar a la UI
        if (player.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            controller.UnlockDash();
            
            if (ItemUnlockUI.Instance != null)
            {
                // Ahora toma el mensaje que escribas directamente en el Inspector
                ItemUnlockUI.Instance.ShowUnlock(iconBoots, unlockMessage);
            }
        }
    }
}