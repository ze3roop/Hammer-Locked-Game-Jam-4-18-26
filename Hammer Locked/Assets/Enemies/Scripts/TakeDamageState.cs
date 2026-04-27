using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class TakeDamageState : IEnemyState
{
    readonly EnemyBrain b;
    readonly EnemyDataSO d; 

    public TakeDamageState(EnemyBrain brain, EnemyDataSO data) { b = brain; d = data; }

    public void OnEnter()
    {
        b.animator.Play("Take Damage", 0, 0f);
        b.animator.Update(0f); // forces Animator to apply immediately
        b.Move.Stop();
        b.Move.ZeroVelocity();

        b.hitEffect.Play(); 

        b.OnTakeDamage += OnTakeDamage;
    }

    public void OnTakeDamage()
    {
        // b.animator.Play("Take Damage", 0, 0f);
        // b.animator.Update(0f); // forces Animator to apply immediately
    }
    public void OnUpdate()
    {
        AnimatorStateInfo stateInfo = b.animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Take Damage") && stateInfo.normalizedTime >= 1f)
        {
            b.SetState(b.Chase);  
        }
    }

    public void OnExit()
    {
        //throw new System.NotImplementedException(); 
        b.OnTakeDamage -= OnTakeDamage;
    }
}
