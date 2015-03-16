using UnityEngine;
using System.Collections;

public class StunTrap : Ability {
	public string name = "StunTrap";
	public string discription = "Traps them";
	
	private bool activated = false;
	private Object StunTrapObject;
	private GameObject Entity;
	private Camera EntityCamera;
	public int numberStuns = 0;
	
	public StunTrap(GameObject entity){
		Entity = entity;
		EntityCamera = Entity.GetComponentInChildren<Camera>();
	}
	
	public void OnActivate(){
		
	}
	
	public void Activate(){
		if(activated){
			OnActivate();
			activated = !activated;
		}
		if(Input.GetMouseButtonUp(0) && numberStuns < 5){
			Ray CameraRay = EntityCamera.ViewportPointToRay(new Vector3(0.5f,0.5f,0.0f));
			RaycastHit hit;
			if(Physics.Raycast(CameraRay, out hit, 10.0f)){
				Debug.Log ("WTF m8");
				if(hit.collider.tag == "Level"){
					numberStuns++;
					StunTrapObject = Object.Instantiate(Resources.Load("Prefabs/StunTrap"), new Vector3(hit.point.x, hit.point.y + 0.75f, hit.point.z), Quaternion.identity);
					Debug.Log("Boom, Trap Placed");
				}
			}
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