using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using CaomaoFramework;
public class HitFrame : MonoBehaviour
{
    private List<Collider2D> enteredColliders = new List<Collider2D>();
    private bool lastColliderEnable;
    private Collider2DTriggerChecker colliderChecker;
    private int behaviourID;
    private ActorParent theOwner;
    public void Init(ActorParent theOwner)
    {
        this.theOwner = theOwner;
    }
    public void FrameUpdate(float delatime)
    {
        this.colliderChecker.Update();
        bool enabled = this.GetComponent<Collider2D>().enabled;
        if (this.lastColliderEnable != enabled)
        {
            if (!enabled)
            {
                this.enteredColliders.Clear();
            }
            else
            {
                //this.OnHitFrameEnable();
            }
        }
        this.lastColliderEnable = enabled;
    }
    void Awake ()
    {
        this.colliderChecker = new Collider2DTriggerChecker(this.GetComponent<Collider2D>());
        this.colliderChecker.SetTriggerEnterAC(new Action<Collider2D>(this.OnTriggerEnter2DHandler));
        this.lastColliderEnable = base.GetComponent<Collider2D>().enabled;
        this.behaviourID = this.GetInstanceID();
    }
    private void OnEnable()
    {
        theOwner.AddUpdateAction(behaviourID, new Action<float>(this.FrameUpdate));
    }
    private void OnDisable()
    {
        theOwner.RemoveUpdateAction(this.behaviourID, new Action<float>(this.FrameUpdate));
        this.enteredColliders.Clear();
        this.lastColliderEnable = false;
        this.colliderChecker.OnDisable();
    }
    private void OnTriggerEnter2DHandler(Collider2D other)
    {
        if (this.enteredColliders.Contains(other))
        {
            return;
        }
        if (theOwner is ActorMyself)
        {
            theOwner.m_animator.SetBool("IsHited", true);
        }
        Debug.Log("打击");
        //取得打击的点
        Vector3 vector = base.GetComponent<Collider2D>().bounds.ClosestPoint(other.bounds.center);
    }
}
