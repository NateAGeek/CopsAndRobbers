using UnityEngine;
using System;

public class MainServerCode : MonoBehaviour {
    
    public string ipConnection = "127.0.0.1";
    public int portConnection = 6969;
    public string uniqueGameName = "CopsAndRobbers";
	public Transform spawnCops;
	public Transform spawnRobber;
    public Transform spawnMacGuffin;
    public GlobalGameStatusObject status;
    public RoundManager roundManager;
    public Transform macGuffinSpawns;
    public Transform macGuffinParent;

    private int selectedSpawnType = 0;
    private string[] options = new string[] { "Spawn Robber", "Spawn Cop" };
    private int currentRobber = 0;
    private bool roundOn = false;
    private int timeLimit = 120;
    private int startTime = 0;
    private GUIStyle timerStyle;
    private HostData[] hostList;

    void OnConnectedToServer() {
        /*
        if (selectedSpawnType == 0)
        {
            Debug.Log("Should Spawn Robber");
            Network.Instantiate(Resources.Load("Prefabs/Robber"), spawnRobber.position, Quaternion.identity, 0);
        }
        else {
            Debug.Log("Should Spawn Cop");
            Network.Instantiate(Resources.Load("Prefabs/Player"), spawnCops.position, Quaternion.identity, 0);
        }
        Instantiate(Resources.Load("Prefabs/MacGuffin"), spawnMacGuffin.position, Quaternion.identity);
        */
    }

    public void ConnectToServer(HostData h) {
        Network.Connect(h);
    }

    public void DisconnectFromServer()
    {
        Network.Disconnect();
    }

    public void DisconnectServer()
    {
        Network.Disconnect();
        MasterServer.UnregisterHost();
    }

    public void SetServer(string lobbyName) {
        Network.InitializeServer(4, portConnection, !Network.HavePublicAddress());
        MasterServer.RegisterHost(uniqueGameName, lobbyName);
        //Instantiate(Resources.Load("Prefabs/MacGuffin"), spawnMacGuffin.position, Quaternion.identity);
    }

    public void RefreshHostList()
    {
        MasterServer.RequestHostList(uniqueGameName);
    }

    public HostData[] GetHostList()
    {
        return hostList;
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if(msEvent == MasterServerEvent.HostListReceived){
            hostList = MasterServer.PollHostList();
        }
    }

    void OnGUI() {
        /*
        if (!Network.isClient && !Network.isServer) {
            ipConnection = GUI.TextField(new Rect(10, 10, 100, 25), ipConnection);
            int.TryParse(GUI.TextField(new Rect(10, 35, 100, 25), portConnection.ToString()), out portConnection);
            if (GUI.Button(new Rect(10, 60, 200, 25), "Connect To Server (Client)")) {
                ConnectToServer();
            }
            if (GUI.Button(new Rect(10, 100, 200, 25), "Become the Server (Host)")) {
                SetServer();
            }
            //selectedSpawnType = GUI.SelectionGrid(new Rect(10, 125, 200, 50), selectedSpawnType, options, options.Length, "toggle");
        }
        else {
            if (Network.isServer) {
                GUI.Label(new Rect(4, 24, 500, 16), "Connections: " + Network.connections.Length.ToString());
                if(GUI.Button(new Rect(4, 100, 150, 25), "Start Game")){
                    GameObject[] spawnObjects = GameObject.FindGameObjectsWithTag("MacGuffin");
                    for(int i = 0; i < spawnObjects.Length; i++){
                        Network.Instantiate(Resources.Load("Prefabs/MacGuffin"), spawnObjects[i].transform.position, Quaternion.identity, 0);
                    }
                    networkView.RPC("LoadLevel", RPCMode.Others, Network.connections[currentRobber].guid);
                }
            } else if(Network.isClient && roundOn) {
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
                
                string text = String.Format("{0:00}:{1:00}", minutes, seconds);
                GUI.Label(new Rect(10, 10, 100, 20), text, timerStyle);
            }
        }
*/
    }

	// Use this for initialization
	void Start () {
        //MasterServer.ipAddress = "127.0.0.1";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [RPC]
    public void LoadLevel(string robberGuid)
    {
    	if(Network.isServer){
	    	for(int i = 0; i < macGuffinSpawns.childCount; i++){
	    		Transform spawn = macGuffinSpawns.GetChild(i);
	    		GameObject newMac = Network.Instantiate(Resources.Load("Prefabs/MacGuffin"), spawn.position, Quaternion.identity, 0) as GameObject;
	    		newMac.transform.SetParent(macGuffinParent, true);
	    	}
    	}
        if(Network.player.guid == robberGuid){
            status.Avatar = Network.Instantiate(Resources.Load("Prefabs/Robber"), spawnRobber.position, Quaternion.identity, 0) as GameObject;
            status.IsRobber = true;
        } else {
            status.Avatar = Network.Instantiate(Resources.Load("Prefabs/Cop"), spawnCops.position, Quaternion.identity, 0) as GameObject;
            status.IsRobber = false;
        }
        Controller playerController = status.Avatar.gameObject.GetComponent("Controller") as Controller;
        playerController.selectedAbility = status.Ability;
        playerController.guid = Network.player.guid;
        roundOn = true;
        Debug.Log("Load Level");
        roundManager.StartRound(robberGuid);
        GUIManager.SetGUI("GameHUD");
    }

    public void StartGame()
    {
        

        //roundManager.InitializePlayerList(Network.connections);
        roundManager.StartGame();
        
    }

    public void LoadFirstLevel()
    {
        networkView.RPC("LoadLevel", RPCMode.All, roundManager.GetCurrentRobberGuid());
    }

    public void StartRound()
    {
        networkView.RPC("LoadLevel", RPCMode.All, roundManager.GetCurrentRobberGuid());
    }

    public string IPConnection
    {
        get
        {
            return ipConnection;
        }
        set
        {
            ipConnection = value;
        }
    }

    public int PortConnection
    {
        get
        {
            return portConnection;
        }
        set
        {
            portConnection = value;
        }
    }
    
}
