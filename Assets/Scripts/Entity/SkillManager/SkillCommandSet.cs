using System.Collections;
using System.Collections.Generic;
using System;
public enum SkillCondition
{
    OnlyGound,
    OnlyAir,
    GroundAndAir
}
[Serializable]
public class SkillCommandSet
{
    public List<PlayerCommand> commands;//玩家按键命令组合，比如一个技能需要上，右，出拳

    public string skillName;//技能名称

    public SkillCondition condition;//技能条件限制，有只能在地上释放，有只能在天上，有天上地上都可以

    [NonSerialized]
    public int idx;

    [NonSerialized]
    public int nextCombineValidFC;

    [NonSerialized]
    public PlayerCommandType tunedPlayerCmdType = PlayerCommandType.End;

    public void Reset()
    {
        this.idx = 0;
        this.nextCombineValidFC = int.MaxValue;
        this.tunedPlayerCmdType = PlayerCommandType.End;
    }
}