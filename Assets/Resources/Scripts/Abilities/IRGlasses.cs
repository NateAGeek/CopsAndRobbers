using UnityEngine;
using System.Collections;

public class IRGlasses : Ability {
	public string name = "IRGlasses";
	public string discription = "Can See Stuff In The Dark";
	
	private bool activated = false;
	private bool glassesOn = false;
	private GameObject Entity;
	
	public IRGlasses(GameObject entity){
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
			glassesOn = !glassesOn;
			Entity.GetComponentInChildren<PostProcessingIRGlasses>().enabled = glassesOn;
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