using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class WorldTextTrigger : MonoBehaviour
{
    [Header("Configuración de Lore")]
    [Tooltip("¿Qué tipo de objeto es este?")]
    [SerializeField] private LoreData.LoreType miTipoDeLore;

    [Header("Referencia al Texto")]
    [SerializeField] private TextMeshPro worldText;
    
    [Tooltip("Tiempo que tarda en aparecer/desaparecer el texto")]
    [SerializeField] private float fadeSpeed = 5f;

    private Color targetColor;
    private Color originalColor;
    
    // Controla si este objeto ya obtuvo su texto
    private bool textoAsignado = false; 

    private void Start()
    {
        if (worldText != null)
        {
            originalColor = worldText.color;
            targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
            worldText.color = targetColor; // Inicia invisible
        }

        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void Update()
    {
        if (worldText != null)
        {
            // Transición suave del color (Fade in / Fade out)
            worldText.color = Color.Lerp(worldText.color, targetColor, Time.deltaTime * fadeSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Le pedimos el texto al Manager SOLO la primera vez que el jugador entra.
            // Así, si el jugador se aleja y vuelve, el objeto seguirá teniendo el mismo texto (coherencia de mundo).
            if (!textoAsignado && LoreManager.Instance != null && worldText != null)
            {
                worldText.text = LoreManager.Instance.ObtenerTextoAleatorioUnico(miTipoDeLore);
                textoAsignado = true;
            }

            // Vuelve el texto visible
            targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Oculta el texto
            targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        }
    }
}