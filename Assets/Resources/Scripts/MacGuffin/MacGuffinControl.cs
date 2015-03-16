using UnityEngine;
using System.Collections;

public class MacGuffinControl : MonoBehaviour {
	public int timeLimit = 120; //in seconds
	private int startTime = 0;
	private bool RobberControl = false;
	private GUIStyle timerStyle;
	private RoundManager roundManager;

	// Use this for initialization
	void Start () {
		roundManager = GameObject.Find("Round Manager").GetComponent("RoundManager") as RoundManager;
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
			Controller robberControl = collision.gameObject.GetComponent("Controller") as Controller;
			networkView.RPC("RobberPoints", RPCMode.All);
			Network.Destroy(gameObject);
		}
	}

	[RPC]
	public void RobberPoints()
	{
		roundManager.AwardRobber(100);
	}
}
