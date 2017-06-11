using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaomaoFramework;
public class SkillData : GameData<SkillData>
{
    public static string fileName = "skillData";
    public List<int> skillAction { get; set; }
    public List<int> cd { get; set; }
    public List<int> dependSkill { get;set; }
    /// <summary>
    /// 技能类型，比如普通攻击，技能1，技能2
    /// 0-普工，1-技能1 。。。。
    /// </summary>
    public int type { get; set; }
    /// <summary>
    /// 武器类型
    /// </summary>
    public int weaponType { get; set; }
}
