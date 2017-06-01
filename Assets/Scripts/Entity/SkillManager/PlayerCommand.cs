using System;
using UnityEngine;

[Serializable]
public class PlayerCommand
{
    public PlayerCommandType pct;

    public float combineDuration;
}
public enum PlayerCommandType
{
    Forward,
    Backward,
    Punch,//攻击
    Charge,//蓄力
    Jump,
    JumpUp,
    ChargeRelease,//蓄力释放
    Dodge,//躲闪
    Up,
    Down,
    Endure,//阻挡
    Roll,
    Interactive,
    End
}