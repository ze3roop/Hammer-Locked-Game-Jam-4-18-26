using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public void OnMovement(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            GameEventsManager.Instance.inputEvents.MovementPressed(context.ReadValue<Vector2>());
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            GameEventsManager.Instance.inputEvents.MovementCanceled(context.ReadValue<Vector2>());
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        GameEventsManager.Instance.inputEvents.Look(context.ReadValue<Vector2>());
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            GameEventsManager.Instance.inputEvents.JumpStarted();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            GameEventsManager.Instance.inputEvents.JumpCanceled();
        }
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            GameEventsManager.Instance.inputEvents.SprintStarted();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            GameEventsManager.Instance.inputEvents.SprintCanceled();
        }
    }
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            GameEventsManager.Instance.inputEvents.CrouchStarted();   
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            GameEventsManager.Instance.inputEvents.CrouchCanceled();
        }
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            GameEventsManager.Instance.inputEvents.FireStarted();
        }
        else if (context.phase == InputActionPhase.Performed)
        {
            GameEventsManager.Instance.inputEvents.FirePerforming();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            GameEventsManager.Instance.inputEvents.FireCanceled();
        }
    }
}
