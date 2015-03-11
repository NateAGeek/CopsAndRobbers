using UnityEngine;
using System.Collections;

public class MacGuffinControl : MonoBehaviour {
	public int timeLimit = 120; //in seconds
	private int startTime = 0;
	private bool RobberControl = false;
	private GUIStyle timerStyle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {

	}

	void OnGUI() {
		
		
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Robber") {
			PlayerControl robberControl = collision.gameObject.GetComponent("PlayerControl") as PlayerControl;
			robberControl.points += 100;
			Network.Destroy(gameObject);
		}
	}

}
