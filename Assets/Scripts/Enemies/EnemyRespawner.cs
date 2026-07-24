using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyRespawner : MonoBehaviour
{
    private Vector3 startPosition;
    private Enemy enemyScript;

    private void Awake()
    {
        startPosition = transform.position;
        enemyScript = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        Bench.OnBenchRested += Respawn;
    }

    private void OnDisable()
    {
        Bench.OnBenchRested -= Respawn;
    }

    private void Respawn()
    {
        transform.position = startPosition;
        
        gameObject.SetActive(true);
        
        if (enemyScript != null)
        {
            enemyScript.ResetHealth();
        }
    }
}