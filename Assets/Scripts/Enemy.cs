using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]

public class Enemy : MonoBehaviour
{
    [Header("Estadísticas de Vida")]
    [SerializeField] private float maxHealth = 30f;
    [SerializeField] private float damageToPlayer = 10f;

    [Header("Interfaz Visual")]
    [SerializeField] private EnemyHealthBar healthBar;

    [Header("Físicas de Retroceso (Knockback)")]
    [SerializeField] private float knockbackForceX = 5f;
    [SerializeField] private float knockbackForceY = 2f;
    [SerializeField] private float knockbackDuration = 0.2f;

    private float currentHealth;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private IEnemyAI aiScript;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        aiScript = GetComponent<IEnemyAI>();

        currentHealth = maxHealth;
    }

    private void Start()
    {
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(float damage, Transform damageSource = null)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

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
        gameObject.SetActive(false);
    }

    private IEnumerator KnockbackRoutine(Transform source)
    {
        if (aiScript != null) aiScript.ToggleAI(false);

        rb.linearVelocity = Vector2.zero;

        float dir = (source.position.x > transform.position.x) ? -1 : 1;
        rb.AddForce(new Vector2(knockbackForceX * dir, knockbackForceY), ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        rb.linearVelocity = Vector2.zero;

        if (aiScript != null) aiScript.ToggleAI(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
            {
                player.TakeDamage(damageToPlayer, transform);
            }
        }
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }
}