using UnityEngine;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour {

	private Stack<IGUIState> stateStack;
	private Dictionary<string, IGUIState> stateCache;

	// Use this for initialization
	void Start () {
		IGUIState first = new FindGameGUI();
		stateStack.Push(first);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		IGUIState currentState = stateStack.Peek();
		currentState.drawGUI();

	}
}
