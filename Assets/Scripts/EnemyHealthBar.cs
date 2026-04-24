using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealthBar : MonoBehaviour
{
    #region Variables: Configuración
    [Header("Referencias Visuales")]
    [SerializeField] private Image fillImage;     // La imagen con Fill Method: Horizontal
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Ajustes de Visibilidad")]
    [SerializeField] private float fadeSpeed = 2f;
    [SerializeField] private float timeVisible = 3f;
    #endregion

    #region Variables: Estado Interno
    private Coroutine fadeCoroutine;
    private Transform cam;
    #endregion

    #region Ciclo de Vida
    private void Awake()
    {
        // Cache de la cámara principal
        if (Camera.main != null) cam = Camera.main.transform;

        // Auto-asignación de CanvasGroup si se olvidó en el inspector
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();

        // Iniciar invisible
        if (canvasGroup != null) canvasGroup.alpha = 0;
    }

    private void LateUpdate()
    {
        // Billboard: Mantiene la barra siempre orientada hacia la cámara
        if (cam != null)
        {
            transform.LookAt(transform.position + cam.forward);
        }
    }
    #endregion

    #region Lógica de Interfaz
    /// <summary>
    /// Actualiza el porcentaje de la barra y gestiona el ciclo de visibilidad.
    /// </summary>
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        // 1. Actualizar el relleno visual
        if (fillImage != null)
        {
            // Mathf.Clamp01 asegura que el valor esté entre 0 y 1 para evitar errores visuales
            fillImage.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
        }

        // 2. Controlar el desvanecimiento (Fade)
        if (canvasGroup != null)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(HealthBarLifeCycle());
        }
    }

    private IEnumerator HealthBarLifeCycle()
    {
        // Mostrar instantáneamente al recibir daño
        canvasGroup.alpha = 1;

        // Esperar el tiempo configurado
        yield return new WaitForSeconds(timeVisible);

        // Desvanecimiento suave
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        canvasGroup.alpha = 0;
    }
    #endregion
}