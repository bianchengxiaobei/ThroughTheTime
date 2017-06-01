using System;
using System.Collections.Generic;
using UnityEngine;
using CaomaoFramework;
using UnityEngine.UI;
using CinemaDirector;
[Serializable]
public class DlgMenu : UIBase
{
    private Button startButton;
    private Button exitButton;
    private Button loadButton;
    private Button settingButton;
    public DlgMenu()
    {
        this.mResName = "Guis/DlgMenu";
        this.mResident = false;
    }
    public override void Init()
    {
        EventDispatch.AddListener("DlgMenu", this.Show);
        EventDispatch.AddListener("DlgMenuHide", this.Hide);
    }

    public override void OnDisable()
    {
        
    }

    public override void OnEnable()
    {
        
    }

    public override void Realse()
    {
        EventDispatch.RemoveListener("DlgMenu", this.Show);
        EventDispatch.RemoveListener("DlgMenuHide", this.Hide);
    }

    protected override void InitWidget()
    {
        this.exitButton = this.mRoot.FindChild("ExitButton").GetComponent<Button>();
        this.startButton = this.mRoot.FindChild("StartButton").GetComponent<Button>();
        this.settingButton = this.mRoot.FindChild("SettingButton").GetComponent<Button>();
        this.loadButton = this.mRoot.FindChild("LoadButton").GetComponent<Button>();

        this.exitButton.onClick.AddListener(this.ExitGame);
        this.startButton.onClick.AddListener(this.StartGame);
    }
    protected override void OnAddListener()
    {
        
    }

    protected override void OnRemoveListener()
    {
        
    }

    protected override void RealseWidget()
    {
        
    }
    private void StartGame()
    {
        EventDispatch.Broadcast("DlgMenuHide");
        GameController.Init();
        GameController.Start();           
    }
    private void ExitGame()
    {
        //首先这边应该有保存档案
        Application.Quit();
    }
}