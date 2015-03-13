using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DummyObjectScript : MonoBehaviour {

	//Public Prefrancese
	public Vector2 sensitivity  = new Vector2(1.0f, 1.0f);
	public float   speed        = 7.0f;
	public float   jumpVelocity = 5.0f;

	//Component Vars
	//	private Rigidbody rigidbody   = GetComponent<Rigidbody>();

	// Movement Vars
	private Vector2   rotationMin = new Vector2(-360.0f, -60.0f);
	private Vector2   rotationMax = new Vector2(360.0f, 60.0f);
	private Vector3   rotation    = new Vector3(0.0f, 0.0f, 0.0f);
	private bool      onGround    = false;

	//Abilities are: SlowBeam, GrapHook, Parkour, StunTrap, IRGlasses
	private Dictionary<string, Ability> Abilities = new Dictionary<string, Ability>();
	private Dictionary<string, PassiveAbility> PassiveAbilities = new Dictionary<string, PassiveAbility>();
	private string selectedAbility = "IRGlasses";

    void Start() {
		Abilities["SlowBeam"] = new SlowBeam(gameObject);
		Abilities["StunTrap"] = new StunTrap(gameObject);
		Abilities["IRGlasses"] = new IRGlasses(gameObject);

		Debug.Log ("Abbility: " + selectedAbility);
    }

    void Update() {
		//Do the Calculations for rotation
		rotation.x += Input.GetAxis ("Mouse X") * sensitivity.x;
		rotation.y += Input.GetAxis ("Mouse Y") * sensitivity.y;
		rotation.x = Mathf.Clamp (rotation.x, rotationMin.x, rotationMax.x);
		rotation.y = Mathf.Clamp (rotation.y, rotationMin.y, rotationMax.y);

		transform.localEulerAngles = new Vector3(-rotation.y, rotation.x, 0.0f);

		//Movement Controls
		Vector3 targetVelocity = new Vector3(Input.GetAxis ("Horizontal"), 0.0f, Input.GetAxis ("Vertical"));
		targetVelocity = transform.TransformDirection(targetVelocity);

		rigidbody.velocity = new Vector3(targetVelocity.x * speed, rigidbody.velocity.y, targetVelocity.z * speed);

		if (onGround && Input.GetKeyDown ("space")){
			onGround = false;
			rigidbody.velocity = Vector3.up * jumpVelocity;	
		}

		Abilities[selectedAbility].Activate();
    }

	void FixedUpdate() {

	}

    void OnCollisionEnter(Collision hit) {
		if (hit.gameObject.tag == "Level") {
			onGround = true;		
		}
		Abilities[selectedAbility].OnCollisionExit(hit);
    }

	void OnCollisionExit(Collision hit) {
		Abilities[selectedAbility].OnCollisionExit(hit);
	}

	void OnTriggerEnter(Collider hit){
		Abilities[selectedAbility].OnTriggerEnter(hit);
	}
	void OnTriggerExit(Collider hit){
		Abilities[selectedAbility].OnTriggerExit(hit);
	}

}