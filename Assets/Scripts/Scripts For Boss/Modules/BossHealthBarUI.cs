using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _healthBarFill;
    [SerializeField] private BossHealth _bossHealth;
    [SerializeField] private BossController _bossController;
    [SerializeField] private GameObject _bossHealthBarContainer;

    [Header("Settings")]
    [SerializeField] private bool _showOnlyInCombat = true;

    private void Start()
    {
        // Buscar referencias si no están asignadas
        if (_bossHealth == null)
            _bossHealth = Object.FindAnyObjectByType<BossHealth>();

        if (_bossController == null)
            _bossController = Object.FindAnyObjectByType<BossController>();

        if (_bossHealth != null)
        {
            // Suscribirse al evento de daño
            _bossHealth.OnDamageTaken += UpdateHealthBar;
            
            // Inicializar la barra
            UpdateHealthBar(_bossHealth.CurrentHealth, _bossHealth.MaxHealth);
        }
        else
        {
            Debug.LogWarning("⚠️ BossHealthBarUI: No se encontró BossHealth en la escena.");
        }

        // 🔹 SUSCRIBIRSE AL EVENTO DE DESPERTAR DEL BOSS
        if (_bossController != null)
        {
            _bossController.OnBossAwaken += ShowBar;
            Debug.Log("📊 BossHealthBarUI: Suscrito a OnBossAwaken");
        }
        else
        {
            Debug.LogWarning("⚠️ BossHealthBarUI: No se encontró BossController en la escena.");
        }

        // 🔹 OCULTAR LA BARRA AL INICIO
        if (_bossHealthBarContainer != null)
        {
            _bossHealthBarContainer.SetActive(false);
            Debug.Log("📊 Barra de vida del boss OCULTA al inicio");
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (_healthBarFill != null)
        {
            float fillAmount = (float)currentHealth / maxHealth;
            _healthBarFill.fillAmount = fillAmount;
            
            // Cambiar de color según la vida
            if (fillAmount > 0.5f)
                _healthBarFill.color = Color.green;
            else if (fillAmount > 0.25f)
                _healthBarFill.color = Color.yellow;
            else
                _healthBarFill.color = Color.red;
        }

        // Ocultar la barra si el boss ha muerto
        if (_bossHealthBarContainer != null && _showOnlyInCombat)
        {
            _bossHealthBarContainer.SetActive(currentHealth > 0);
            if (currentHealth <= 0)
                Debug.Log("📊 Barra de vida del boss OCULTA (boss muerto)");
        }
    }

    // 🔹 MÉTODO PARA MOSTRAR LA BARRA (llamado cuando el boss despierta)
    public void ShowBar()
    {
        if (_bossHealthBarContainer != null)
        {
            _bossHealthBarContainer.SetActive(true);
            Debug.Log("📊 Barra de vida del boss MOSTRADA (boss despierto)");
        }
    }

    // 🔹 MÉTODO PARA OCULTAR LA BARRA
    public void HideBar()
    {
        if (_bossHealthBarContainer != null)
        {
            _bossHealthBarContainer.SetActive(false);
            Debug.Log("📊 Barra de vida del boss OCULTA");
        }
    }

    private void OnDestroy()
    {
        // Desuscribirse para evitar errores
        if (_bossHealth != null)
            _bossHealth.OnDamageTaken -= UpdateHealthBar;
        
        if (_bossController != null)
            _bossController.OnBossAwaken -= ShowBar;
        
        Debug.Log("📊 BossHealthBarUI: Limpiando suscripciones");
    }
}