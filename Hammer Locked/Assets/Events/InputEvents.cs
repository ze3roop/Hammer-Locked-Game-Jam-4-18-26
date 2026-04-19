using System;
using System.Numerics;
using UnityEngine;

public class InputEvents
{
    // MOVEMENT
    public event Action<UnityEngine.Vector2> OnMovementPressed;
    public void MovementPressed(UnityEngine.Vector2 movement) { OnMovementPressed?.Invoke(movement); }

    public event Action<UnityEngine.Vector2> OnMovementCanceled;
    public void MovementCanceled(UnityEngine.Vector2 movement) { OnMovementCanceled?.Invoke(movement); }

    // LOOK
    public event Action<UnityEngine.Vector2> OnLook;
    public void Look(UnityEngine.Vector2 look) { OnLook?.Invoke(look);  }

    // JUMP
    public event Action OnJumpStarted;
    public void JumpStarted() { OnJumpStarted?.Invoke(); }

    public event Action OnJumpCanceled;
    public void JumpCanceled() { OnJumpCanceled?.Invoke(); }

    // SPRINT
    public event Action OnSprintStarted;
    public void SprintStarted() { OnSprintStarted?.Invoke(); }

    public event Action OnSprintCanceled;
    public void SprintCanceled() { OnSprintCanceled?.Invoke(); }

    // CROUCH
    public event Action OnCrouchStarted; 
    public void CrouchStarted() { OnCrouchStarted?.Invoke(); }

    public event Action OnCrouchCanceled;
    public void CrouchCanceled() { OnCrouchCanceled?.Invoke(); }

    // FIRE
    public event Action OnFireStarted;
    public void FireStarted() { OnFireStarted?.Invoke(); }

    public event Action OnFirePerforming;
    public void FirePerforming() { OnFirePerforming?.Invoke(); }

    public event Action OnFireCanceled;
    public void FireCanceled() { OnFireCanceled?.Invoke(); }
}
