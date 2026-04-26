using UnityEngine;
using UnityEngine.AI;

public class RepositionState : IEnemyState
{
    readonly EnemyBrain b;
    readonly EnemyDataSO d;

    float _leaveTime;

    Vector3 moveToPosition; 

    public RepositionState(EnemyBrain brain, EnemyDataSO data) { b = brain; d = data; }

    public void OnEnter()
    {
        b.currentCoverPoint = CoverManager.Instance.FindCover(); 
        moveToPosition = b.currentCoverPoint.transform.position; 
    }

    public void OnUpdate()
    {
        b.Move.MoveTo(moveToPosition); 

        if(b.Move.HasArrived())
        {
            b.SetState(b.InCover); 
            // Debug.Log("Reached Position!");
        }
    }

    public void OnExit() { }
}
