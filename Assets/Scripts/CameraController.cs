using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	//zoom vars
	public int cameraStartZoom = 20;
	public int cameraZoomMax = 100;
	public int cameraZoomMin = 3;

	//move vars
	float mainSpeed = 20; //regular speed
	float shiftAdd = 20; //multiplied by how long shift is held.  Basically running
	float maxShift = 100; //Maximum speed when holdin gshift
	private float totalRun= 1;

	//paralex scrolling background
	Transform bgTransform;
	float bgSpeed;
	Vector2 b;

	//focus variables
	float infoPanelWidth;
	float InfoPanelHeight;
	Vector2 focusCenter;
	Vector2 cameraCenter;
	Vector2 focusOffset;
	public bool isFocussed = false;

	void Awake () {
		cameraZoomMax = 100;
		cameraZoomMin = 3;

		isFocussed = false;
	}

	// Use this for initialization
	void Start () {
		Camera.main.orthographicSize = cameraStartZoom;

		bgTransform = GameObject.Find ("BackgroundController").GetComponent<Transform> ();
		bgSpeed = mainSpeed / 2;

		//for focus, just checked: is constant despite Camera.main.orthographicSize
		//infoPanelWidth = GameObject.Find("InfoWindow").GetComponent<RectTransform>().sizeDelta.x;
		if (GameObject.Find ("InfoWindow")) {
			infoPanelWidth = GameObject.Find ("InfoWindow").GetComponent<RectTransform> ().rect.width;
		}
		if (GameObject.Find ("EmpireOverview")) {
			InfoPanelHeight = GameObject.Find ("EmpireOverview").GetComponent<RectTransform> ().rect.height;
		}

		/* not working properly but used to work..
		//calculate offset based on empire UI and planet info UI
		cameraCenter = Camera.main.ScreenToWorldPoint(new Vector2 (Screen.width / 2, Screen.height / 2));
		focusCenter = Camera.main.ScreenToWorldPoint(new Vector2 ((Screen.width - infoPanelWidth) / 2, (Screen.height - InfoPanelHeight) / 2));
		focusOffset = cameraCenter - focusCenter;
		print ("screen width = " + Screen.width + ", height = " + Screen.height);
		print ("cameraCenter = " + cameraCenter + ", focusCenter = " + focusCenter + ", focusOffset = " + focusOffset);
		*/

		cameraCenter = Camera.main.ScreenToWorldPoint(new Vector2 (Screen.width / 2, Screen.height / 2));
		focusCenter = Camera.main.ScreenToWorldPoint(new Vector2 ((Screen.width - infoPanelWidth) / 2, (Screen.height - InfoPanelHeight) / 2));
		focusOffset = cameraCenter - focusCenter;
		print ("screen width = " + Screen.width + ", height = " + Screen.height);
		//print ("cameraCenter = " + cameraCenter + ", focusCenter = " + focusCenter + ", focusOffset = " + focusOffset);
	}
	
	// Update is called once per frame
	void Update () {
		//cam zoom
		CameraZoom ();

		//cam movement

			Vector2 p = GetBaseInput ();
			if (Input.GetKey (KeyCode.LeftShift)) {
				totalRun += Time.deltaTime;
				p = p * totalRun * shiftAdd;
				p.x = Mathf.Clamp (p.x, -maxShift, maxShift);
				p.y = Mathf.Clamp (p.y, -maxShift, maxShift);

				//bgTransform
				b = p;

				//also reset isfocussed state, so you can move away from a planet after closer inspection!
			} else {
				totalRun = Mathf.Clamp (totalRun * 0.5f, 1f, 1000f);

				//bgTransform
				b = p * bgSpeed;

				//camera
				p = p * mainSpeed;
			}

			//camera
			p = p * Time.deltaTime;
			transform.Translate (p);

			//bgcontroller
			b = b * Time.deltaTime;
			bgTransform.Translate (b);

		//cam focus on planet
		if (isFocussed && SelectionMaster.instance.selectedPlanets.Count == 1){
			Focus(SelectionMaster.instance.selectedObjects[0].gameObject.GetComponent<Transform>());
		}
		//print ("count = " + SelectionMaster.instance.selectedPlanets.Count);
	}

	//zoom camera, duh
	public void CameraZoom(){
		if (Input.GetAxis ("Mouse ScrollWheel") < 0 && Camera.main.orthographicSize < cameraZoomMax) {
			Camera.main.orthographicSize += 1;
		}
		if (Input.GetAxis ("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize > cameraZoomMin) {
			Camera.main.orthographicSize -= 1;
		}
	}

	//move cam up/down/left/right
	private Vector2 GetBaseInput() {
		Vector2 p_Velocity = new Vector2();
		if (Input.GetKey (KeyCode.W)){
			p_Velocity += new Vector2(0, 1);
		}
		if (Input.GetKey (KeyCode.S)){
			p_Velocity += new Vector2(0, -1);
		}
		if (Input.GetKey (KeyCode.A)){
			p_Velocity += new Vector2(-1, 0);
		}
		if (Input.GetKey (KeyCode.D)){
			p_Velocity += new Vector2(1, 0);
		}
		return p_Velocity;
	}

	//notes/bugs: 	needs to always center the camera with an offset, just havent figured out how to conver the static recttransform values of the focusoffset to actual camera position with much succes:P
	public void Focus (Transform target){
		Camera.main.orthographicSize = cameraZoomMin;
		//@@ bool isfocussed, so that this function runs every cycle.. :-x
		isFocussed = true;
		transform.position = new Vector3 (target.position.x + 1.4f, target.position.y - 0.8f, transform.position.z);
		//print ("transform = " + transform.position + ", target = " + target.position + ", transform-target = " + (transform.position - target.position) + ", focusOffset = " + focusOffset);
	}
}
