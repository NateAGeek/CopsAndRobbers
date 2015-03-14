using UnityEngine;
using System.Collections;

public class GrapHookObject : MonoBehaviour {

	public Vector3 StartPoint;
	public Vector3 EndPoint;
	
	// Use this for initialization
	void Start () {
//		StartNode.transform = StartPoint;
//		EndNode.transform   = EndPoint;

		GetComponent<LineRenderer>().SetPosition(0, StartPoint);
		GetComponent<LineRenderer>().SetPosition(1, EndPoint);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
