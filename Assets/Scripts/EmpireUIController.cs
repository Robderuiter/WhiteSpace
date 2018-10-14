using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmpireUIController : MonoBehaviour {

	Empire empire;
	int nPanels = 3;
	GameObject empirePlanetsPanel;
	GameObject empireShipsPanel;
	GameObject empireResourcesPanel;
	int timesRan;
	float panelWidth;

	//rectTransforms for gameobject placement on ui
	RectTransform empirePlanetsRect;
	RectTransform empireShipsRect;

	GameObject shipImgPrefab;
	GameObject planetImgPrefab;
	GameObject planetIcon;
	GameObject shipIcon;

	//ui
	int uiTextSize = 16;
	float neededIconSize;

	public int n;

	//lists of icons
	public List<GameObject> planetIcons = new List<GameObject>();
	public List<GameObject> shipIcons = new List<GameObject>();


	void Awake(){
		//load resources (jaja)
		shipImgPrefab = Resources.Load<GameObject> ("ShipImg");
		planetImgPrefab = Resources.Load<GameObject> ("PlanetImg");

		//get ref to panels
		empirePlanetsPanel = GameObject.Find ("EmpirePlanets");
		empireShipsPanel = GameObject.Find ("EmpireShips");
		empireResourcesPanel = GameObject.Find ("EmpireResources");

		//get all rectTransforms
		empirePlanetsRect = empirePlanetsPanel.GetComponent<RectTransform>();
		empireShipsRect = empireShipsPanel.GetComponent<RectTransform> ();

		//get width to use for icon size
		neededIconSize = empirePlanetsRect.rect.height / 2f;

		//set sizes of all empire subpanels, //@@ @max i know i know, runs 3 times outside of a loop, boohoo! :P
		timesRan = 0;
		panelWidth = Screen.width / nPanels;
		PositionEmpireUI(empirePlanetsPanel);
		PositionEmpireUI(empireShipsPanel);
		PositionEmpireUI(empireResourcesPanel);
	}

	//run once to adjust ui panel sizes to screen size
	void PositionEmpireUI(GameObject panel){
		RectTransform rectT = panel.GetComponent<RectTransform>();
		rectT.sizeDelta = new Vector2(panelWidth, rectT.rect.height);
		rectT.localPosition = new Vector2 (panelWidth * timesRan - Screen.width / 2, 0);
		timesRan++;
	}


	//newer work as of 12-10-2018
	//called from empire when adding a new planet to its planets list
	public void AddToPlanetUI(Planet planet){
		//create icon
		GameObject planetIcon = Instantiate (planetImgPrefab, empirePlanetsPanel.transform.position, transform.rotation);

		//set right sprite
		Sprite planetIconSprite = Resources.Load<Sprite> ("PlanetTypes/" + planet.typeName);
		planetIcon.GetComponent<Image> ().sprite = planetIconSprite;

		//set size
		planetIcon.GetComponent<RectTransform> ().sizeDelta = new Vector2 (neededIconSize, neededIconSize);

		//set text
		Text planetText = planetIcon.GetComponentInChildren<Text>();
		planetText.text = planet.defName;
		planetText.fontSize = uiTextSize;

		//set source (for moving camera on click) on uibutton
		planetIcon.GetComponent<UIButton>().source = planet.gameObject;

		//set parent and name
		planetIcon.transform.SetParent (empirePlanetsPanel.transform);
		planetIcon.name = planet.defName;

		planetIcon.transform.localPosition = new Vector2 (neededIconSize * Empire.instance.planets.Count * 1.2f, empirePlanetsRect.rect.height - neededIconSize - planetText.fontSize / 2);
		planetIcon.transform.localRotation = new Quaternion (0,0,0,0);

		planetIcons.Add (planetIcon);
	}

	//called from empire when removing a new planet to its planets list
	public void RemoveFromPlanetUI(Planet planet){
		foreach (GameObject planetIcon in planetIcons) {
			if (planet.defName == planetIcon.name) {
				planetIcons.Remove (planetIcon);
				Destroy (planetIcon);

				break;
			}
		}
	}

	public void AddToShipUI(Ship ship){
		GameObject shipIcon = Instantiate (shipImgPrefab, empireShipsPanel.transform.position, transform.rotation);

		//set text
		Text shipText = shipIcon.GetComponentInChildren<Text>();
		shipText.text = ship.defName;
		shipText.fontSize = 16;

		//to focus camera when clicked on UI button
		shipIcon.GetComponent<UIButton>().source = ship.gameObject;

		shipIcon.transform.SetParent(empireShipsPanel.transform);

		shipIcon.transform.localPosition = new Vector2 (neededIconSize * Empire.instance.ships.Count * 1.2f, empireShipsRect.rect.height - neededIconSize);

		ship.name = "Ship " + n;
		shipIcon.name = "Ship " + n;
		n++;

		//add shipIcon to shipiconlist
		shipIcons.Add (shipIcon);
	}

	public void RemoveFromShipUI(Ship ship){
		foreach (GameObject shipIcon in shipIcons) {
			if (ship.name == shipIcon.name) {
				shipIcons.Remove (shipIcon);
				Destroy (shipIcon);

				break;
			}
		}
	}
}
