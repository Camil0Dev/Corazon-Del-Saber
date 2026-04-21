using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EfectoEntrada : MonoBehaviour
{
    public Image panelTransicion;
    public float duracionFade = 1f;

    void Start()
    {
        if (panelTransicion != null)
        {
            // Iniciamos con el panel en negro
            panelTransicion.gameObject.SetActive(true);
            StartCoroutine(AclararPantalla());
        }
    }

    IEnumerator AclararPantalla()
    {
        Color colorActual = panelTransicion.color;
        float tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            // Reducimos el Alpha de 1 a 0
            colorActual.a = 1f - (tiempo / duracionFade);
            panelTransicion.color = colorActual;
            yield return null;
        }

        // Al terminar, desactivamos el panel para que no interfiera
        panelTransicion.gameObject.SetActive(false);
    }
}