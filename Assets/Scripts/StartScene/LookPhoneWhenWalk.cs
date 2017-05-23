using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPhoneWhenWalk : MonoBehaviour
{
    public float rate;
    private float time;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime * rate;
        transform.Translate(Vector3.up * Mathf.Cos(time) * Time.deltaTime);
	}
}
