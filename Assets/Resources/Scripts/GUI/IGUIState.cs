using UnityEngine;
using System.Collections;

public interface IGUIState {

	void drawGUI();

	void onPush();

	void onPop();

	void onActive();

	void onDeactive();
}
