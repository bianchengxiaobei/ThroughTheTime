using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaomaoFramework;
using CaomaoFramework.EntityFsm;
public class FSMMotion : FSMParent
{
    public FSMMotion()
    {
        m_theFSM.Add(MotionState.IDLE, new StateIdle());
        m_theFSM.Add(MotionState.WALKING, new StateWalking());
    }
    public override void ChangeStatus(EntityParent owner, string newState, params object[] args)
    {
        if (owner.CurrentMotionState == newState && newState != MotionState.ATTACKING)
        {
            return;
        }
        if (!m_theFSM.ContainsKey(newState))
        {
            return;
        }
        m_theFSM[owner.CurrentMotionState].Exit(owner, args);
        m_theFSM[newState].Enter(owner, args);
        m_theFSM[newState].Process(owner, args);
    }
}
