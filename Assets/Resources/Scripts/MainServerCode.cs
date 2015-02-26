using UnityEngine;
using System;

public class MainServerCode : MonoBehaviour {
    
    public string ipConnection = "127.0.0.1";
    public int portConnection = 6969;
	public Transform spawnCops;
	public Transform spawnRobber;
    public Transform spawnMacGuffin;

    private int selectedSpawnType = 0;
    private string[] options = new string[] { "Spawn Robber", "Spawn Cop" };
    private int currentRobber = 0;
    private bool roundOn = false;
    private int timeLimit = 120;
    private int startTime = 0;
    private GUIStyle timerStyle;

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

    public void ConnectToServer() {
        Network.Connect(ipConnection, portConnection);
    }

    public void SetServer() {
        Network.InitializeServer(4, portConnection, true);
        //Instantiate(Resources.Load("Prefabs/MacGuffin"), spawnMacGuffin.position, Quaternion.identity);
    }

    void OnGUI() {
        
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
    }

	// Use this for initialization
	void Start () {
        timerStyle = new GUIStyle();
        timerStyle.fontSize = 40;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [RPC]
    public void LoadLevel(string robberGuid)
    {
        if(Network.player.guid == robberGuid){
            Network.Instantiate(Resources.Load("Prefabs/Robber"), spawnRobber.position, Quaternion.identity, 0);
        } else {
            Network.Instantiate(Resources.Load("Prefabs/Player"), spawnCops.position, Quaternion.identity, 0);
        }
        roundOn = true;
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
