using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaomaoFramework;
using CinemaDirector;
public class GameController
{
    #region 字段
    /// <summary>
    /// int=>场景中模型GUID
    /// </summary>
    private static Dictionary<int, EntityParent> gameObjects = new Dictionary<int, EntityParent>();
    /// <summary>
    /// 是否在城镇内
    /// </summary>
    public static bool isInTown = true;
    /// <summary>
    /// 是否显示技能特效
    /// </summary>
    public static bool isShowSkillFx = true;
    /// <summary>
    /// 是否正在加载场景
    /// </summary>
    public static bool isLoadingScene = false;
    /// <summary>
    /// 主角
    /// </summary>
    public static EntityMyself thePlayer;
    /// <summary>
    /// 世界节点
    /// </summary>
    public static Transform WorldPoint;
    public static EntityParent m_currentEntity;
    public static Dictionary<uint, EntityParent> entities = new Dictionary<uint, EntityParent>();
    #endregion
    #region 属性
    /// <summary>
    /// 游戏场景所有的实例物体
    /// </summary>
    public static Dictionary<int, EntityParent> GameObjects = new Dictionary<int, EntityParent>();
    /// <summary>
    /// 游戏中可见的实体，key=>uid,value=>entity
    /// </summary>
    public static Dictionary<uint, EntityParent> Entities
    {
        get { return entities; }
    }
    public static Cutscene currentCutScene;
    #endregion
    #region 构造方法
    static GameController()
    {
        AddListeners();
    }
    #endregion
    #region 公有方法
    public static void Init()
    {

    }
    public static void Start()
    {
        SceneBase s = new SceneBase();
        SceneManager.singleton.CreateScene(0, s, () =>
        {
            EventDispatch.Broadcast<EntityParentAttachDataBase,Action<GameObject>>("OnEntityAttached", new EntityParentAttachDataBase(),(gameobject)=>
            {
                Cutscene a = GameObject.Find("Chapter_1_Go").GetComponent<Cutscene>();
                a.GetComponentInChildren<CharacterTrackGroup>().Actor = gameobject.transform;
                a.transform.FindChild("BlackHole").GetComponent<ActorTrackGroup>().Actor = GameObject.Find("blackhole").transform;
                a.transform.FindChild("Player").GetComponent<ActorTrackGroup>().Actor = gameobject.transform;
                a.transform.FindChild("PlayerHip").GetComponent<ActorTrackGroup>().Actor = gameobject.transform.FindChild("Bones/Hip");
                a.transform.FindChild("PlayerLeftArmIk").GetComponent<ActorTrackGroup>().Actor = gameobject.transform.FindChild("IK/Left Arm");
                a.transform.FindChild("PlayerLeftLegIk").GetComponent<ActorTrackGroup>().Actor = GameObject.Find("IK/Left Leg").transform;
                a.transform.FindChild("PlayerRightArmIk").GetComponent<ActorTrackGroup>().Actor = GameObject.Find("IK/Right Arm").transform;
                a.transform.FindChild("PlayerRightLegIk").GetComponent<ActorTrackGroup>().Actor = GameObject.Find("IK/Right Leg").transform;
                a.transform.FindChild("PlayerRightFootIk").GetComponent<ActorTrackGroup>().Actor = GameObject.Find("IK/Right Foot").transform;
                a.transform.FindChild("PlayerLeftFootIk").GetComponent<ActorTrackGroup>().Actor = GameObject.Find("IK/Left Foot").transform;
                a.Optimize();
                a.CutsceneFinished += (sender, args) =>
                {
                    EventDispatch.Broadcast<string[], EntityParent>("DlgDialogue", new string[] { "Unity Tutorial: Zelda-Style Dialogue Part 1", "wefefwefewfwefewffewfwe" },thePlayer);
                };
                currentCutScene = a;
                UnityMonoDriver.s_instance.StartCoroutine(PlayCurrentScene());
                //a.Play();
            });
        });
        SceneManager.singleton.LoadScene();      
    }
    public static IEnumerator PlayCurrentScene()
    {
        if (currentCutScene != null)
        {
            currentCutScene.Play();
            yield return null;
        }
    }
    /// <summary>
    /// 实体进入到游戏世界中
    /// </summary>
    /// <param name="entity"></param>
    public static void OnEnterWorld(EntityParent entity)
    {
        //主要是处理缓存实体到字典中
    }
    #endregion
    #region 私有方法
    private static void AddListeners()
    {
        EventDispatch.AddListener<EntityParentAttachDataBase,Action<GameObject>>("OnEntityAttached", OnEntityAttached);
    }
    private static void OnEntityAttached(EntityParentAttachDataBase roleInfo,Action<GameObject> onEnterCallback)
    {
        bool ab = false;
        if (GameController.thePlayer == null)
        {
            ab = true;
            thePlayer = new EntityMyself();
        }
        m_currentEntity = thePlayer;
        thePlayer.SetEntityInfo(roleInfo);
        if (ab)
        {
            thePlayer.OnEnterWorld(()=>
            {
                if (onEnterCallback != null)
                    onEnterCallback(thePlayer.GameObject);
            });
        }
    }
    #endregion
}
