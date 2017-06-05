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
    /// <summary>
    /// 取得节点下的所有T组件,存到List中
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="trans"></param>
    /// <param name="result"></param>
    /// <param name="includeInActive"></param>
    public static void GetComponentsInChildren<T>(Transform trans, List<T> result, bool includeInActive = false)
    {
        T[] components = trans.GetComponents<T>();
        for (int i = 0; i < components.Length; i++)
        {
            result.Add(components[i]);
        }
        for (int j = 0; j < trans.childCount; j++)
        {
            Transform child = trans.GetChild(j);
            if (includeInActive || child.gameObject.activeSelf)
            {
                UnityTool.GetComponentsInChildren<T>(child, result, includeInActive);
            }
        }
    }
    /// <summary>
    /// 两个碰撞器是否交互
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <returns></returns>
    public static bool IsColliderTounching(Collider2D c1, Collider2D c2)
    {
        return c1.bounds.min.x < c2.bounds.max.x && c1.bounds.max.x > c2.bounds.min.x && c1.bounds.min.y < c2.bounds.max.y && c1.bounds.max.y > c2.bounds.min.y;
    }
}
