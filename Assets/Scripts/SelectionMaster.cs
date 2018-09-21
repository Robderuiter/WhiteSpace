using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionMaster : MonoBehaviour {

	//this is where a large part of the selection is done, another important part is the SelectionBoxCollider (for use of OnTriggerEnter2D) and Selectable which is on every selectable gameobject

	//## = commented out because of selectionmaster rewrite, dd21-09-2018

	public static SelectionMaster instance;

	public List<Selectable> selectedObjects = new List<Selectable>();
	public List<Selectable> selectedShips = new List<Selectable>();
	public List<Selectable> selectedPlanets = new List<Selectable>();
	public List<Selectable> selectedModules = new List<Selectable>();
	public List<Selectable> selectedWithRMB = new List<Selectable>();

	//group selection
	Vector2 selectionStartPos;
	Vector2 selectionEndPos;
	bool isSelecting;
	Texture2D selectionBoxTexture;
	Rect selectionRect = new Rect(-10000,-10000,0,0);
	GameObject selectionColliderPrefab;
	GameObject selectionBox;
	Vector2 selectionStartPosWorld;
	Vector2 selectionEndPosWorld;
	//Vector2 selectionStartPosForWorld;
	float selectionWidth;
	float selectionHeight;
	Vector2 selectionCenter;
	public SelectableType selectedType;
	public SelectableType selectableType;
	public bool isLeftClick = false;

	//single selection timer
	float timeMouseDown;
	float timeMouseUp;
	float singleClickTime = 0.15f;
	bool isSingleClick;

	//Commands / right mouse button
	//Vector2 mousePos;
	Ship selectableShip;
	public bool isRightClick = false;
	//Module selectableModule;

	CameraController camController;

	void Awake(){
		//create selectionBox texture
		selectionBoxTexture = new Texture2D(1,1);
		selectionBoxTexture.SetPixel (0,0,new Color(0,0,0,0.1f));
		selectionBoxTexture.Apply ();

		//create selectionCollider
		selectionColliderPrefab = (GameObject)Resources.Load("SelectionCollider");
		selectionBox = Instantiate(selectionColliderPrefab, new Vector2(10000,10000), transform.rotation);
		//for some auspicious, unknown, mysteeerriiiiooouuuusss reason, it doesn't instantiate at the given new vector2, therefore move it after spawning:
		selectionBox.transform.position = new Vector2(-10000,-10000);
		selectionBox.SetActive (false);

		//set static (global) instance
		instance = this;
	}

	//purely for testing camera focus
	void Start(){
		camController = Camera.main.GetComponent<CameraController> ();

		//Camera.main.GetComponent<CameraController> ().Focus (GameObject.Find("FocusTest").GetComponent<Transform>());

		//print ("focustest transform = " + GameObject.Find("FocusTest").GetComponent<Transform>() + ", camcontroller = " + Camera.main.GetComponent<CameraController> ());
	}
	
	// Update is called once per frame
	void Update () {
		//draw group selection box on left mousebuttondown
		if (Input.GetMouseButtonDown(0)){
			//"save" time to check for single click
			timeMouseDown = Time.time;

			selectionStartPos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
			//selectionStartPosForWorld = Input.mousePosition;
			isSelecting = true;
		}

		//start selecting stuff on left mousebuttonup
		if (Input.GetMouseButtonUp(0)){
			//deselect all objects and clear the lists
			//##	DeselectSelectedObjects();
			//##	ClearFilteredSelection();

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
		//##	SetSelectionBox();
		} else if(selectionBox.activeSelf && isLeftClick){
			//if there are any selectable in selectedsubtypes, filter, then select them
			if (selectedShips.Count > 0 || selectedPlanets.Count > 0 || selectedModules.Count > 0) {
				//## FilterSelection ();
			}

			//turn off the selectionBox
			selectionBox.SetActive (false);

			isLeftClick = false;
		}

		//if right mouse button
		if (Input.GetMouseButtonUp (1)) {
			//probeersel camera focus on planet
			camController.isFocussed = false;

			//need to check which object is hit here as well..
			//mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			//needed for SelectionBoxCollider class
			isRightClick = true;

			//needed for SetSelectionBox()
			isSingleClick = true;
			//selectionStartPosForWorld = Input.mousePosition;
			selectionEndPos = Input.mousePosition;

		 //##	SetSelectionBox ();
		} else if (isRightClick && selectedWithRMB.Count > 0) {

		}
	}

	void OnGUI(){
		//set GUI box color
		GUI.skin.box.normal.background = selectionBoxTexture;

		//until mousebuttonup, draw box based on mousepos
		if (isSelecting && Time.time - timeMouseDown > singleClickTime) {
			selectionEndPos = Input.mousePosition;
			selectionRect.x = Mathf.Min(selectionStartPos.x,selectionEndPos.x) ;
			selectionRect.y = Mathf.Min(selectionStartPos.y, Screen.height - selectionEndPos.y) ;
			selectionRect.width = Mathf.Abs(selectionStartPos.x-selectionEndPos.x) ;
			selectionRect.height = Mathf.Abs(Screen.height - selectionStartPos.y - selectionEndPos.y);

			//instantiate prefab gameobject with collider + image
			GUI.Box (selectionRect,selectionBoxTexture);
		}
	}

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
