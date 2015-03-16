using UnityEngine;
using System.Collections;

public class GrapGun : Ability {
	public string name = "Example";
	public string discription = "Example Type";
	
	private bool activated = false;
	private GameObject Entity;
	private Camera EntityCamera;
	private GameObject GrapHook;
	
	public GrapGun(GameObject entity){
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

		if(Input.GetMouseButtonUp(0)){
			Ray CameraRay = EntityCamera.ViewportPointToRay(new Vector3(0.5f,0.5f,0.0f));
			RaycastHit hit;
			if(Physics.Raycast(CameraRay, out hit, 15.0f)){
				if(hit.collider.tag == "Edge"){
					GrapHook = Object.Instantiate(Resources.Load("Prefabs/GrapHook"), hit.point, hit.transform.rotation) as GameObject;
					GrapHook.GetComponent<GrapHookObject>().StartPoint = Entity.transform.position;
					GrapHook.GetComponent<GrapHookObject>().EndPoint = hit.point;
					Entity.rigidbody.useGravity = false;
					Entity.rigidbody.AddForce(Entity.transform.forward * 100.0f, ForceMode.VelocityChange);
					Debug.Log("Boom, Hook Placed");
				}
			}
		}

	}
	
	public void OnCollisionEnter(Collision entityHit){

	}
	
	public void OnCollisionExit(Collision entityHit){
		
	}
	
	public void OnTriggerEnter(Collider entityHit){
		if (entityHit.tag == "Edge") {
			Entity.rigidbody.useGravity = true;
			Entity.rigidbody.velocity = Vector3.zero;
			Object.Destroy(GrapHook);
		}
	}
	public void OnTriggerExit(Collider entityHit){
		
	}
	
	public void OnOver(){
		
	}
	
	public void OnRemove(){
		
	}
}