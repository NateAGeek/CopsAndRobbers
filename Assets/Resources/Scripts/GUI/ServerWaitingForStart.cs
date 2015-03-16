using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerWaitingForStart : MonoBehaviour, IGUIState {

	public Text connectionsText;
	private MainServerCode serverControl;

	void Start()
	{
		serverControl = GameObject.Find("GlobalServerObject").GetComponent("MainServerCode") as MainServerCode;
	}

	void OnPlayerConnected()
	{
		connectionsText.text = "Connections: " + Network.connections.Length.ToString();
	}

	void OnPlayerDisconnected()
	{
		connectionsText.text = "Connections: " + Network.connections.Length.ToString();
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
		connectionsText.text = "Connections: " + Network.connections.Length.ToString();

		gameObject.SetActive(true);
	}

	public void onDeactive()
	{
		gameObject.SetActive(false);
	}

	public void StartGameBtnClicked()
	{
		serverControl.StartGame();
	}

	public void LeaveLobbyBtnClicked()
	{
		serverControl.DisconnectServer();
		GUIManager.SetGUI("");
	}
}
