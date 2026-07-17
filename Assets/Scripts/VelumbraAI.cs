using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class VelumbraAI : MonoBehaviour, IEnemyAI
{
    #region Variables: Configuración
    [Header("IA: Combate")]
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float detectionRange = 6f;
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("IA: Wandering (Vagar)")]
    [SerializeField] private float wanderSpeed = 1.5f;
    [SerializeField] private float wanderRadius = 3f;
    [SerializeField] private float waitTimeAtEdge = 1.5f;

    [Header("Combate: Daño")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private float attackDamage = 15f;

    [Header("Detección de Suelo")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float edgeCheckOffset = 0.6f;
    [SerializeField] private float edgeCheckLength = 1f;
    #endregion

    #region Variables: Estado Interno
    private Transform player;
    private Animator anim;
    private Rigidbody2D rb;

    private float lastAttackTime;
    private bool isAttacking;
    private Vector2 startPosition;
    private Vector2 wanderTarget;
    private float waitTimer;
    private bool isWaiting;

    private readonly int IdleState = Animator.StringToHash("Idle");
    private readonly int RunState = Animator.StringToHash("Run");
    private readonly int AttackTrigger = Animator.StringToHash("Attack");
    #endregion

    #region Ciclo de Vida
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        startPosition = transform.position;
        PickNewWanderTarget();
    }

    private void Update()
    {
        if (isAttacking) return;

        if (ShouldReturnToIdle())
        {
            StopMovement();
            return;
        }

        HandleBehavior();
    }
    #endregion

    #region Lógica de Comportamiento
    private void HandleBehavior()
    {
        float sqrDistanceToPlayer = (transform.position - player.position).sqrMagnitude;
        
        bool canSeePlayer = CanSeePlayer(); 

        if (sqrDistanceToPlayer <= (attackRange * attackRange) && canSeePlayer) 
        {
            if (Time.time >= lastAttackTime + attackCooldown)
                Attack();
            else
                StopMovement();
        }
        else if (sqrDistanceToPlayer <= (detectionRange * detectionRange) && canSeePlayer) 
        {
            ChasePlayer();
        }
        else
        {
            Wander(); 
        }
    }

    private bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 eyePosition = new Vector2(transform.position.x, transform.position.y + 0.5f);
        Vector2 playerCenter = new Vector2(player.position.x, player.position.y + 0.5f);

        Vector2 directionToPlayer = playerCenter - eyePosition;
        float distance = directionToPlayer.magnitude;

        RaycastHit2D hit = Physics2D.Raycast(eyePosition, directionToPlayer.normalized, distance, obstacleLayer);

        return hit.collider == null;
    }

    private void ChasePlayer()
    {
        float directionX = Mathf.Sign(player.position.x - transform.position.x);

        if (IsNearEdge(directionX))
        {
            StopMovement();
            FlipSprite(directionX);
            return;
        }

        Move(directionX, chaseSpeed);
    }

    private void Wander()
    {
        if (isWaiting)
        {
            StopMovement();
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                PickNewWanderTarget();
            }
        }
        else
        {
            float directionX = Mathf.Sign(wanderTarget.x - transform.position.x);

            if (IsNearEdge(directionX))
            {
                HandleEdgeDetection(directionX);
                return;
            }

            Move(directionX, wanderSpeed);

            if (Mathf.Abs(transform.position.x - wanderTarget.x) < 0.2f)
            {
                StartWaiting(waitTimeAtEdge);
            }
        }
    }
    #endregion

    #region Acciones y Movimiento
    private void Move(float dirX, float currentSpeed)
    {
        anim.Play(RunState);
        rb.linearVelocity = new Vector2(dirX * currentSpeed, rb.linearVelocity.y);
        FlipSprite(dirX);
    }

    private void StopMovement()
    {
        anim.Play(IdleState);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    private void Attack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        rb.linearVelocity = Vector2.zero;
        anim.SetTrigger(AttackTrigger);
    }

    public void PerformDamage() 
    {
        if (attackPoint == null) return;

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius);
        foreach (Collider2D hit in hitObjects)
        {
            if (hit.CompareTag("Player") && hit.TryGetComponent<PlayerController>(out var pc))
            {
                pc.TakeDamage(attackDamage, transform);
            }
        }
    }

    public void EndAttack() => isAttacking = false; 
    #endregion

    #region Helpers y Utilidades
    private bool IsNearEdge(float directionX)
    {
        Vector2 origin = new Vector2(transform.position.x + (directionX * edgeCheckOffset), transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, edgeCheckLength, groundLayer);
        return hit.collider == null;
    }

    private void HandleEdgeDetection(float currentDir)
    {
        StartWaiting(waitTimeAtEdge);
        float safeX = transform.position.x + (-currentDir * wanderRadius);
        wanderTarget = new Vector2(safeX, transform.position.y);
    }

    private void StartWaiting(float duration)
    {
        isWaiting = true;
        waitTimer = duration;
    }

    private void PickNewWanderTarget()
    {
        float randomX = Random.Range(startPosition.x - wanderRadius, startPosition.x + wanderRadius);
        wanderTarget = new Vector2(randomX, transform.position.y);
    }

    private void FlipSprite(float directionX)
    {
        if (directionX == 0) return;
        transform.rotation = Quaternion.Euler(0, directionX > 0 ? 180 : 0, 0);
    }

    private bool ShouldReturnToIdle() => player == null || !player.gameObject.activeInHierarchy;
    #endregion

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Vector2 center = Application.isPlaying ? startPosition : (Vector2)transform.position;
        Gizmos.color = Color.cyan;
        Vector3 leftBound = new Vector3(center.x - wanderRadius, center.y, 0);
        Vector3 rightBound = new Vector3(center.x + wanderRadius, center.y, 0);

        Gizmos.DrawLine(leftBound, rightBound);
        Gizmos.DrawSphere(leftBound, 0.15f);
        Gizmos.DrawSphere(rightBound, 0.15f);

        Gizmos.color = Color.green;
        Vector2 rO = new Vector2(transform.position.x + edgeCheckOffset, transform.position.y);
        Vector2 lO = new Vector2(transform.position.x - edgeCheckOffset, transform.position.y);
        Gizmos.DrawLine(rO, rO + Vector2.down * edgeCheckLength);
        Gizmos.DrawLine(lO, lO + Vector2.down * edgeCheckLength);

        if (attackPoint != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
        
        if (Application.isPlaying && player != null)
        {
            Vector2 eyePosition = new Vector2(transform.position.x, transform.position.y + 0.5f);
            Vector2 playerCenter = new Vector2(player.position.x, player.position.y + 0.5f);
            
            Gizmos.color = CanSeePlayer() ? Color.blue : Color.gray;
            Gizmos.DrawLine(eyePosition, playerCenter);
        }
    }
    #endregion
    public void ToggleAI(bool isEnabled)
    {
        this.enabled = isEnabled;
    }
}