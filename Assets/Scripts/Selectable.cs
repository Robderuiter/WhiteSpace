using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectableType {Ship, Planet, Module};

public class Selectable : MonoBehaviour {

	//selection
	public bool isSelected;

	//selectionBar
	GameObject selectionBarPrefab;
	GameObject selectionBar;
	float newBarSize;

	//infowindow
	GameObject infoWindow;
	public InfoWindow infoW;

	public SelectableType selectableType;

	//canvas ref
	GameObject canv;

	void Awake () {
		isSelected = false;

		selectionBarPrefab = Resources.Load<GameObject> ("SelectionBar");

		//check which kind of gameobject this is and set type
		if (gameObject.tag == "Ship") {
			selectableType = SelectableType.Ship;
		}
		if (gameObject.tag == "Planet") {
			selectableType = SelectableType.Planet;
		}
		if (gameObject.tag == "Module") {
			selectableType = SelectableType.Module;
		}
	}

	void Start (){
		canv = GameObject.Find ("Canvas");

		//get infowindow stuff
		infoWindow = GameObject.Find ("InfoWindow");
		infoW = infoWindow.GetComponent<InfoWindow>();

		//sets inactive after first selectable that runs start, nullref after:P
		//infoWindow.SetActive (false);
	}

	public void Select(){
		isSelected = true;

		//create and add a selectionbar (duh!)
		CreateSelectionBar ();

		//set infowindow to active(duh!)
		ShowInfoWindow ();

		print ("Select has run");

	}

	public void Deselect(){
		isSelected = false;

		//remove selection floating box (hp bar) thingamajig
		DestroySelectionStuff();
	}


	//dit moet ik niet hier doen, maar in selecitonmaster, dan is het makkelijker bijhouden welke al een selectionbar heeft en kan ik de list meteen leeggooien bij new select
	void CreateSelectionBar () {
		//create selection floating box (hp bar) thingamajig
		selectionBar = (GameObject)Instantiate(selectionBarPrefab, transform.position, new Quaternion(0,0,0,0));

		//set parent, change local pos to 0 and (not correct yet) set rotation to 0 (world)
		selectionBar.transform.parent = transform;
		selectionBar.transform.localPosition = new Vector2(0,0);
		//selectionBar.transform.localRotation = new Quaternion (0,0,0,0);

		//determine parent size and scale accordingly 
		if (gameObject.tag == "Planet") {
			newBarSize = (gameObject.GetComponent<SpriteRenderer>().size.x * gameObject.transform.localScale.x * Mathf.PI) / (selectionBar.GetComponent<SpriteRenderer> ().size.x * gameObject.GetComponent<Planet>().planetSize);
		}
		if (gameObject.tag == "Ship") {
			newBarSize = (gameObject.GetComponent<SpriteRenderer> ().size.x * gameObject.transform.localScale.x) / (selectionBar.GetComponent<SpriteRenderer> ().size.x * selectionBar.transform.localScale.x);
		}

		//actually set spriterenderer and localscale to newBarSize
		selectionBar.GetComponent<SpriteRenderer> ().size = new Vector2 (newBarSize,newBarSize);
		selectionBar.transform.localScale = new Vector2 (newBarSize,newBarSize);
	}

	//zie comment boven createSelectionBar
	void ShowInfoWindow(){
		//infoWindow.SetActive (true);

		infoW.selectedObject = gameObject;

		infoW.UpdateFloatingInfo ();

		print ("showInfoWindow has run");

	}

	//zie comment boven createSelectionBar
	void HideInfoWindow(){
		infoWindow.SetActive (false);
	}

	//zie comment boven createSelectionBar
	void DestroySelectionStuff(){
		//Destroy (infoWindowGO);
		Destroy (selectionBar);
		infoW = null;
	}
}
