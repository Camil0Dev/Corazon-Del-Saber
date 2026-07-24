using UnityEngine;
using UnityEngine.Rendering.Universal; // Necesario para Light 2D

[RequireComponent(typeof(Light2D))]
public class FlickerLight2D : MonoBehaviour
{
    private Light2D light2D;
    private float baseIntensity;

    [Header("Configuración del Parpadeo")]
    [Tooltip("Intensidad mínima que alcanzará la luz")]
    [SerializeField] private float minIntensity = 0.5f;
    [Tooltip("Intensidad máxima que alcanzará la luz")]
    [SerializeField] private float maxIntensity = 1.5f;
    [Tooltip("Velocidad del parpadeo")]
    [SerializeField] private float speed = 3f;

    private void Awake()
    {
        light2D = GetComponent<Light2D>();
        baseIntensity = light2D.intensity;
    }

    void Update()
    {
        // Usa una onda senoidal combinada con el tiempo para crear un ciclo suave de luz
        float noise = Mathf.Sin(Time.time * speed) * 0.5f + 0.5f; // Valor entre 0 y 1
        light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}