using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HitFrame : MonoBehaviour
{
    private List<Collider2D> enteredColliders = new List<Collider2D>();
    private bool lastColliderEnable;
    private Collider2DTriggerChecker colliderChecker;
    private int behaviourID;
    void Awake ()
    {
        this.colliderChecker = new Collider2DTriggerChecker(base.GetComponent<Collider2D>());
        this.colliderChecker.SetTriggerEnterAC(new Action<Collider2D>(this.OnTriggerEnter2DHandler));
        this.lastColliderEnable = base.GetComponent<Collider2D>().enabled;
        this.behaviourID = this.GetInstanceID();
    }
	
	void Update ()
    {
		
	}
    private void OnTriggerEnter2DHandler(Collider2D other)
    {

    }
}
