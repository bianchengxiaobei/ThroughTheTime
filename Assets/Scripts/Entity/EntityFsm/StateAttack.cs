using System;
using System.Collections.Generic;
using CaomaoFramework;
using CaomaoFramework.EntityFsm;
using CaomaoFramework.Data;
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
        int skillId = (int)args[0];
        SkillData data = SkillData.dataMap[skillId];
        theOwner.motor.speed = 0;//设置Animator的speed变量为0，代表是Idle
        theOwner.motor.targetSpeed = 0;//移动速度为0
        for (int i = 0; i < data.skillAction.Count; i++)
        {
            SkillActionData action = SkillActionData.dataMap[data.skillAction[i]];
            if (i == 0)
            {
                this.ProcessHit(theOwner, skillId, data.skillAction[0]);
                if (theOwner is EntityMyself)
                {
                    theOwner.motor.enableStick = action.enableStick;
                }
            }
            if (i + 1 == data.skillAction.Count)
            {
                break;
            }
        }
    }
    private void ProcessHit(EntityParent theOwner, int skillId, int actionId)
    {
        SkillActionData actionData = SkillActionData.dataMap[actionId];
        SkillData data = SkillData.dataMap[skillId];
        // 回调，基于计时器。 在duration 后切换回 idle 状态
        int duration = actionData.duration;

        theOwner.OnAttacking(actionId);
    }
}
