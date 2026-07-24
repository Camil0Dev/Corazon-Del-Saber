using UnityEngine;

public class SceneMusicManager : MonoBehaviour
{
    [Header("Música del Nivel")]
    [Tooltip("El clip de audio que debe sonar al entrar a esta escena")]
    [SerializeField] private AudioClip levelMusic;

    private void Start()
    {
        if (AudioManager.Instance != null && levelMusic != null)
        {
            AudioManager.Instance.PlayMusic(levelMusic);
        }
    }
}