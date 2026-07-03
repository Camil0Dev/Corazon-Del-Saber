using UnityEngine;

[CreateAssetMenu(fileName = "NewBossData", menuName = "Boss/Data")]
public class BossData : ScriptableObject
{
    [Header("Health")]
    public int maxHealth = 100;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float detectionRange = 5f;
    public float attackRange = 2f;
    public float groundY = 0f; // Altura fija del suelo (para evitar flotar)

    [Header("Attack")]
    public float attackCooldown = 2f;
    public int attackDamage = 15;
    public float attackDuration = 0.5f;
    public float attackPointRadius = 1.5f; // Radio del área de ataque

    [Header("Awake")]
    public float awakeDuration = 2f;
}