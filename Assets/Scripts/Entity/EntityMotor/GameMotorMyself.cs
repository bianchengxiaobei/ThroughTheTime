using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CaomaoFramework;
public class GameMotorMyself : GameMotor
{
    public Animator animator;
    public CharacterController2D characterController;
    public float speedV = 5;
    private NavMeshPath path;   
    private bool m_isMovingOn = false;
    private bool isMovingToTargetWithoutNav = false;

    private float accelerationTimeAirborne = .2f;
    private float accelerationTimeGrounded = .1f;

    private Vector2 directionalInput = Vector2.zero;
    private float velocityXSmoothing;
    private AnimatorStateInfo baseLayerInfo;

    private void Awake()
    {
        characterController = transform.GetComponent<CharacterController2D>();
        enableStick = true;
        animator = this.GetComponent<Animator>();
        
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        
    }
    private void ApplyGravity()
    {
        if (isFlying)
        {
            moveDir2D.y += gravity * Time.deltaTime;
        }
        else
        {
            moveDir2D.y = directionalInput.y;
        }
    }
    private void FixedUpdate()
    {
        baseLayerInfo = animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex("Base Layer"));
        isRolling = baseLayerInfo.IsName("Roll");
        //float targetVelocityX = directionalInput.x;
        //moveDir2D.x = Mathf.SmoothDamp(moveDir2D.x, targetVelocityX, ref velocityXSmoothing, (characterController.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne));
        moveDir2D.x = transform.eulerAngles.y == 0 ? directionalInput.x : -directionalInput.x;
        ApplyGravity();
        if (!canMove)
        {
            return;
        }
        if (!animator.runtimeAnimatorController)
        {
            return;
        }
        directionalInput = GameInputManager.Instance.moveVector;
        //if (enableStick && GameInputManager.Instance != null && GameInputManager.Instance.IsMoving)
        //{
        //    characterController.Rotate(characterController.collisions.faceDir);
        //}
        speed = AccelerateSpeed(speed, targetSpeed);
        animator.SetFloat("Speed", speed);
        if (!isRolling)
            characterController.Move(moveDir2D * Time.deltaTime* speedV, directionalInput,isFlying);
  
       // if ((characterController.collisions.above || characterController.collisions.below) && isFlying == false)
        //{
          //  moveDir2D.y = 0;
       // }
        if (transform.position.y <= jumpPos.y - 0.1f&& isFlying)
        {
            isFlying = false;
            moveDir2D.y = 0;
            transform.position = new Vector3(transform.position.x,jumpPos.y,transform.position.z);
        }
    }
    public override void SetSpeed(float value)
    {
        targetSpeed = value;       
    }
    public override void Jump()
    {
        if (animator.IsInTransition(0)) return;
        bool jumpCondition = !isFlying;
        if (jumpCondition)
        {
            SetFlying(true);
            animator.CrossFadeInFixedTime("Jump", 0.2f);
            moveDir2D.y = maxJumpVelocity;
            jumpPos = transform.position;
        }
        else
        {
            return;
        }
    }
    public override void JumpMin()
    {
        if (moveDir2D.y > minJumpVelocity)
        {
            moveDir2D.y = minJumpVelocity;
        }
    }
    public override void Roll()
    {
        if (animator.IsInTransition(0)) return;
        bool rollCondition = (directionalInput != Vector2.zero) && !isFlying;
        //bool rollCondition = !isFlying;
        if (!rollCondition || isRolling) return;
        //moveDir2D.x = directionalInput.x;
        animator.CrossFadeInFixedTime("Roll", 0.1f);
    }
    public override bool IsOnGround()
    {
        return !isFlying;
    }
}
