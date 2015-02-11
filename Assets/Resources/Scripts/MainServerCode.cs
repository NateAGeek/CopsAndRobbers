using UnityEngine;
using System.Collections;

public class MainServerCode : MonoBehaviour {
    
    public string ipConnection = "127.0.0.1";
    public int portConnection = 6969;
    

    void OnConnectedToServer() {
        Network.Instantiate(Resources.Load("Prefabs/Player"), new Vector3(0, 0, 0), Quaternion.identity, 0);
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
        }
        else {
            GUI.Label(new Rect(4, 24, 500, 16), "Connections: "+Network.connections.Length.ToString());
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
