using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : Resource {

	//setting the reference to the currentResources that called it
	public Power (CurrentResources res){
		myParentsResources = res;
	}

	public override void Calc(float multiplier){
		change = 0;
		//Debug.Log ("flora.change = " + myParentsResources.flora.change + ", flora.consumption = " + myParentsResources.flora.consumption + ", flora.amount = " + myParentsResources.flora.amount);
	}
}
