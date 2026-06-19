using UnityEngine;
using Unity.Cinemachine;

public class CameraTransition : MonoBehaviour
{
    public CinemachineCamera cameraA;
    public CinemachineCamera cameraB;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (cameraA.Priority > cameraB.Priority)
        {
            cameraA.Priority = 10;
            cameraB.Priority = 20;
        }
        else
        {
            cameraB.Priority = 10;
            cameraA.Priority = 20;
        }
    }
}