using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        if(GameManager.Instance != null)
        {
            GameObject spawn =
                GameObject.Find(GameManager.Instance.nextSpawn);

            if(spawn != null)
                transform.position = spawn.transform.position;
        }
    }
}