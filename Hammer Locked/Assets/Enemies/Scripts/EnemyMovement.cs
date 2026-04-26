using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent Agent;

    void Awake()
    {
    }

    public void MoveTo(Vector3 pos)
    {
        Agent.isStopped = false;
        Agent.SetDestination(pos);
    }

    public void Stop()
    {
        Agent.isStopped = true;
        Agent.ResetPath();
    }

    public void ZeroVelocity()
    {
        Agent.velocity = Vector3.zero; 
    }

    public float MovementSpeed()
    {
        return Agent.velocity.magnitude; 
    }

    public void FaceTarget(Vector3 targetPos, float turnSpeed = 720f)
    {
        Vector3 dir = (targetPos - transform.position);
        dir.y = 0f;
        if (dir.sqrMagnitude< .0001f) return;

        Quaternion desired = Quaternion.LookRotation(dir.normalized);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desired, turnSpeed * Time.deltaTime);   
    }

    public bool HasArrived(float stoppingDistancePadding = 0.1f)
    {
        if (Agent.pathPending) return false;
        if (Agent.remainingDistance > Agent.stoppingDistance + stoppingDistancePadding) return false;
        return !Agent.hasPath || Agent.velocity.sqrMagnitude < 0.01f;
    }
}
