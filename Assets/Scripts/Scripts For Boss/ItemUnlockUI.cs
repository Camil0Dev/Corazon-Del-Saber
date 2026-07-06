using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class ItemUnlockUI : MonoBehaviour
{
    public static ItemUnlockUI Instance;

    [Header("Referencias UI")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Image itemIcon;
    
    // 🔹 CAMBIAMOS EL TIPO DE DATO A TextMeshProUGUI
    [SerializeField] private TextMeshProUGUI itemNameText; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (popupPanel != null) popupPanel.SetActive(false);
    }

    public void ShowUnlock(Sprite icon, string message)
    {
        StartCoroutine(ShowRoutine(icon, message));
    }

    private IEnumerator ShowRoutine(Sprite icon, string message)
    {
        if (itemIcon != null) itemIcon.sprite = icon;
        if (itemNameText != null) itemNameText.text = message;
        
        popupPanel.SetActive(true);
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(0.5f);

        yield return new WaitUntil(() => Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame);

        // Descongelamos y ocultamos
        Time.timeScale = 1f;
        popupPanel.SetActive(false);
    }
}