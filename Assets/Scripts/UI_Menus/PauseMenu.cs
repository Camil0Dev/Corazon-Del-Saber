using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 🔹 Requerido para manipular los Sliders

public class PauseMenu : MonoBehaviour
{
    [Header("Paneles de UI")]
    [Tooltip("Arrastra aquí el GameObject del PausePanel")]
    [SerializeField] private GameObject pausePanel;
    
    [Tooltip("Arrastra aquí el Panel de Opciones que está en este Canvas")]
    [SerializeField] private GameObject optionsPanel;

    [Header("Sliders de Audio (Opciones)")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

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

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            if (masterSlider != null) masterSlider.value = AudioManager.Instance.GetMasterVolume();
            if (musicSlider != null) musicSlider.value = AudioManager.Instance.GetMusicVolume();
            if (sfxSlider != null) sfxSlider.value = AudioManager.Instance.GetSFXVolume();

            if (masterSlider != null) 
                masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
            
            if (musicSlider != null) 
                musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
            
            if (sfxSlider != null) 
                sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
        }
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