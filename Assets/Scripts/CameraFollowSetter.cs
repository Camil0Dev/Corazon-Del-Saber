using UnityEngine;
using Unity.Cinemachine;

public class CameraFollowSetter : MonoBehaviour
{
    void Start()
    {
        CinemachineCamera cam = GetComponent<CinemachineCamera>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player != null)
            cam.Follow = player.transform;
    }
}