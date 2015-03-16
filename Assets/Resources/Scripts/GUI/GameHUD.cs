using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameHUD : MonoBehaviour, IGUIState {

	public Text pointsText;
	public Text timerText;
	public Text roleText;
	public GlobalGameStatusObject status;

	// Use this for initialization
	void Start () {
		
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
		if(status.IsRobber){
			setRole("Robber");
		} else {
			setRole("Cop");
		}
		setPoints(status.Points.ToString());
		gameObject.SetActive(true);
	}

	public void onDeactive()
	{
		gameObject.SetActive(false);
	}

	public void setPoints(string p)
	{
		pointsText.text = p;
	}

	public void setTimer(string t)
	{
		timerText.text = t;
	}

	public void setRole(string r)
	{
		roleText.text = r;
	}
}
