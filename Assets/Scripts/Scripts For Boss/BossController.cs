using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class BossController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BossData _data;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _spriteTransform;
    [SerializeField] private BossTrigger _trigger;
    [SerializeField] private bool _spriteDefaultFacingRight = false;

    [Header("Attack Settings")]
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRadius = 1.5f;

    // 🔹 EVENTO QUE SE DISPARA CUANDO EL BOSS DESPIERTA
    public System.Action OnBossAwaken;

    private BossStateMachine _stateMachine;
    private BossMovement _movement;
    private BossAttack _attack;
    private Animator _animator;
    private Rigidbody2D _rb;

    public BossMovement Movement => _movement;
    public BossAttack Attack => _attack;
    public BossTrigger Trigger => _trigger;
    public Animator Animator => _animator;
    public BossData Data => _data;
    public Transform Player => _player;
    public Transform AttackPoint => _attackPoint;
    public float AttackRadius => _attackRadius;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        ResetAnimatorParameters();

        if (_player == null)
            _player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (_spriteTransform == null)
            _spriteTransform = transform;

        if (_trigger == null)
            _trigger = GetComponentInChildren<BossTrigger>();

        if (_trigger == null)
            Debug.LogError("¡BossTrigger no encontrado! Asigna manualmente el GameObject con el script.");

        if (_attackPoint == null)
            _attackPoint = transform;

        _movement = new BossMovement(_rb, transform, _data.moveSpeed, _spriteTransform, _spriteDefaultFacingRight);
        _attack = new BossAttack(_data.attackCooldown, _data.attackDamage, _player, transform, _attackPoint, _attackRadius);

        _stateMachine = new BossStateMachine();
        _stateMachine.AddState(new DormantState(_stateMachine, this));
        _stateMachine.AddState(new AwakeState(_stateMachine, this));
        _stateMachine.AddState(new IdleState(_stateMachine, this));
        _stateMachine.AddState(new WalkState(_stateMachine, this));
        _stateMachine.AddState(new AttackState(_stateMachine, this));
        _stateMachine.AddState(new DeathState(_stateMachine, this));

        _stateMachine.ChangeState<DormantState>();
    }

    private void ResetAnimatorParameters()
    {
        _animator.SetBool("IsDormant", false);
        _animator.SetBool("IsAwake", false);
        _animator.SetBool("IsIdle", false);
        _animator.SetBool("IsWalking", false);
        _animator.SetBool("IsAttacking", false);
        _animator.SetBool("IsDead", false);
    }

    private void Update() => _stateMachine.Update();
    private void FixedUpdate() => _stateMachine.FixedUpdate();

    // 🔹 MÉTODO PARA FORZAR UNA ANIMACIÓN ESPECÍFICA
    public void ForceAnimation(string stateName, float normalizedTime = 0f, int layer = 0)
    {
        _animator.Play(stateName, layer, normalizedTime);
        Debug.Log($"🎬 Forzando animación: {stateName}");
    }

    // 🔹 MÉTODO PARA ACTUALIZAR PARÁMETROS Y FORZAR TRANSICIÓN
    public void TransitionToAnimation(string stateName, string boolParam, bool value)
    {
        _animator.SetBool(boolParam, value);
        ForceAnimation(stateName);
        Debug.Log($"🔄 Transición forzada a: {stateName} ( {boolParam} = {value} )");
    }

    // 🔹 MÉTODO PARA MATAR AL BOSS
    public void Die() => _stateMachine.ChangeState<DeathState>();

    // 🔹 MÉTODO PARA DISPARAR EL EVENTO DE DESPERTAR
    public void TriggerBossAwaken()
    {
        Debug.Log("🔊 ¡Boss despierto! Notificando a la UI...");
        OnBossAwaken?.Invoke();
    }

    // 🔹 MÉTODO PARA VERIFICAR SI EL JUGADOR ESTÁ EN EL ÁREA DE ATAQUE
    public bool IsPlayerInAttackRange()
    {
        if (_player == null)
        {
            Debug.LogWarning("❌ Player es NULL en BossController");
            return false;
        }

        float distance = Vector2.Distance(_attackPoint.position, _player.position);
        bool inRange = distance <= _attackRadius;

        if (Time.frameCount % 30 == 0)
        {
            Debug.Log($"📏 Distancia al jugador: {distance:F2} | Radio de ataque: {_attackRadius} | ¿En rango? {inRange}");
        }

        return inRange;
    }

    // 🔹 MÉTODO PARA APLICAR DAÑO AL JUGADOR (LLAMADO DESDE ANIMATION EVENT)
    public void ApplyDamageToPlayer()
    {
        if (_player == null)
        {
            Debug.LogWarning("❌ Player es NULL en ApplyDamageToPlayer");
            return;
        }

        float distance = Vector2.Distance(_attackPoint.position, _player.position);
        Debug.Log($"💥 Aplicando daño: Distancia = {distance:F2} | Radio = {_attackRadius}");

        if (distance <= _attackRadius)
        {
            PlayerController playerController = _player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(_data.attackDamage, transform);
                Debug.Log($"💥 ¡Jugador recibe {_data.attackDamage} de daño! Vida restante: {playerController.GetCurrentHealth()}");
            }
            else
            {
                Debug.LogWarning("❌ El jugador no tiene componente PlayerController");
            }
        }
        else
        {
            Debug.Log($"❌ Jugador fuera del área de ataque (Distancia: {distance:F2} > {_attackRadius})");
        }
    }

    // 🔹 MÉTODO DE DIAGNÓSTICO PARA VER PARÁMETROS DEL ANIMATOR
    public void DebugAnimatorParameters()
    {
        Debug.Log($"📊 Animator Params - Dormant: {_animator.GetBool("IsDormant")}, " +
                  $"Awake: {_animator.GetBool("IsAwake")}, " +
                  $"Idle: {_animator.GetBool("IsIdle")}, " +
                  $"Walking: {_animator.GetBool("IsWalking")}, " +
                  $"Attacking: {_animator.GetBool("IsAttacking")}, " +
                  $"Dead: {_animator.GetBool("IsDead")}");
    }

    // 🔹 DIBUJAR EL GIZMO DE ATAQUE EN EL EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null) return;

        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
        Gizmos.DrawSphere(_attackPoint.position, _attackRadius);

        Gizmos.color = new Color(1f, 0.5f, 0f, 1f);
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, _attackPoint.position);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_attackPoint.position, 0.15f);
    }
}