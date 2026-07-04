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
        if (_bossHealth != null)
        {
            _bossHealth.OnDamageTaken += UpdateHealthBar;
            UpdateHealthBar(_bossHealth.CurrentHealth, _bossHealth.MaxHealth);
        }

        if (_bossController != null)
        {
            _bossController.OnBossAwaken += ShowBar;
        }

        if (_bossHealthBarContainer != null)
        {
            _bossHealthBarContainer.SetActive(false);
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (_healthBarFill != null)
        {
            float fillAmount = (float)currentHealth / maxHealth;
            _healthBarFill.fillAmount = fillAmount;
            
            if (fillAmount > 0.5f) _healthBarFill.color = Color.green;
            else if (fillAmount > 0.25f) _healthBarFill.color = Color.yellow;
            else _healthBarFill.color = Color.red;
        }

        if (_bossHealthBarContainer != null && _showOnlyInCombat)
        {
            _bossHealthBarContainer.SetActive(currentHealth > 0);
        }
    }

    public void ShowBar()
    {
        if (_bossHealthBarContainer != null)
            _bossHealthBarContainer.SetActive(true);
    }

    public void HideBar()
    {
        if (_bossHealthBarContainer != null)
            _bossHealthBarContainer.SetActive(false);
    }

    private void OnDestroy()
    {
        if (_bossHealth != null)
            _bossHealth.OnDamageTaken -= UpdateHealthBar;
        
        if (_bossController != null)
            _bossController.OnBossAwaken -= ShowBar;
    }
}