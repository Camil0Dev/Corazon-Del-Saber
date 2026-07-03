using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private BossData _data;
    [SerializeField] private BossController _controller;

    private int _currentHealth;
    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _data.maxHealth;

    // Evento que se dispara cuando el boss muere
    public System.Action OnDeath;
    // Evento que se dispara cuando el boss recibe daño
    public System.Action<int, int> OnDamageTaken; // (vidaActual, vidaMaxima)

    private void Awake()
    {
        if (_data == null)
        {
            Debug.LogError("❌ BossHealth: BossData no asignado");
            return;
        }

        if (_controller == null)
            _controller = GetComponent<BossController>();

        _currentHealth = _data.maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (_controller == null || _controller.Animator.GetBool("IsDead"))
            return;

        _currentHealth = Mathf.Max(0, _currentHealth - damage);
        Debug.Log($"💥 Boss recibe {damage} de daño. Vida restante: {_currentHealth}/{_data.maxHealth}");

        // Disparar evento de daño (para actualizar UI, camera shake, etc.)
        OnDamageTaken?.Invoke(_currentHealth, _data.maxHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("💀 Boss derrotado");
        OnDeath?.Invoke();
        _controller.Die();
    }

    public void ResetHealth()
    {
        _currentHealth = _data.maxHealth;
        OnDamageTaken?.Invoke(_currentHealth, _data.maxHealth);
    }

    public float GetHealthPercentage()
    {
        return (float)_currentHealth / _data.maxHealth;
    }
}