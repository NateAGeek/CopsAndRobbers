using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour {
	
	//Public Prefrancese
	public Vector2 sensitivity  = new Vector2(10.0f, 10.0f);
	public float   speed        = 1.0f;
	public float   jumpVelocity = 5.0f;
	public string selectedAbility = "GrapHook";
	
	//Component Vars
	//	private Rigidbody rigidbody   = GetComponent<Rigidbody>();
	private Camera camera;
	
	// Movement Vars
	private Vector2   rotationMin = new Vector2(-360.0f, -60.0f);
	private Vector2   rotationMax = new Vector2(360.0f, 60.0f);
	private Vector3   rotation    = new Vector3(0.0f, 0.0f, 0.0f);
	private bool      onGround    = false;
	
	//Abilities are: SlowBeam, GrapHook, Parkour, StunTrap, IRGlasses
	public Dictionary<string, Ability> Abilities = new Dictionary<string, Ability>();
	public Dictionary<string, PassiveAbility> PassiveAbilities = new Dictionary<string, PassiveAbility>();
	
	void Start() {
		if (networkView.isMine) {
			Abilities ["SlowBeam"] = new SlowBeam (gameObject);
			Abilities ["StunTrap"] = new StunTrap (gameObject);
			Abilities ["IRGlasses"] = new IRGlasses (gameObject);
			Abilities ["GrapHook"] = new GrapGun (gameObject);
		
			//PassiveAbilities["ParkourPassive"] = new ParkourPassive(gameObject);
			PassiveAbilities ["StunTrapPassive"] = new StunTrapPassive (gameObject);
			PassiveAbilities ["SlowBeamPassive"] = new SlowBeamPassive (gameObject);
		
			Debug.Log ("Abbility: " + selectedAbility);
		}
	}
	
	void Update() {
		if (networkView.isMine) {
			//Do the Calculations for rotation
			rotation.x += Input.GetAxis ("Mouse X") * sensitivity.x;
			rotation.y += Input.GetAxis ("Mouse Y") * sensitivity.y;
			rotation.x = rotation.x;
			rotation.y = Mathf.Clamp (rotation.y, rotationMin.y, rotationMax.y);
			
			transform.localEulerAngles = new Vector3 (0.0f, rotation.x, 0.0f);
			camera.transform.localEulerAngles = new Vector3 (-rotation.y, 0.0f, 0.0f);

			Debug.Log ("On Ground?"+onGround);

			//Movement Controls
			if (onGround) {
					Vector3 targetVelocity = new Vector3 (Input.GetAxis ("Horizontal"), 0.0f, Input.GetAxis ("Vertical"));
					targetVelocity = transform.TransformDirection (targetVelocity);
					targetVelocity = new Vector3 (targetVelocity.x * speed, rigidbody.velocity.y, targetVelocity.z * speed);
					Vector3 velocityChange = targetVelocity - rigidbody.velocity;
			
					rigidbody.AddForce (velocityChange, ForceMode.VelocityChange);
			}
			
			if (Input.GetKeyDown (KeyCode.LeftControl)) {
					transform.localScale -= new Vector3 (0.0f, 0.25f, 0.0f);
			}
			if (onGround && Input.GetKeyDown ("space")) {
					rigidbody.AddForce (Vector3.up * jumpVelocity, ForceMode.VelocityChange);
			}
			if (Input.GetKeyUp (KeyCode.LeftControl)) {
					transform.localScale += new Vector3 (0.0f, 0.25f, 0.0f);
			}
			
			//(Passive)Abilities
			foreach (PassiveAbility p in PassiveAbilities.Values) {
					p.Activate ();
			}
			Abilities [selectedAbility].Activate ();
		}
	}
	
	void FixedUpdate() {
		
	}
	
	void OnCollisionEnter(Collision hit) {
		
		if (networkView.isMine) {
			foreach (PassiveAbility p in PassiveAbilities.Values) {
					p.OnCollisionEnter (hit);
			}
			Abilities [selectedAbility].OnCollisionExit (hit);
		}
	}
	
	void OnCollisionStay(Collision hit){
		if (networkView.isMine) {
			if (hit.gameObject.tag == "Level") {
				onGround = true;		
			}
		}
	}
	
	void OnCollisionExit(Collision hit) {
		if (networkView.isMine) {
			if (hit.gameObject.tag == "Level") {
					onGround = false;		
			}
		
			//(Passive)Abilities
			foreach (PassiveAbility p in PassiveAbilities.Values) {
					p.OnCollisionExit (hit);
			}
			Abilities [selectedAbility].OnCollisionExit (hit);
		}
	}
	
	void OnTriggerEnter(Collider hit){
		if (networkView.isMine) {
			//(Passive)Abilities
			foreach (PassiveAbility p in PassiveAbilities.Values) {
					p.OnTriggerEnter (hit);
			}
			Abilities [selectedAbility].OnTriggerEnter (hit);
		}
	}
	void OnTriggerExit(Collider hit){
			if (networkView.isMine) {
			//(Passive)Abilities
			foreach (PassiveAbility p in PassiveAbilities.Values) {
					p.OnTriggerExit (hit);
			}
			Abilities [selectedAbility].OnTriggerExit (hit);
		}
	}
	void OnDisconnectedFromServer(NetworkDisconnection info) {
		Network.Destroy(gameObject);
	}
	
	void OnNetworkInstantiate(NetworkMessageInfo info) {
		camera = GetComponentInChildren<Camera> ();
		if(networkView.isMine){
			camera.active = true;
		} else {
			camera.active = false;
		}
	}
	
	void OnNetworkInstantiate(NetworkMessageInfo info)
	{
		camera = GetComponentInChildren<Camera> ();
		if(networkView.isMine){
			camera.active = true;
		} else {
			camera.active = false;
		}
	}
}