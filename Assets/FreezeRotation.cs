using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotation : MonoBehaviour {

	Vector3 rot;
	//Quaternion temp;

	// Use this for initialization
	void Awake () {
		rot = new Vector3 (0,0,90);
		//temp = new Quaternion(0,0,0,0);
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (rot);
		//transform.rotation = temp;
	}
}
