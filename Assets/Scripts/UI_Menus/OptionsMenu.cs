using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Sliders de Audio")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            if (masterSlider != null) 
                masterSlider.value = AudioManager.Instance.GetMasterVolume();
            
            if (musicSlider != null) 
                musicSlider.value = AudioManager.Instance.GetMusicVolume();
            
            if (sfxSlider != null) 
                sfxSlider.value = AudioManager.Instance.GetSFXVolume();

            if (masterSlider != null) 
                masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
            
            if (musicSlider != null) 
                musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
            
            if (sfxSlider != null) 
                sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
        }
        else
        {
            Debug.LogWarning("¡AudioManager no encontrado! Asegúrate de arrancar desde la Pantalla de Entrada.");
        }
    }
}