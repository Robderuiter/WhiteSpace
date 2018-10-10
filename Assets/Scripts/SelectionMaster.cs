using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectionMaster : MonoBehaviour {

	//## = commented out because of selectionmaster rewrite, dd21-09-2018

	public static SelectionMaster instance;

	public List<Collider2D> selectedObjects = new List<Collider2D>();
	public List<Collider2D> selectedShips = new List<Collider2D>();
	public List<Collider2D> selectedPlanets = new List<Collider2D>();
	public List<Collider2D> selectedModules = new List<Collider2D>();
	public List<Collider2D> selectedWithRMB = new List<Collider2D>();

	//group selection
	Vector2 selectionStartPos;
	Vector2 invertedStartPos;
	Vector2 selectionEndPos;
	bool isSelecting;
	public bool isLeftClick;

	//selection box
	Texture2D selectionBoxTexture;
	Rect selectionRect = new Rect(-10000,-10000,0,0);
	BoxCollider2D selectBox; 

	//single selection timer
	float timeMouseDown;
	float timeMouseUp;
	float singleClickTime = 0.15f;
	bool isSingleClick;

	//Commands / right mouse button
	Ship selectableShip;
	public bool isRightClick;

	//camera
	CameraController camController;

	//ui
	GameObject infoWindowGO;
	//InfoWindow infoWindow;

	void Awake(){
		//create selectionBox texture
		selectionBoxTexture = new Texture2D(1,1);
		selectionBoxTexture.SetPixel (0,0,new Color(0,0,0,0.1f));
		selectionBoxTexture.Apply ();

		//set static (global) instance
		instance = this;

		selectBox = gameObject.GetComponent<BoxCollider2D> ();
		selectBox.enabled = false;

		//
		isRightClick = false;
		isLeftClick = false;
		isSingleClick = false;
	}

	//purely for testing camera focus
	void Start(){
		camController = Camera.main.GetComponent<CameraController> ();

		//find, then turn off the infowindow
		infoWindowGO = GameObject.Find ("InfoWindow");
		//infoWindow = infoWindowGO.GetComponent<InfoWindow> ();
		infoWindowGO.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		//draw group selection box on left mousebuttondown
		if (Input.GetMouseButtonDown(0)){
			//"save" time + start pos to check for single click
			timeMouseDown = Time.time;

			selectionStartPos = Input.mousePosition;
			invertedStartPos = new Vector2(selectionStartPos.x, Screen.height - selectionStartPos.y);

			isSelecting = true;
		}

		//start selecting stuff on left mousebuttonup
		if (Input.GetMouseButtonUp(0)){
			ClearSelections ();
			infoWindowGO.SetActive (false);
			camController.isFocussed = false;

			isLeftClick = true;
			isRightClick = false;

			//stop drawing gui box
			isSelecting = false;

			//default isSingleClick to false
			isSingleClick = false;

			//set isSingleClick to true if time between mouseclicks was small enough
			timeMouseUp = Time.time;
			if (timeMouseUp - timeMouseDown < singleClickTime) {
				isSingleClick = true;
			}

			//gonna need this to draw the selectionbox
			selectionEndPos = Input.mousePosition;

			//turn on, center and resize the selection boxcollider
			SetSelectionBox();
		} 
		//this is where you turn off the selectionbox, and so where postselectionfiltering should apply
		else if(selectBox.isActiveAndEnabled && isLeftClick){
			//if there are any selectable in selectedsubtypes, filter, then select them
			if (selectedShips.Count > 0 || selectedPlanets.Count > 0 || selectedModules.Count > 0) {
				FilterPostSelection ();
			}

			//turn off the selectionBox
			selectBox.enabled = false;
			isLeftClick = false;
			isRightClick = false;
		}

		if (Input.GetMouseButtonDown (1)) {
			selectionStartPos = Input.mousePosition;
			invertedStartPos = new Vector2(selectionStartPos.x, Screen.height - selectionStartPos.y);
		}

		//if right mouse button, do commands depending on current selection
		if (Input.GetMouseButtonUp (1)) {
			//selectedWithRMB.Clear ();

			//used for moving ships
			Vector2 RMBtarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			//stuff for modules: selecting target to attach to with rmb
			isLeftClick = false;
			isRightClick = true;
			isSingleClick = true;
			selectionEndPos = Input.mousePosition;


			SetSelectionBox ();

			//if any ship is selected, move to location
			if (selectedShips.Count > 0) {
				//go through all selected objects
				foreach (Collider2D col in selectedObjects) {
					print (RMBtarget);
					//actually move the ship
					//col.gameObject.GetComponent<Ship>().RotateShip (RMBtarget);
					//@@ doing 2 calls now, should just be 1..
					col.gameObject.GetComponent<Ship> ().target = RMBtarget;
					col.gameObject.GetComponent<Ship> ().hasToMove = true;

					//Vector2.Lerp (col.gameObject.GetComponent<Transform> ().position, RMBtarget, 10f);
				}
			} //if module and when clicked on a planet/ship, attachmodule
			else if (selectedModules.Count > 0 && selectedWithRMB.Count > 0) {
				

			} //if module or planet and NOT clicked on a planet/ship, clear selection
			else if (selectedPlanets.Count > 0) {
				//deselect on rmb anywhere
				ClearSelections();
			}
		}


		/*
		//if right mouse button, command
		if (Input.GetMouseButtonUp (1)) {
			//probeersel camera focus on planet
			camController.isFocussed = false;

			//needed for SelectionBoxCollider class
			isLeftClick = false;
			isRightClick = true;


			//needed for SetSelectionBox()
			isSingleClick = true;
			//##selectionStartPosForWorld = Input.mousePosition;
			selectionEndPos = Input.mousePosition;

			//SetSelectionBox ();
		} else if (isRightClick && selectedWithRMB.Count > 0) {

		}
		*/
	}

	//drawing the box visually on screen
	void OnGUI(){
		//set GUI box color
		GUI.skin.box.normal.background = selectionBoxTexture;

		//until mousebuttonup, draw box based on mousepos, uses inverted mousePos because gui and screen use different corners for (0,0), for some reason...
		if (isSelecting && Time.time - timeMouseDown > singleClickTime) {
			selectionEndPos = Input.mousePosition;
			selectionRect.x = Mathf.Min (invertedStartPos.x, selectionEndPos.x);
			selectionRect.y = Mathf.Min (invertedStartPos.y, Screen.height - selectionEndPos.y);
			selectionRect.width = Mathf.Abs (invertedStartPos.x - selectionEndPos.x);
			selectionRect.height = Mathf.Abs (Screen.height - invertedStartPos.y - selectionEndPos.y);

			//instantiate prefab gameobject with collider + image
			GUI.Box (selectionRect, selectionBoxTexture);
		} 
	}

	//this is where the selection magic happens: in Update() we flick the colliders on/off, the moment it turns on, we add collided objects to seperate lists:
	void OnTriggerEnter2D (Collider2D otherCol){
		//print ("ontriggerenter otherCol.gameObject = " + otherCol.gameObject);
		if (isLeftClick) {
			if (otherCol.gameObject.tag == "Ship") {
				selectedShips.Add (otherCol);
				//print ("selectedShips.count = " + selectedShips.Count + ", added " + otherCol.gameObject);
			}

			if (otherCol.gameObject.tag == "Planet") {
				selectedPlanets.Add (otherCol);
				//print ("selectedPlanets.count = " + selectedPlanets.Count + ", added " + otherCol.gameObject);
			}

			if (otherCol.gameObject.tag == "Module") {
				selectedModules.Add (otherCol);
				//print ("selectedModules.count = " + selectedModules.Count + ", added " + otherCol.gameObject);
			}
		} 

		//if rightclick
		if (isRightClick) {
			//if module has been selected
			if (selectedModules.Count > 0 && selectedShips.Count == 0 && selectedPlanets.Count == 0) {
				//if clicked on another ship/planet, attach module
				if (otherCol.gameObject.tag == "Ship" || otherCol.gameObject.tag == "Planet") {
					selectedWithRMB.Add (otherCol);
					isRightClick = false;
					isSingleClick = false;
					selectBox.enabled = false;

					//@@ enable the following and it should work
					otherCol.gameObject.GetComponent<Module> ().AttachModule ();
				}
			}
		}
	}

	//calculate size of the selection box based on mouse input
	void SetSelectionBox(){
		float selectionWidth;
		float selectionHeight;

		selectBox.enabled = true;

		//convert screen to world point, calculate box center
		Vector2 selectionStartPosWorld = Camera.main.ScreenToWorldPoint (selectionStartPos);
		Vector2 selectionEndPosWorld = Camera.main.ScreenToWorldPoint (selectionEndPos);
		Vector2 selectionCenter = new Vector2 ((selectionEndPosWorld.x + selectionStartPosWorld.x) / 2, (selectionEndPosWorld.y + selectionStartPosWorld.y) / 2);

		//set box width and height, check for single click, else do "normal" group selection
		if (isSingleClick) {
			selectionWidth = 1;
			selectionHeight = 1;
		} else {
			selectionWidth = selectionEndPosWorld.x - selectionStartPosWorld.x;
			selectionHeight = selectionEndPosWorld.y - selectionStartPosWorld.y;
		}
		//print ("selectionWidth = " + selectionWidth + ", selectionHeight = " + selectionHeight);

		//set center and size
		selectBox.transform.position = selectionCenter;
		selectBox.size = new Vector2 (Mathf.Abs(selectionWidth), Mathf.Abs(selectionHeight));

		//print ("selectStartPos = " + selectionStartPos + ", selectionStartPosWorld = " + selectionStartPosWorld + ", selectbox transform.pos = " + selectBox.transform.position + ", size = " + selectBox.size + ", enabled = " + selectBox.enabled);
	}
		
	//clear all the selection lists
	void ClearSelections(){
		selectedObjects.Clear ();
		selectedPlanets.Clear ();
		selectedShips.Clear ();
		selectedModules.Clear ();
		selectedWithRMB.Clear ();

		//print ("selected objects, planets, ships and modules cleared");
	}

	//filter selection to selectedObjects list based on number of selectedStuff in each subcategory (ship, planet, module)
	void FilterPostSelection(){
		//always prioritize selecting ships
		if (selectedShips.Count > 0){
			selectedObjects = selectedShips;
		}
		//otherwise prefer selecting ships
		else if (selectedPlanets.Count > 0){
			selectedObjects = selectedPlanets;

			if (selectedObjects.Count == 1) {
				infoWindowGO.SetActive (true);
				camController.isFocussed = true;
			}
		}
		//if nothing else is in the selection, select modules
		else if (selectedModules.Count > 0){
			selectedObjects = selectedModules;
		}
		//print ("selectedObjects = " + selectedObjects + ", .count = " + selectedObjects.Count);
	}
}
