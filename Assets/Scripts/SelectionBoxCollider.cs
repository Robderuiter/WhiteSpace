using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBoxCollider : MonoBehaviour {


	//## commented since selectionmaster rewrite dd21-09-2018
	void OnTriggerEnter2D (Collider2D otherCol){
		//first check if the othercollider's gameobject has a selectable, to prevent errors
		if (otherCol.gameObject.GetComponent<Selectable> ()) {
			Selectable otherSelectable = otherCol.GetComponent<Selectable> ();

			//if leftclick
			if (SelectionMaster.instance.isLeftClick) {
				//if ctrl, select modules, check for left shift should be elsewhere, according to max
				if (Input.GetKey ("left shift")) {
					if (otherSelectable.selectableType == SelectableType.Module) {
						SelectionMaster.instance.selectedObjects.Add (otherSelectable);
						SelectionMaster.instance.selectedType = SelectableType.Module;
					}
				} 
					
				//else add to seperate lists (and run filterselection in the SelectionMaster class)
				else {
					if (otherSelectable.selectableType == SelectableType.Ship) {
						SelectionMaster.instance.selectedShips.Add (otherSelectable);
					} else if (otherSelectable.selectableType == SelectableType.Planet) {
						SelectionMaster.instance.selectedPlanets.Add (otherSelectable);
					} else if (otherSelectable.selectableType == SelectableType.Module) {
						SelectionMaster.instance.selectedModules.Add (otherSelectable);
					}
					//print ("ships.count = " + SelectionMaster.instance.selectedShips.Count + "planets.count = " + SelectionMaster.instance.selectedPlanets.Count + "modules.count = " + SelectionMaster.instance.selectedModules.Count);
				}
			} 

			//if right click
			else if (SelectionMaster.instance.isRightClick) {
				
			}  
		}
	}

}

