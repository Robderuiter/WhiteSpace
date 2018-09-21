using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	//zoom vars
	public int cameraCurrentZoom = 20;
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
	float empireInfoPanelHeight;
	//float cameraHeight;
	Vector2 focusCenter;
	Vector2 cameraCenter;
	Vector2 focusOffset;
	public bool isFocussed = false;

	void Awake () {
		//cameraHeight = -10;
	}

	// Use this for initialization
	void Start () {
		Camera.main.orthographicSize = cameraCurrentZoom;

		bgTransform = GameObject.Find ("BackgroundController").GetComponent<Transform> ();
		bgSpeed = mainSpeed / 2;

		//for focus, just checked: is constant despite Camera.main.orthographicSize
		infoPanelWidth = GameObject.Find("InfoWindow").GetComponent<RectTransform>().sizeDelta.x;
		empireInfoPanelHeight = GameObject.Find("EmpireOverview").GetComponent<RectTransform>().sizeDelta.y;

		//calculate offset based on empire UI and planet info UI
		cameraCenter = Camera.main.ScreenToWorldPoint(new Vector2 (Screen.width / 2, Screen.height / 2));
		focusCenter = Camera.main.ScreenToWorldPoint(new Vector2 ((Screen.width - infoPanelWidth) / 2, (Screen.height - empireInfoPanelHeight) / 2));
		focusOffset = cameraCenter - focusCenter;
		//print ("cameraCenter = " + cameraCenter + ", focusCenter = " + focusCenter + ", focusOffset = " + focusOffset);
	}
	
	// Update is called once per frame
	void Update () {
		CameraZoom ();

		Vector2 p = GetBaseInput();
		if (Input.GetKey (KeyCode.LeftShift)){
			totalRun += Time.deltaTime;
			p  = p * totalRun * shiftAdd;
			p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
			p.y = Mathf.Clamp(p.y, -maxShift, maxShift);

			//bgTransform
			b = p;

			//also reset isfocussed state, so you can move away from a planet after closer inspection!
		}
		else{
			totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);

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

		//probeersel cam focus op planet
		if (isFocussed) {
			Focus(SelectionMaster.instance.selectedPlanets[0].GetComponent<Transform>());
		}
	}

	public void CameraZoom(){
		if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			if (cameraCurrentZoom < cameraZoomMax) {
				cameraCurrentZoom += 1;
				Camera.main.orthographicSize = Mathf.Max (Camera.main.orthographicSize + 1);
			}

		}
		if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			if (cameraCurrentZoom > cameraZoomMin) {
				cameraCurrentZoom -= 1;
				Camera.main.orthographicSize = Mathf.Min (Camera.main.orthographicSize - 1);
			}

		}
	}

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

	// most likely called from selectionmaster in case of a planet selected :P
	public void Focus (Transform target){
		//set zoom level
		Camera.main.orthographicSize = 2.5f;
	
		//actually move the camera
		transform.Translate(new Vector3(target.position.x - transform.position.x + focusOffset.x, target.position.y - transform.position.y - focusOffset.y,0));
	}
}
