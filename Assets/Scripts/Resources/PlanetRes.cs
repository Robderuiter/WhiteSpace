using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRes : CurrentResources {

	// Use this for initialization
	new void Start () {
		planet = GetComponentInParent<Planet>();
		//print ("planetModifiers on currentresources = " + planetModifiers[0] + ", also nresMultipliers = " + planet.nResourceMultipliers + ", planet.resourcemult[0] = " + planet.resourceMultipliers[0]);

		//add resources
		pop = new Population (this);
		food = new Food (this);
		water = new Water (this);
		oxygen = new Oxygen (this);
		flora = new Flora (this);
		fauna = new Fauna (this);
		stone = new Stone (this);
		//@@ volgorde moet later aangepast..
		res = new Resource[7]{pop, food, water, oxygen, flora, fauna, stone};
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update ();
	}

	//called on planet type subclasses (Earthlike, Desert..)
	public override void InitResources(){
		float[] resourceTresholds = gameObject.GetComponentInParent<Planet>().resourceTresholds;
		pop.amount = Random.Range(resourceTresholds[0],resourceTresholds[1]);
		food.amount = Random.Range(resourceTresholds[2],resourceTresholds[3]);
		water.amount = Random.Range(resourceTresholds[4],resourceTresholds[5]);
		water.max = water.amount;
		//so that water is already set to account for current population
		water.Calc (resourceMultipliers[2]);

		oxygen.amount = Random.Range(resourceTresholds[8], resourceTresholds[9]);
		stone.amount = Random.Range (resourceTresholds [12], resourceTresholds [13]);
		flora.amount = Random.Range (resourceTresholds[18], resourceTresholds[19]);
		fauna.amount = Random.Range(resourceTresholds[20], resourceTresholds[21]);

	}
}
