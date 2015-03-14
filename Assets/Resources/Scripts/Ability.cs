using UnityEngine;

interface Ability {

	void OnActivate();
	void Activate();

	void OnOver();
	void OnRemove();

	//Collision
	void OnCollisionEnter(Collision hit);
	void OnCollisionExit(Collision hit);

	//Triggers
	void OnTriggerEnter(Collider hit);
	void OnTriggerExit(Collider hit);
}

/*
 * 
 * 
 * An example of the Ability interface
 * 
 * 
using UnityEngine;
using System.Collections;

public class Example : Ability {
	public string name = "Example";
	public string discription = "Example Type";
	
	private bool activated = false;
	private GameObject Entity;
	
	public Example(GameObject entity){
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
*/