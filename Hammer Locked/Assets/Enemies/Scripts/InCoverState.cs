using UnityEngine;

public class InCoverState : IEnemyState
{
    readonly EnemyBrain b;
    readonly EnemyDataSO d;

    public InCoverState(EnemyBrain brain, EnemyDataSO data) { b = brain; d = data; }

    float waitToAttack_duration; 

    public void OnEnter()
    {
        //if timer isn't already happening/we aren't already in cover
        if (!b.IsInCover)
        {
            b.IsInCover = true;
            b.inCover_duration = 0f; 
        }
       b.Move.Stop();
       waitToAttack_duration = Random.Range(1, 5);  
    //    Debug.Log("In Cover");
    //    Debug.Log("Wait to attack duration: " + waitToAttack_duration);
    }

    public void OnUpdate()
    {
        waitToAttack_duration -= Time.deltaTime; 

        if(waitToAttack_duration < 0)
        {
            b.SetState(b.Attack);
        }

        //if enemy is taking damage, reposition or attack back. 
    }

    public void OnExit()
    {
        
    }
}
