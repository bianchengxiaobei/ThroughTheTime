using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaomaoFramework;
public class ActorPlayer<T> : ActorParent<T> where T : EntityPlayer
{
	void Start ()
    {
		
	}
	void Update ()
    {
        ActChange();
	}
}
