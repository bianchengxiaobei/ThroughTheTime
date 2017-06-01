using System; 
using System.Collections;
using UnityEngine;
using CaomaoFramework;
using UnityEngine.UI;
using DG.Tweening;
[Serializable]
public class DlgDialogue : UIBase
{
	private Text m_dialogueText;
    private Image m_dialoguoContinueIcon;
    private EntityParent theOwner;
    private string[] Dialogues;
    private float Interval = 0.07f;
    private int m_iCurrentIndex = 0;
    private int m_iCurrentStringIndex = 0;
    private bool m_bIsFinished = false;
    private bool m_continueButton = true;
    private bool m_bIsDisplaying = false;
    private bool m_bSmooth = true;
    private float m_fOffsetX = 2.5f;
    private float m_fOffsetY = 2.5f;
    public DlgDialogue()
    {
        this.mResName = "Guis/DlgDialogue";
        this.mResident = true;
    }
    public override void Init()
    {
        EventDispatch.AddListener<string[],EntityParent>("DlgDialogue", this.ShowDialogue);
    }

    public override void OnDisable()
    {
        
    }

    public override void OnEnable()
    {
        this.m_dialoguoContinueIcon.gameObject.SetActive(false);
        this.m_continueButton = true;
    }

    public override void Realse()
    {
        EventDispatch.RemoveListener<string[],EntityParent>("DlgDialogue", this.ShowDialogue);
    }

    protected override void InitWidget()
    {
		this.m_dialogueText = this.mRoot.FindChild("DialogueBG/DialogueText").GetComponent<Text>();
        this.m_dialoguoContinueIcon = this.mRoot.FindChild("DialogueBG/DialogueContinueIcon").GetComponent<Image>();      
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
    public override void Update(float deltaTime)
    {
        if (this.m_continueButton && !m_bIsDisplaying)
        {
            this.m_continueButton = false;
            if (!m_bIsFinished)
                UnityMonoDriver.s_instance.StartCoroutine(ShowDialogue(this.Dialogues));
            else
            {
                UnityMonoDriver.s_instance.StopCoroutine(ShowDialogue(this.Dialogues));
                this.Hide();
            }
        }
        if (GameInputManager.Instance.GetInteractiveButtonDown())
        {
            this.m_continueButton = true;
        }
        Translate();
    }
    private void Translate()
    {
        if (theOwner != null && theOwner.GameObject != null && theOwner.IsVisiable == true)
        {
            Vector2 entityPos = theOwner.Transform.position;
            entityPos.x += this.m_fOffsetX;entityPos.y += this.m_fOffsetY;
            Vector2 entityScreenPos = Camera.main.WorldToScreenPoint(entityPos);
            Vector2 pos = this.m_bSmooth ? Vector2.Lerp(this.m_dialoguoContinueIcon.transform.parent.position,
                entityScreenPos,0.6f) : entityScreenPos;
            this.m_dialoguoContinueIcon.transform.parent.position = pos;
        }
    }
    private void ShowDialogue(string[] dias,EntityParent parent)
    {
        this.Dialogues = dias;
        this.theOwner = parent;
        this.Show();
    }
    private IEnumerator ShowDialogue(string[] currentString)
    {
        int length = currentString[m_iCurrentStringIndex].Length;
        int allLength = currentString.Length;
        this.m_dialogueText.text = "";
        this.m_iCurrentIndex = 0;
        while (m_iCurrentIndex < length && m_bIsFinished == false)
        {
            this.m_dialogueText.text += currentString[m_iCurrentStringIndex][m_iCurrentIndex];
            this.m_iCurrentIndex++;
            m_bIsDisplaying = true;
            if (this.m_iCurrentIndex < length)
            {
                if (m_continueButton)
                {
                    this.m_dialogueText.text = currentString[m_iCurrentStringIndex];
                    m_iCurrentStringIndex++;
                    this.m_continueButton = false;
                    m_bIsDisplaying = false;
                    if (m_iCurrentStringIndex >= allLength)
                    {
                        m_bIsFinished = true;
                        break;
                    }
                    break;
                }
                else
                {
                    yield return new WaitForSeconds(Interval);
                }
            }
            else
            {
                m_iCurrentStringIndex++;
                this.m_continueButton = false;
                m_bIsDisplaying = false;
                if (m_iCurrentStringIndex >= allLength)
                {
                    m_bIsFinished = true;
                    break;
                }
            }
        }
        this.m_dialoguoContinueIcon.gameObject.SetActive(true);
    }
}