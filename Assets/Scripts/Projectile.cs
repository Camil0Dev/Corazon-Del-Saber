using UnityEngine;

public class Projectile2D : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float lifetime = 4f;

    [Header("Combate")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject impactEffect;
    
    [Header("Colisiones")]
    [SerializeField] private LayerMask whatDestroysProjectile; // <--- NUEVO: Reemplaza a los Tags

    private Vector2 moveDirection;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    public void SetDirection(Vector2 newDirection)
    {
        moveDirection = newDirection.normalized;

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Detectar al Jugador
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<PlayerController>(out PlayerController player))
            {
                player.TakeDamage(damage, transform); 
            }
            Impact();
            return;
        }

        // 2. Detectar si el objeto golpeado pertenece a la LayerMask (Suelo, Paredes, etc.)
        // Esta línea compara si la capa del objeto colisionado está dentro de las seleccionadas en la LayerMask
        if ((whatDestroysProjectile.value & (1 << collision.gameObject.layer)) > 0)
        {
            Impact();
        }
    }

    private void Impact()
    {
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}