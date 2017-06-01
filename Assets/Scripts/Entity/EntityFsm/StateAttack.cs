using System;
using System.Collections.Generic;
using CaomaoFramework;
using CaomaoFramework.EntityFsm;

public class StateAttack : IEntityState
{
    // 进入该状态
    public void Enter(EntityParent theOwner, params Object[] args)
    {
        theOwner.CurrentMotionState = MotionState.ATTACKING;
    }

    // 离开状态
    public void Exit(EntityParent theOwner, params Object[] args)
    {

    }

    // 状态处理
    public void Process(EntityParent theOwner, params Object[] args)
    {

    }
}
