using UnityEngine;

public class BossArenaTrigger : MonoBehaviour
{
    [Header("Barreras de la Arena")]
    [Tooltip("Arrastra aquí los GameObjects de las barreras invisibles")]
    [SerializeField] private GameObject[] doors; 

    [Header("El Jefe")]
    [Tooltip("Arrastra aquí el GameObject del Boss")]
    [SerializeField] private GameObject bossObject; 

    [Header("Audio Épico")]
    [Tooltip("La canción que sonará durante el combate")]
    [SerializeField] private AudioClip bossMusic;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            foreach (GameObject door in doors)
            {
                if (door != null) door.SetActive(true);
            }

            if (bossObject != null)
            {
                bossObject.SetActive(true);
            }

            if (AudioManager.Instance != null && bossMusic != null)
            {
                AudioManager.Instance.PlayMusic(bossMusic);
            }

            gameObject.SetActive(false);
        }
    }
}