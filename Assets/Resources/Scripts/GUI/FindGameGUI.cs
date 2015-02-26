using UnityEngine;
using System.Collections;

public class FindGameGUI : IGUIState {

	private MainServerCode serverControl;
	private string nextState;

	public FindGameGUI()
	{
		serverControl = GameObject.Find("GlobalServerObject").GetComponent("MainServerCode") as MainServerCode;
		nextState = GUIName();
	}

	public void drawGUI()
	{
		int port;
		serverControl.IPConnection = GUI.TextField(new Rect(10, 10, 100, 25), serverControl.IPConnection);
        int.TryParse(GUI.TextField(new Rect(10, 35, 100, 25), serverControl.PortConnection.ToString()), out port);
        serverControl.PortConnection = port;
        if (GUI.Button(new Rect(10, 60, 200, 25), "Connect To Server (Client)")) {
            serverControl.ConnectToServer();
            nextState = "ClientWaitingForStart";
        }
        if (GUI.Button(new Rect(10, 100, 200, 25), "Become the Server (Host)")) {
            serverControl.SetServer();
            nextState = "ServerWaitingForStart";
        }
	}

	public string nextGUI()
	{
		return nextState;
	}

	public string GUIName()
	{
		return "FindGameGUI";
	}
}
