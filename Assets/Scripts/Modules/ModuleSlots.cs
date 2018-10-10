using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSlots {

	GameObject myParent;
	public int nSlots;
	public Module[] addedModules;

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

		//Debug.Log("myParent = " + myParent);
	}

	//called on Planet and Ship on Start(), thus runs once
	public void CalcModuleSlots(float spineLength, float usabilityModifier){
		//calculate the amount of modules based on given length (=circumference planet or "spinelength" of ship
		nSlots = (int)Mathf.Abs((spineLength * usabilityModifier) / (Module.modSize * 1.5f));
		//Debug.Log("nSlots = " + nSlots + ", spineLength = " + spineLength + ", usabilityModifier = " + usabilityModifier + ", modSize = " + Module.modSize);

		//instantiate (?) the addedModules array with nSlots as the length
		addedModules = new Module[nSlots];
	}
}
