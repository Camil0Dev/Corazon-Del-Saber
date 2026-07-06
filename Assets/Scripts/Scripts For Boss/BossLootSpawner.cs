using UnityEngine;

public class BossLootSpawner : MonoBehaviour
{
    [Header("Referencias del Jefe")]
    [Tooltip("Arrastra aquí el GameObject del Boss que tiene el script BossHealth")]
    [SerializeField] private BossHealth bossHealth;

    [Header("Loot (Prefabs)")]
    [SerializeField] private GameObject blueOrbPrefab;
    [SerializeField] private GameObject treasureChestPrefab;

    [Header("Puntos de Aparición (Spawns)")]
    [SerializeField] private Transform orbSpawnPoint;
    [SerializeField] private Transform chestSpawnPoint;

    private void OnEnable()
    {
        // Nos suscribimos al evento de muerte. 
        // Cuando el jefe muera, ejecutará nuestra función SpawnLoot automáticamente.
        if (bossHealth != null)
        {
            bossHealth.OnDeath += SpawnLoot;
        }
    }

    private void OnDisable()
    {
        // En C#, SIEMPRE debes desuscribirte de los eventos al apagar el script
        // para evitar fugas de memoria o errores de referencia nula.
        if (bossHealth != null)
        {
            bossHealth.OnDeath -= SpawnLoot;
        }
    }

    private void SpawnLoot()
    {
        // Instanciamos el Orbe Azul
        if (blueOrbPrefab != null && orbSpawnPoint != null)
        {
            Instantiate(blueOrbPrefab, orbSpawnPoint.position, Quaternion.identity);
        }

        // Instanciamos el Cofre con las Botas
        if (treasureChestPrefab != null && chestSpawnPoint != null)
        {
            Instantiate(treasureChestPrefab, chestSpawnPoint.position, Quaternion.identity);
        }
    }
}