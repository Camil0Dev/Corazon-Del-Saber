using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    #region Variables: Configuración
    [Header("Estadísticas de Vida")]
    [SerializeField] private float maxHealth = 30f;
    [SerializeField] private float damageToPlayer = 10f;

    [Header("Interfaz Visual")]
    [Tooltip("Arrastra aquí el prefab de la barra de vida que está como hijo del enemigo")]
    [SerializeField] private EnemyHealthBar healthBar;

    [Header("Físicas de Retroceso (Knockback)")]
    [SerializeField] private float knockbackForceX = 5f;
    [SerializeField] private float knockbackForceY = 2f;
    [SerializeField] private float knockbackDuration = 0.2f;
    #endregion

    #region Variables: Estado y Referencias
    private float currentHealth;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private MonoBehaviour aiScript; // Soporta VelumbraAI u otros scripts de comportamiento
    #endregion

    #region Ciclo de Vida
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Intentamos obtener la IA (VelumbraAI en este caso)
        aiScript = GetComponent<VelumbraAI>();

        currentHealth = maxHealth;
    }

    private void Start()
    {
        // Inicializamos la barra al inicio para que el script de la barra sepa el maxHealth
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }
    #endregion

    #region Lógica de Combate: Daño Recibido
    public void TakeDamage(float damage, Transform damageSource = null)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth); // Evita vida negativa

        // Actualizar barra de vida (solo visual, sin texto)
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        // Feedback visual de golpe
        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (damageSource != null)
        {
            StartCoroutine(KnockbackRoutine(damageSource));
        }
    }

    private IEnumerator DamageFlash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        // Aquí podrías añadir partículas de muerte antes del Destroy
        Destroy(gameObject);
    }
    #endregion

    #region Físicas: Movimiento y Colisión
    private IEnumerator KnockbackRoutine(Transform source)
    {
        // Desactivamos la IA momentáneamente para que no interfiera con el empuje
        if (aiScript != null) aiScript.enabled = false;

        rb.linearVelocity = Vector2.zero; // Limpiar inercia anterior

        // Calcular dirección basada en la posición del atacante
        float dir = (source.position.x > transform.position.x) ? -1 : 1;

        // Aplicar fuerza de impacto
        rb.AddForce(new Vector2(knockbackForceX * dir, knockbackForceY), ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        // Reactivar IA tras el impacto
        if (aiScript != null) aiScript.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Daño al jugador por contacto
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
            {
                player.TakeDamage(damageToPlayer, transform);
            }
        }
    }
    #endregion
}