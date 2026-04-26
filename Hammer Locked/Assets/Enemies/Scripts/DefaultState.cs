using UnityEngine;
using UnityEngine.Video;

public class DefaultState : IEnemyState
{
    readonly EnemyBrain b;
    readonly EnemyDataSO d;

    public DefaultState(EnemyBrain brain, EnemyDataSO data) { b = brain; d = data; }

    public void OnEnter()
    {
        // Do all the conditions and state decision checks here. No point in having an empty frame by
        // putting everything in OnUpdate()

        //need a way to check if we should go to cover

        //Checks:
        // are we in cover already? 
        if (b.IsInCover)
        {

            Debug.Log("Default: In Cover");

            // First ask: Is this cover even viable? 
            //IF not, we either need to attack (if we have LOS and player is too close to us) or find new cover
            if (!CoverManager.Instance.IsCoverPointViable(b.currentCoverPoint))
            {
                Debug.Log("Default: Cover Not Viable");
                if(b.DistToTarget() < 3f && b.HasTargetLOS())
                {
                    Debug.Log("Default: b.DistToTarget() < 3f && b.HasTargetLOS()");
                    //player is too close, let's just keep attacking (this is where strafing would be good)
                    b.SetState(b.Attack);
                    return;
                }
                else // if player is in still nearby/ in LOS, otherwise we would go into search or patrol or idle or smthing
                {
                    Debug.Log("Default: Reposition");
                    b.SetState(b.Reposition);
                    return;
                }
                
            }
            else
            {
                //How long have we been in cover? Should we go back to InCover state? 
                if(b.inCover_duration > 10f) // in cover for at least 10 seconds
                {
                    Debug.Log("Default: Been in cover longer than 10 seconds");
                    //maybe flip a coin here? 
                    int randomInt = Random.Range(0, 2); 
                    if(randomInt == 0)
                    {
                        Debug.Log("Default: 1 in 3 chance, so go into cover");
                        // 1 in 3 chance to stay in cover
                        b.SetState(b.InCover);
                    }
                    else
                    {
                        Debug.Log("Default: 2/3 chance to attack/reposition");
                        if(b.DistToTarget() < 3f && b.HasTargetLOS())
                        {
                            Debug.Log("Default: b.DistToTarget() < 3f && b.HasTargetLOS()");
                            b.SetState(b.Attack);
                            return;
                        }
                        // find new cover
                        Debug.Log("Default: Reposition");
                        b.SetState(b.Reposition);
                        return;
                    }
                }
                else
                {
                    //let's just stay in cover, since we know the cover is viable, and we haven't been here very long
                    Debug.Log("Default: stay in cover, since we know the cover is viable, and we haven't been here very long");
                    b.SetState(b.InCover);
                }
            }
        }
        else
        {
            Debug.Log("Default: Not in Cover");
            // should we find cover or attack again or something else? 
            // let's just focus on whether to go into cover or attack - can figure out the other states later
            // I need checks for Attacks as well. 
            if(b.DistToTarget() < 1f && b.HasTargetLOS())
            {
                Debug.Log("Default: b.DistToTarget() < 1f && b.HasTargetLOS()");
                //player is too close, let's just keep attacking (this is where strafing would be good)
                b.SetState(b.Attack);
                return;
            }
            else // if player is in still nearby/ in LOS, otherwise we would go into search or patrol or idle or smthing
            {
                Debug.Log("Default: Reposition"); 
                b.SetState(b.Reposition);
                return;
            }
        }
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
        
    }
}
