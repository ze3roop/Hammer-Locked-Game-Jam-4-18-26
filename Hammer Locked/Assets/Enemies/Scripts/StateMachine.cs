using UnityEngine; 
using System.Diagnostics;

public class StateMachine
{
    public IEnemyState Current { get; private set; }

    public void ChangeState(IEnemyState next)
    {
        //if (Current == next) return;
        Current?.OnExit();
        Current = next;
        Current?.OnEnter();
    }

    public void OnUpdate()
    {
        Current?.OnUpdate();
        //  UnityEngine.Debug.Log(Current); 
    }
        
}