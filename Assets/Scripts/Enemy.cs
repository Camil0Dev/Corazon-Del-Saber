using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Estadísticas")]
    [SerializeField] private float maxHealth = 30f;
    [SerializeField] private float damageToPlayer = 10f; // NUEVO: Daño que le hace a Eira al tocarla
    private float currentHealth;

    // Referencias
    private SpriteRenderer spriteRenderer; // MEJORA: Cachear el componente

    private void Awake()
    {
        currentHealth = maxHealth;
        // Guardamos la referencia una sola vez al inicio
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Enemigo dañado. Vida restante: {currentHealth}");

        // Efecto visual simple: Parpadeo rojo
        StartCoroutine(DamageFlash());

        if (currentHealth <= 0) Die();
    }

    private System.Collections.IEnumerator DamageFlash()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
        }
    }

    private void Die()
    {
        Debug.Log("Enemigo derrotado");
        Destroy(gameObject);
    }

    // NUEVO: Lógica para hacerle daño a Eira
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Asegúrate de que Eira tenga la etiqueta (Tag) "Player" en el editor de Unity
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
            {
                player.TakeDamage(damageToPlayer);
            }
        }
    }
}