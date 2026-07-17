using UnityEngine;
using System;

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

    public Action OnBossAwaken;

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
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) _player = playerObj.transform;
        }

        if (_spriteTransform == null) _spriteTransform = transform;
        
        if (_trigger == null) _trigger = GetComponentInChildren<BossTrigger>();

        if (_attackPoint == null) _attackPoint = transform;

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

    public void ForceAnimation(string stateName, float normalizedTime = 0f, int layer = 0)
    {
        _animator.Play(stateName, layer, normalizedTime);
    }

    public void Die() => _stateMachine.ChangeState<DeathState>();

    public void TriggerBossAwaken() => OnBossAwaken?.Invoke();

    public bool IsPlayerInAttackRange()
    {
        if (_player == null) return false;
        return Vector2.Distance(_attackPoint.position, _player.position) <= _attackRadius;
    }

    public void ApplyDamageToPlayer()
    {
        _attack.ApplyDamage();
    }

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