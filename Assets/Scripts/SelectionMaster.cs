using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionMaster : MonoBehaviour {

	//## = commented out because of selectionmaster rewrite, dd21-09-2018

	public static SelectionMaster instance;

	public List<Selectable> selectedObjects = new List<Selectable>();
	public List<Selectable> selectedShips = new List<Selectable>();
	public List<Selectable> selectedPlanets = new List<Selectable>();
	public List<Selectable> selectedModules = new List<Selectable>();
	public List<Selectable> selectedWithRMB = new List<Selectable>();

	//group selection
	Vector2 selectionStartPos;
	Vector2 invertedStartPos;
	Vector2 selectionEndPos;
	bool isSelecting;
	Texture2D selectionBoxTexture;
	Rect selectionRect = new Rect(-10000,-10000,0,0);
	GameObject selectionColliderPrefab;
	GameObject selectionBox;
	//Vector2 selectionStartPosWorld;
	//Vector2 selectionEndPosWorld;
	//Vector2 selectionStartPosForWorld;
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

		camController.Focus(GameObject.Find ("Sun").GetComponent<Transform>());
		//print (GameObject.Find ("Sun").GetComponent<Transform>());
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
			//SetSelectionBox();
		} //else if(selectionBox.activeSelf && isLeftClick){
		else if(selectBox.isActiveAndEnabled && isLeftClick){
			//if there are any selectable in selectedsubtypes, filter, then select them
			if (selectedShips.Count > 0 || selectedPlanets.Count > 0 || selectedModules.Count > 0) {
				//## FilterSelection ();
				print ("something .count > 0");
			}

			//turn off the selectionBox
			selectBox.enabled = false;

			isLeftClick = false;
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

	/*
	void SetSelectionBox(){
		selectBox.enabled = true;

		//get stuff to calculate with
		selectionStartPosWorld = Camera.main.ScreenToWorldPoint (selectionStartPosForWorld);
		selectionEndPosWorld = Camera.main.ScreenToWorldPoint (selectionEndPos);
		selectionCenter = new Vector2 ((selectionEndPosWorld.x + selectionStartPosWorld.x) / 2, (selectionEndPosWorld.y + selectionStartPosWorld.y) / 2);

		//check for single click, else do "normal" group selection
		if (isSingleClick){
			selectionWidth = 1;
			selectionHeight = 1;
		} else {
			selectionWidth = Mathf.Abs (selectionEndPosWorld.x - selectionStartPosWorld.x);
			selectionHeight = Mathf.Abs (selectionEndPosWorld.y - selectionStartPosWorld.y);
			//print ("selectionWidth = " + selectionWidth + ", selectionHeight = " + selectionHeight);
		}

		//set center and size
		selectBox.transform.position = selectionCenter;
		selectBox.size = new Vector2 (selectionWidth, selectionHeight);
	}
	*/

	/* //## needs to be rewritten to be included on the SelectionMaster gameObject itself
	//activate, work some magic (:P) if singleclick and then set the size for the selectionbox
	void SetSelectionBox(){
		selectionBox.SetActive (true);

		//get stuff to calculate with
		selectionStartPosWorld = Camera.main.ScreenToWorldPoint (selectionStartPosForWorld);
		selectionEndPosWorld = Camera.main.ScreenToWorldPoint (selectionEndPos);
		selectionCenter = new Vector2 ((selectionEndPosWorld.x + selectionStartPosWorld.x) / 2, (selectionEndPosWorld.y + selectionStartPosWorld.y) / 2);

		//check for single click, else do "normal" group selection
		if (isSingleClick){
			selectionWidth = 1;
			selectionHeight = 1;
		} else {
			selectionWidth = Mathf.Abs (selectionEndPosWorld.x - selectionStartPosWorld.x);
			selectionHeight = Mathf.Abs (selectionEndPosWorld.y - selectionStartPosWorld.y);
			//print ("selectionWidth = " + selectionWidth + ", selectionHeight = " + selectionHeight);
		}

		//set center and size
		selectionBox.transform.position = selectionCenter;
		selectionBox.GetComponent<BoxCollider2D> ().size = new Vector2 (selectionWidth, selectionHeight);
	}
	*/

	/*
	//@@ hier lijkt iets fout te gaan, selectedType print is altijd ship, ook als je ctrl/shift drukt (en deze functie nooit zou moeten runnen)
	//filter the currently selectedobjects, ship > planet > module unless ctrl/shift are pressed (which are pre-filtered in SelectionBoxCollider)
	public void FilterSelection(){
		if (selectedShips.Count > 0) {
			SelectSelectedObjects(selectedShips);
			selectedType = SelectableType.Ship;
			print ("selectedType = " + selectedType);
		} else if (selectedModules.Count > 0 && selectedShips.Count < 1) {
			SelectSelectedObjects(selectedModules);
			selectedType = SelectableType.Module;
			print ("selectedType = " + selectedType);
		} else if (selectedPlanets.Count > 0) {
			//only runs once.. xD
			//camController.Focus (selectedPlanets[0].GetComponent<Transform>());

			camController.isFocussed = true;
		}
	}

	//clears the temporary lists used for selection filtering
	void ClearFilteredSelection(){
		selectedShips.Clear ();
		selectedPlanets.Clear ();
		selectedModules.Clear ();
	}
	*/

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
