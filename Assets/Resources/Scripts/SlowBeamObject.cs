using UnityEngine;
using System.Collections;

public class SlowBeamObject : MonoBehaviour {

	public float Speed = 0.5f;
	public Vector3 initVelocity;


	// Use this for initialization
	void Start () {
		rigidbody.velocity = transform.forward * Speed;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision){

	}
}
