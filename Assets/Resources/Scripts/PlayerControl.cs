using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    public GameObject cam;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;


    public float speed = 10.0f;
    public float gravity = 10.0f;
    public float maxVelocityChange = 10.0f;
    public bool canJump = true;
    public float jumpHeight = 2.0f;
    private bool grounded = false;
    float rotationY = 0F;

    void Start() {
        if (networkView.isMine)
        {
            cam.camera.active = true;
        }
        else {
            cam.camera.active = false;
        }
    }

    void Update()
    {
        if (networkView.isMine)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

            if (Input.GetKeyDown("w"))
            {
                rigidbody.velocity = Vector3.forward;
            }
        }
    }

    void Awake()
    {
        rigidbody.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (networkView.isMine)
        {
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *= speed;

            Vector3 velocity = rigidbody.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
            grounded = false;
        }
    }

    void OnCollisionStay()
    {
        grounded = true;
    }
}