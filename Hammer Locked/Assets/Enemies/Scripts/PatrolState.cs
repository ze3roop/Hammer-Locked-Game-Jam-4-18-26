using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IEnemyState
{
    readonly EnemyBrain b;
    readonly EnemyDataSO d; 

    private float pickNewDestinationTimer;

    bool isInPatrolPosition; 

    float wanderRadius = Random.Range(2f, 10f); 

    public PatrolState(EnemyBrain brain, EnemyDataSO data) { b = brain; d = data; }

    public void OnEnter()
    {
        Debug.Log("In PATROL State");
        pickNewDestinationTimer = 0f;
    }
    public void OnUpdate()
    {
        
        //if we have line of sight then chase player
        if (b.HasTargetLOS())
        {
            b.SetState(b.Chase); 
        }
        if(Vector3.Distance(b.startingPosition, b.transform.position) > 11)
        {
            isInPatrolPosition = false;
            b.Move.MoveTo(b.startingPosition);
            // Debug.Log("Moving to patrol position");
        }
        else
        {
            // Debug.Log("IN PATROL POSITION");
            isInPatrolPosition = true;
        }
            
        //if it is time to move to a new spot & we have arrived to the original destination, then find a new spot to go to
        if(isInPatrolPosition && Time.time >= pickNewDestinationTimer && b.Move.HasArrived())
        {
            //add 1.5 seconds to our pick timer. 
            pickNewDestinationTimer = Time.time + Random.Range(2, 5);
            // take a random position within our radius and multiply it by wanderRadius 
            Vector3 random = b.transform.position + Random.insideUnitSphere * wanderRadius;
            //Try to move to the random position, if it fails then this will get ran in the next Update frame until we find a location we can go to
            if (NavMesh.SamplePosition(random, out var hit, wanderRadius, NavMesh.AllAreas))
                b.Move.MoveTo(hit.position);
        }
    }

    public void OnExit()
    {
        //throw new System.NotImplementedException();
    }
}
