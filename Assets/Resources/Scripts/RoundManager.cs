using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class RoundManager : MonoBehaviour {
	public GlobalGameStatusObject status;
	public Transform scoreboard;
	public Transform playerListing;
	private int roundStart;
	private bool inProgress;
	private MainServerCode serverControl;
	private int robberIndex;
	private List<PlayerState> players;
	private Transform scoresList;
	private Transform roundDisplay;
	
	public int timeLimit;
	public GameHUD display;

	// Use this for initialization
	void Start () {
		serverControl = GameObject.Find("GlobalServerObject").GetComponent("MainServerCode") as MainServerCode;
		scoresList = scoreboard.Find("Scores List");
		roundDisplay = scoreboard.Find("Round Text");
		inProgress = false;
		roundStart = 0;
		robberIndex = 0;
		players = new List<PlayerState>();
	}
	
	// Update is called once per frame
	void Update () {
		if(inProgress){
			int currentTime = (int)Time.time;
			int timeElapsed = currentTime - roundStart;
			int timerVal = timeLimit - timeElapsed;
			if(timerVal < 0){
				timerVal = 0;
			}
			int minutes = timerVal / 60;
			int seconds = timerVal % 60;
			
			display.setTimer(String.Format("{0:00}:{1:00}", minutes, seconds));
		}
	}

	[RPC]
	public void StartRound()
	{
		inProgress = true;
		roundStart = (int)Time.time;
		foreach(PlayerState p in players){
			Debug.Log(p.Guid);
		}
	}

	[RPC]
	public void EndRound()
	{
		inProgress = false;
	}

	public void StartGame()
	{
		robberIndex = UnityEngine.Random.Range(0, players.Count);
	}

	public void InitializePlayerList(NetworkPlayer[] p)
	{
		for(int i = 0; i < p.Length; i++){
            networkView.RPC("InitializePlayer", RPCMode.All, p[i]);
        }
        networkView.RPC("InitializePlayer", RPCMode.All, Network.player);
	}

	public string GetCurrentRobberGuid()
	{
		return players[robberIndex].Guid;
	}

	[RPC]
	public void InitializePlayer(NetworkPlayer p)
	{
		PlayerState newPlayer = new PlayerState();
		newPlayer.Guid = p.guid;
		players.Add(newPlayer);
		Transform newListing = Instantiate(playerListing, Vector3.zero, Quaternion.identity) as Transform;
		newListing.SetParent(scoresList, false);
		Text playerName = newListing.Find("Player Name").gameObject.GetComponent("Text") as Text;
		Text playerScore = newListing.Find("Player Score").gameObject.GetComponent("Text") as Text;
		playerName.text = newPlayer.Guid;
		playerScore.text = newPlayer.Points.ToString();
	}
}
