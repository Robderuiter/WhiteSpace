using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Resource {

	//setting the reference to the currentResources that called it
	public Stone (CurrentResources res){
		myParentsResources = res;
	}
}
