using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource {

	//alle temperature wordt in degree Kelvin of KELVIN


	//ref to CurrentResources
	public CurrentResources myParentsResources;

	//max zegt: amount gebruiken om op te slaan, pop.amount of food.amount, etc
	public float change; //slordige variable voor temp calcs
	public float amount;
	public float consumption = 1;
	public float max = Mathf.Infinity;
	public float storage;

	//deprecated?
	public void ClampToZero(){
		if (amount <= 0) {
			amount = 0;
		}
	}

	//called on almost every resource type in a custom function with a custom print, then Change()
	public float Change(float minAmount, float minChance, float maxChance){
		return (minAmount + amount * Random.Range (minChance, maxChance));
	}

	public float Consume(float consumers, float consumptionPer){
		return (- consumers * consumptionPer); //- because this should always be negative, change = -whatever
	}

	public float calcRelChange(Resource otherResource, float multiplier){
		return multiplier * otherResource.amount;
	}

	public void ApplyChange(){
		amount = amount + change;
		amount = (amount > max ? max : amount);
		amount = (amount < 0 ? 0 : amount);
	}

	public virtual void Calc(float multiplier){

	}

	/*
	public void ExtractResource(float nRes){
		//print ("water = " + water);
		water = water - nRes;
		//print ("water = " + water);
	}
	*/

	/*
	Resources: 

	Pop
	Food
	Water
	Oxygen
	Power
	Flora
	Fauna
	Stone
	Iron
	Pollution
	Temperature
	Gold
	Research
	*/
}
