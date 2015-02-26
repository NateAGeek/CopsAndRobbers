using UnityEngine;
using System.Collections;

public interface IGUIState {

	void drawGUI();

	string nextGUI();

	string GUIName();
}
