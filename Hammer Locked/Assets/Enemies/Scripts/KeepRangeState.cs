using UnityEngine;
using UnityEngine.AI;

public class GetInRangeState : IEnemyState
{
    readonly EnemyBrain b;
    readonly EnemyDataSO d;

    float _nextPick;

    public GetInRangeState(EnemyBrain brain, EnemyDataSO data) { b = brain; d = data; }

    public void OnEnter() { Debug.Log("IN RANGE STATE"); _nextPick = 0f; }

    public void OnUpdate()
    {
        // if (!b.RecentlySawTarget()) { b.SetState(b.Search); return; }

        float distance = b.DistToTarget();
        // if (distance >= d.preferredRangeMin && distance <= d.preferredRangeMax && b.HasTargetLOS())
        // {
        //     b.SetState(b.Attack);
        //     return;
        // }

        // Pick a point either away or toward the target to re-enter preferred band
        if (Time.time >= _nextPick)
        {
            _nextPick = Time.time + 0.35f;

            // Vector3 vectorToTarget = (b.Target.position - b.transform.position);
            // vectorToTarget.y = 0f;

            // Vector3 direction = vectorToTarget.normalized;
            // float desiredDistance = Mathf.Clamp(distance, d.preferredRangeMin, d.preferredRangeMax);

            // Vector3 desiredPos = b.Target.position - direction * desiredDistance; // around preferred range band

            Vector3 vectorToTarget = b.transform.position - b.Target.position; 
            vectorToTarget.y = 0; 

            float magnitude = vectorToTarget.magnitude;
            float desiredDistance = Mathf.Clamp(distance, d.preferredRangeMin, d.preferredRangeMax); 
            float difference = Mathf.Abs(desiredDistance - magnitude); 

            Vector3 directionVector = vectorToTarget.normalized; 
            Vector3 directionVectorScaled = directionVector * difference; 
            Vector3 desiredPos = b.transform.position + directionVectorScaled; 

            Vector3 candidate = Vector3.Lerp(b.transform.position, desiredPos, 0.75f);

            if (NavMesh.SamplePosition(candidate, out var hit, 3.0f, NavMesh.AllAreas))
                b.Move.MoveTo(hit.position);
        }

        b.Move.FaceTarget(b.Target.position);
    }

    public void OnExit() { }
}
