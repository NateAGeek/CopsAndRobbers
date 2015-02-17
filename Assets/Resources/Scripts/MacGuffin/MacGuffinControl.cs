using UnityEngine;
using System.Collections;

public class MacGuffinControl : MonoBehaviour {
	public int timeLimit = 120; //in seconds
	private int startTime = 0;
	private bool RobberControl = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {

	}

	void OnGUI() {
		if(RobberControl){
			int currentTime = (int)Time.time - startTime;
			int currentCountdown = timeLimit - currentTime;
			int minutes;
			int seconds;
			if(currentCountdown <= 0){
				minutes = 0;
				seconds = 0;
			} else {
				minutes = currentCountdown / 60;
				seconds = currentCountdown % 60;
			}
			
			string text = minutes + " : " + seconds;
			GUI.Label(new Rect(10, 10, 100, 20), text);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Robber") {
			networkView.RPC("startRobberTimer", RPCMode.Others);
		}
	}

	[RPC]
	void startRobberTimer()
	{
		startTime = (int)Time.time;
		RobberControl = true;
	}
}
