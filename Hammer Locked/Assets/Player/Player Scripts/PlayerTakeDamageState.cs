using UnityEngine;

public class PlayerTakeDamageState : PlayerBaseState
{
    public override void OnEnter(PlayerStateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        sm.animator.Play("Human|Hammer_TakeDamage");

        GameEventsManager.Instance.audioEvents.PlaySound(sm.hurtSounds, sm.transform.position);
        GameEventsManager.Instance.audioEvents.PlaySound(sm.groanSounds, sm.transform.position);
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        AnimatorStateInfo stateInfo = sm.animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Human|Hammer_TakeDamage") && stateInfo.normalizedTime >= 1f)
        {
            sm.SetNextState(sm.PlayerIdleState);
        }
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}
