using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealthBar : MonoBehaviour
{
    #region Variables: Configuraci�n
    [Header("Referencias Visuales")]
    [SerializeField] private Image fillImage;
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
        if (Camera.main != null) cam = Camera.main.transform;

        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup != null) canvasGroup.alpha = 0;
    }

    private void LateUpdate()
    {
        if (cam != null)
        {
            transform.LookAt(transform.position + cam.forward);
        }
    }
    #endregion

    #region L�gica de Interfaz

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
        }

        if (canvasGroup != null)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(HealthBarLifeCycle());
        }
    }

    private IEnumerator HealthBarLifeCycle()
    {
        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(timeVisible);

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        canvasGroup.alpha = 0;
    }
    #endregion
}