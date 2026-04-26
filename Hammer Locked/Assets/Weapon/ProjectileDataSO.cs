using UnityEngine;

[CreateAssetMenu(menuName = "ProjectileData")]
public class ProjectileDataSO : ScriptableObject
{
    public GameObject projectile_prefab; 
    public float projectileSpeed; 
}
