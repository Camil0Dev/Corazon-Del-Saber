using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Paneles de UI")]
    [Tooltip("Arrastra aquí el GameObject del PausePanel")]
    [SerializeField] private GameObject pausePanel;
    
    [Tooltip("Arrastra aquí el Panel de Opciones que está en este Canvas")]
    [SerializeField] private GameObject optionsPanel;

    private PlayerControls controls;
    private InputAction pauseAction;

    private bool isPaused = false;

    private void Awake()
    {
        controls = new PlayerControls();
        
        pauseAction = controls.Gameplay.Pause;
        pauseAction.performed += TogglePause;

        if (pausePanel != null) pausePanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    private void TogglePause(InputAction.CallbackContext context)
    {
        if (optionsPanel != null && optionsPanel.activeSelf)
        {
            CloseOptions();
            return; 
        }

        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        
        if (pausePanel != null) pausePanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
            
        controls.Gameplay.Enable();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        
        if (pausePanel != null) pausePanel.SetActive(true);
            
        controls.Gameplay.Disable();
    }

    public void OpenOptions()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(true);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene("MenuPrincipal");
    }

    private void OnDestroy()
    {
        if (pauseAction != null)
        {
            pauseAction.performed -= TogglePause;
        }
    }
}