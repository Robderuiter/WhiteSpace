using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Resource {

	//setting the reference to the currentResources that called it
	public Food (CurrentResources res){
		myParentsResources = res;
		consumption = 1;
	}

	public override void Calc(float multiplier){
		if (myParentsResources.pop.amount > 0 && amount > 0) {
			change = Eat ();
			//Debug.Log ("CalcFood change = " + change);
		}
		//needed: if specific module is present on planet, produce food

		//add in planet multiplier
		change *= multiplier;
	}

	float Eat(){
		change = Consume(myParentsResources.pop.amount, consumption);
		//Debug.Log ("The colonists on " + myParentsResources.planet.typeName + " have eaten " + amount + " food.");
		return (change);
	}
}
