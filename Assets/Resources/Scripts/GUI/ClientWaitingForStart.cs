using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClientWaitingForStart : MonoBehaviour, IGUIState {

	void Start()
	{

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
}
