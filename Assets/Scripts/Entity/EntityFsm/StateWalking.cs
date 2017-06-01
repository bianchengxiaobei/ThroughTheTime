using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaomaoFramework;
using CaomaoFramework.EntityFsm;
public class StateWalking : IEntityState
{
    public void Enter(EntityParent theOwner, params object[] args)
    {
        theOwner.CurrentMotionState = MotionState.WALKING;
        if (theOwner is EntityMyself)
        {
            theOwner.animator.speed = theOwner.moveSpeedRate * theOwner.gearMoveSpeedRate;
        }
    }
    public void Exit(EntityParent theOwner, params object[] args)
    {
        theOwner.ApplyRootMotion(true);
        theOwner.SetSpeed(1);
        if (theOwner is EntityBeast)
        {
            theOwner.motor.SetExtraSpeed(0);
            theOwner.motor.isMovingToTarget = false;
            return;
        }
        if (theOwner is EntityMyself)
        {
            theOwner.animator.speed = 1;
        }
    }
    public void Process(EntityParent theOwner, params object[] args)
    {
        GameMotor theMotor = theOwner.motor;
        if (theOwner is EntityBeast || (theOwner is EntityPlayer && !(theOwner is EntityMyself)))
        {
            theOwner.ApplyRootMotion(false);
            theOwner.SetSpeed(1);
            theMotor.SetSpeed(0.4f);
            if (theOwner.Speed == 0)
            {
                theMotor.SetExtraSpeed(6);
            }
            else
            {
                theMotor.SetExtraSpeed(theOwner.Speed);
            }
            return;
        }
        else
        {
            theOwner.ApplyRootMotion(true);
            theMotor.SetSpeed(1.0f);
            //theMotor.SetExtraSpeed(0.4f);
        }
        theMotor.isMovable = true;
    }
}
