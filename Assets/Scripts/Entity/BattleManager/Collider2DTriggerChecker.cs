using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Collider2DTriggerChecker
{
    private Collider2D theOwnerCollider;
    private Action<Collider2D> triggerEnterAC;
    private Action<Collider2D> triggerExitAC;
    private Action<Collider2D> triggerStayedAC;
    //上一帧所有的碰撞器
    private List<Collider2D> lastFrameStayedCols;
    //当前帧处理的碰撞器
    private List<Collider2D> thisFrameStayedCols;

    private bool lastFrameTheOwnerEnabled;

    private static List<Collider2D> colsCache = new List<Collider2D>();
    //当前帧数
    private static int currFrameCount = -1;
    public Collider2DTriggerChecker(Collider2D h)
    {
        this.theOwnerCollider = h;
        this.lastFrameStayedCols = new List<Collider2D>();
        this.thisFrameStayedCols = new List<Collider2D>();
    }
    public void Update()
    {
        if (this.lastFrameTheOwnerEnabled && !this.theOwnerCollider.enabled)
        {
            this.Reset();
        }
        //记录主角碰撞是否激活
        this.lastFrameTheOwnerEnabled = this.theOwnerCollider.enabled;
        //主角碰撞没有激活就直接return
        if (!this.theOwnerCollider.enabled)
        {
            return;
        }
        if (Collider2DTriggerChecker.currFrameCount != Time.frameCount)
        {
            Collider2DTriggerChecker.colsCache.Clear();
            //初始化在World节点下的所欲碰撞加入colsCache
            UnityTool.GetComponentsInChildren<Collider2D>(GameController.WorldPoint, Collider2DTriggerChecker.colsCache, false);
            Collider2DTriggerChecker.currFrameCount = Time.frameCount;
        }
        for (int i = 0; i < Collider2DTriggerChecker.colsCache.Count; i++)
        {
            Collider2D collider2D = Collider2DTriggerChecker.colsCache[i];
            if (collider2D != this.theOwnerCollider)
            {
                if (collider2D.isTrigger)
                {
                    if (!Physics2D.GetIgnoreLayerCollision(this.theOwnerCollider.gameObject.layer, collider2D.gameObject.layer))
                    {
                        //如果主角碰撞器碰到其他碰撞器，并且this.thisFrameStayedCols链表没有包含这个碰撞器
                        if (UnityTool.IsColliderTounching(this.theOwnerCollider, collider2D) && !this.thisFrameStayedCols.Contains(collider2D))
                        {
                            this.thisFrameStayedCols.Add(collider2D);
                        }
                    }
                }
            }
        }
        if (this.triggerEnterAC != null)
        {
            for (int j = 0; j < this.thisFrameStayedCols.Count; j++)
            {
                if (!this.lastFrameStayedCols.Contains(this.thisFrameStayedCols[j]))
                {
                    this.triggerEnterAC(this.thisFrameStayedCols[j]);
                }
            }
        }
        if (this.triggerStayedAC != null)
        {
            for (int k = 0; k < this.thisFrameStayedCols.Count; k++)
            {
                this.triggerStayedAC(this.thisFrameStayedCols[k]);
            }
        }
        if (this.triggerExitAC != null)
        {
            for (int l = 0; l < this.lastFrameStayedCols.Count; l++)
            {
                if (!this.thisFrameStayedCols.Contains(this.lastFrameStayedCols[l]))
                {
                    this.triggerExitAC(this.lastFrameStayedCols[l]);
                }
            }
        }
        this.lastFrameStayedCols.Clear();
        for (int m = 0; m < this.thisFrameStayedCols.Count; m++)
        {
            this.lastFrameStayedCols.Add(this.thisFrameStayedCols[m]);
        }
        this.thisFrameStayedCols.Clear();
    }
    public void OnDisable()
    {
        this.Reset();
    }
    public void SetTriggerEnterAC(Action<Collider2D> ac)
    {
        this.triggerEnterAC = ac;
    }
    public void SetTriggerExitAC(Action<Collider2D> ac)
    {
        this.triggerExitAC = ac;
    }
    public void SetTriggerStayedAC(Action<Collider2D> ac)
    {
        this.triggerStayedAC = ac;
    }
    private void Reset()
    {
        this.lastFrameStayedCols.Clear();
        this.thisFrameStayedCols.Clear();
    }
}
