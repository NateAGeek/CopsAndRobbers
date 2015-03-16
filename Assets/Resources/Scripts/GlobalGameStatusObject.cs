using UnityEngine;
using System.Collections.Generic;

public class GlobalGameStatusObject : MonoBehaviour {
	private int points;
	private bool isRobber;
	private bool betweenRoundReady;
	private GameObject playerAvatar;

	void Start()
	{
		points = 0;
		isRobber = false;
	}

	public int Points{
		get{
			return points;
		}

		set{
			points = value;
		}
	}

	public bool IsRobber {
		get {
			return isRobber;
		}

		set {
			isRobber = value;
		}
	}

	public bool IsReady {
		get {
			return betweenRoundReady;
		}

		set {
			betweenRoundReady = value;
		}
	}

	public GameObject Avatar {
		get {
			return playerAvatar;
		}

		set {
			playerAvatar = value;
		}
	}
}
