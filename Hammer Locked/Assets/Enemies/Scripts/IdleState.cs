using UnityEngine;
using UnityEngine.Video;

public class IdleState : IEnemyState
{
    readonly EnemyBrain b;
    readonly EnemyDataSO d;

    public IdleState(EnemyBrain brain, EnemyDataSO data) { b = brain; d = data; }

    public void OnEnter()
    {
        b.Move.Stop();
        b.Move.ZeroVelocity();
        b.animator.Play("Idle", 0, 0f); 
        b.animator.Update(0f); // forces Animator to apply immediately
    }

    public void OnUpdate()
    {
        if (b.HasTargetLOS())
        {
            b.SetState(b.Chase);
        }
    }

    public void OnExit()
    {
        
    }
}
