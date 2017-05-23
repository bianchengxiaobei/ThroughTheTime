using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using CaomaoFramework;
using CinemaDirector;
public class StartState : ClientStateBase
{
    private Cutscene startCutScene;
    public override void OnEnter()
    {
        //GameObject startObj = GameObject.Instantiate(ResourceManager.singleton.Load("CutScenes/StartStory")) as GameObject;
        startCutScene = GameObject.Find("StartStory").GetComponent<Cutscene>();
        //startCutScene = startObj.GetComponent<Cutscene>();
        startCutScene.CutsceneFinished += ChangeStateToMenu;
        startCutScene.Optimize();
        UnityMonoDriver.s_instance.StartCoroutine(PlayCutScene());
    }
    public override void OnLeave()
    {
        startCutScene.Stop();
        startCutScene = null;      
    }
    IEnumerator PlayCutScene()
    {
        this.startCutScene.Play();
        yield return null;
    }
    private void ChangeStateToMenu(object sender, CutsceneEventArgs args)
    {
        ClientGameStateManager.singleton.ChangeGameState("MenuState");
    }
}
