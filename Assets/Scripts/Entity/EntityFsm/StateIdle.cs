using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaomaoFramework;
using CaomaoFramework.EntityFsm;
public class StateIdle : IEntityState
{
    /// <summary>
    ///  进入该状态
    /// </summary>
    /// <param name="theOwner"></param>
    /// <param name="args"></param>
    public void Enter(EntityParent theOwner, params object[] args)
    {
        theOwner.CurrentMotionState = MotionState.IDLE;
    }

    // 离开状态
    public void Exit(EntityParent theOwner, params object[] args)
    {
    }

    // 状态处理
    public void Process(EntityParent theOwner, params object[] args)
    {
        // 播放 idle 动画
        if (theOwner == null)
        {
            return;
        }
        if (theOwner.CanMove() && theOwner.motor != null)
        {
            theOwner.motor.enableStick = true;
        }
        GameMotor theMotor = theOwner.motor;
        // 设置速度
        if (theMotor != null)
        {
            theMotor.SetSpeed(0.0f);
            //theMotor.SetExtraSpeed(0f);
        }
        if (theOwner.charging)
        {
            return;
        }
        if (theOwner is EntityPlayer && GameController.isInTown)
        {
            //theOwner.SetAction(-1);
        }
        else
        {
            //theOwner.SetAction(0);
        }
       // theOwner.SetActionByStateFlagInIdleState();
    }
}
