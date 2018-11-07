using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRes : CurrentResources {

	// Use this for initialization
	new void Start () {
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
