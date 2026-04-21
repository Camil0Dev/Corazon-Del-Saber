using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuNavegacion : MonoBehaviour
{
    [Header("Configuración de Transición")]
    public Image panelTransicion;

    [Header("Paneles y UI")]
    public GameObject panelOpciones;
    public GameObject panelControles;
    public GameObject tituloPrincipal;
    public GameObject contenedorBotones;

    public void IrAlMenu()
    {
        if (panelTransicion != null)
        {
            StartCoroutine(TransicionFade());
        }
        else
        {
            SceneManager.LoadScene("MenuPrincipal");
        }
    }

    IEnumerator TransicionFade()
    {
        panelTransicion.raycastTarget = true;

        Color colorFinal = panelTransicion.color;
        float tiempo = 0f;
        float duracionTransicion = 1f;

        while (tiempo < duracionTransicion)
        {
            tiempo += Time.deltaTime;
            colorFinal.a = tiempo / duracionTransicion;
            panelTransicion.color = colorFinal;
            yield return null;
        }

        SceneManager.LoadScene("MenuPrincipal");
    }

    public void AbrirOpciones()
    {
        if (panelOpciones != null) panelOpciones.SetActive(true);

        // Limpiamos la pantalla
        if (tituloPrincipal != null) tituloPrincipal.SetActive(false);
        if (contenedorBotones != null) contenedorBotones.SetActive(false);
    }

    public void CerrarOpciones()
    {
        if (panelOpciones != null) panelOpciones.SetActive(false);

        // Restauramos el menú
        if (tituloPrincipal != null) tituloPrincipal.SetActive(true);
        if (contenedorBotones != null) contenedorBotones.SetActive(true);
    }

    public void AbrirControles()
    {
        if (panelControles != null) panelControles.SetActive(true);

        if (tituloPrincipal != null) tituloPrincipal.SetActive(false);
        if (contenedorBotones != null) contenedorBotones.SetActive(false);
    }

    public void CerrarControles()
    {
        if (panelControles != null) panelControles.SetActive(false);

        if (tituloPrincipal != null) tituloPrincipal.SetActive(true);
        if (contenedorBotones != null) contenedorBotones.SetActive(true);
    }

    public void SalirDelJuego()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit();
    }
}