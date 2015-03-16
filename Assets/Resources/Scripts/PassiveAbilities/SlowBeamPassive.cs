using UnityEngine;
using System.Collections;

public class SlowBeamPassive : PassiveAbility {
	public string name = "Slow Beam Passive";
	public string discription = "Can get slowed down m8";
	
	private bool activated = false;
	private GameObject Entity;
	private bool slow = false;
	private float slowTime = 10.0f;
	private float slowTimer = 0.0f;
	
	public SlowBeamPassive(GameObject entity){
		Entity = entity;
	}
	
	public void OnActivate(){
		
	}
	
	public void Activate(){
		if(activated){
			OnActivate();
			activated = !activated;
		}
		//Update Code
		if (slow) {
			slowTimer += Time.deltaTime;
		}
		if (slowTimer >= slowTime) {
			slow = false;
			Entity.GetComponentInChildren<DummyObjectScript>().speed += 8.0f;
			slowTimer = 0.0f;
		}
	}
	
	public void OnCollisionEnter(Collision entityHit){
		
	}
	
	public void OnCollisionExit(Collision entityHit){
		
	}
	
	public void OnTriggerEnter(Collider entityHit){
		if (entityHit.tag == "SlowBeam") {
			Entity.GetComponentInChildren<DummyObjectScript>().speed -= 8.0f;
			slow = true;
		}
	}
	public void OnTriggerExit(Collider entityHit){
		
	}
	
	public void OnOver(){
		
	}
	
	public void OnRemove(){
		
	}
}