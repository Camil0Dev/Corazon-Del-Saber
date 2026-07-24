using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [Header("Configuración de Generación")]
    [Tooltip("Arrastra aquí el Prefab del enemigo (ej: Velumbra o FloatingEnemy)")]
    public GameObject enemyPrefab;

    [Tooltip("¿Hacia dónde debe mirar al aparecer? (1 = Derecha, -1 = Izquierda)")]
    [Range(-1, 1)]
    public int direccionInicial = 1;

    private void Start()
    {
        if (enemyPrefab != null)
        {
            GameObject spawnedEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            
            if (direccionInicial == -1)
            {
                Vector3 scale = spawnedEnemy.transform.localScale;
                scale.x *= -1;
                spawnedEnemy.transform.localScale = scale;
            }

            spawnedEnemy.transform.SetParent(transform.parent);
            
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawSphere(transform.position, 0.4f);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 1f);
    }
}