using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 🔹 Necesario para manipular la imagen negra
using System.Collections; // 🔹 Necesario para las Corrutinas
using System;

public class Bench : MonoBehaviour
{
    [Header("Referencias")]
    public Transform sitPoint;
    public GameObject interactText;
    
    [Header("Audio")]
    public AudioClip benchSound;

    [Header("Efecto Visual (Parpadeo)")]
    [Tooltip("Arrastra aquí una Imagen UI negra")]
    public Image fadeImage;
    [Tooltip("Cuánto tarda en oscurecerse y aclararse")]
    public float fadeDuration = 0.5f;

    public static event Action OnBenchRested;

    private void Start()
    {
        if (interactText != null) interactText.SetActive(false);
        
        // Nos aseguramos de que la pantalla negra empiece 100% transparente
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0);
            fadeImage.raycastTarget = false; // Evita que bloquee clics del menú
        }
    }

    public void ActivateBench(GameObject player)
    {
        if (interactText != null) interactText.SetActive(false);
        
        StartCoroutine(RestSequence(player));

        PlayerController jugador = player.GetComponent<PlayerController>();
        if (jugador != null)
        {
            jugador.RestoreFullHealth();
        }
    }

    private IEnumerator RestSequence(GameObject player)
    {
        if (fadeImage != null)
        {
            float timer = 0;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = timer / fadeDuration;
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null; 
            }
            fadeImage.color = new Color(0, 0, 0, 1);
        }

        player.transform.position = sitPoint.position;
        SaveGame(sitPoint.position);
        
        if (AudioManager.Instance != null && benchSound != null)
        {
            AudioManager.Instance.PlaySFX(benchSound, false);
        }
        
        OnBenchRested?.Invoke();
        Debug.Log("Banco activado: Partida Guardada en PlayerPrefs.");

        yield return new WaitForSeconds(0.2f);

        if (fadeImage != null)
        {
            float timer = 0;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = 1f - (timer / fadeDuration);
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
            fadeImage.color = new Color(0, 0, 0, 0);
        }
    }

    private void SaveGame(Vector3 savePosition)
    {
        PlayerPrefs.SetString("SavedScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("PlayerX", savePosition.x);
        PlayerPrefs.SetFloat("PlayerY", savePosition.y);
        PlayerPrefs.Save();
    }
}