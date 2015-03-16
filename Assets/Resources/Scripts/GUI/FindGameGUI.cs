using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FindGameGUI : MonoBehaviour, IGUIState {
	public Text lobbyName;
	public Button createGameBtn;
	public Transform lobbyBtnPrefab;
	public Transform lobbyList;
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
		clearLobbyList();
		gameObject.SetActive(true);
	}

	public void onDeactive()
	{
		gameObject.SetActive(false);
	}

	public void clearLobbyList()
	{
		int lobbyCount = lobbyList.childCount;
		List<Transform> lobbyBtnList = new List<Transform>();
		for(int i = 0; i < lobbyCount; i++){
			lobbyBtnList.Add(lobbyList.GetChild(i));
		}
		foreach(Transform t in lobbyBtnList){
			Destroy(t.gameObject);
		}
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
		clearLobbyList();

		serverControl.RefreshHostList();
		HostData[] hostList = serverControl.GetHostList();
		if(hostList != null){
			for(int i = 0; i < hostList.Length; i++){
				Transform newBtn = Instantiate(lobbyBtnPrefab, Vector3.zero, Quaternion.identity) as Transform;
				newBtn.SetParent(lobbyList, false);
				Text newText = newBtn.GetChild(0).gameObject.GetComponent("Text") as Text;
				newText.text = hostList[i].gameName;
				Button b = newBtn.gameObject.GetComponent("Button") as Button;
				HostData h = hostList[i];
				b.onClick.AddListener(() => ConnectToLobby(h));
			}
		}
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

	public void ConnectToLobby(HostData h)
	{
		serverControl.ConnectToServer(h);

		GUIManager.SetGUI("ClientWaitingForStart");
	}
}
