using UnityEngine;
using System.Collections;

public class BetweenRoundGUI : MonoBehaviour, IGUIState {
	public RoundManager roundManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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

	public void ReadyBtnChecked(bool ready)
	{
		roundManager.SetReady(ready);
	}
}
