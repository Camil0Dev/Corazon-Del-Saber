using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    #region Variables: Configuración
    [Header("Movimiento")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 16f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private float fallMultiplier = 2.5f;

    [Header("Habilidades")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("Combate")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackRate = 2f;

    [Header("Salud y Feedback")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float invulnerabilityDuration = 1f;
    [SerializeField] private float knockbackForceX = 5f;
    [SerializeField] private float knockbackForceY = 4f;
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private Image healthBarFill;

    [Header("Detección")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;
    #endregion

    #region Variables: Estado Interno
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private PlayerControls controls;

    private Vector2 moveInput;
    private float currentHealth;
    private float nextAttackTime;
    private float originalGravity;
    private int facingDirection = 1;

    private bool isGrounded;
    private bool canDash = true;
    private bool isDashing;
    private bool isAttacking;
    private bool isKnockedBack;
    private bool isInvulnerable;
    private bool isDead;
    #endregion

    #region Ciclo de Vida e Input
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        originalGravity = rb.gravityScale;
        currentHealth = maxHealth;

        SetupInputSystem();
    }

    private void SetupInputSystem()
    {
        controls = new PlayerControls();

        controls.Gameplay.Jump.performed += _ => Jump();
        controls.Gameplay.Jump.canceled += _ => JumpCancel();
        controls.Gameplay.Dash.performed += _ => StartDash();
        controls.Gameplay.Attack.performed += _ => Attack();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        if (isDead) return;

        HandleInputState();
        CheckGrounded();
        UpdateAnimations();
        UpdateHUD();
    }

    private void FixedUpdate()
    {
        if (isDead || isDashing || isKnockedBack || isAttacking) return;

        ApplyMovement();
        ApplyGravity();
    }

    private void HandleInputState()
    {
        if (!isDashing && !isKnockedBack && !isAttacking)
        {
            moveInput = controls.Gameplay.Move.ReadValue<Vector2>();
            FlipController();
        }
        else
        {
            moveInput = Vector2.zero;
        }
    }
    #endregion

    #region Lógica: Movimiento y Salto
    private void ApplyMovement()
    {
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        if (isGrounded && !isAttacking && !isDead)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
        }
    }

    private void JumpCancel()
    {
        if (rb.linearVelocity.y > 0)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
    }

    private void ApplyGravity()
    {
        rb.gravityScale = (rb.linearVelocity.y < -0.1f) ? originalGravity * fallMultiplier : originalGravity;
    }

    private void FlipController()
    {
        if (moveInput.x == 0) return;

        int newDir = moveInput.x > 0 ? 1 : -1;
        if (newDir != facingDirection)
        {
            facingDirection = newDir;
            transform.rotation = Quaternion.Euler(0, facingDirection == 1 ? 0 : 180, 0);
        }
    }
    #endregion

    #region Lógica: Dash
    private void StartDash()
    {
        if (canDash && !isAttacking && !isDead) StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        canDash = false;
        isDashing = true;
        anim.SetTrigger("dash");

        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(facingDirection * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    #endregion

    #region Lógica: Combate
    private void Attack()
    {
        if (Time.time < nextAttackTime || isDashing || isDead) return;

        // "Coyote bounce": Impulso ligero si ataca cayendo
        if (!isGrounded && rb.linearVelocity.y < 0)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 2f);

        isAttacking = true;
        rb.linearVelocity = Vector2.zero;

        anim.SetTrigger("attack");
        anim.Update(0);

        nextAttackTime = Time.time + 1f / attackRate;
        StartCoroutine(EndAttackRoutine(0.35f));
    }

    private IEnumerator EndAttackRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    public void HitTarget() // Llamado por Animation Event
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.TryGetComponent<Enemy>(out Enemy target))
                target.TakeDamage(attackDamage, transform);
        }
    }
    #endregion

    #region Lógica: Salud y Daño
    public void TakeDamage(float damage, Transform source = null)
    {
        if (isInvulnerable || isDashing || isDead) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        if (currentHealth <= 0) Die();
        else
        {
            StartCoroutine(InvulnerabilityRoutine());
            StartCoroutine(KnockbackRoutine(source));
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        UpdateHUD();
        anim.SetTrigger("die");

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        rb.gravityScale = originalGravity;

        controls.Disable();
        gameObject.layer = 2; // Capa Ignore Raycast/Default para no molestar enemigos

        StartCoroutine(DeathSequenceRoutine());
    }

    private IEnumerator DeathSequenceRoutine()
    {
        yield return new WaitForSeconds(1.8f);
        gameObject.SetActive(false);
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        float elapsed = 0f;
        while (elapsed < invulnerabilityDuration)
        {
            sr.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.2f;
        }
        isInvulnerable = false;
    }

    private IEnumerator KnockbackRoutine(Transform source)
    {
        isKnockedBack = true;
        rb.linearVelocity = Vector2.zero;

        int dir = (source != null && source.position.x > transform.position.x) ? -1 : 1;
        rb.AddForce(new Vector2(knockbackForceX * dir, knockbackForceY), ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);
        isKnockedBack = false;
    }
    #endregion

    #region Auxiliares
    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void UpdateAnimations()
    {
        if (isDead) return;

        anim.SetFloat("speed", Mathf.Abs(moveInput.x));
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isDashing", isDashing);
    }

    private void UpdateHUD()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = currentHealth / maxHealth;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (groundCheck) Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.color = Color.yellow;
        if (attackPoint) Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    #endregion
}