using UnityEngine;

public class SimpleParallax : MonoBehaviour
{
    [Tooltip("0 = Se mueve igual que la cámara (pegado). 1 = No se mueve (lejos). 0.5 = Efecto 3D")]
    [SerializeField] private float parallaxEffect = 0.8f; 
    
    private Transform cam;
    private Vector3 lastCamPos;

    private void Start()
    {
        // Encontramos la cámara principal automáticamente
        cam = Camera.main.transform;
        lastCamPos = cam.position;
    }

    private void LateUpdate()
    {
        // Calculamos cuánto se movió la cámara en este frame
        Vector3 deltaMovement = cam.position - lastCamPos;
        
        // Movemos el fondo en base a ese movimiento y el multiplicador
        transform.position += deltaMovement * parallaxEffect;
        
        lastCamPos = cam.position;
    }
}