using UnityEngine;

public class SantuarioAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip santuarioMusic;
    
    private void Start()
    {
        if (AudioManager.Instance != null && santuarioMusic != null)
        {
            AudioManager.Instance.PlayMusic(santuarioMusic);
        }
    }
}