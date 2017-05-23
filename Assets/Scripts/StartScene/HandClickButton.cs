using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class HandClickButton : MonoBehaviour {
    public Transform hand;
    private Vector3 orginPos;
    public GameObject listenButton;
    public Transform disListenButton;
    public GameObject callingText;
    // Use this for initialization
    void Start () {
        orginPos = hand.transform.localPosition;
        if (hand)
        {
            hand.DOLocalMove(new Vector3(3.8f, -7.03f, 0f), 1f);
            StartCoroutine("WaitTime");          
        }       
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(1f);
        hand.DOLocalMove(orginPos, 1f);
        listenButton.transform.DOPunchScale(new Vector3(3.2f, 3.2f, 1.2f),0.2f);
        yield return new WaitForSeconds(0.2f);
        listenButton.SetActive(false);
        disListenButton.DOLocalMove(new Vector3(0.39f, -3.08f, 0f),0.5f);
        callingText.SetActive(true);
    }
}
