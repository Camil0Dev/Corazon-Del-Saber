using UnityEngine;

public class BossLootSpawner : MonoBehaviour
{
    [Header("Referencias del Jefe")]
    [SerializeField] private BossHealth bossHealth;

    [Header("Loot (Prefabs)")]
    [SerializeField] private GameObject blueOrbPrefab;
    [SerializeField] private GameObject treasureChestPrefab;

    [Header("Punto del Cofre (Estático)")]
    [Tooltip("El cofre sí debe tener un punto fijo para evitar que aparezca dentro de una pared")]
    [SerializeField] private Transform chestSpawnPoint;

    [Header("Salida del Nivel")]
    [SerializeField] private GameObject exitTeleporter;

    [Header("Barreras para abrir")]
    [SerializeField] private GameObject[] doorsToOpen;

    private void OnEnable()
    {
        if (bossHealth != null) bossHealth.OnDeath += SpawnLoot;
    }

    private void OnDisable()
    {
        if (bossHealth != null) bossHealth.OnDeath -= SpawnLoot;
    }

    private void SpawnLoot()
    {
        if (bossHealth != null)
        {
            Collider2D[] bossColliders = bossHealth.GetComponentsInChildren<Collider2D>();
            foreach (Collider2D col in bossColliders)
            {
                col.enabled = false;
            }

            Rigidbody2D bossRb = bossHealth.GetComponent<Rigidbody2D>();
            if (bossRb != null) 
            {
                bossRb.bodyType = RigidbodyType2D.Static;
            }
            foreach (GameObject door in doorsToOpen)
            {
            if (door != null) door.SetActive(false);
            }
        }

        if (blueOrbPrefab != null && bossHealth != null)
        {
            Vector3 bossDeathPosition = bossHealth.transform.position;
            Instantiate(blueOrbPrefab, bossDeathPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("El orbe no se instanció porque falta el Prefab en el Inspector.");
        }

        // 🔹 3. EL COFRE
        if (treasureChestPrefab != null && chestSpawnPoint != null)
        {
            Instantiate(treasureChestPrefab, chestSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("¡Falta asignar el Prefab del Cofre o su Punto de Spawn en el Inspector!");
        }

        if (exitTeleporter != null)
        {
            exitTeleporter.SetActive(true);
        }
    }
}