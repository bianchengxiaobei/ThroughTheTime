using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour {
    private float time;
    private Vector3 originPos;
    public float lastTime;
	// Use this for initialization
	void Start () {
        originPos = transform.position;
        Destroy(this, lastTime);
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        transform.Translate(Vector3.up);
        if (time > Random.Range(0.2f,0.4f))
        {
            transform.position = originPos;
            time = 0;
        }
	}
}
