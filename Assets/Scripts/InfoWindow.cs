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


	/*
	//floating info window
	CurrentResources currentRes;
	GameObject floatingInfoPanel;
	Text[] infoPanelText;
	Text[] infoPanelChanges;
	Image[] infoPanelImg;
	Camera cam;
	Planet currentPlanet;
	Color dGReen;
	Color red;

	//is temp, needs to go once selectionController has been rewritten
	public GameObject selectedObject;

	//flavor sprites
	Sprite earthlikeFlavor;
	Sprite desertFlavor;
	Sprite barrenFlavor;
	Sprite shipFlavor;

	void Awake(){
		//colors for text
		dGReen = new Color (0, 0.5f, 0);
		red = new Color (0.7f, 0, 0);
	}

	void Update(){
		if (selectedObject) {
			gameObject.transform.position = cam.WorldToScreenPoint (selectedObject.transform.position);

			if (TimeController.instance.timer == 5){
				UpdateFloatingInfo ();
			}
		}
			
	}

	public void updateFlavorGUI(){
		infoPanelImg = GetComponentsInChildren<Image>();

		if (!earthlikeFlavor) {
			GetFlavorImages ();
		}

		if (selectedObject.tag == "Planet") {
			string objectName = selectedObject.GetComponent<Planet> ().typeName;
			//print (objectName);
			if (objectName == "Earthlike") {
				infoPanelImg[2].sprite = earthlikeFlavor;
			}
			if (objectName == "Desert") {
				infoPanelImg[2].sprite = desertFlavor;
			}
			if (objectName == "Barren") {
				infoPanelImg[2].sprite = barrenFlavor;
			}

		} 
		if (selectedObject.tag == "Ship") {
			infoPanelImg [2].sprite = shipFlavor;
		}
	}
		
	void GetFlavorImages(){
		//get flavor images
		earthlikeFlavor = Resources.Load<Sprite>("FlavorSprites/planetearth");
		desertFlavor = Resources.Load<Sprite>("FlavorSprites/planetdesert");
		barrenFlavor = Resources.Load<Sprite>("FlavorSprites/planetbarren");
		shipFlavor = Resources.Load<Sprite> ("FlavorSprites/shipflavor");

	}

	public void UpdateFloatingInfo(){
		//get current resources, not efficient at all currently
		cam = GameObject.Find("Main Camera").GetComponent<Camera>();

		infoPanelText = GetComponentsInChildren<Text>();

		//for (int k = 0;k < infoPanelText.Length;k++){
		//	print("infoPanelRects [" + k + "] = " + infoPanelText [k]);
		//}

		gameObject.transform.position = cam.WorldToScreenPoint(selectedObject.transform.position);
		currentRes = selectedObject.GetComponent<CurrentResources> ();
		updateFlavorGUI ();
	
		if (selectedObject.tag == "Planet") {
			//type
			infoPanelText [1].text = currentRes.planet.typeName;
			if (selectedObject.GetComponent<Planet> ().isScanned) {
				//name
				infoPanelText [0].text = currentRes.planet.planetName;

				//resources
				UpdateAmountInfo (currentRes.pop, 2);
				UpdateAmountInfo (currentRes.food, 3);
				UpdateAmountInfo (currentRes.water, 4);
				UpdateAmountInfo (currentRes.oxygen, 5);
				UpdateAmountInfo (currentRes.power, 6);
				UpdateAmountInfo (currentRes.flora, 7);
				UpdateAmountInfo (currentRes.fauna, 8);

				//building slots
				currentPlanet = selectedObject.GetComponent<Planet> ();
				infoPanelText [15].text = currentPlanet.nModulesAttached + "/" + currentPlanet.nBuildingSlots;

				//update all change amounts
				UpdateChangeInfo (currentRes.pop, 16);
				UpdateChangeInfo (currentRes.food, 17);
				UpdateChangeInfo (currentRes.water, 18);
				UpdateChangeInfo (currentRes.oxygen, 19);
				UpdateChangeInfo (currentRes.power, 20);
				UpdateChangeInfo (currentRes.flora, 21);
				UpdateChangeInfo (currentRes.fauna, 22);

			} 
		}else {

		}
	}

	void UpdateAmountInfo(Resource res, int n){
		infoPanelText[n].text = res.amount.ToString ("0");
	}

	void UpdateChangeInfo(Resource res, int n){
		if (res.change > 0){
			infoPanelText[n].color = dGReen;
			infoPanelText[n].text = "+" + res.change.ToString("0");
		}
		if (res.change < 0){
			infoPanelText[n].color = red;
			infoPanelText[n].text = res.change.ToString("0");
		}
	}
	*/

