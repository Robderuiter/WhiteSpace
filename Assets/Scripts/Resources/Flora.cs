using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flora : Resource {

	//dehydration vars
	float dehydrationAmount = 1;
	float minDehydrationChance = 0.01f;
	float maxDehydrationChance = 0.2f;

	//birth vars
	float minCreateAmount = 1;
	float minCreateChance = 0.01f;
	float maxCreateChance = 0.2f;

	//setting the reference to the currentResources that called it
	public Flora (CurrentResources res){
		myParentsResources = res;

		//water consumption, currently very high so testing is easier
		consumption = 0.3f;
	}

	public override void Calc(float multiplier){
		change = 0;
		if (myParentsResources.water.amount <= 0 || amount < 0) {
			change += Change (-dehydrationAmount, -minDehydrationChance, -maxDehydrationChance);
		}
		if (myParentsResources.water.amount > 0 && amount > 0){
			minCreateAmount = amount * Random.Range(minCreateChance, maxCreateChance);
			change += Change (minCreateAmount, minCreateChance, maxCreateChance);
			}
		change *= multiplier;
		//Debug.Log ("change = " + change + ", multiplier = " + multiplier);
		//Debug.Log("flora change = " + change + ", flora.amount = " + amount);

		//change other resources
		myParentsResources.water.change += -change * consumption;
		//Debug.Log (this + ", water.amount = " + myParentsResources.water.amount + ", water.change = " + myParentsResources.water.change);
	}
}
