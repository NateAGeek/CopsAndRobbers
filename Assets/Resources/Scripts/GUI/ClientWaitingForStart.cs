using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClientWaitingForStart : MonoBehaviour, IGUIState {
	private MainServerCode serverControl;

	void Start()
	{
		serverControl = GameObject.Find("GlobalServerObject").GetComponent("MainServerCode") as MainServerCode;
	}

	void OnDisconnectedFromServer()
	{
		if(gameObject.activeSelf){
			GUIManager.SetGUI("");
		}
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

	public void LeaveLobbyBtnClicked()
	{
		serverControl.DisconnectFromServer();
	}
}
