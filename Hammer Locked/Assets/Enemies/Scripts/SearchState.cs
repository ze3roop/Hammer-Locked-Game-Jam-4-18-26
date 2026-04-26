using UnityEngine;
using UnityEngine.AI;

public class SearchState : IEnemyState
{
    readonly EnemyBrain b;
    readonly EnemyDataSO d;

    float _until;
    float _nextPick;

    public SearchState(EnemyBrain brain, EnemyDataSO data) { b = brain; d = data; }

    public void OnEnter()
    {
        _until = Time.time + 3.0f;
        _nextPick = 0f;

        Debug.Log(" IN SEARCH STATE");
    }

    public void OnUpdate()
    {
        if (b.HasTargetLOS())
        {
            b.SetState(b.Chase);
            return;
        }

        if (Time.time >= _until)
        {
            b.SetState(b.Patrol);
            return;
        }

        if (Time.time >= _nextPick && b.Move.HasArrived())
        {
            _nextPick = Time.time + 0.8f;
            Vector3 random = b.transform.position + Random.insideUnitSphere * 4f;
            if (NavMesh.SamplePosition(random, out var hit, 4f, NavMesh.AllAreas))
                b.Move.MoveTo(hit.position);
        }
    }

    public void OnExit() { }
}
