using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DummyControl : MonoBehaviour
{
    public GameObject cam;

	private string[] ab = {"SlowBeam", "GrapHook", "StunTrap", "IRGlasses"};

    public float sensitivityX = 15.0f;
    public float sensitivityY = 15.0f;

    public float minimumX = -360.0f;
    public float maximumX = 360.0f;

    public float minimumY = -60.0f;
    public float maximumY = 60.0f;


    public float walkSpeed = 3.0f;
    public float runSpeed = 7.0f;
    public float gravity = 10.0f;
    public float jumpVelocity = 6.0f;
    public float maxWalkSpeed = 3.0f;
    public float maxRunSpeed = 7.0f;

	private bool isRunning = false;
    private bool grounded = false;
    private float rotationY = 0F;
    public int points = 0;
    private GUIStyle pointsStyle;

	private Dictionary<string, Ability> Abilities = new Dictionary<string, Ability>();

	//Abilities are: SlowBeam, GrapHook, Parkour, StunTrap, IRGlasses
	private string currentAbility = "IRGlasses";


	//Slowbeam Vars
	private float chargeTimer = 0.0f;
	private bool charging = false;
	public float chargeTime = 5.0f;

	//Grap Hook Vars
	private bool isHooked = false;

	//Parkour Vars
	private bool isParkout = false;
	private float ParkcourTime = 5.0f;
	private float ParkcourTimer = 0.0f;
	private float speed = 0.0f;

	//StunTrap Vars
	private bool isStunned = false;
	private float StunnedTime = 5.0f;
	private float StunnedTimer = 0.0f;

	//IRGlasses Vars

    void Start() {
		Abilities["SlowBeam"] = new SlowBeam(gameObject);
		Abilities["StunTrap"] = new StunTrap(gameObject);
		Abilities["IRGlasses"] = new IRGlasses(gameObject);

		//currentAbility = ab[Random.Range(0, 3)];
		Debug.Log ("Abbility:"+currentAbility);
        Screen.showCursor = false;
        cam.camera.active = true;
        pointsStyle = new GUIStyle();
        pointsStyle.fontSize = 40;
    }

    void Update()
    {

		float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
		rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
		rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
		transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		
		if (Input.GetKeyDown("w"))
		{
		    rigidbody.velocity = Vector3.forward;
		}
		if (Input.GetKeyDown("s"))
		{
		    rigidbody.velocity = -Vector3.forward;
		}
		if (Input.GetKeyDown("a"))
		{
		    rigidbody.velocity = Vector3.right;
		}
		if (Input.GetKeyDown("d"))
		{
		    rigidbody.velocity = -Vector3.right;
		}
		if (Input.GetKeyDown (KeyCode.LeftControl)) {
			transform.localScale -= new Vector3(0.0f, 0.25f, 0.0f);
		}
		if (Input.GetKeyUp ((KeyCode.LeftControl))) {
			transform.localScale += new Vector3(0.0f, 0.25f, 0.0f);
		}

		if (isStunned) {
			StunnedTimer += Time.deltaTime;
		}

		if (isParkout) {
			ParkcourTimer += Time.deltaTime;
		}

		if (Input.GetMouseButtonDown(0) && currentAbility == "GrapHook") {
			Ray CameraRay = cam.camera.ViewportPointToRay(new Vector3(0.5f,0.5f,0.0f));
			RaycastHit hit;
			if(Physics.Raycast(CameraRay, out hit)){
				if(hit.collider.tag == "Edge"){
					rigidbody.useGravity = false;
					Vector3 hookDir = hit.point - CameraRay.origin;
					rigidbody.AddForce( hookDir * 1000.0f);
				}
			}
		}
		if (Input.GetMouseButtonUp (0) && currentAbility == "GrapHook") {
			rigidbody.useGravity = true;		
		}
		Abilities[currentAbility].Activate();
    }

    void Awake()
    {
        rigidbody.freezeRotation = true;
    }

    void FixedUpdate()
    {
            float maxVelocitySpeed;

            if(Input.GetButton("Run") && !isParkout){
                speed = runSpeed;
                maxVelocitySpeed = maxRunSpeed;
				isRunning = true;
			} else if(!isParkout){
				isRunning = false;
                speed = walkSpeed;
                maxVelocitySpeed = maxWalkSpeed;
			}else{
				maxVelocitySpeed = 3.0f;
			}
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *= speed;

            Vector3 velocity = rigidbody.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocitySpeed, maxVelocitySpeed);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocitySpeed, maxVelocitySpeed);
            velocityChange.y = 0;
			if (!isStunned) {
				rigidbody.AddForce (velocityChange, ForceMode.VelocityChange);
			}
			if (grounded && Input.GetKeyDown("space") && !isStunned){
                rigidbody.velocity = Vector3.up * jumpVelocity;
                grounded = false;
            }
    }

    void OnGUI()
    {
         GUI.Label(new Rect(50, 10, 100, 20), points.ToString());
    }

    void OnCollisionEnter(Collision collision)
    {

		if (currentAbility == "Parkour" && collision.collider.tag == "Parkour_Surface" && ParkcourTimer <= ParkcourTime) {
			grounded = false;
			isParkout = true;
			speed = 4.0f;
			//rigidbody.useGravity = false;
		}else{
			isParkout = false;
			grounded = false;
		}
        if (collision.gameObject.tag == "Level")
        {
			ParkcourTimer = 0.0f;
			speed = 7.0f;
            grounded = true;
        }
        if (collision.gameObject.tag == "Robber") {
            Debug.Log("Robber Lost!");
            GUI.Label(new Rect(400, 400, 100, 100), "GAME OVER!");
        }

    }

	void OnCollisionExit(Collision collision) {
		if (collision.collider.tag == "Parkour_Surface") {
			isParkout = false;
			grounded = false;
			rigidbody.useGravity = true;	
		}
		if (collision.collider.tag == "StunTrap") {
			Debug.Log ("Should Un Stun?");
			StunnedTimer = 0.0f;		
		}
	}

	void OnTriggerEnter(Collider hit){
		if (hit.collider.tag == "StunTrap" && StunnedTimer <= StunnedTime) {
			isStunned = true;
		}
		if (hit.tag == "Edge") {
			rigidbody.velocity = Vector3.zero;
		}
	}
	void OnTriggerExit(Collider hit){
		if (hit.collider.tag == "StunTrap") {
			StunnedTimer = 0.0f;
			isStunned = false;
			Destroy(hit.gameObject);
		}
	}

}