using System;
using System.Collections.Generic;
using UnityEngine;
using CaomaoFramework;
using CaomaoFramework.Data;
public class PlayerSkillManager : SkillManagerBase
{
    private int lastSkillID = 0;
    private int attackCount = 1;
    private float lastAttackTime = 0.0f;
    /// <summary>
    /// 默认时间间隔为0.2s
    /// </summary>
    private int intervalTime = 200;
    /// <summary>
    /// 每个技能存储着释放的时间点
    /// </summary>
    private Dictionary<int, float> skillLastCastTime = new Dictionary<int, float>();
    /// <summary>
    /// 连续技能，0-前置技能id，1-前置技能次数，2-后置技能id
    /// </summary>
    private Dictionary<int, List<int>> dependenceSkill = new Dictionary<int, List<int>>();
    private Dictionary<int, int> comboSkillPeriod = new Dictionary<int, int>();
    private Dictionary<int, int> skillEnergyConsume = new Dictionary<int, int>();
    private Dictionary<int, int> skillCoolTime = new Dictionary<int, int>();
    private Dictionary<int, int> skillInterval = new Dictionary<int, int>();
    private SkillMapping skillMapping = new SkillMapping();
    public PlayerSkillManager(EntityParent entity)
        : base(entity)
    {
        theOwner = entity;
        this.AddListeners();
        try
        {
            Init();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void AddListeners()
    {
        EventDispatch.AddListener(Events.OnChangeWeapon, this.UpdateSkillData);
    }
    private void Init()
    {
        // 初始化 技能冷却时间： 技能cd， 连续技cd，连续技最大有效时间，组技能cd, 
        foreach (KeyValuePair<int, SkillData> pair in SkillData.dataMap)
        {
            if (pair.Value.cd.Count == 3)
            {
                // 技能 cd
                skillCoolTime[pair.Key] = pair.Value.cd[0];
                if (pair.Value.cd[2] > 0)
                {
                    // 记录连续技最大有效时间
                    comboSkillPeriod[pair.Key] = pair.Value.cd[1];
                }
                skillInterval[pair.Key] = pair.Value.cd[2];
            }
            else
            {
                Debug.LogError("format error: spell cool time:" + pair.Key);
            }
        }
        // 依赖技能： 前置技能， 前置次数， 后置技能
        foreach (KeyValuePair<int, SkillData> pair in SkillData.dataMap)
        {
            if (pair.Value.dependSkill != null && pair.Value.dependSkill.Count == 3)
            {
                dependenceSkill[pair.Key] = pair.Value.dependSkill;
            }
            else
            {
                Debug.LogError("format error: spell dependence:" + pair.Key);
            }
        }
    }
    public int GetNormalAttackId()
    {
        int interval = (int)((Time.realtimeSinceStartup - lastAttackTime)*1000);
        if (dependenceSkill.ContainsKey(lastSkillID) && this.comboSkillPeriod.ContainsKey(lastSkillID))
        {
            int nextSkill = dependenceSkill[lastSkillID][2];
            int count = dependenceSkill[lastSkillID][1];
            //if (attackCount < count)
            //{
            //    attackCount++;
            //}
            //else
            //{
            //attackCount = 1;
            //Debug.Log("Interval:"+interval);
            //Debug.Log("SkillInterval:" + skillInterval[lastSkillID]);
            //if (nextSkill > 0 && interval >= skillInterval[lastSkillID] && interval < this.comboSkillPeriod[lastSkillID])
            if (nextSkill > 0 && interval <= skillCoolTime[lastSkillID])
            {
                    Debug.Log("2");
                    lastSkillID = nextSkill;
                    return nextSkill;
            }
            //}
        }
        lastSkillID = skillMapping.normalAttack;
        return skillMapping.normalAttack;
    }
    /// <summary>
    /// 是否是连续技能
    /// </summary>
    /// <param name="skillId"></param>
    /// <returns></returns>
    public bool HasDependence(int skillId)
    {
        //如果有并且有后续技能id，就说明是连续技能
        if (dependenceSkill.ContainsKey(skillId) && dependenceSkill[skillId][2] > 0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 更新技能cd数据
    /// </summary>
    public void UpdateSkillCooltime(int skillId)
    {
        try
        {
            this.intervalTime = skillInterval[skillId];
            lastAttackTime = Time.realtimeSinceStartup;
            skillLastCastTime[skillId] = lastAttackTime;
        }
        catch (Exception e)
        {
            Debug.Log("SkillId:" + skillId);

        }

    }
    /// <summary>
    /// 更新技能数据
    /// </summary>
    public void UpdateSkillData()
    {
        skillMapping.Clear();
        foreach (var data in SkillData.dataMap)
        {
            if (null == data.Value)
            {
                Debug.LogError("找不到该技能:" + data.Key);
                continue;
            }
            int weaponType = (GameController.thePlayer as EntityMyself).GetWeaponType();
            switch (data.Value.type)
            {
                case 0:
                    if (data.Value.weaponType == weaponType && data.Value.dependSkill[0] == 0 && data.Value.dependSkill[1] == 0)
                    {
                        skillMapping.normalAttack = data.Value.id;
                    }
                    break;
            }
        }
        //这边应该更新skill界面

    }
    public override void Compensation(float t)
    {
        //lastAttackTime += t;
        //List<int> key = new List<int>();
        //foreach (var item in skillLastCastTime)
        //{
        //    key.Add(item.Key);
        //}
        //for (int i = 0; i < key.Count; i++)
        //{
        //    skillLastCastTime[key[i]] += t;
        //}
    }
    /// <summary>
    /// 取得连续技能触发的时间间隔
    /// </summary>
    /// <param name="skillId"></param>
    /// <returns></returns>
    public int GetSkillIntervalTime(int skillId)
    {
        if (!skillInterval.ContainsKey(skillId))
        {
            return 0;
        }
        Debug.Log("正在:" + skillInterval[skillId]);
        return skillInterval[skillId];
    }
    /// <summary>
    /// 是否还处在连续技能技能中
    /// </summary>
    /// <returns></returns>
    public bool IsIntervalCooldown()
    {
        int attackInterval = (int)((Time.realtimeSinceStartup - lastAttackTime)*1000);
        Debug.Log("AttackInterval:" + attackInterval);
        Debug.Log("IntervalTime:" + intervalTime);
        if (attackInterval < intervalTime)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 是否技能在冷却中
    /// </summary>
    /// <param name="skillId"></param>
    /// <returns></returns>
    public bool IsSkillCooldown(int skillId)
    {
        if (!SkillData.dataMap.ContainsKey(skillId))
        {
            return true;
        }
        if (!this.skillLastCastTime.ContainsKey(skillId))
        {
            skillLastCastTime[skillId] = 0;
        }
        int skillInterval = (int)((Time.realtimeSinceStartup - this.skillLastCastTime[skillId])*1000);
        if (!this.skillCoolTime.ContainsKey(skillId))
        {
            skillCoolTime[skillId] = 0;
        }
        if (skillInterval < this.skillCoolTime[skillId])
        {
            return true;
        }
        return false;
    }
    public void ClearComboSkill()
    {
        lastSkillID = 0;
    }

    public override void OnAttacking(int hitActionId)
    {
        SkillActionData data = SkillActionData.dataMap[hitActionId] as SkillActionData;
        //动作
        int action = data.action;
        if (action > 0)
        {
            Debug.Log("OnAttacking:" + action);
            theOwner.SetAction(action);
        }
    }
}
