using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class RoundManager : MonoBehaviour {
	public GlobalGameStatusObject status;
	public Transform scoreboard;
	public Transform playerListing;
	private int roundStart;
	private int roundNumber;
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
		roundNumber = 0;
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
	public void StartRound(string robberGuid)
	{
		foreach(PlayerState p in players){
			if(p.Guid == robberGuid){
				p.IsRobber = true;
			} else {
				p.IsRobber = false;
			}
		}
		for(int i = 0; i < scoresList.childCount; i++){
			Transform listing = scoresList.GetChild(i);
			Text playerName = listing.Find("Player Name").gameObject.GetComponent("Text") as Text;
			Text playerScore = listing.Find("Player Score").gameObject.GetComponent("Text") as Text;
			if(playerName.text == robberGuid){
				playerName.color = new Color(0.0f, 0.0f, 0.0f);
				playerScore.color = new Color(0.0f, 0.0f, 0.0f);
			} else {
				playerName.color = new Color(0.039f, 0.039f, 0.882f);
				playerScore.color = new Color(0.039f, 0.039f, 0.882f);
			}
		}
		roundNumber++;
		Text roundText = roundDisplay.gameObject.GetComponent("Text") as Text;
		roundText.text = "Round " + roundNumber;
		inProgress = true;
		roundStart = (int)Time.time;
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
            networkView.RPC("InitializePlayer", RPCMode.All, p[i].guid);
        }
        networkView.RPC("InitializePlayer", RPCMode.All, Network.player.guid);
	}

	public string GetCurrentRobberGuid()
	{
		return players[robberIndex].Guid;
	}

	[RPC]
	public void InitializePlayer(string guid)
	{
		PlayerState newPlayer = new PlayerState();
		newPlayer.Guid = guid;
		players.Add(newPlayer);
		Transform newListing = Instantiate(playerListing, Vector3.zero, Quaternion.identity) as Transform;
		newListing.SetParent(scoresList, false);
		Text playerName = newListing.Find("Player Name").gameObject.GetComponent("Text") as Text;
		Text playerScore = newListing.Find("Player Score").gameObject.GetComponent("Text") as Text;
		playerName.text = newPlayer.Guid;
		playerScore.text = newPlayer.Points.ToString();
	}
}
