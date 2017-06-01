using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaomaoFramework;
public class UnityTool : UnityToolBase
{
    /// <summary>
    /// 返回所在时间内的帧数
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static int TimeToFrameCount(float time)
    {
        return (int)(time / Time.deltaTime);
    }
}
