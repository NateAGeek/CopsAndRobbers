using UnityEngine;
using System.Collections;

public class StunTrap : Ability {
	public string name = "StunTrap";
	public string discription = "Traps them";
	
	private bool activated = false;
	private Object StunTrapObject;
	private GameObject Entity;
	
	public StunTrap(GameObject entity){
		Entity = entity;
	}
	
	public void OnActivate(){
		
	}
	
	public void Activate(){
		if(activated){
			OnActivate();
			activated = !activated;
		}
		if(Input.GetMouseButtonUp(0)){
			Ray CameraRay = Entity.GetComponentInChildren<Camera>().ViewportPointToRay(new Vector3(0.5f,0.5f,0.0f));
			RaycastHit hit;
			if(Physics.Raycast(CameraRay, out hit, 5.0f)){
				if(hit.collider.tag == "Level"){
					StunTrapObject = Object.Instantiate(Resources.Load("Prefabs/StunTrap"), hit.point, hit.transform.rotation);
					Debug.Log("Boom, Trap Placed");
				}
			}
		}
	}
	
	public void OnOver(){
		
	}
	
	public void OnRemove(){
		
	}
}