using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scannerController : MonoBehaviour {

	// without this scanner would displace itself whenever ship hit anything

	// Update is called once per frame
	void Update () {
		if (transform.localPosition.x != 0 || transform.localPosition.y != 0){
			transform.localPosition = new Vector2(0,0);
		}
	}
}
