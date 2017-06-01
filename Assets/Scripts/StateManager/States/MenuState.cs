using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CaomaoFramework;

public class MenuState : ClientStateBase
{
    string a = "adfdfd";
    public override void OnEnter()
    {
        EventDispatch.Broadcast("DlgMenu");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
    public override void OnLeave()
    {
        
    }
}
