using UnityEngine;

public class ChaseState : IEnemyState
{
    readonly EnemyBrain b;
    readonly EnemyDataSO d;

    float _nextRepath;

    public ChaseState(EnemyBrain brain, EnemyDataSO data) { b = brain; d = data; }

    public void OnEnter() { 
        Debug.Log("IN CHASE STATE"); 
        _nextRepath = 0f; 
        b.animator.Play("Walk"); 
    }

    public void OnUpdate()
    {
        //when we initially see a target, in EnemySenses, we store a LastSeenTime
        //we we call RecentlySawTarget, we are checking if our lastseentime is less than 
        //the duration we have before we officially lose sight. 

        //Once we lose sight, start searching for the target. 
        if (!b.RecentlySawTarget())
        {
            b.SetState(b.Search);
            return;
        }

        float dist = b.DistToTarget();

        // //Decide what to do based on enemy Archetype:
        // if (d.archetype == EnemyArchetype.Rusher && d.meleeCapable && dist <= d.meleeRange)
        // {
        //     b.SetState(b.Melee);
        //     return;
        // }

        // // if too close reposition (or keep range)
        // if (d.archetype == EnemyArchetype.Sniper && dist < d.preferredRangeMin)
        // {
        //     b.SetState(b.Reposition);
        //     return;
        // }

        if (dist <= d.attackRange && b.HasTargetLOS())
        {
            // // if too far or too close then get into range
            // if (dist < d.preferredRangeMin || dist > d.preferredRangeMax)
            //     b.SetState(b.GetInRange);
            // else
            Debug.Log("Set State to Melee");
            b.SetState(b.Melee);

            return;
        }

        // use our movement component to move towards the target which refreshed at a specificied interval
        if (Time.time >= _nextRepath)
        {
            _nextRepath = Time.time + d.repathInterval;
            b.Move.MoveTo(b.Target.position);
        }

        b.Move.FaceTarget(b.Target.position);
    }

    public void OnExit() { }
}
