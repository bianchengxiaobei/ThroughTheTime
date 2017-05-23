using UnityEngine;
using DG.Tweening;
public class CallPhonePointAnim : MonoBehaviour
{
    public float speed;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.RotateAround(transform.parent.position, Vector3.forward, Time.deltaTime * speed);
	}
}
