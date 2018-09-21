using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoWindow : MonoBehaviour {

	//infowindow stuff
	//GameObject infoPanel;
	//float infoPanelWidth;
	Text[] infoHeaderText;
	Image[] infoHeaderImg;
	CurrentResources currentRes;

	//flavor sprites
	Sprite earthlikeFlavor;
	Sprite desertFlavor;
	Sprite barrenFlavor;
	Sprite shipFlavor;

	//should replace with Selectable
	public GameObject selectedObject;

	void Awake (){
		//infoPanel = this.gameObject;

		//infoPanelWidth = infoPanel.GetComponent<RectTransform> ().sizeDelta.x;

		infoHeaderText = GameObject.Find ("InfoHeader").GetComponentsInChildren<Text> ();
		infoHeaderImg = GameObject.Find ("InfoHeader").GetComponentsInChildren<Image> ();
	}

	public void UpdateFloatingInfo(){
		currentRes = selectedObject.GetComponent<CurrentResources> ();

		for (int i = 0; i < infoHeaderText.Length; i++) {
			print ("infoHeaderText[" + i + "] = " + infoHeaderText[i]);
		}

		for (int j = 0; j < infoHeaderImg.Length; j++) {
			print ("infoHeaderImg[" + j + "] = " + infoHeaderImg[j]);
		}


		infoHeaderText[0].text = currentRes.planet.typeName;

		updateFlavorGUI ();
	}

	void GetFlavorImages(){
		//get flavor images
		earthlikeFlavor = Resources.Load<Sprite> ("FlavorSprites/planetearth");
		desertFlavor = Resources.Load<Sprite> ("FlavorSprites/planetdesert");
		barrenFlavor = Resources.Load<Sprite> ("FlavorSprites/planetbarren");
		shipFlavor = Resources.Load<Sprite> ("FlavorSprites/shipflavor");

	}

	public void updateFlavorGUI(){
		if (!earthlikeFlavor) {
			GetFlavorImages ();
		}

		if (selectedObject.tag == "Planet") {
			string objectName = selectedObject.GetComponent<Planet> ().typeName;
			//print (objectName);
			if (objectName == "Earthlike") {
				infoHeaderImg[2].sprite = earthlikeFlavor;
			}
			if (objectName == "Desert") {
				infoHeaderImg[2].sprite = desertFlavor;
			}
			if (objectName == "Barren") {
				infoHeaderImg[2].sprite = barrenFlavor;
			}

		} 
		if (selectedObject.tag == "Ship") {
			infoHeaderImg [1].sprite = shipFlavor;
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

