using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class TreeMonument : MonoBehaviour
{
    [Header("Visuales")]
    public SpriteRenderer spriteRenderer;
    public Sprite treeWithOrbSprite;

    [Header("UI y Flujo")]
    public GameObject interactText;
    public GameObject lorePanel;
    public GameObject endDemoPanel;
    [Tooltip("NUEVO: Panel para cuando interactúas pero aún no tienes el orbe")]
    public GameObject incompletePanel; 

    [Header("Tiempos")]
    [Tooltip("Segundos antes de permitir presionar una tecla para salir al menú")]
    public float delayBeforeEndDemoClick = 2.5f; 

    private bool alreadyActivated = false;
    
    private bool isShowingLore = false;
    private bool isShowingIncomplete = false;
    private bool canExitDemo = false;

    private void Start()
    {
        if (interactText) interactText.SetActive(false);
        if (lorePanel) lorePanel.SetActive(false);
        if (endDemoPanel) endDemoPanel.SetActive(false);
        if (incompletePanel) incompletePanel.SetActive(false);
    }

    private void Update()
    {
        if (WasAnyKeyPressedThisFrame())
        {
            if (isShowingLore)
            {
                isShowingLore = false;
                StartCoroutine(TransitionToEndDemo());
            }
            else if (isShowingIncomplete)
            {
                isShowingIncomplete = false;
                if (incompletePanel) incompletePanel.SetActive(false);
                Time.timeScale = 1f; // Restaurar el tiempo
            }
            else if (canExitDemo)
            {
                canExitDemo = false;
                ReturnToMainMenu();
            }
        }
    }

    public void InteractMonument()
    {
        if (alreadyActivated || isShowingLore || isShowingIncomplete || canExitDemo) return;

        if (interactText) interactText.SetActive(false);

        if (PlayerPrefs.GetInt("HasBlueOrb", 0) == 1)
        {
            alreadyActivated = true;
            Time.timeScale = 0f; // Pausa el mundo

            if (spriteRenderer != null && treeWithOrbSprite != null)
                spriteRenderer.sprite = treeWithOrbSprite;

            if (lorePanel != null) lorePanel.SetActive(true);
            
            StartCoroutine(EnableLoreSkip());
        }
        else
        {
            if (incompletePanel != null)
            {
                incompletePanel.SetActive(true);
                Time.timeScale = 0f;
                StartCoroutine(EnableIncompleteSkip());
            }
        }
    }

    private IEnumerator EnableLoreSkip()
    {
        yield return new WaitForSecondsRealtime(0.5f); 
        isShowingLore = true;
    }

    private IEnumerator EnableIncompleteSkip()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        isShowingIncomplete = true;
    }

    private IEnumerator TransitionToEndDemo()
    {
        if (lorePanel != null) lorePanel.SetActive(false);
        if (endDemoPanel != null) endDemoPanel.SetActive(true);

        yield return new WaitForSecondsRealtime(delayBeforeEndDemoClick);
        canExitDemo = true;
    }

    private bool WasAnyKeyPressedThisFrame()
    {
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) return true;
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) return true;
        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame) return true;
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !alreadyActivated)
        {
            if (interactText) interactText.SetActive(true);
            if (collision.TryGetComponent<PlayerController>(out var player))
            {
                player.SetCurrentMonument(this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (interactText) interactText.SetActive(false);
            if (collision.TryGetComponent<PlayerController>(out var player))
            {
                player.ClearCurrentMonument();
            }
        }
    }

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MenuPrincipal");
    }
}