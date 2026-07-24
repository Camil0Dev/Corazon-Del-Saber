using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    [SerializeField] string spawnID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.nextSpawn = spawnID;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}