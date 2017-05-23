using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CaomaoFramework;

public class MenuState : ClientStateBase
{
    public override void OnEnter()
    {
        EventDispatch.Broadcast("DlgMenu");
        SceneManager.LoadScene("Menu");
    }
    public override void OnLeave()
    {
        
    }
}
