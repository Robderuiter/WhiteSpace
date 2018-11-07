using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerRes : CurrentResources {

	// Use this for initialization
	new void Start () {
		stone = new Stone (this);
		res = new Resource[1] {stone};
	}
}
