using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaomaoFramework;
using CaomaoFramework.Data;
public class PlayerSkillManager : SkillManagerBase
{
    private int lastSkillID = 0;
    private float lastAttackTime = 0.0f;
    private Dictionary<int, float> skillLastCastTime = new Dictionary<int, float>();
    private Dictionary<int, List<int>> dependenceSkill = new Dictionary<int, List<int>>();
    private Dictionary<int, int> skillEnergyConsume = new Dictionary<int, int>();
    private Dictionary<int, float> skillCoolTime = new Dictionary<int, float>();
    public PlayerSkillManager(EntityParent entity) 
        : base(entity)
    {
        theOwner = entity;
    }
    private void AddListeners()
    {

    }
    private void Init()
    {
    }

}
