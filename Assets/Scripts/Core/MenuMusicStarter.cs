using UnityEngine;

public class MenuMusicStarter : MonoBehaviour
{
    [Tooltip("Arrastra aquí el clip de audio MusicLoopMenu")]
    [SerializeField] private AudioClip menuMusicClip;

    private void Start()
    {
        if (AudioManager.Instance != null && menuMusicClip != null)
        {
            AudioManager.Instance.PlayMusic(menuMusicClip);
        }
    }
}