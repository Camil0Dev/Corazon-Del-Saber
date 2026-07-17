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
    [TextArea(2, 4)]
    [SerializeField] private string loreMessage = "Orbe del Saber obtenido.\nUn extraño mecanismo en la superficie parece resonar con su energía...";

    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.enabled = false; 
    }

    private void Start()
    {
        StartCoroutine(EmergeRoutine());
    }

    private IEnumerator EmergeRoutine()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + new Vector3(0, riseHeight, 0);
        float elapsed = 0f;

        while (elapsed < riseDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / riseDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        col.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            PlayerPrefs.SetInt("HasBlueOrb", 1);
            PlayerPrefs.Save();

            if (ItemUnlockUI.Instance != null)
            {
                ItemUnlockUI.Instance.ShowUnlock(orbIcon, loreMessage);
            }

            Destroy(gameObject);
        }
    }
}