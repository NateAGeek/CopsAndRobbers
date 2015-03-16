using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameHUD : MonoBehaviour, IGUIState {

	public Text pointsText;
	public Text timerText;
	public Text roleText;
	public Text abilityText;
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
		Controller playerController = status.Avatar.GetComponent("Controller") as Controller;
		if(status.IsRobber){
			setRole("Robber");
		} else {
			setRole("Cop");
		}
		switch(playerController.selectedAbility){
			case "StunTrap":
				abilityText.text = "Stun Trap";
				break;
			case "SlowBeam":
				abilityText.text = "Slow Beam";
				break;
			case "IRGlasses":
				abilityText.text = "Infrared Glasses";
				break;
			case "GrapHook":
				abilityText.text = "Grappling Hook";
				break;
			default:
				abilityText.text = "";
				break;
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
