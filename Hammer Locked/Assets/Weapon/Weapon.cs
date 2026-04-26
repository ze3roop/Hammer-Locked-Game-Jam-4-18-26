using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponDataSO weaponData;
    public ProjectileDataSO projectileData; 

    public Transform muzzlePoint; 

    private float fireRate_duration; 

    public void TryFire(Transform target)
    {
        if(Time.time < fireRate_duration) return; 
        
        Vector3 direction = (target.position - muzzlePoint.position).normalized; 
        Quaternion rotation = Quaternion.LookRotation(direction); 
        // Vector3 projectileRotation = 
        GameObject projectile = Instantiate(projectileData.projectile_prefab, muzzlePoint.position, rotation); 
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.SetDirection(direction);
        projectileScript.SetDamage(weaponData.damage);

        //pass the data to the prefab script? 
        projectileScript.SetProjectileData(projectileData); 

        fireRate_duration = Time.time + weaponData.fireRate; 
    }

    public void TryFire(Ray ray, LayerMask enemyLayerMask)
    {
            /* 
                Note: A Ray defines a direction that goes infinitely forward, and Physics.Raycast decides how far along that ray to actually check.
            */ 
        if(Time.time < fireRate_duration) return; 
        
         

        fireRate_duration = Time.time + weaponData.fireRate;
        float weaponRange = 100f; //temporary 
            //Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            if (Physics.Raycast(ray, out RaycastHit hit, weaponRange)) //enemyLayerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.blue, 5f);
                //  Vector3 direction = (target.position - muzzlePoint.position).normalized; 
                Vector3 direction = (hit.point - muzzlePoint.position).normalized; 
                Quaternion rotation = Quaternion.LookRotation(direction); 
                // Vector3 projectileRotation = 
                GameObject projectile = Instantiate(projectileData.projectile_prefab, muzzlePoint.position, rotation); 
                Projectile projectileScript = projectile.GetComponent<Projectile>();
                projectileScript.SetDirection(direction);
                projectileScript.SetDamage(weaponData.damage);

                //pass the data to the prefab script? 
                projectileScript.SetProjectileData(projectileData);
            }
    }
}
