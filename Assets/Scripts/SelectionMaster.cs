using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	Texture2D selectionBoxTexture;
	Rect selectionRect = new Rect(-10000,-10000,0,0);
	//GameObject selectionBox;
	Vector2 selectionStartPosWorld;
	Vector2 selectionEndPosWorld;
	Vector2 selectionStartPosForWorld;
	float selectionWidth;
	float selectionHeight;
	Vector2 selectionCenter;
	public SelectableType selectedType;
	public SelectableType selectableType;
	public bool isLeftClick = false;

	//new box collider on selectionmaster itself
	BoxCollider2D selectBox; 

	//single selection timer
	float timeMouseDown;
	float timeMouseUp;
	float singleClickTime = 0.15f;
	bool isSingleClick;

	//Commands / right mouse button
	Ship selectableShip;
	public bool isRightClick = false;

	CameraController camController;

	void Awake(){
		//create selectionBox texture
		selectionBoxTexture = new Texture2D(1,1);
		selectionBoxTexture.SetPixel (0,0,new Color(0,0,0,0.1f));
		selectionBoxTexture.Apply ();

		//set static (global) instance
		instance = this;

		selectBox = gameObject.GetComponent<BoxCollider2D> ();
		selectBox.enabled = false;
	}

	//purely for testing camera focus
	void Start(){
		camController = Camera.main.GetComponent<CameraController> ();
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

			isLeftClick = true;

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
			//print ("selectBox.enabled = " + selectBox.enabled);
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
			selectionRect.x = Mathf.Min(invertedStartPos.x,selectionEndPos.x) ;
			selectionRect.y = Mathf.Min(invertedStartPos.y, Screen.height - selectionEndPos.y) ;
			selectionRect.width = Mathf.Abs(invertedStartPos.x-selectionEndPos.x) ;
			selectionRect.height = Mathf.Abs(Screen.height - invertedStartPos.y - selectionEndPos.y);

			//instantiate prefab gameobject with collider + image
			GUI.Box (selectionRect,selectionBoxTexture);
		}
	}

	//this is where the selection magic happens: in Update() we flick the colliders on/off, the moment it turns on, we add collided objects to seperate lists:
	void OnTriggerEnter2D (Collider2D otherCol){
		//print ("ontriggerenter otherCol.gameObject = " + otherCol.gameObject);
		if (otherCol.gameObject.tag == "Ship") {
			selectedShips.Add (otherCol);
			//print ("selectedShips.count = " + selectedShips.Count + ", added " + otherCol.gameObject);
		}

		if (otherCol.gameObject.tag == "Planet") {
			selectedPlanets.Add (otherCol);
			//print ("selectedPlanets.count = " + selectedPlanets.Count + ", added " + otherCol.gameObject);
		}

		if (otherCol.gameObject.tag == "Module"){
			selectedModules.Add (otherCol);
			//print ("selectedModules.count = " + selectedModules.Count + ", added " + otherCol.gameObject);
		}
	}

	//calculate size of the selection box based on mouse input
	void SetSelectionBox(){
		selectBox.enabled = true;
		//print ("selectBox.enabled = " + selectBox.enabled);

		//convert screen to world point, calculate box center
		selectionStartPosWorld = Camera.main.ScreenToWorldPoint (selectionStartPos);
		selectionEndPosWorld = Camera.main.ScreenToWorldPoint (selectionEndPos);
		selectionCenter = new Vector2 ((selectionEndPosWorld.x + selectionStartPosWorld.x) / 2, (selectionEndPosWorld.y + selectionStartPosWorld.y) / 2);

		//set box width and height, check for single click, else do "normal" group selection
		if (isSingleClick){
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
	}
		
	//clear all the selection lists for LMB
	void ClearSelections(){
		selectedObjects.Clear ();
		selectedPlanets.Clear ();
		selectedShips.Clear ();
		selectedModules.Clear ();

		print ("selected objects, planets, ships and modules cleared");
	}

	//filter selection to selectedObjects list based on number of selectedStuff in each subcategory (ship, planet, module)
	void FilterPostSelection(){
		//always prioritize selecting ships
		if (selectedShips.Count > 0){
			selectedObjects = selectedShips;
		}
		else if (selectedPlanets.Count > 0){
			selectedObjects = selectedPlanets;

			if (selectedObjects.Count == 1) {
				//crude?
				camController.isFocussed = true;
				camController.Focus (selectedObjects[0].GetComponent<Transform>());
			}
		}
		else if (selectedModules.Count > 0){
			selectedObjects = selectedModules;
		}
		print ("selectedObjects = " + selectedObjects + ", .count = " + selectedObjects.Count);
	}

	/*
	//run the Select function on each selected object, creating a "selection box" and an infowindow
	void SelectSelectedObjects(List<Selectable> selected){
		selectedObjects = selected;
		foreach (Selectable selectable in selectedObjects){
			selectable.Select ();
		}
	}

	//removes selection box and infowindow from each selected object
	void DeselectSelectedObjects(){
		foreach (Selectable selectable in selectedObjects) {
			selectable.Deselect ();
		}
		selectedObjects.Clear ();
	}
	*/
}
