using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpireRes : CurrentResources {

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
}
