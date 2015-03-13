using UnityEngine;
using System.Collections;

public class Parkour : PassiveAbility {
	public string name = "Parkour";
	public string discription = "Allows the player to use the Parkour ";
	
	private bool activated = false;
	private GameObject Entity;
	
	public Parkour(GameObject entity){
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