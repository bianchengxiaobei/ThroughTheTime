using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaomaoFramework;
using CaomaoFramework.EntityFsm;
public class PlayerBattleManager : BattleManagerBase
{
    private List<int> preCmds = new List<int>();
    public PlayerBattleManager(EntityParent _theOwner, SkillManagerBase _skillManager) 
        : base(_theOwner, _skillManager)
    {
        this.m_skillManager = _skillManager;
    }
    public override void CastSkill(int skillId)
    {
        theOnwer.ChangeMotionState(MotionState.ATTACKING, skillId);
    }
    public override void OnAttacking(int nSkillID)
    {
        m_skillManager.OnAttacking(nSkillID);
    }
    public void TriggerSkill(string _skillName, PlayerCommandType tunedPCT = PlayerCommandType.End)
    {
        if (theOnwer.stiff || theOnwer.charging || string.IsNullOrEmpty(_skillName))
        {
            return;
        }
        string skillName = _skillName;
        //这边判断技能正在回复
        if (GameInputManager.Instance.IsMoving)
        {
            bool isForwardPressing = false;
            if (GameInputManager.Instance.moveVector.x > 0)
            {
                isForwardPressing = true;
            }
            (theOnwer as EntityMyself).SetForward(isForwardPressing);
        }
        else if (tunedPCT != PlayerCommandType.End)
        {
            if (tunedPCT == PlayerCommandType.Forward)
            {
                (theOnwer as EntityMyself).SetForward(true);
            }
            else if (tunedPCT == PlayerCommandType.Backward)
            {
                (theOnwer as EntityMyself).SetForward(false);
            }
        }
        //能力是否足够
        theOnwer.CastSkill(skillName);
    }
    public void NormalAttack()
    {
        if (theOnwer.IsDead || theOnwer.stiff || theOnwer.charging)
        {
            return;
        }
        //连续技能时间间隔内
        //if ((this.m_skillManager as PlayerSkillManager).IsIntervalCooldown())
        //{
        //    Debug.Log("1");
        //    preCmds.Add(0);
        //    return;
        //}
        int curSkill = (this.m_skillManager as PlayerSkillManager).GetNormalAttackId();
        //if (curSkill == theOnwer.currSkillID && theOnwer.currSkillID != -1)
        //{
        //    Debug.Log("3");
        //    preCmds.Add(0);
        //    return;
        //}
        if ((this.m_skillManager as PlayerSkillManager).IsSkillCooldown(curSkill))
        {
            Debug.Log("4");
            //(this.m_skillManager as PlayerSkillManager).ClearComboSkill();
            //preCmds.Add(0);
            return;
        }
        //如果没有连续技能就，没有下一条指令
        //if (!(this.m_skillManager as PlayerSkillManager).HasDependence(curSkill))
        //{
        //    Debug.Log("5");
        //    this.preCmds.Clear();
        //}
        (this.m_skillManager as PlayerSkillManager).UpdateSkillCooltime(curSkill);
        EntityMyself.preSkillTime = Time.realtimeSinceStartup;
        theOnwer.CastSkill(curSkill);
        //TimerManager.AddTimer((uint)((m_skillManager as PlayerSkillManager).GetSkillIntervalTime(curSkill)), 0, NextNormalAttack);
    }
    private void NextNormalAttack()
    {
        if (preCmds.Count == 0)
        {
            return;
        }
        Debug.Log("Next");
        preCmds.RemoveAt(0);
        NormalAttack();
    }
}
