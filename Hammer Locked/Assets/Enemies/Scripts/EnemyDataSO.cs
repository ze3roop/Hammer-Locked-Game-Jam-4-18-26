using UnityEngine;

public enum EnemyArchetype
{
    Grunt,
    Sniper,
    Rusher
}

[CreateAssetMenu(menuName = "AI/Enemy Data")]
public class EnemyDataSO : ScriptableObject
{
    public EnemyArchetype archetype = EnemyArchetype.Grunt;

    [Header("Senses")]
    public float sightRange = 25f;
    public float loseSightAfterSeconds = 2.0f;

    [Header("Ranges")]
    public float preferredRangeMin = 8f;   // wants to be at least this far
    public float preferredRangeMax = 15f;  // wants to be at most this far
    public float attackRange = 18f;

    [Header("Movement")]
    public float wanderRadius = 10f;
    public float repathInterval = 0.25f;

    [Header("Combat")]
    public float fireCooldown = 0.15f;
    public float reloadSeconds = 1.5f;

    [Header("Sniper-only")]
    public bool useVantagePoints = false;
    public float repositionMinSeconds = 2f;
    public float repositionMaxSeconds = 4f;

    [Header("Rusher-only")]
    public bool meleeCapable = false;
    public float meleeRange = 2.2f;
}
