﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BetweenRoundGUI : MonoBehaviour, IGUIState {
	public RoundManager roundManager;
	public Transform abilitiesMenu;

	private string selectedAbility;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < abilitiesMenu.childCount; i++){
			Transform abSelect = abilitiesMenu.GetChild(i);
			Button abBtn = abSelect.gameObject.GetComponent("Button") as Button;
			Text abText = abSelect.Find("Text").gameObject.GetComponent("Text") as Text;
			abBtn.onClick.AddListener(() => AbilityBtnClicked(abText.text));
		}
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

	public void AbilityBtnClicked(string ability)
	{
		selectedAbility = ability;
		for(int i = 0; i < abilitiesMenu.childCount; i++){
			Transform abSelect = abilitiesMenu.GetChild(i);
			Button abBtn = abSelect.gameObject.GetComponent("Button") as Button;
			Text abText = abSelect.Find("Text").gameObject.GetComponent("Text") as Text;
			if(abText.text == ability){
				abBtn.interactable = false;
			} else {
				abBtn.interactable = true;
			}
		}
	}
}
