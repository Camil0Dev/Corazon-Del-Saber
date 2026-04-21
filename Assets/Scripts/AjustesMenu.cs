using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AjustesMenu : MonoBehaviour
{
    [Header("Configuración de Audio")]
    public AudioMixer mezcladorAudio;

    [Header("Configuración de Resolución")]
    public TMP_Text textoResolucion;
    public Toggle togglePantalla;

    private Vector2Int[] resoluciones = new Vector2Int[]
    {
        new Vector2Int(1280, 720),
        new Vector2Int(1600, 900),
        new Vector2Int(1920, 1080)
    };

    private int indiceActual = 2;

    void Start()
    {
        // 1. Cargar las opciones guardadas (Si es la primera vez, usa FullScreen y 1080p por defecto)
        int resolucionGuardada = PlayerPrefs.GetInt("ResIndex", 2);
        bool pantallaCompletaGuardada = PlayerPrefs.GetInt("FullScreen", 1) == 1;

        indiceActual = resolucionGuardada;

        // 2. Aplicar la resolución y pantalla al arrancar
        Screen.SetResolution(resoluciones[indiceActual].x, resoluciones[indiceActual].y, pantallaCompletaGuardada);
        ActualizarTextoPantalla();

        // 3. Actualizar el Toggle SIN disparar el evento de clic
        if (togglePantalla != null)
        {
            togglePantalla.SetIsOnWithoutNotify(pantallaCompletaGuardada);
        }
    }

    public void CambiarVolumenMusica(float valor)
    {
        mezcladorAudio.SetFloat("VolumenMusica", Mathf.Log10(valor) * 20);
    }

    public void CambiarVolumenMaestro(float valor)
    {
        mezcladorAudio.SetFloat("VolumenMaestro", Mathf.Log10(valor) * 20);
    }

    public void ActivarPantallaCompleta(bool esCompleta)
    {
        Screen.fullScreen = esCompleta;

        // Guardar la preferencia del jugador
        PlayerPrefs.SetInt("FullScreen", esCompleta ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ResolucionSiguiente()
    {
        if (indiceActual < resoluciones.Length - 1) { indiceActual++; AplicarResolucion(); }
    }

    public void ResolucionAnterior()
    {
        if (indiceActual > 0) { indiceActual--; AplicarResolucion(); }
    }

    private void AplicarResolucion()
    {
        ActualizarTextoPantalla();
        Screen.SetResolution(resoluciones[indiceActual].x, resoluciones[indiceActual].y, Screen.fullScreen);

        // Guardar la preferencia del jugador
        PlayerPrefs.SetInt("ResIndex", indiceActual);
        PlayerPrefs.Save();
    }

    private void ActualizarTextoPantalla()
    {
        if (textoResolucion != null)
            textoResolucion.text = resoluciones[indiceActual].x + " x " + resoluciones[indiceActual].y;
    }
}