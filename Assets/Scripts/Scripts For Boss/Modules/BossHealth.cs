using UnityEngine;
using System;

public class BossHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private BossData _data;
    [SerializeField] private BossController _controller;

    private int _currentHealth;
    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _data.maxHealth;

    public Action OnDeath;
    public Action<int, int> OnDamageTaken;

    private void Awake()
    {
        if (_controller == null)
            TryGetComponent(out _controller);

        _currentHealth = _data.maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (_controller == null || _controller.Animator.GetBool("IsDead")) return;

        _currentHealth = Mathf.Max(0, _currentHealth - damage);
        OnDamageTaken?.Invoke(_currentHealth, _data.maxHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        _controller.Die();
    }

    public void ResetHealth()
    {
        _currentHealth = _data.maxHealth;
        OnDamageTaken?.Invoke(_currentHealth, _data.maxHealth);
    }

    public float GetHealthPercentage() => (float)_currentHealth / _data.maxHealth;
}