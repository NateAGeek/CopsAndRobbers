using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameHUD : MonoBehaviour, IGUIState {

	public Text pointsText;
	public Text timerText;
	public Text roleText;


	private int points;
	private int roundTime;
	private int timeLimit;
	private PlayerControl player;

	// Use this for initialization
	void Start () {
		points = 0;
		roundTime = 0;
		timeLimit = 180;
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
		if(player.isRobber){
			roleText.text = "Robber";
		} else {
			roleText.text = "Cop";
		}
		gameObject.SetActive(true);
	}

	public void onDeactive()
	{
		gameObject.SetActive(false);
	}

	public void attachPlayer(PlayerControl p)
	{
		player = p;
	}

}
