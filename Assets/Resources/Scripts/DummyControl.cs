using UnityEngine;
using System.Collections;

public class DummyControl : MonoBehaviour
{
    public GameObject cam;

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

	//Abilities are: SlowBeam, Grap Hook, Parkour
	private string currentAbility = "Parkour";


	//Slowbeam Vars
	private float chargeTimer = 0.0f;
	private bool charging = false;
	public float chargeTime = 5.0f;

	//Grap Hook Vars
	private bool isHooked = false;


    void Start() {
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

		if (Input.GetMouseButtonDown(0) && currentAbility == "GrapHook") {
			Ray CameraRay = cam.camera.ViewportPointToRay(new Vector3(0.5f,0.5f,0.0f));
			RaycastHit hit;
			if(Physics.Raycast(CameraRay, out hit)){
				if(hit.collider.tag == "Edge"){
					rigidbody.useGravity = false;
					Vector3 hookDir = hit.point - CameraRay.origin;
					rigidbody.AddForce( hookDir * hookDir.magnitude);
				}
			}
		}
		if (Input.GetMouseButtonUp (0) && currentAbility == "GrapHook") {
			rigidbody.useGravity = true;		
		}
		if(Input.GetMouseButtonDown(0) && currentAbility == "SlowBeam"){
			charging = true;
		}
		if (charging) {
			chargeTimer += Time.deltaTime;
		}
		if (Input.GetMouseButtonUp(0) && currentAbility == "SlowBeam" && chargeTimer >= chargeTime) {
			charging = false;
			chargeTimer = 0.0f;
			Ray initPos = cam.camera.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0.0f));
			Rigidbody SlowBeam;
			SlowBeam = Instantiate(Resources.Load ("Prefabs/Slow Beam"), initPos.origin, transform.rotation) as Rigidbody;
		} else if(Input.GetMouseButtonUp(0)) {
			charging = false;
			chargeTimer = 0.0f;
		}
    }

    void Awake()
    {
        rigidbody.freezeRotation = true;
    }

    void FixedUpdate()
    {
            float speed;
            float maxVelocitySpeed;

            if(Input.GetButton("Run")){
                speed = runSpeed;
                maxVelocitySpeed = maxRunSpeed;
				isRunning = true;
            } else {
				isRunning = false;
                speed = walkSpeed;
                maxVelocitySpeed = maxWalkSpeed;
            }
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *= speed;

            Vector3 velocity = rigidbody.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocitySpeed, maxVelocitySpeed);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocitySpeed, maxVelocitySpeed);
            velocityChange.y = 0;
            rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
            if (grounded && Input.GetKeyDown("space")){
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
		if (isRunning && currentAbility == "Parkour" && collision.collider.tag == "Parkour_Surface") {
			
		}
        if (collision.gameObject.tag == "Level")
        {
            grounded = true;
        }
        if (collision.gameObject.tag == "Robber") {
            Debug.Log("Robber Lost!");
            GUI.Label(new Rect(400, 400, 100, 100), "GAME OVER!");
        }

    }
	void OnTriggerEnter(Collider hit){
		if (hit.tag == "Edge") {
			rigidbody.velocity = Vector3.zero;
		}
	}
}