using UnityEngine;

public class MeleeState : IEnemyState
{
    readonly EnemyBrain b;
    readonly EnemyDataSO d;
    float _until;

    public MeleeState(EnemyBrain brain, EnemyDataSO data) { b = brain; d = data; }

    public void OnEnter()
    {
        b.Move.Stop();
        b.Move.ZeroVelocity();
        _until = Time.time + 2.35f; // quick “swing”
        b.animator.Play("Hook_Attack", 0, 0f);
        b.animator.Update(0f); // forces Animator to apply immediately
    }

    public void OnUpdate()
    {
        // if (b.Target == null) { b.SetState(b.Search); return; }
        b.Move.Stop();

        b.Move.FaceTarget(b.Target.position);

        // if (Time.time >= _until)
        // {
        //     // apply damage here (overlap sphere, etc.)
        //     b.SetState(b.Chase);
        // }
        // if (!b.animator.IsInTransition(0))
        // {
            AnimatorStateInfo stateInfo = b.animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("Hook_Attack") && stateInfo.normalizedTime >= 1f)
            {
                float dist = b.DistToTarget();

                if (dist <= d.attackRange && b.HasTargetLOS())
                {
                    Debug.Log("Reset Melee State");
                    // b.animator.;
                    b.SetState(b.Melee); 
                }
                else
                {
                    Debug.Log("Set State to Chase");
                    b.SetState(b.Chase);
                }
            }
        // }
    }

    public void OnExit()
    {
        b.Combat.DisableAttackHitbox();
    }
}
