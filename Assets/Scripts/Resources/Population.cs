using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population : Resource {

	//dehydration vars
	float dehydrationAmount = 1;
	float minDehydrationChance = 0.01f;
	float maxDehydrationChance = 0.2f;

	//birth vars
	float minBirthAmount = 1;
	float minBirthChance = 0.01f;
	float maxBirthChance = 0.1f;

	//starvation vars
	float minStarvationAmount = 1;
	float minStarvationChance = 0.01f;
	float maxStarvationChance = 0.1f;

	//setting the reference to the currentResources that called it
	public Population (CurrentResources res){
		myParentsResources = res;
	}

	public override void Calc(float multiplier){
		//change = 0;
			if (myParentsResources.water.amount <= 0) {
				change += Dehydrate ();
				//Debug.Log ("Dehydrate(): " + change);
			}
			if (myParentsResources.food.amount <= 0) {
				change += Starve ();
				//Debug.Log ("Starve(): " + change);
			}
			if (myParentsResources.water.amount > 0 && myParentsResources.food.amount > 0){
				change += Birth ();
				//Debug.Log ("Birth(): " + change);
			}

		change *= multiplier;
		//Debug.Log ("change = " + change + ", multiplier = " + multiplier);
	}

	float Dehydrate(){
		change = Change (-dehydrationAmount, -minDehydrationChance, -maxDehydrationChance);
		//Debug.Log ("Oh no! " + change + " people on " + myParentsResources.gameObject + " have died of thirst! Get some water over to them ASAP");
		return (change);
	}

	float Starve(){
		change = Change (-minStarvationAmount, -minStarvationChance, -maxStarvationChance);
		//Debug.Log ("Oh no! " + change + " people on " + myParentsResources.gameObject + " have died of starvation! Get some food over to them ASAP");
		return (change);
	}

	float Birth(){
		change = Change (minBirthAmount, minBirthChance, maxBirthChance);
		//Debug.Log ("Congratulations! There were " + change + " people born on " + myParentsResources.gameObject + "!");
		return (change);
	}
}