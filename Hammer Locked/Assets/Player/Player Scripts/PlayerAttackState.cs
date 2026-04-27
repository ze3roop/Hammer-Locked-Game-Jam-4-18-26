using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    float damage = 50f; 
    public override void OnEnter(PlayerStateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        sm.animator.Play("Human|HammerSwing1");
        sm.OnApplyDamage += OnApplyDamage; 

        GameEventsManager.Instance.audioEvents.PlaySound(sm.swooshSound, sm.transform.position); 

    }
    public void OnApplyDamage(GameObject other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TryTakeDamage(damage);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        AnimatorStateInfo stateInfo = sm.animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Human|HammerSwing1") && stateInfo.normalizedTime >= 1f)
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

        sm.OnApplyDamage -= OnApplyDamage; 
    }
}
