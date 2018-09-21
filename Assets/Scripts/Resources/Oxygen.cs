using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oxygen : Resource {

	//setting the reference to the currentResources that called it
	public Oxygen (CurrentResources res){
		myParentsResources = res;
		consumption = 1;
	}

	public override void Calc(float multiplier){
		change = - myParentsResources.pop.change * multiplier;
		//Debug.Log ("flora.change = " + myParentsResources.flora.change + ", flora.consumption = " + myParentsResources.flora.consumption + ", flora.amount = " + myParentsResources.flora.amount);
	}
}
