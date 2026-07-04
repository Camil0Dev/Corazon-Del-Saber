using UnityEngine;

public class BossAttack
{
    private float _cooldown;
    private int _damage;
    private float _lastAttackTime;
    private Transform _player;
    private Transform _bossTransform;
    private Transform _attackPoint;
    private float _attackRadius;

    public BossAttack(float cooldown, int damage, Transform player, Transform bossTransform, Transform attackPoint, float attackRadius)
    {
        _cooldown = cooldown;
        _damage = damage;
        _lastAttackTime = -cooldown;
        _player = player;
        _bossTransform = bossTransform;
        _attackPoint = attackPoint;
        _attackRadius = attackRadius;
    }

    public bool CanAttack() => Time.time >= _lastAttackTime + _cooldown;

    public void PerformAttack()
    {
        if (!CanAttack()) return;
        _lastAttackTime = Time.time;
    }

    public void ApplyDamage()
    {
        if (_player == null) return;

        float distance = Vector2.Distance(_attackPoint.position, _player.position);

        if (distance <= _attackRadius && _player.TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.TakeDamage(_damage, _bossTransform);
        }
    }
}