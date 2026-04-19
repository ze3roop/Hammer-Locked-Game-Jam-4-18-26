using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerBaseState : PlayerStateManager
{
    public MovementState state;
    public enum MovementState { walking, sprinting, crouching, air }
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private float moveSpeed;

    

    public override void OnEnter(PlayerStateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        #region Movement Methods
        MovementInput();
        SpeedControl();
        StateHandler();

        // if(sm.rb.velocity)

        // handle drag
        if (sm.grounded)
            sm.rb.linearDamping = sm.groundDrag;
        else
            sm.rb.linearDamping = 0;
        #endregion

        // #region Weapon Methods
        // if (sm.fireStarted) // && canFire
        // {
        //     Weapon weapon = sm.GetComponentInChildren<Weapon>();
        //     weapon.TryFire(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f)), sm.enemyLayerMask); 
        // }
        // #endregion
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        
        MovePlayer();
    }
    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
    }
    public override void OnExit()
    {
        base.OnExit();
    }
    #region MOVEMENT
    private void MovementInput()
    {
        horizontalInput = sm.movement.x; // Input.GetAxisRaw("Horizontal");
        verticalInput = sm.movement.y; // Input.GetAxisRaw("Vertical");

        // when to jump
        if(sm.jumpStarted && sm.readyToJump && sm.grounded)
        {
            sm.readyToJump = false;

            Jump();

            //potentially replace this with a timer in OnUpdate() 
            CoroutineRunner.Instance.StartCoroutine(JumpCooldownRoutine()); 
        }

        // start crouch
        if (sm.crouchStarted)
        {
            //m.transform.localScale = new Vector3(sm.transform.localScale.x, sm.crouchYScale, sm.transform.localScale.z);
        }

        // stop crouch
        if (!sm.crouchStarted)
        {
            //sm.transform.localScale = new Vector3(sm.transform.localScale.x, sm.startYScale, sm.transform.localScale.z);
        }
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !sm.exitingSlope)
        {
            if (sm.rb.linearVelocity.magnitude > moveSpeed)
                sm.rb.linearVelocity = sm.rb.linearVelocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(sm.rb.linearVelocity.x, 0f, sm.rb.linearVelocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                sm.rb.linearVelocity = new Vector3(limitedVel.x, sm.rb.linearVelocity.y, limitedVel.z);
            }
        }
    }

    private void StateHandler()
    {
        // Mode - Crouching
        if (sm.crouchStarted)
        {
            state = MovementState.crouching;
            moveSpeed = sm.crouchSpeed;
        }

        // Mode - Sprinting
        else if(sm.grounded && sm.sprintStarted)
        {
            state = MovementState.sprinting;
            moveSpeed = sm.sprintSpeed;
        }

        // Mode - Walking
        else if (sm.grounded)
        {
            state = MovementState.walking;
            moveSpeed = sm.walkSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }
    }
    private void MovePlayer()
    {
        //Debug.Log("Vertical Input: " + verticalInput + " Horizontal Input: " + horizontalInput); 
        // calculate movement direction
        moveDirection = sm.orientation.forward * verticalInput + sm.orientation.right * horizontalInput;

        //Debug.Log("Orientation.forward: " + sm.orientation.forward + "---- Orientation.right: " + sm.orientation.right); 

        // on slope
        if (OnSlope() && !sm.exitingSlope)
        {
            sm.rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (sm.rb.linearVelocity.y > 0)
                sm.rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if(sm.grounded)
            sm.rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(!sm.grounded)
            sm.rb.AddForce(moveDirection.normalized * moveSpeed * 10f * sm.airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        sm.rb.useGravity = !OnSlope();

        // sm.transform.rotation =  Quaternion.LookRotation(moveDirection, Vector3.up);  
    }

    private void Jump()
    {
        Debug.Log("JUMPING");
        sm.exitingSlope = true;

        // reset y velocity
        sm.rb.linearVelocity = new Vector3(sm.rb.linearVelocity.x, 0f, sm.rb.linearVelocity.z);

        sm.rb.AddForce(sm.transform.up * sm.jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        Debug.Log("RESET JUMP");
        sm.readyToJump = true;

        sm.exitingSlope = false;
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(sm.transform.position, Vector3.down, out sm.slopeHit, sm.playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, sm.slopeHit.normal);
            return angle < sm.maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, sm.slopeHit.normal).normalized;
    }

    IEnumerator JumpCooldownRoutine()
    {
        yield return new WaitForSeconds(sm.jumpCooldownMax);
        ResetJump(); 
    }
    #endregion
}
