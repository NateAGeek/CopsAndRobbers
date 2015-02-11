using UnityEngine;
using System.Collections;

public class MainServerCode : MonoBehaviour {
    
    public string ipConnection = "127.0.0.1";
    public int portConnection = 6969;
	public Transform spawnCops;
	public Transform spawnRobber;

    private int selectedSpawnType = 0;
    private string[] options = new string[] { "Spawn Robber", "Spawn Cop" };

    void OnConnectedToServer() {
        if (selectedSpawnType == 0)
        {
            Debug.Log("Should Spawn Robber");
            Network.Instantiate(Resources.Load("Prefabs/Robber"), spawnRobber.position, Quaternion.identity, 0);
        }
        else {
            Debug.Log("Should Spawn Cop");
            Network.Instantiate(Resources.Load("Prefabs/Player"), spawnCops.position, Quaternion.identity, 0);
        }
    }

    void ConnectToServer() {
        Network.Connect(ipConnection, portConnection);
    }

    void SetServer() {
        Network.InitializeServer(4, portConnection, true);
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
            selectedSpawnType = GUI.SelectionGrid(new Rect(10, 125, 200, 50), selectedSpawnType, options, options.Length, "toggle");
        }
        else {
            if (Network.isServer) {
                GUI.Label(new Rect(4, 24, 500, 16), "Connections: " + Network.connections.Length.ToString());
            }
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
