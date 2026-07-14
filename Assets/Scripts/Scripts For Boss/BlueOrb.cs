using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class BlueOrb : MonoBehaviour
{
    [Header("Animación de Emergencia")]
    [Tooltip("Qué tan alto subirá el orbe desde el cuerpo del boss")]
    [SerializeField] private float riseHeight = 1.5f;
    [Tooltip("Cuánto tiempo tardará en subir")]
    [SerializeField] private float riseDuration = 2f;
    
    [Header("Recompensa UI")]
    [SerializeField] private Sprite orbIcon;
    [TextArea(2, 4)] // Hace que la caja de texto en el Inspector sea más grande
    [SerializeField] private string loreMessage = "Orbe del Saber obtenido.\nUn extraño mecanismo en la superficie parece resonar con su energía...";

    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        // 🔹 Apagamos la colisión para que el jugador no lo agarre mientras sube
        col.enabled = false; 
    }

    private void Start()
    {
        // El orbe empieza a flotar hacia arriba apenas aparece en la escena
        StartCoroutine(EmergeRoutine());
    }

    private IEnumerator EmergeRoutine()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + new Vector3(0, riseHeight, 0);
        float elapsed = 0f;

        while (elapsed < riseDuration)
        {
            // Lerp mueve el objeto del punto A al B de forma súper suave
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / riseDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        col.enabled = true; // ¡El orbe ya subió, Eira ya puede agarrarlo!
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            // 🔹 INVENTARIO EXPRÉS: Guardamos que tenemos el orbe usando PlayerPrefs
            PlayerPrefs.SetInt("HasBlueOrb", 1);
            PlayerPrefs.Save();

            // Mostramos el texto sugestivo usando el sistema que ya creaste
            if (ItemUnlockUI.Instance != null)
            {
                ItemUnlockUI.Instance.ShowUnlock(orbIcon, loreMessage);
            }

            // Destruimos el orbe de la escena
            Destroy(gameObject);
        }
    }
}