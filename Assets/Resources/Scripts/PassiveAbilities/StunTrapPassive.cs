using UnityEngine;
using System.Collections;

public class StunTrapPassive : PassiveAbility {
	public string name = "StunTrapPassive";
	public string discription = "Can become stuned...";

	private GameObject Entity;
	private GameObject Trap;

	private float stunTime   = 5.0f;
	private float stunTimer  = 0.0f;
	private bool  isStunned  = false;
	private bool  activated  = false;

	public StunTrapPassive(GameObject entity){
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
		if (isStunned) {
			stunTimer += Time.deltaTime;	
		}
		if (stunTimer >= stunTime) {
			isStunned = false;
			MonoBehaviour.Destroy(Trap);
			Entity.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
		}
	}
	
	public void OnCollisionEnter(Collision entityHit){
		
	}
	
	public void OnCollisionExit(Collision entityHit){
		
	}
	
	public void OnTriggerEnter(Collider entityHit){
		if(entityHit.gameObject.tag == "StunTrap"){
			Trap = entityHit.gameObject;
			isStunned = true;
			Entity.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		}
	}
	public void OnTriggerExit(Collider entityHit){
		
	}
	
	public void OnOver(){
		
	}
	
	public void OnRemove(){
		
	}
}