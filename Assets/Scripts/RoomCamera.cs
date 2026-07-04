using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RoomCamera : MonoBehaviour
{
    [Header("Cámara Virtual")]
    [SerializeField] private GameObject virtualCamera;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            if (virtualCamera != null && !virtualCamera.activeSelf)
            {
                // Disparamos el parpadeo negro global
                if (ScreenFader.Instance != null) 
                    ScreenFader.Instance.Blink();

                virtualCamera.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            if (virtualCamera != null)
                virtualCamera.SetActive(false);
        }
    }
}