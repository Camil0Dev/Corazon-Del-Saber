using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ManejadorTransiciones : MonoBehaviour
{
    public static ManejadorTransiciones Instancia;

    [Header("Configuraci�n")]
    public Image panelNegro;
    public float duracionFade = 1f;

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void CargarEscena(string nombreEscena)
    {
        StartCoroutine(Transicion(nombreEscena));
    }

    private IEnumerator Transicion(string nombreEscena)
    {
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

        SceneManager.LoadScene(nombreEscena);

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
        color.a = 1f;
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