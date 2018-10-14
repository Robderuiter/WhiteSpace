using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour {
	
	public GameObject source;

	public void MoveToSource(){
		if (source.tag == "Ship") {
			//move camera
			Camera.main.transform.position = new Vector3 (source.transform.position.x, source.transform.position.y, Camera.main.transform.position.z);
			Camera.main.orthographicSize = Camera.main.GetComponent<CameraController>().cameraZoomMin;

			//add source to selectedPlanets
			SelectionMaster.instance.selectedShips.Add(source.GetComponent<Collider2D>());

			//filter the selection on selectionMaster, like normal selection
			SelectionMaster.instance.FilterPostSelection ();

			//workaround to cancel SelectionMasters ClearSelection() on GetMouseButtonUp(0)
			SelectionMaster.instance.uiButtonWasPressed = true;
		}

		if (source.tag == "Planet") {
			//start off with clearing the previous selection
			SelectionMaster.instance.ClearSelections ();

			//add source to selectedPlanets
			SelectionMaster.instance.selectedPlanets.Add(source.GetComponent<Collider2D>());

			//filter the selection on selectionMaster, like normal selection
			SelectionMaster.instance.FilterPostSelection ();

			//workaround to cancel SelectionMasters ClearSelection() on GetMouseButtonUp(0)
			SelectionMaster.instance.uiButtonWasPressed = true;

			//focus the camera, needs to be run once from the source, because i make crappy code :P
			Camera.main.GetComponent<CameraController> ().Focus(source.transform);
		}
	}
}
