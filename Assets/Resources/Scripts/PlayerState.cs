using UnityEngine;
using System.Collections;

public class PlayerState {
	private int points;
	private bool isRobber;
	private string guid;
	private bool betweenRoundReady;
	private string name;

	public PlayerState()
	{
		points = 0;
		isRobber = false;
		guid = "";
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

	public string Guid {
		get {
			return guid;
		}

		set {
			guid = value;
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

	public string Name {
		get {
			return name;
		}

		set {
			name = value;
		}
	}
}
