using System;
using System.Collections.Generic;
using UnityEngine;
using CaomaoFramework;
using UnityEngine.UI;
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
    }

    protected override void InitWidget()
    {
        this.exitButton = this.mRoot.FindChild("ExitButton").GetComponent<Button>();
        this.startButton = this.mRoot.FindChild("StartButton").GetComponent<Button>();
        this.settingButton = this.mRoot.FindChild("SettingButton").GetComponent<Button>();
        this.loadButton = this.mRoot.FindChild("LoadButton").GetComponent<Button>();

        this.exitButton.onClick.AddListener(this.ExitGame);

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
    private void ExitGame()
    {
        //首先这边应该有保存档案
        Application.Quit();
    }
}