using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoWindow : MonoBehaviour {

	//this gameobject will be switched on/off in selectionmaster and get fed the target object to display, since there's only one infowindow and it only has to work when something is selected, it should be ok to use update 
	//notes: gotta do some size checking, probably on awake: use sizes depending on screen size/other element's sizes

	Planet selectedPlanet;

	//UI element containers
	Text[] infoHeaderText;
	Image[] infoHeaderImg;
	Text[] infoResourceText;

	//flavorsprites
	Sprite earthlikeFlavor;
	Sprite desertFlavor;
	Sprite barrenFlavor;
	Sprite shipFlavor;

	//colors for change text
	Color green;
	Color red;

	void Awake(){
		infoHeaderText = GameObject.Find ("InfoHeader").GetComponentsInChildren<Text> ();
		infoHeaderImg = GameObject.Find ("InfoHeader").GetComponentsInChildren<Image> ();
		infoResourceText = GameObject.Find ("Resources").GetComponentsInChildren<Text> ();

		green = new Color (0, 0.5f, 0);
		red = new Color (0.7f, 0, 0);
	}

	void Start(){
		
	}

	void Update(){
		//display detailed info on single selected object
		if (SelectionMaster.instance.selectedObjects.Count == 1) {
			//if selectedObject is planet
			if (SelectionMaster.instance.selectedObjects [0].GetComponent<Planet> ()) {
				//get selected planet's Planet
				selectedPlanet = SelectionMaster.instance.selectedObjects [0].GetComponent<Planet> ();

				//change text
				infoHeaderText [0].text = selectedPlanet.planetName;
				infoHeaderText[1].text = selectedPlanet.typeName;
				infoHeaderText[2].text = "0"; //@@ NOT WORKING YET
				infoHeaderText[3].text = " / " + selectedPlanet.nBuildingSlots.ToString();

				//change flavorsprite
				infoHeaderImg [0].sprite = selectedPlanet.flavorSprite;

				//update resource info
				UpdateAmountInfo (selectedPlanet.currentRes.pop, 0);
				UpdateChangeInfo (selectedPlanet.currentRes.pop, 1);

				UpdateAmountInfo (selectedPlanet.currentRes.flora, 2);
				UpdateChangeInfo (selectedPlanet.currentRes.flora, 3);

				UpdateAmountInfo (selectedPlanet.currentRes.fauna, 4);
				UpdateChangeInfo (selectedPlanet.currentRes.fauna, 5);

				UpdateAmountInfo (selectedPlanet.currentRes.food, 6);
				UpdateChangeInfo (selectedPlanet.currentRes.food, 7);

				UpdateAmountInfo (selectedPlanet.currentRes.water, 8);
				UpdateChangeInfo (selectedPlanet.currentRes.water, 9);

				UpdateAmountInfo (selectedPlanet.currentRes.oxygen, 10);
				UpdateChangeInfo (selectedPlanet.currentRes.oxygen, 11);

				UpdateAmountInfo (selectedPlanet.currentRes.power, 12);
				UpdateChangeInfo (selectedPlanet.currentRes.power, 13);
			}
		}

		//display less detailed info on multiple objects
		if (SelectionMaster.instance.selectedObjects.Count > 1) {

		}
	}

	void UpdateAmountInfo(Resource res, int n){
		infoResourceText[n].text = res.amount.ToString ("0");
	}

	void UpdateChangeInfo(Resource res, int n){
		if (res.change > 0){
			infoResourceText[n].color = green;
			infoResourceText[n].text = "+" + res.change.ToString("0");
		}
		if (res.change < 0){
			infoResourceText[n].color = red;
			infoResourceText[n].text = res.change.ToString("0");
		}
	}
}