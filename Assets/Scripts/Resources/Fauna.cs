using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fauna : Resource {

	//dehydration vars
	float dehydrationAmount = 1;
	float minDehydrationChance = 0.01f;
	float maxDehydrationChance = 0.2f;

	//birth vars
	float minBirthAmount = 1;
	float minBirthChance = 0.05f;
	float maxBirthChance = 0.2f;

	//starvation vars
	float minStarvationAmount = 1;
	float minStarvationChance = 0.01f;
	float maxStarvationChance = 0.1f;

	public Fauna (CurrentResources res){
		myParentsResources = res;

		consumption = 0.3f;
	}

	public override void Calc(float multiplier){
		change = 0;
		if (myParentsResources.water.amount <= 0) {
			change = Change (-dehydrationAmount, -minDehydrationChance, -maxDehydrationChance);
			//Debug.Log ("Oh no! " + change + " animals on " + myParentsResources.gameObject + " have died of thirst! Get some water over to them ASAP");
		}
		if (myParentsResources.flora.amount <= 0) {
			change = Change (-minStarvationAmount, -minStarvationChance, -maxStarvationChance);
			//Debug.Log ("Oh no! " + change + " animals on " + myParentsResources.gameObject + " have died of starvation! Get some food over to them ASAP");
		}
		if (myParentsResources.water.amount > 0 && myParentsResources.flora.amount > 0 && amount > 0){
			//minBirthAmount = amount * minBirthChance;

			change = Change (minBirthAmount, minBirthChance, maxBirthChance);

			//change other resources
			myParentsResources.water.change += - change * consumption;
			myParentsResources.flora.change += -change * consumption;

			//Debug.Log ("Congratulations! There were " + change + " animals born on " + myParentsResources.gameObject + "!");
		}

		change *= multiplier;
		//Debug.Log ("change = " + change + ", multiplier = " + multiplier);
	}
}
