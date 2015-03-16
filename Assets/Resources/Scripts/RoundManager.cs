using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class RoundManager : MonoBehaviour {
	public GlobalGameStatusObject status;
	public Transform scoreboard;
	public Transform betweenRoundMenu;
	public Transform playerListing;
	public Transform readyListing;
	public Text playerNameField;
	public Text serverNameField;
	private int roundStart;
	private int roundNumber;
	private bool inProgress;
	private MainServerCode serverControl;
	private int robberIndex;
	private int firstRobberIndex;
	private string firstRobberGuid;
	private List<PlayerState> players;
	private Transform scoresList;
	private Transform roundDisplay;
	private Transform readyList;
	private Toggle readyCheck;
	private Text roundInfo;
	private Text nextRobberName;
	
	public int timeLimit;
	public int roundLimit;
	public GameHUD display;

	// Use this for initialization
	void Start () {
		serverControl = GameObject.Find("GlobalServerObject").GetComponent("MainServerCode") as MainServerCode;
		scoresList = scoreboard.Find("Scores List");
		roundDisplay = scoreboard.Find("Round Text");
		readyList = betweenRoundMenu.Find("Ready List");
		readyCheck = betweenRoundMenu.Find("Ready Check").gameObject.GetComponent("Toggle") as Toggle;
		roundInfo = betweenRoundMenu.Find("Round Info").gameObject.GetComponent("Text") as Text;
		nextRobberName = betweenRoundMenu.Find("Next Robber Info").Find("Next Robber Name").gameObject.GetComponent("Text") as Text;
		inProgress = false;
		roundStart = 0;
		roundNumber = 1;
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

			if(Network.isServer){
				if(timerVal <= 0){
					robberIndex = (robberIndex + 1) % players.Count;
					networkView.RPC("EndRound", RPCMode.All, players[robberIndex].Guid, players[robberIndex].Name);
				}
			}
		}
	}

	[RPC]
	public void StartRound(string robberGuid)
	{
		Screen.showCursor = false;
		string name = "";
		foreach(PlayerState p in players){
			if(p.Guid == robberGuid){
				p.IsRobber = true;
				name = p.Name;
			} else {
				p.IsRobber = false;
			}
		}
		for(int i = 0; i < scoresList.childCount; i++){
			Transform listing = scoresList.GetChild(i);
			Text playerName = listing.Find("Player Name").gameObject.GetComponent("Text") as Text;
			Text playerScore = listing.Find("Player Score").gameObject.GetComponent("Text") as Text;
			if(playerName.text == name){
				playerName.color = new Color(0.0f, 0.0f, 0.0f);
				playerScore.color = new Color(0.0f, 0.0f, 0.0f);
			} else {
				playerName.color = new Color(0.039f, 0.039f, 0.882f);
				playerScore.color = new Color(0.039f, 0.039f, 0.882f);
			}
		}
		Text roundText = roundDisplay.gameObject.GetComponent("Text") as Text;
		roundText.text = "Round " + roundNumber;
		inProgress = true;
		roundStart = (int)Time.time;
	}

	[RPC]
	public void EndRound(string guid, string name)
	{
		Screen.showCursor = true;
		inProgress = false;
		status.IsReady = false;
		foreach(PlayerState p in players){
			p.IsReady = false;
		}
		for(int i = 0; i < readyList.childCount; i++){
			Transform listing = readyList.GetChild(i);
			Toggle playerReady = listing.Find("Ready Display").gameObject.GetComponent("Toggle") as Toggle;
			playerReady.isOn = false;
		}
		readyCheck.isOn = false;
		
		if(guid == firstRobberGuid){
			roundInfo.text = "End of Round " + roundNumber;
			roundNumber++;
		} else {
			roundInfo.text = "Round " + roundNumber;
		}
		nextRobberName.text = name;

		Network.Destroy(status.Avatar);
		if(roundNumber <= 4){
			GUIManager.SetGUI("BetweenRoundGUI");
		} else {
			EndGame();
		}
	}

	public void StartGame()
	{
		Debug.Log("Start Game");
		InitializePlayerList();
	}

	[RPC]
	public void SetFirstRobber(string guid)
	{
		firstRobberGuid = guid;
	}

	public void EndGame()
	{
		inProgress = false;
		string winner = GetLeader();
		for(int i = 0; i < scoresList.childCount; i++){
			Transform listing = scoresList.GetChild(i);
			Text playerName = listing.Find("Player Name").gameObject.GetComponent("Text") as Text;
			Text playerScore = listing.Find("Player Score").gameObject.GetComponent("Text") as Text;
			if(playerName.text == winner){
				playerName.color = new Color(1.0f, 0.843f, 0.0f);
				playerScore.color = new Color(1.0f, 0.843f, 0.0f);
			} else {
				playerName.color = new Color(0.0f, 0.0f, 0.0f);
				playerScore.color = new Color(0.0f, 0.0f, 0.0f);
			}
		}
		Text roundText = roundDisplay.gameObject.GetComponent("Text") as Text;
		roundText.text = "Round " + roundNumber;
		GUIManager.EndGame();
	}

	public string GetLeader()
	{
		int maxScore = 0;
		string leader = "";
		foreach(PlayerState p in players){
			if (p.Points >= maxScore){
				maxScore = p.Points;
				leader = p.Guid;
			}
		}
		return leader;
	}

	public void InitializePlayerList()
	{
		networkView.RPC("SendPlayerNameRequest", RPCMode.Others);

		//for(int i = 0; i < p.Length; i++){
        //    networkView.RPC("InitializePlayer", RPCMode.All, p[i].guid);
        //}
        //status.Name = serverNameField.text;
        //networkView.RPC("InitializePlayer", RPCMode.All, Network.player.guid, serverNameField.text);
	}

	[RPC]
	public void SendPlayerNameRequest()
	{
		status.Name = playerNameField.text;
		networkView.RPC("SendPlayerName", RPCMode.Server, playerNameField.text);
	}

	[RPC]
	public void SendPlayerName(string name, NetworkMessageInfo info)
	{
		//networkView.RPC("InitializePlayer", RPCMode.All, info.sender.guid, name);
		Debug.Log("SendPlayerName");
		Debug.Log("Player Count: " + players.Count);
		Debug.Log("Network connection count: " + Network.connections.Length);
		InitializePlayer(info.sender.guid, name);
		if(players.Count >= Network.connections.Length){
			foreach(PlayerState p in players){
				networkView.RPC("InitializePlayer", RPCMode.Others, p.Guid, p.Name);
			}
			status.Name = serverNameField.text;
			InitializePlayer(Network.player.guid, serverNameField.text);
        	networkView.RPC("InitializePlayer", RPCMode.Others, Network.player.guid, serverNameField.text);
        	robberIndex = UnityEngine.Random.Range(0, Network.connections.Length + 1);
			firstRobberIndex = robberIndex;
			networkView.RPC("SetFirstRobber", RPCMode.All, players[firstRobberIndex].Guid);
			GUIManager.StartGame();
			serverControl.StartRound();
		}
	}

	public string GetCurrentRobberGuid()
	{
		return players[robberIndex].Guid;
	}

	[RPC]
	public void InitializePlayer(string guid, string name)
	{
		PlayerState newPlayer = new PlayerState();
		newPlayer.Guid = guid;
		newPlayer.Name = name;
		players.Add(newPlayer);
		Transform newListing = Instantiate(playerListing, Vector3.zero, Quaternion.identity) as Transform;
		newListing.SetParent(scoresList, false);
		Text playerName = newListing.Find("Player Name").gameObject.GetComponent("Text") as Text;
		Text playerScore = newListing.Find("Player Score").gameObject.GetComponent("Text") as Text;
		//playerName.text = newPlayer.Name;
		playerName.text = newPlayer.Name;
		playerScore.text = newPlayer.Points.ToString();
		Transform newReadyListing = Instantiate(readyListing, Vector3.zero, Quaternion.identity) as Transform;
		newReadyListing.SetParent(readyList, false);
		Text readyName = newReadyListing.Find("Ready Name").gameObject.GetComponent("Text") as Text;
		readyName.text = newPlayer.Name;
	}

	public void SetReady(bool ready)
	{
		status.IsReady = ready;
		networkView.RPC("SetPlayerReady", RPCMode.All, Network.player.guid, ready);
	}

	[RPC]
	public void SetPlayerReady(string guid, bool ready)
	{
		string name = "";
		foreach(PlayerState p in players){
			if(p.Guid == guid){
				name = p.Name;
				p.IsReady = ready;
			}
		}
		for(int i = 0; i < readyList.childCount; i++){
			Transform listing = readyList.GetChild(i);
			Text readyName = listing.Find("Ready Name").gameObject.GetComponent("Text") as Text;
			
			if(readyName.text == name){
				Toggle playerReady = listing.Find("Ready Display").gameObject.GetComponent("Toggle") as Toggle;
				playerReady.isOn = ready;
			}
		}
		if(Network.isServer){
			bool allReady = true;
			foreach(PlayerState p in players){
				if(!p.IsReady){
					allReady = false;
					break;
				}
			}
			if(allReady){
				serverControl.StartRound();
			}
		}
	}

	public void AwardRobber(int points)
	{
		if(status.IsRobber){
			status.Points += points;
		}

		foreach(PlayerState p in players){
			if(p.IsRobber){
				p.Points += points;
			}
		}
	}
}
