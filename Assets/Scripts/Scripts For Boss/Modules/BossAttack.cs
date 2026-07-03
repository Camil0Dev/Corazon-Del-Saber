using UnityEngine;

public class BossAttack
{
    private float _cooldown;
    private int _damage;
    private float _lastAttackTime;
    private Transform _player;
    private Transform _bossTransform;
    private Transform _attackPoint; // 🔹 NUEVO: Punto de ataque
    private float _attackRadius;    // 🔹 NUEVO: Radio de ataque

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
        if (!CanAttack())
        {
            Debug.Log("⏳ Ataque en cooldown");
            return;
        }

        _lastAttackTime = Time.time;
        Debug.Log($"⚔️ ¡Boss ataca! Daño: {_damage}");

        // 🔹 AHORA EL DAÑO SE APLICA DESDE EL ANIMATION EVENT
        // Este método solo registra el ataque y actualiza el cooldown
    }

    // 🔹 MÉTODO PARA APLICAR DAÑO (llamado desde BossController)
    public void ApplyDamage()
    {
        if (_player == null)
        {
            Debug.LogWarning("❌ Player es NULL en BossAttack");
            return;
        }

        float distance = Vector2.Distance(_attackPoint.position, _player.position);
        Debug.Log($"💥 BossAttack: Distancia = {distance:F2} | Radio = {_attackRadius}");

        if (distance <= _attackRadius)
        {
            PlayerController playerController = _player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(_damage, _bossTransform);
                Debug.Log($"💥 ¡Jugador recibe {_damage} de daño!");
            }
            else
            {
                Debug.LogWarning("❌ El jugador no tiene PlayerController");
            }
        }
        else
        {
            Debug.Log($"❌ Jugador fuera del área de ataque (Distancia: {distance:F2} > {_attackRadius})");
        }
    }
}