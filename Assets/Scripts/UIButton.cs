using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour {

	public GameObject source;

	public void MoveToSource(){
		Camera.main.transform.position = new Vector3(source.transform.position.x, source.transform.position.y, Camera.main.transform.position.z);
	}
}
