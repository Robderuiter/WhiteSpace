﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSlots {

	//this is to be added to any gameobject that can hold modules

	GameObject myParent;
	public int nSlots;
	public Module[] addedModules;
	public Transform[] moduleSlotPositions;

	public ModuleSlots (Ship ship, Planet planet){
		//Debug.Log ("ship = " + ship + ", planet = " + planet);

		//set myParent gameObject depending on whether you right clicked a planet or mouse (in SelectionMaster)
		if (ship) {
			//Debug.Log ("ship");
			myParent = ship.gameObject;
		}
		if (planet) {
			//Debug.Log ("planet");
			myParent = planet.gameObject;
		}

		addedModules = new Module[nSlots];
		//Debug.Log("myParent = " + myParent);
	}

	//	@descript:	calculate the amount of modules allowed to be attached
	//	@param1:	spineLength is either the circumference of the planet or the "spineLength" of the ship: aka the space that allows for placing modules
	//	@param2:	usabilityModifier is a modifier designed mainly for future use of unbuildable terrain on planets
	//	@called:	called on Planet and Ship on Start(), thus runs once
	public void CalcModuleSlots(float spineLength, float usabilityModifier){
		//calculate the amount of modules based on given length (=circumference planet or "spinelength" of ship
		nSlots = (int)Mathf.Abs((spineLength * usabilityModifier) / (Module.modSize * 1.5f));
		//Debug.Log("nSlots = " + nSlots + ", spineLength = " + spineLength + ", usabilityModifier = " + usabilityModifier + ", modSize = " + Module.modSize);

		//instantiate (?) the addedModules array with nSlots as the length
		addedModules = new Module[nSlots];
	}

	/*
	public void CalcModuleSlotPositions(){
		//init the array
		moduleSlotPositions = new Transform[nSlots-1];

		//planet
		if (myParent.gameObject.tag == "Planet") {
			transform.localPosition = new Vector2(0,0);
		}
		//ship
		if (myParent.gameObject.tag == "Ship") {
			transform.localPosition = new Vector2(0,0);
		}
	}
	*/
}
