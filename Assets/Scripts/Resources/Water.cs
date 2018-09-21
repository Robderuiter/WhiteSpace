using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Resource {

	//setting the reference to the currentResources that called it
	public Water (CurrentResources res){
		myParentsResources = res;
		consumption = 1;
	}

	public override void Calc(float multiplier){
		// change = (- myParentsResources.pop.change - myParentsResources.flora.change * myParentsResources.flora.consumption) * multiplier;
		change = - myParentsResources.pop.change * multiplier;
		//Debug.Log ("flora.change = " + myParentsResources.flora.change + ", flora.consumption = " + myParentsResources.flora.consumption + ", flora.amount = " + myParentsResources.flora.amount);
	}
}
