using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaomaoFramework;
public class PlayerBattleManager : BattleManagerBase
{
    private List<int> preCmds = new List<int>();
    public PlayerBattleManager(EntityParent _theOwner, SkillManagerBase _skillManager) 
        : base(_theOwner, _skillManager)
    {
        this.m_skillManager = _skillManager;
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
}
