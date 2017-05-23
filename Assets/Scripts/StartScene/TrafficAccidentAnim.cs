using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TrafficAccidentAnim : MonoBehaviour {
    public GameObject air;
    public GameObject soul;
    private SpriteRenderer soulRender;
    private SpriteRenderer airRender;
	// Use this for initialization
	void Start () {
        soul.transform.DOLocalMove(new Vector3(2.0f, 1.73f, 0), 2f);
        air.transform.DOLocalMove(new Vector3(-1.98f, -7.55f, 0f),1f);
        soulRender = soul.GetComponent<SpriteRenderer>();
        airRender = air.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        soulRender.color = new Color(1, 1,1, Mathf.Lerp(0.9f, 0f, 30f*Time.deltaTime));
        airRender.color = new Color(1, 1,1, Mathf.Lerp(0.9f, 0f, 50f*Time.deltaTime));
    }
}
