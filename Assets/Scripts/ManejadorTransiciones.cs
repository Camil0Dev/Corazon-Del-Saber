using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ManejadorTransiciones : MonoBehaviour
{
    // Esta variable estática nos permite llamarlo desde CUALQUIER otro script del juego
    public static ManejadorTransiciones Instancia;

    [Header("Configuración")]
    public Image panelNegro;
    public float duracionFade = 1f;

    private void Awake()
    {
        // El patrón Singleton: Asegura que solo exista UN manejador de transiciones en todo el juego
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject); // ¡Esta es la magia! Hace que no se destruya al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Si ya hay uno y cargamos la escena inicial, destruimos la copia
        }
    }

    private void Start()
    {
        // Al arrancar el juego, hacemos un Fade In (de negro a transparente)
        StartCoroutine(FadeIn());
    }

    // Esta es la función que llamarás desde tus botones o puertas
    public void CargarEscena(string nombreEscena)
    {
        StartCoroutine(Transicion(nombreEscena));
    }

    private IEnumerator Transicion(string nombreEscena)
    {
        // 1. FADE OUT (La pantalla se oscurece)
        panelNegro.raycastTarget = true;
        float tiempo = 0f;
        Color color = panelNegro.color;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            color.a = tiempo / duracionFade;
            panelNegro.color = color;
            yield return null;
        }

        // 2. CARGAR LA NUEVA ESCENA (El juego cambia de nivel en secreto detrás de la pantalla negra)
        SceneManager.LoadScene(nombreEscena);

        // 3. FADE IN (La pantalla se aclara y revela el nuevo nivel)
        tiempo = 0f;
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            color.a = 1f - (tiempo / duracionFade);
            panelNegro.color = color;
            yield return null;
        }

        panelNegro.raycastTarget = false;
    }

    private IEnumerator FadeIn()
    {
        panelNegro.raycastTarget = true;
        float tiempo = 0f;
        Color color = panelNegro.color;
        color.a = 1f; // Empieza 100% negro
        panelNegro.color = color;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            color.a = 1f - (tiempo / duracionFade);
            panelNegro.color = color;
            yield return null;
        }
        panelNegro.raycastTarget = false;
    }
}