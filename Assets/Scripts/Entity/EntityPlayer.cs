using System.Collections;
using System;
using UnityEngine;
using CaomaoFramework;
public class EntityPlayer : EntityParent
{
    public override void OnEnterWorld(Action call = null)
    {
        base.OnEnterWorld();
        itemInfo = new Hashtable();
    }
}
