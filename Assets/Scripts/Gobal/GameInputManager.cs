using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
public class GameInputManager : MonoBehaviour
{
    public static GameInputManager Instance;
    private InputManager m_oRewiredInputManager;
    private bool m_bIsInitialized = false;
    public Vector2 moveVector;
    private Player m_oCurrentRewiredPlayer;
    public int playerId = 0;
    /// <summary>
    /// 是否在移动
    /// </summary>
    public bool IsMoving
    {
        get
        {
            return Mathf.Abs(moveVector.x) > 0f || Mathf.Abs(moveVector.y) > 0f;
        }
    }
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (!ReInput.isReady)
        {
            return;
        }
        Init();
    }
	void Update ()
    {
        if (!ReInput.isReady) return;
        GetInput();
	}
    public bool GetMoveUpDown()
    {
        return this.m_oCurrentRewiredPlayer.GetButtonDown("Move Up");
    }
    public bool GetMoveDownDown()
    {
        return this.m_oCurrentRewiredPlayer.GetButtonDown("Move Down");
    }
    public bool GetMoveRightDown()
    {
        return this.m_oCurrentRewiredPlayer.GetButtonDown("Move Right");
    }
    public bool GetMoveLeftDown()
    {
        return this.m_oCurrentRewiredPlayer.GetButtonDown("Move Left");
    }
    public bool GetAttackButtonDown()
    {
        return this.m_oCurrentRewiredPlayer.GetButtonDown("Attack");
    }
    public bool GetRollButtonDown()
    {
        return this.m_oCurrentRewiredPlayer.GetButtonDown("Roll");
    }
    public bool GetMapOpenButtonDown()
    {
        return this.m_oCurrentRewiredPlayer.GetButtonDown("Map");
    }
    public bool GetJumpButtonDown()
    {
        return this.m_oCurrentRewiredPlayer.GetButtonDown("Jump");
    }
    public bool GetJumpButtonUp()
    {
        return this.m_oCurrentRewiredPlayer.GetButtonUp("Jump");
    }
    public bool GetInteractiveButtonDown()
    {
        return this.m_oCurrentRewiredPlayer.GetButtonUp("Interactive");
    }
    private void Init()
    {
        if (this.m_bIsInitialized) return;
        if (!ReInput.isReady) return;

        if (null == this.m_oRewiredInputManager)
        {
            this.m_oRewiredInputManager = GameObject.FindObjectOfType<InputManager>();
            if (null == this.m_oRewiredInputManager)
            {
                Debug.LogError("null == this.m_oRewiredInputManager");
                return;
            }
        }
        m_oCurrentRewiredPlayer = ReInput.players.GetPlayer(this.playerId);
        if (null != GameInputManager.Instance)
        {
            Debug.LogError("GameInputManager.Instance 已经存在");
            return;
        }
        GameInputManager.Instance = this;
        m_bIsInitialized = true;
    }
    private void GetInput()
    {
        moveVector.x = this.m_oCurrentRewiredPlayer.GetAxis("Move Horizontal");
        moveVector.y = this.m_oCurrentRewiredPlayer.GetAxis("Move Vertical");
    }
}
