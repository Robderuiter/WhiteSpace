using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmpireUIController : MonoBehaviour {

	Empire empire;
	int nPanels = 3;
	//GameObject empireOverviewPanel;
	GameObject empirePlanetsPanel;
	GameObject empireShipsPanel;
	GameObject empireResourcesPanel;
	int timesRan;
	float panelWidth;

	//rectTransforms for gameobject placement on ui
	RectTransform empirePlanetsRect;
	RectTransform empireShipsRect;
	//RectTransform empireResourcesRect;

	GameObject shipImgPrefab;
	GameObject planetImgPrefab;
	GameObject planetIcon;
	GameObject shipIcon;
	Sprite planetIconSprite;
	float imgWidth;

	float textHeight;
	float imgBuffer = 5;

	void Awake(){
		//load resources (jaja)
		shipImgPrefab = Resources.Load<GameObject> ("ShipImg");
		planetImgPrefab = Resources.Load<GameObject> ("PlanetImg");

		//find empire gameobject and get ref to Empire component
		empire = GetComponent<Empire>();
		//print ("empire = " + empire);
	}

	void Start(){
		//get ref to panels
		//empireOverviewPanel = GameObject.Find("EmpireOverview");
		empirePlanetsPanel = GameObject.Find ("EmpirePlanets");
		empireShipsPanel = GameObject.Find ("EmpireShips");
		empireResourcesPanel = GameObject.Find ("EmpireResources");

		//get all rectTransforms
		empirePlanetsRect = empirePlanetsPanel.GetComponent<RectTransform>();
		empireShipsRect = empireShipsPanel.GetComponent<RectTransform> ();
		//empireResourcesRect = empireResourcesPanel.GetComponent<RectTransform>();

		//set sizes of all empire subpanels, //@@ @max i know i know, runs 3 times outside of a loop, boohoo! :P
		timesRan = 0;
		panelWidth = Screen.width / nPanels;
		PositionEmpireUI(empirePlanetsPanel);
		PositionEmpireUI(empireShipsPanel);
		PositionEmpireUI(empireResourcesPanel);

		//get the height of a title, used for placing spawned ui image gameobjects
		textHeight = empirePlanetsPanel.GetComponentInChildren<Text>().rectTransform.rect.height;

		//get width of used image
		imgWidth = shipImgPrefab.GetComponent<Image>().rectTransform.rect.width;

		UpdateEmpirePlanetUI ();
		UpdateEmpireShipUI ();
	}
		
	//run once to adjust ui panel sizes to screen size
	void PositionEmpireUI(GameObject panel){
		RectTransform rectT = panel.GetComponent<RectTransform>();
		rectT.sizeDelta = new Vector2(panelWidth, rectT.rect.height);
		rectT.localPosition = new Vector2 (panelWidth * timesRan - Screen.width / 2, 0);
		print (rectT.localPosition);
		timesRan++;
	}

	//call this whenever something changes for empire wide planets/ships/resources, moet meer samengevoegd worden naar enkele functies maar voor nu superfijn om ze apart uit te schrijven (ivm unieke sprites/sizes/positions)
	public void UpdateEmpireUI(){
		// werkt nog niet goed
		UpdateEmpirePlanetUI();
		UpdateEmpireShipUI();
	}

	//public because certain events need to call for just a planet empireUI update
	public void UpdateEmpirePlanetUI(){
		foreach (Planet planet in empire.planets) {
			if (planet.isInEmpire == false) {
				planetIcon = Instantiate (planetImgPrefab, empirePlanetsPanel.transform.position, transform.rotation);
				float neededImgSize = empirePlanetsRect.rect.height / 3;
				planetIcon.GetComponent<RectTransform> ().sizeDelta = new Vector2 (neededImgSize, neededImgSize);
				//planetIcon.GetComponent<Image> ().sprite = null;
				planetIconSprite = Resources.Load<Sprite> ("PlanetTypes/" + planet.typeName);
				planetIcon.GetComponent<Image> ().sprite = planetIconSprite;
				planetIcon.transform.SetParent (empirePlanetsPanel.transform);
				planetIcon.transform.localPosition = new Vector2 ((neededImgSize + imgBuffer) * empire.planets.Count, empirePlanetsRect.rect.height - textHeight * 2 - imgBuffer);
				planet.isInEmpire = true;
			}
		}
	}

	//public because certain events need to call for just a ship empireUI update
	public void UpdateEmpireShipUI(){
		foreach (Ship ship in empire.ships) {
			if (ship.isInEmpire == false) {
				shipIcon = Instantiate (shipImgPrefab, empirePlanetsPanel.transform.position, empirePlanetsPanel.transform.rotation);
				shipIcon.transform.SetParent (empireShipsPanel.transform);
				shipIcon.transform.localPosition = new Vector2 ((imgWidth + imgBuffer) * empire.ships.Count, empireShipsRect.rect.height - textHeight * 2);
				shipIcon.transform.localRotation = new Quaternion (0,0,0,0);
				ship.isInEmpire = true;
			}
		}
	}

	public void UpdateEmpireResourceUI(){

	}

}
