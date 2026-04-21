using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Estadísticas de Movimiento")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 16f;
    [SerializeField][Range(0f, 1f)] private float jumpCutMultiplier = 0.5f;

    [Header("Estadísticas de Gravedad")]
    [SerializeField] private float fallMultiplier = 2.5f;

    [Header("Estadísticas de Dash (Botas del Impulso)")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("Detección de Suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;

    // --- NUEVO: SISTEMA DE COMBATE Y VIDA ---
    [Header("Combate")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackRate = 2f; // Cuántos ataques por segundo
    private float nextAttackTime = 0f;

    [Header("Salud y Energía")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private bool isInvulnerable = false;
    [SerializeField] private float invulnerabilityDuration = 1f;
    // ----------------------------------------

    // Referencias y Estados
    private Rigidbody2D rb;
    private PlayerControls controls;
    private SpriteRenderer spriteRenderer; // <--- NUEVA REFERENCIA
    private Vector2 moveInput;
    private bool isGrounded;

    // Variables de Dash
    private bool canDash = true;
    private bool isDashing;
    private float originalGravity;
    private int facingDirection = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // <--- ASIGNACIÓN
        originalGravity = rb.gravityScale;
        controls = new PlayerControls();

        // Inicializar Vida
        currentHealth = maxHealth;

        // Suscripciones al New Input System
        controls.Gameplay.Jump.performed += ctx => Jump();
        controls.Gameplay.Jump.canceled += ctx => JumpCancel();
        controls.Gameplay.Dash.performed += ctx => StartDash();

        // --- NUEVO: Suscripción del botón de ataque ---
        controls.Gameplay.Attack.performed += ctx => Attack();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        if (isDashing) return;

        moveInput = controls.Gameplay.Move.ReadValue<Vector2>();
        CheckGrounded();
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        ApplyMovement();
        ApplyGravity();
    }

    private void ApplyMovement()
    {
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);

        if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingDirection = 1;
        }
        else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingDirection = -1;
        }
    }

    private void ApplyGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = originalGravity * fallMultiplier;
        }
        else
        {
            rb.gravityScale = originalGravity;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void JumpCancel()
    {
        if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }
    }

    private void StartDash()
    {
        if (canDash)
        {
            StartCoroutine(DashRoutine());
        }
    }

    private IEnumerator DashRoutine()
    {
        canDash = false;
        isDashing = true;

        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(facingDirection * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // --- NUEVOS MÉTODOS DE COMBATE Y VIDA ---
    private void Attack()
    {
        // Control de cooldown del ataque
        if (Time.time < nextAttackTime) return;

        Debug.Log("Eira ataca");

        // Detectar enemigos en rango
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Verificamos si el objeto tocado tiene el script "Enemy"
            if (enemy.TryGetComponent<Enemy>(out Enemy enemyComponent))
            {
                enemyComponent.TakeDamage(attackDamage);
            }
        }

        // Calcular cuándo podemos volver a atacar
        nextAttackTime = Time.time + 1f / attackRate;
    }

    public void TakeDamage(float damage)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        Debug.Log($"Eira ha recibido daño. Salud restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvulnerabilityRoutine());
        }
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        // Efecto visual: Parpadeo en rojo durante el tiempo de invulnerabilidad
        float blinkInterval = 0.1f; // Qué tan rápido parpadea
        float timePassed = 0f;

        while (timePassed < invulnerabilityDuration)
        {
            // Cambia a rojo
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(blinkInterval);

            // Cambia a normal (blanco es el color por defecto que no altera el sprite)
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(blinkInterval);

            timePassed += blinkInterval * 2;
        }

        // Medida de seguridad: asegurar que siempre termine en su color normal
        spriteRenderer.color = Color.white;
        isInvulnerable = false;
    }

    private void Die()
    {
        Debug.Log("Eira ha sido derrotada.");
        // Lógica de muerte: deshabilitar controles, mostrar pantalla de Game Over, etc.
        this.enabled = false;
    }
    // ----------------------------------------

    private void CheckGrounded()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualizar GroundCheck
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        // --- NUEVO: Visualizar AttackPoint ---
        if (attackPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}