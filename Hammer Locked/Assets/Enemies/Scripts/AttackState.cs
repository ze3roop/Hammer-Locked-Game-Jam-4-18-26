using UnityEngine; 


public class AttackState : IEnemyState
{
    readonly EnemyBrain b;
    readonly EnemyDataSO d;

    float activeFiringMode_duration; 

    public AttackState(EnemyBrain brain, EnemyDataSO data) { b = brain; d = data; }

    public void OnEnter() { 
        b.Move.Stop(); 
        activeFiringMode_duration = Random.Range(4, 5); 
        // Debug.Log(activeFiringMode_duration);
    }

    public void OnUpdate()
    {
        activeFiringMode_duration -= Time.deltaTime;
        b.Move.FaceTarget(b.Target.position);
        //b.Combat.TryShoot(d.fireCooldown, b.Muzzle, b.Target);

        if(activeFiringMode_duration < 0)
        {
            b.SetState(b.Default);
        }
    }

    public void OnExit()
    {
        b.Combat.DisableAttackHitbox();
    }
}

        //float dist = b.DistToTarget();
        //if (!b.RecentlySawTarget()) { b.SetState(b.Search); return; }
// // Sniper: if target gets too close, reposition
        // if (d.archetype == EnemyArchetype.Sniper && dist < d.preferredRangeMin)
        // {
        //     // b.SetState(b.Reposition);
        //     return;
        // }

        // // Rusher: if in melee range, melee
        // if (d.archetype == EnemyArchetype.Rusher && d.meleeCapable && dist <= d.meleeRange)
        // {
        //     // b.SetState(b.Melee);
        //     return;
        // }

        // If out of attack range or no LOS, chase again
        // if (dist > d.attackRange || !b.HasTargetLOS())
        // {
        //     b.SetState(b.Chase);
        //     return;
        // }