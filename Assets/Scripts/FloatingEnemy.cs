using UnityEngine;

public class FloatingEnemy : MonoBehaviour, IEnemyAI
{
    [Header("Comportamiento: Wandering")]
    [SerializeField] private float wanderSpeed = 1.5f;
    [SerializeField] private float wanderRadius = 3f;
    [SerializeField] private float waitTimeAtPoint = 2f;

    [Header("Comportamiento: Combate")]
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackCooldown = 2.5f;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Disparo")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float aimOffsetY = 1f;

    private Transform playerTransform;
    private Animator animator;
    
    private Vector2 startPosition;
    private Vector2 currentWanderTarget;
    private float nextAttackTime = 0f;
    private float waitTimer = 0f;
    private bool isFacingRight = false;
    private bool isAttacking = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        GetNewWanderTarget();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    private void Update()
    {
        if (playerTransform == null) return;

        float sqrDistanceToPlayer = (transform.position - playerTransform.position).sqrMagnitude;

        if (sqrDistanceToPlayer <= (detectionRange * detectionRange) && CanSeePlayer())
        {
            LookAtTarget(playerTransform.position);

            if (sqrDistanceToPlayer <= (attackRange * attackRange))
            {
                if (Time.time >= nextAttackTime && !isAttacking)
                {
                    Attack();
                }
            }
        }
        else if (!isAttacking)
        {
            Wander();
        }
    }

    private void Wander()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentWanderTarget, wanderSpeed * Time.deltaTime);
        LookAtTarget(currentWanderTarget);

        if (Vector2.Distance(transform.position, currentWanderTarget) < 0.1f)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                GetNewWanderTarget();
                waitTimer = waitTimeAtPoint;
            }
        }
    }

    private void GetNewWanderTarget()
    {
        Vector2 randomDir = Random.insideUnitCircle * wanderRadius;
        currentWanderTarget = startPosition + randomDir;
    }

    private void LookAtTarget(Vector2 targetPos)
    {
        if (targetPos.x > transform.position.x && !isFacingRight) Flip();
        else if (targetPos.x < transform.position.x && isFacingRight) Flip();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void Attack()
    {
        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;
        animator.SetTrigger("Attack");
    }

    public void Shoot() 
    {
        if (fireballPrefab == null || firePoint == null || playerTransform == null) return;

        GameObject projectileObj = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        if (projectileObj.TryGetComponent<Projectile2D>(out Projectile2D projectileScript))
        {
            Vector3 targetPosition = new Vector3(playerTransform.position.x, playerTransform.position.y + aimOffsetY, playerTransform.position.z);
            Vector2 directionToPlayer = (targetPosition - firePoint.position).normalized;
            projectileScript.SetDirection(directionToPlayer);
        }
    }

    public void EndAttack() 
    {
        isAttacking = false;
    }

    private bool CanSeePlayer()
    {
        if (playerTransform == null) return false;

        Vector2 directionToPlayer = playerTransform.position - transform.position;
        float distance = directionToPlayer.magnitude;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, distance, obstacleLayer);

        return hit.collider == null;
    }

    public void ToggleAI(bool isEnabled)
    {
        this.enabled = isEnabled;
    }

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Vector2 center = Application.isPlaying ? startPosition : (Vector2)transform.position;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(center, wanderRadius);

        if (Application.isPlaying && playerTransform != null)
        {
            Gizmos.color = CanSeePlayer() ? Color.blue : Color.gray;
            Gizmos.DrawLine(transform.position, playerTransform.position);
        }
    }
    #endregion
}