using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AjustesMenu : MonoBehaviour
{
    [Header("Configuración de Resolución")]
    public TMP_Text textoResolucion;
    public Toggle togglePantalla;
    
    // public Slider sliderMaestro;
    // public Slider sliderMusica;
    // public Slider sliderSFX;

    private Vector2Int[] resoluciones = new Vector2Int[]
    {
        new Vector2Int(1280, 720),
        new Vector2Int(1600, 900),
        new Vector2Int(1920, 1080)
    };

    private int indiceActual = 2;

    void Start()
    {
        int resolucionGuardada = PlayerPrefs.GetInt("ResIndex", 2);
        bool pantallaCompletaGuardada = PlayerPrefs.GetInt("FullScreen", 1) == 1;

        indiceActual = Mathf.Clamp(resolucionGuardada, 0, resoluciones.Length - 1);

        Screen.SetResolution(resoluciones[indiceActual].x, resoluciones[indiceActual].y, pantallaCompletaGuardada);
        ActualizarTextoPantalla();

        if (togglePantalla != null)
        {
            togglePantalla.SetIsOnWithoutNotify(pantallaCompletaGuardada);
        }

        SincronizarSlidersAudio();
    }


    public void CambiarVolumenMaestro(float valor)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMasterVolume(valor);
        }
        PlayerPrefs.SetFloat("MasterVolume", valor);
        PlayerPrefs.Save();
    }

    public void CambiarVolumenMusica(float valor)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(valor);
        }
        PlayerPrefs.SetFloat("MusicVolume", valor);
        PlayerPrefs.Save();
    }

    public void CambiarVolumenSFX(float valor)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(valor);
        }
        PlayerPrefs.SetFloat("SFXVolume", valor);
        PlayerPrefs.Save();
    }

    private void SincronizarSlidersAudio()
    {
        if (AudioManager.Instance == null) return;

        /*
        if (sliderMaestro != null) sliderMaestro.value = AudioManager.Instance.GetMasterVolume();
        if (sliderMusica != null) sliderMusica.value = AudioManager.Instance.GetMusicVolume();
        if (sliderSFX != null) sliderSFX.value = AudioManager.Instance.GetSFXVolume();
        */
    }


    public void ActivarPantallaCompleta(bool esCompleta)
    {
        Screen.fullScreen = esCompleta;

        PlayerPrefs.SetInt("FullScreen", esCompleta ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ResolucionSiguiente()
    {
        if (indiceActual < resoluciones.Length - 1) 
        { 
            indiceActual++; 
            AplicarResolucion(); 
        }
    }

    public void ResolucionAnterior()
    {
        if (indiceActual > 0) 
        { 
            indiceActual--; 
            AplicarResolucion(); 
        }
    }

    private void AplicarResolucion()
    {
        ActualizarTextoPantalla();
        Screen.SetResolution(resoluciones[indiceActual].x, resoluciones[indiceActual].y, Screen.fullScreen);

        PlayerPrefs.SetInt("ResIndex", indiceActual);
        PlayerPrefs.Save();
    }

    private void ActualizarTextoPantalla()
    {
        if (textoResolucion != null)
            textoResolucion.text = resoluciones[indiceActual].x + " x " + resoluciones[indiceActual].y;
    }
}