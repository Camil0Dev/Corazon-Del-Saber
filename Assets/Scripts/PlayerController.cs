﻿using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    #region Variables: Configuración
    [Header("Movimiento")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private float fallMultiplier = 2.5f;

    [Header("Habilidades")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("Combate")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 1.4f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackRate = 2f;

    [Header("Salud y Feedback")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float invulnerabilityDuration = 0.5f;
    [SerializeField] private float knockbackForceX = 2f;
    [SerializeField] private float knockbackForceY = 1f;
    [SerializeField] private float knockbackDuration = 0.1f;
    [SerializeField] private Image healthBarFill;

    [Header("Detección")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.27f;

    [Header("Sonidos de Combate")]
    [SerializeField] private AudioClip swordSwingSound;
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
    private bool canDash = false;
    private bool isDashing;
    private bool isAttacking;
    private bool isKnockedBack;
    private bool isInvulnerable;
    private bool isDead;
    private Bench currentBench;

    private readonly int AnimSpeed = Animator.StringToHash("speed");
    private readonly int AnimIsGrounded = Animator.StringToHash("isGrounded");
    private readonly int AnimYVelocity = Animator.StringToHash("yVelocity");
    private readonly int AnimIsDashing = Animator.StringToHash("isDashing");
    private readonly int AnimJump = Animator.StringToHash("jump");
    private readonly int AnimDash = Animator.StringToHash("dash");
    private readonly int AnimAttack = Animator.StringToHash("attack");
    private readonly int AnimDie = Animator.StringToHash("die");
    #endregion

    #region Ciclo de Vida e Input
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        originalGravity = rb.gravityScale;
        currentHealth = maxHealth;

        UpdateHUD();

        SetupInputSystem();
    }

    private void SetupInputSystem()
    {
        controls = new PlayerControls();

        controls.Gameplay.Jump.performed += _ => Jump();
        controls.Gameplay.Jump.canceled += _ => JumpCancel();
        controls.Gameplay.Dash.performed += _ => StartDash();
        controls.Gameplay.Attack.performed += _ => Attack();
        controls.Gameplay.Interact.performed += _ => Interact();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        if (isDead) return;

        HandleInputState();
        CheckGrounded();
        UpdateAnimations();
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

    private void Interact()
    {
        if (currentBench != null)
        {
            currentBench.ActivateBench(gameObject);
        }
        else if (currentChest != null)
        {
            currentChest.InteractChest(gameObject);
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
            anim.SetTrigger(AnimJump);
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
        anim.SetTrigger(AnimDash);

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

        if (!isGrounded && rb.linearVelocity.y < 0)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 2f);

        isAttacking = true;
        rb.linearVelocity = Vector2.zero;

        anim.SetTrigger(AnimAttack);
        anim.Update(0);

        nextAttackTime = Time.time + 1f / attackRate;
        StartCoroutine(EndAttackRoutine(0.35f));

        if (AudioManager.Instance != null && swordSwingSound != null)
        {
            AudioManager.Instance.PlaySFX(swordSwingSound, true); 
        }
    }

    private IEnumerator EndAttackRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    public void HitTarget()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D target in targets)
        {
            if (target.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(attackDamage, transform);
            }

            if (target.TryGetComponent<BossHealth>(out BossHealth bossHealth))
            {
                bossHealth.TakeDamage((int)attackDamage);
            }
        }
    }
    #endregion

    #region Lógica: Salud y Daño
    public void TakeDamage(float damage, Transform source = null)
    {
        if (isInvulnerable || isDashing || isDead) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        UpdateHUD();

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

        StopAllCoroutines();

        UpdateHUD();
        anim.SetTrigger(AnimDie);

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        rb.gravityScale = originalGravity;

        controls.Disable();
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        StartCoroutine(DeathSequenceRoutine());
    }

    private IEnumerator DeathSequenceRoutine()
    {
        yield return new WaitForSeconds(1.8f);

        if (PlayerPrefs.HasKey("SavedScene"))
        {
            PlayerPrefs.SetInt("LoadFromSave", 1);
            PlayerPrefs.Save();

            string sceneToLoad = PlayerPrefs.GetString("SavedScene");
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            PlayerPrefs.SetInt("LoadFromSave", 0);
            PlayerPrefs.Save();
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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

        anim.SetFloat(AnimSpeed, Mathf.Abs(moveInput.x));
        anim.SetBool(AnimIsGrounded, isGrounded);
        anim.SetFloat(AnimYVelocity, rb.linearVelocity.y);
        anim.SetBool(AnimIsDashing, isDashing);
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

    public float GetCurrentHealth() => currentHealth;
    private TreasureChest currentChest;

    public void UnlockDash()
    {
        canDash = true;
        Debug.Log("¡Dash Desbloqueado permanentemente!");
    }
    #endregion

    #region Bench
    public void SetCurrentBench(Bench bench) => currentBench = bench;
    public void ClearCurrentBench() => currentBench = null;
    #endregion

    #region Treasure Chest
    public void SetCurrentChest(TreasureChest chest) => currentChest = chest;
    public void ClearCurrentChest() => currentChest = null;
    #endregion
}