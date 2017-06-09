using System;
using System.Collections.Generic;
using UnityEngine;
using CaomaoFramework;
using CaomaoFramework.Data;
using CaomaoFramework.Resource;
using CaomaoFramework.Effect;
using CaomaoFramework.EntityFsm;
using CinemaDirector;
public partial class EntityMyself : EntityPlayer
{
    #region 字段
    private bool m_bIsCreatingModel = false;
    //上次使用技能的时间
    public static float preSkillTime = 0;
    //是否现在朝向左
    private bool m_bIsCurrLeft = false;
    //是否现在死亡了
    private bool m_bIsDead = false;

    #endregion
    #region 属性
    /// <summary>
    /// 是否朝向左边
    /// </summary>
    public bool IsCurrLeft
    {
        get
        {
            return this.m_bIsCurrLeft;
        }
        set
        {
            this.m_bIsCurrLeft = value;
        }
    }
    /// <summary>
    /// 是否现在死亡
    /// </summary>
    public bool IsDead
    {
        get
        {
            return this.m_bIsDead;
        }
        set
        {
            this.m_bIsDead = value;
        }
    }
    #endregion
    #region 重写方法
    public override void OnEnterWorld(Action callback = null)
    {
        base.OnEnterWorld();
        CreateModel(callback);
    }
    public override void CreateModel(Action callback = null)
    {
        CreateActualModel(callback);
    }
    public override void CreateActualModel(Action callback = null)
    {
        m_bIsCreatingModel = true;
        ResourceManager.singleton.LoadModel("Prefabs/Character/Player", new AssetRequestFinishedEventHandler((assetRequest) =>
        {
            GameObject gameobject = assetRequest.AssetResource.MainAsset as GameObject;
            gameobject = GameObject.Instantiate(gameobject) as GameObject;
            gameobject.tag = "Player";
            if (fsmMotion == null)
            {
                fsmMotion = new FSMMotion();
            }
            ActorMyself actor = gameobject.GetComponent<ActorMyself>();
            if (actor == null)
            {
                actor = gameobject.AddComponent<ActorMyself>();
            }
            motor = gameobject.GetComponent<GameMotorMyself>();
            if (motor == null)
            {
                motor = gameobject.AddComponent<GameMotorMyself>();
            }            
            animator = gameobject.GetComponent<Animator>();
            audioSource = gameobject.GetComponent<AudioSource>();
            if (null == audioSource)
            {
                audioSource = gameobject.AddComponent<AudioSource>();
            }
            audioSource.rolloffMode = AudioRolloffMode.Custom;
            effectHandler = gameobject.AddComponent<EffectHandler>();
            actor.motor = motor;
            actor.Entity = this;
            GameObject = gameobject;
            Transform = gameobject.transform;
            Transform.gameObject.layer = 8;
            Transform.SetParent(GameController.WorldPoint);
            UpdatePosition();
            SetForward(true);
            gameobject.SetActive(false);
            gameobject.SetActive(true);
            this.IsVisiable = true;
            m_bIsCreatingModel = false;
            if (callback != null)
            {
                callback();
            }
        }));
    }
    public override void Idle()
    {
        //首先应该判断是否死亡
        if (null == m_battleManager)
        {
            ChangeMotionState(MotionState.IDLE);
        }
        else
        {
            this.m_battleManager.Idle();
        }
    }
    public override void CastSkill(string skillName)
    {
        //int skillId = -1;
        //foreach (var data in SkillData.dataMap.Values)
        //{
        //    if (data.name.Equals(skillName))
        //    {
        //        skillId = data.id;
        //    }
        //}
        //if (this.m_battleManager != null)
        //{
        //    this.m_battleManager.CastSkill(skillId);
        //}
        Debug.Log("Skill");
    }

    #endregion
    #region 公有方法
    public void TriggerSkill(string _skillName, PlayerCommandType tunedPCT = PlayerCommandType.End)
    {
        if (m_battleManager != null)
        {
            (this.m_battleManager as PlayerBattleManager).TriggerSkill(_skillName, tunedPCT);
        }
    }
    public void SetForward(bool forward)
    {
        this.m_bIsCurrLeft = !forward;
        Vector3 eulerAngles = Transform.rotation.eulerAngles;
        eulerAngles.y = (this.m_bIsCurrLeft ? 0f : 180f);//转180度，180度默认是向右，也就是前|||0度是向左，也就是后
        Transform.rotation = Quaternion.Euler(eulerAngles);
    }
    #endregion
    #region 私有方法
    #endregion
}
