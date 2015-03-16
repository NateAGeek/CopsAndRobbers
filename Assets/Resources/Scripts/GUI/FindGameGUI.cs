using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FindGameGUI : MonoBehaviour, IGUIState {
	public Text lobbyName;
	public Button createGameBtn;
	private MainServerCode serverControl;

	void Start()
	{
		serverControl = GameObject.Find("GlobalServerObject").GetComponent("MainServerCode") as MainServerCode;
	}

	public void drawGUI()
	{
        
	}

	public void onPush()
	{
		onActive();
	}

	public void onPop()
	{
		onDeactive();
	}

	public void onActive()
	{
		gameObject.SetActive(true);
	}

	public void onDeactive()
	{
		gameObject.SetActive(false);
	}

	public void ConnectToServerBtnClicked()
	{
		/*
		string ip;
		int port;
		if(ipAddress.text.Length > 0){
			ip = ipAddress.text;
		} else {
			ip = defaultIP.text;
		}
		if(portConn.text.Length > 0){
			int.TryParse(portConn.text, out port);
		} else {
			int.TryParse(defaultPort.text, out port);
		}
		
		serverControl.IPConnection = ip;
		serverControl.PortConnection = port;
		serverControl.ConnectToServer();
		
		GUIManager.SetGUI("ClientWaitingForStart");
		*/
	}

	public void BecomeServerBtnClicked()
	{
		/*
		int port;
		if(portConn.text.Length > 0){
			int.TryParse(portConn.text, out port);
		} else {
			int.TryParse(defaultPort.text, out port);
		}

		serverControl.PortConnection = port;
		serverControl.SetServer();

		GUIManager.SetGUI("ServerWaitingForStart");
		*/
		serverControl.SetServer(lobbyName.text);

		GUIManager.SetGUI("ServerWaitingForStart");
		
	}

	public void LobbyNameOnChange(string lobby)
	{
		if(lobby.Length <= 0){
			createGameBtn.interactable = false;
		} else {
			createGameBtn.interactable = true;
		}
	}
}
