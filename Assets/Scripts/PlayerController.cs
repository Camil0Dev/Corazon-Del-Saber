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
    [SerializeField] private float fallMultiplier = 2.5f; // <--- NUEVO: Hace que la caída sea más rápida y pesada

    [Header("Estadísticas de Dash (Botas del Impulso)")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("Detección de Suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;

    // Referencias y Estados
    private Rigidbody2D rb;
    private PlayerControls controls;
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
        originalGravity = rb.gravityScale;
        controls = new PlayerControls();

        // Suscripciones al New Input System
        controls.Gameplay.Jump.performed += ctx => Jump();
        controls.Gameplay.Jump.canceled += ctx => JumpCancel();
        controls.Gameplay.Dash.performed += ctx => StartDash();
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
        ApplyGravity(); // <--- NUEVO: Evaluamos la gravedad en cada frame físico
    }

    private void ApplyMovement()
    {
        // Actualizado a linearVelocity
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

    // <--- NUEVO METODO COMPLETO --->
    private void ApplyGravity()
    {
        // Si Eira está cayendo (su velocidad vertical es negativa), aumentamos su escala de gravedad
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = originalGravity * fallMultiplier;
        }
        else
        {
            // Si está subiendo o en el suelo, restauramos la gravedad original
            rb.gravityScale = originalGravity;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            // Actualizado a linearVelocity
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void JumpCancel()
    {
        // Actualizado a linearVelocity
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
        // Actualizado a linearVelocity
        rb.linearVelocity = new Vector2(facingDirection * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        // Restauramos a la originalGravity en lugar de 1f, para respetar configuraciones del Rigidbody
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void CheckGrounded()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
    }

    // Este método dibuja una esfera roja en el editor para que veas el GroundCheck
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}