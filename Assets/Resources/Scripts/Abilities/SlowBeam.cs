using UnityEngine;
using System.Collections;

public class SlowBeam : Ability {
	public string name = "SlowBeam";
	public string discription = "Slows them down";

	private float chargeTime = 5.0f;
	private float chargeTimer = 0.0f;

	private bool activated = false;
	private bool charging  = false;
	private Object SlowBeamObject;
	private GameObject Entity;

	public SlowBeam(GameObject entity){
		Entity = entity;
	}

	public void OnActivate(){

	}

	public void Activate(){
		if(activated){
			OnActivate();
			activated = !activated;
		}
		if (Input.GetMouseButtonDown(0)) {
			charging = true;	
		}
		if(charging && chargeTimer <= chargeTime){
			chargeTimer += Time.deltaTime;
			Debug.Log("Charging Up The Slow Beam: "+chargeTimer);
		}
		if(Input.GetMouseButtonUp(0) && chargeTimer >= chargeTime){
			chargeTimer = 0.0f;
			charging = false;
			SlowBeamObject = Object.Instantiate(Resources.Load("Prefabs/SlowBeam"), Entity.GetComponentInChildren<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f)), Entity.GetComponentInChildren<Camera>().transform.rotation);
			Debug.Log ("Boom, shot Slow Beam");
		}
		else if(Input.GetMouseButtonUp(0)){
			chargeTimer = 0.0f;
			charging = false;
		}
	}

	public void OnCollisionEnter(Collision entityHit){

	}

	public void OnCollisionExit(Collision entityHit){

	}

	public void OnTriggerEnter(Collider entityHit){

	}
	public void OnTriggerExit(Collider entityHit){

	}

	public void OnOver(){

	}

	public void OnRemove(){

	}
}
