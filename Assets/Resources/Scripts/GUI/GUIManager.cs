using UnityEngine;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour {

	public GameObject FindGameGUIObj;
	public GameObject ServerWaitingGUIObj;
	public GameObject ClientWaitingGUIObj;
	public GameObject GameHUDGUIObj;
	public GameObject ScoreboardGUIObj;

	private static GUIManager instance;

	private IGUIState findGameGUI;
	private IGUIState serverWaiting;
	private IGUIState clientWaiting;
	private IGUIState gameHUD;

	private Stack<IGUIState> stateStack;

	// Use this for initialization
	void Start () {
		instance = this;
		findGameGUI = FindGameGUIObj.GetComponent("FindGameGUI") as IGUIState;
		serverWaiting = ServerWaitingGUIObj.GetComponent("ServerWaitingForStart") as IGUIState;
		clientWaiting = ClientWaitingGUIObj.GetComponent("ClientWaitingForStart") as IGUIState;
		gameHUD = GameHUDGUIObj.GetComponent("GameHUD") as IGUIState;
		stateStack = new Stack<IGUIState>();
		stateStack.Push(findGameGUI);
		stateStack.Peek().onPush();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Scoreboard") && stateStack.Peek() == gameHUD){
			ScoreboardGUIObj.SetActive(true);
		} else if(Input.GetButtonUp("Scoreboard")){
			ScoreboardGUIObj.SetActive(false);
		}
	}

	void OnGUI() {
		/*
		IGUIState currentState = stateStack.Peek();
		currentState.drawGUI();
		string nextState = currentState.nextGUI();
		if(nextState.Length != 0){
			IGUIState nextGUI;
			if(nextState != currentState.GUIName()){
				if(stateCache.ContainsKey(nextState)){
					stateCache.TryGetValue(nextState, out nextGUI);
				} else {
					nextGUI = factory.BuildGUI(nextState);
					stateCache.Add(nextGUI.GUIName(), nextGUI);
				}
				stateStack.Push(nextGUI);
				nextGUI.onPush();
			}
		} else {
			IGUIState lastState = stateStack.Pop();
			lastState.onPop();
		}
		*/
	}

	public void drawNewGUI(string guiName)
	{
		IGUIState oldGUI;
		IGUIState newGUI;
		switch(guiName){
			case "":
				oldGUI = stateStack.Pop();
				oldGUI.onPop();
				newGUI = stateStack.Peek();
				newGUI.onActive();
				return;
			case "FindGameGUI":
				newGUI = findGameGUI;
				break;
			case "ClientWaitingForStart":
				newGUI = clientWaiting;
				break;
			case "ServerWaitingForStart":
				newGUI = serverWaiting;
				break;
			case "GameHUD":
				newGUI = gameHUD;
				break;
			default:
				Debug.Log("GUI Name nonsense");
				return;
		}
		oldGUI = stateStack.Peek();
		stateStack.Push(newGUI);
		oldGUI.onDeactive();
		newGUI.onPush();
	}

	public static void SetGUI(string guiName)
	{
		instance.drawNewGUI(guiName);
	}

}
