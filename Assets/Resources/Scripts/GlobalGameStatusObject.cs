using UnityEngine;
using System.Collections.Generic;

public class GlobalGameStatusObject : MonoBehaviour {
	private int points;
	private bool isRobber;

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

	
}
