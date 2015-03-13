using UnityEngine;
using System;

public class RoundManager : MonoBehaviour {
	private int roundStart;
	private bool inProgress;
	private MainServerCode serverControl;
	public int timeLimit;
	public GameHUD display;

	// Use this for initialization
	void Start () {
		serverControl = GameObject.Find("GlobalServerObject").GetComponent("MainServerCode") as MainServerCode;
		inProgress = false;
		roundStart = 0;
	}
	
	// Update is called once per frame
	void Update () {
		int currentTime = (int)Time.time;
		int timeElapsed = currentTime - roundStart;
		int timerVal = timeLimit - timeElapsed;
		if(timerVal < 0){
			timerVal = 0;
		}
		int minutes = timerVal / 60;
		int seconds = timerVal % 60;
		if(inProgress){
			display.setTimer(String.Format("{0:00}:{1:00}", minutes, seconds));
		}
	}

	public void StartRound()
	{
		inProgress = true;
		roundStart = (int)Time.time;
	}

	public void EndRound()
	{
		inProgress = false;
	}
}
