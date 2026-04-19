using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerStateManager
{
    protected float time { get; set; }
    protected float fixedtime { get; set; }
    protected float latetime { get; set; }

    public PlayerStateMachine sm;
    public virtual void OnEnter(PlayerStateMachine _stateMachine)
    {
        sm = _stateMachine;
    }
    public virtual void OnUpdate(){}
    public virtual void OnFixedUpdate(){}
    public virtual void OnLateUpdate(){}
    public virtual void OnExit(){}
}

