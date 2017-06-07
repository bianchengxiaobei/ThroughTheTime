using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CaomaoFramework;
public struct UpdateACModifyInfo
{
    public bool isAdd;

    public Action<float> ac;

    public UpdateACModifyInfo(bool b, Action<float> a)
    {
        this.isAdd = b;
        this.ac = a;
    }
}
public class ActorMyself : ActorPlayer<EntityMyself>
{
    public GameMotor motor;
    public List<SkillInput> skillInputs;
    private SortedDictionary<int, List<UpdateACModifyInfo>> needModifyACs = new SortedDictionary<int, List<UpdateACModifyInfo>>();
    private SortedDictionary<int, List<Action<float>>> updateACs = new SortedDictionary<int, List<Action<float>>>();
    public List<HitFrame> hitFrames;
    public bool isMoving = false;
    private int dirCmdTakeEffectFrameCount;
    public override void Awake()
    {
        base.Awake();
        foreach (var hit in hitFrames)
        {
            hit.Init(this);
        }
        DontDestroyOnLoad(this);
    }
	void Update ()
    {
        ActChange();
        this.PrecessMotionInput();
        if (null == Entity)
        {
            return;
        }
	}
    private void FixedUpdate()
    {
        foreach (var current in this.needModifyACs)
        {
            for (int i = 0; i < current.Value.Count; i++)
            {
                if (current.Value[i].isAdd)
                {
                    if (!this.updateACs.ContainsKey(current.Key))
                    {
                        this.updateACs.Add(current.Key, new List<Action<float>>());
                    }
                    this.updateACs[current.Key].Add(current.Value[i].ac);
                }
                else if (this.updateACs.ContainsKey(current.Key))
                {
                    this.updateACs[current.Key].Remove(current.Value[i].ac);
                    if (this.updateACs[current.Key].Count == 0)
                    {
                        this.updateACs.Remove(current.Key);
                    }
                }
            }
        }
        this.needModifyACs.Clear();
        var updateAcs = this.updateACs.Values;
        foreach (List<Action<float>> current in updateAcs)
        {
            for (int k = 0; k < current.Count; k++)
            {
                if (!this.IsACInWillRemove(current[k]))
                {
                    current[k](Time.fixedDeltaTime);
                }
            }
        }
    }
    private bool IsACInWillRemove(Action<float> ac)
    {
        foreach (KeyValuePair<int, List<UpdateACModifyInfo>> current in this.needModifyACs)
        {
            for (int i = 0; i < current.Value.Count; i++)
            {
                if (!current.Value[i].isAdd && current.Value[i].ac == ac)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public override void AddUpdateAction(int id, Action<float> ac)
    {
        if (!this.needModifyACs.ContainsKey(id))
        {
            this.needModifyACs.Add(id, new List<UpdateACModifyInfo>());
        }
        this.needModifyACs[id].Add(new UpdateACModifyInfo(true, ac));
    }
    public override void RemoveUpdateAction(int id, Action<float> ac)
    {
        if (!this.needModifyACs.ContainsKey(id))
        {
            this.needModifyACs.Add(id, new List<UpdateACModifyInfo>());
        }
        this.needModifyACs[id].Add(new UpdateACModifyInfo(false, ac));
    }
    private void PrecessMotionInput()
    {
        if (null == Entity)
        {
            return;
        }
        //if (GameInputManager.Instance.GetJumpButtonDown())
        //{
        //    motor.Jump();
        //}
        //if (GameInputManager.Instance.GetJumpButtonUp())
        //{
        //    motor.JumpMin();
        //}
        //if (GameInputManager.Instance.GetRollButtonDown())
        //{
        //    motor.Roll();
        //}
        if (GameInputManager.Instance.GetMoveRightDown())
        {
            this.InputCommand(PlayerCommandType.Forward);
        }
        if (GameInputManager.Instance.GetMoveLeftDown())
        {
            this.InputCommand(PlayerCommandType.Backward);
        }
        if (GameInputManager.Instance.GetMoveUpDown())
        {
            this.InputCommand(PlayerCommandType.Up);
        }
        if (GameInputManager.Instance.GetMoveDownDown())
        {
            this.InputCommand(PlayerCommandType.Down);
        }
        if (GameInputManager.Instance.GetJumpButtonDown())
        {
            this.InputCommand(PlayerCommandType.Jump);
        }
        if (GameInputManager.Instance.GetJumpButtonUp())
        {
            this.InputCommand(PlayerCommandType.JumpUp);
        }
        if (GameInputManager.Instance.GetAttackButtonDown())
        {
            this.InputCommand(PlayerCommandType.Punch);
        }
        if (GameInputManager.Instance.GetRollButtonDown())
        {
            this.InputCommand(PlayerCommandType.Roll);
        }
        if (Time.frameCount != this.dirCmdTakeEffectFrameCount)
        {
            if (motor.enableStick)
            {
                if (GameInputManager.Instance.IsMoving)
                {
                    if (GameInputManager.Instance.moveVector.x != 0)
                    {
                        Entity.SetForward(GameInputManager.Instance.moveVector.x > 0f);
                    }
                    isMoving = true;
                    Entity.Move();
                }
                else if (!this.motor.isMovingToTarget)
                {
                    if (isMoving)
                    {
                        isMoving = false;
                        Entity.Idle();
                    }
                }
            }
        }
    }
    public void InputCommand(PlayerCommandType pct)
    {
        if (pct == PlayerCommandType.End)
        {
            return;
        }
        bool isTuneDirCommand = false;
        //如果按下的是向前走，也就是说按下键盘的D键，但是人物现在是朝左边，就调整朝向
        if (pct == PlayerCommandType.Forward && Entity.IsCurrLeft)
        {
            isTuneDirCommand = true;
        }
        //如果按下的是向后走，也就是说按下键盘的A键，但是人物现在不是朝左边，就调整朝向
        if (pct == PlayerCommandType.Backward && !Entity.IsCurrLeft)
        {
            isTuneDirCommand = true;
        }
        //如果现在角色是朝左边
        if (Entity.IsCurrLeft)
        {
            //不是很懂
            if (pct == PlayerCommandType.Forward)
            {
                pct = PlayerCommandType.Backward;
            }
            else if (pct == PlayerCommandType.Backward)
            {
                pct = PlayerCommandType.Forward;
            }
        }
        bool input = false;
        bool triggerSkillSuccess = false;
        for (int j = 0; j < this.skillInputs.Count; j++)
        {
            SkillInput skillInput = this.skillInputs[j];
            bool inputVaild;//技能输入是否有效
            bool skillSuccess;//技能释放是否成功
            this.CheckSkillInput(skillInput.skillCommand, pct, isTuneDirCommand, false, out inputVaild, out skillSuccess);
            input = (inputVaild || input);
            triggerSkillSuccess = (skillSuccess || triggerSkillSuccess);
        }
        if (!triggerSkillSuccess)
        {
            if (pct == PlayerCommandType.Jump)
            {
                motor.Jump();
            }
            else if (pct == PlayerCommandType.JumpUp)
            {
                motor.JumpMin();
            }
            else if (pct == PlayerCommandType.Roll)
            {
                motor.Roll();
            }
            else if (pct == PlayerCommandType.Interactive)
            {
                
            }
            else if(pct == PlayerCommandType.Punch)
            {
                m_animator.SetBool("IsHited",false);
                m_animator.SetTrigger("Punch");
            }
        }

        if (input && (pct == PlayerCommandType.Forward || pct == PlayerCommandType.Backward || pct == PlayerCommandType.Up || pct == PlayerCommandType.Down))
        {
            this.dirCmdTakeEffectFrameCount = Time.frameCount;
        }
    }
    private void CheckSkillInput(SkillCommandSet scs, PlayerCommandType pct, bool isTuneDirCommand, bool checkRelease, out bool inputValid, out bool skillSucceed)
    {

        inputValid = false;
        skillSucceed = false;
        //如果不检查蓄力，就永远不能发出蓄力技能
        if (!checkRelease && pct == PlayerCommandType.ChargeRelease)
        {
            return;
        }
        //如果技能是只能在空中释放，但是主角却在地上，返回
        if (scs.condition == SkillCondition.OnlyAir && this.motor.IsOnGround())
        {
            return;
        }
        if (scs.condition == SkillCondition.OnlyGound && !this.motor.IsOnGround())
        {
            return;
        }
        List<PlayerCommand> commands = scs.commands;
        //如果当前帧大于技能下个有效帧，就重头开始计算
        if (Time.frameCount > scs.nextCombineValidFC)
        {
            scs.idx = 0;
            scs.tunedPlayerCmdType = PlayerCommandType.End;
            if (this.IsInputCmdMatch(commands, scs.idx, pct, isTuneDirCommand))
            {
                scs.nextCombineValidFC = Time.frameCount + UnityTool.TimeToFrameCount(commands[scs.idx].combineDuration);
                scs.idx++;
            }
            else
            {
                scs.nextCombineValidFC = int.MaxValue;
            }
        }
        else if (this.IsInputCmdMatch(commands, scs.idx, pct, isTuneDirCommand))
        {
            Debug.Log(scs.idx);
            inputValid = true;
            //技能到了技能指令的最后一步，重置，并触发技能
            if (scs.idx >= commands.Count - 1)
            {
                Debug.Log("skill");
                scs.idx = 0;
                scs.nextCombineValidFC = int.MaxValue;
                //this.TriggerSkill(scs.skillName, scs.tunedPlayerCmdType);
                (Entity as EntityMyself).TriggerSkill(scs.skillName, scs.tunedPlayerCmdType);
                skillSucceed = true;
                scs.tunedPlayerCmdType = PlayerCommandType.End;
            }
            else
            {
                scs.nextCombineValidFC = Time.frameCount + UnityTool.TimeToFrameCount(commands[scs.idx].combineDuration);
                scs.idx++;
                if (isTuneDirCommand && Entity.IsCurrLeft)
                {
                    scs.tunedPlayerCmdType = ((pct != PlayerCommandType.Backward) ? PlayerCommandType.Backward : PlayerCommandType.Forward);
                }
            }
        }
    }
    private bool IsInputCmdMatch(List<PlayerCommand> cmds, int cmdIdx, PlayerCommandType rhs, bool isTuneDirCommand)
    {
        if (cmdIdx >= cmds.Count)
        {
            return false;
        }
        PlayerCommandType pct = cmds[cmdIdx].pct;
        return pct == rhs || (isTuneDirCommand && cmdIdx == 0 && (pct == PlayerCommandType.Forward || pct == PlayerCommandType.Backward));
    }
}
