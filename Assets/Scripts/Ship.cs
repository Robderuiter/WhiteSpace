using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

	//control stuff
	float flightSpeed = 0.5f;
	float rotateSpeed = 3f;
	public Vector2 target;
	Vector2 startPos;
	float angleTreshold = 10f;
	public bool doneRotating;
	public bool doneMoving;
	SpriteRenderer scanner;
	CircleCollider2D scannerCol;

	//audio
	AudioSource scannerAudio;
	AudioSource engineAudio;

	//module slots, should become a ModuleSlots Class component added to planet and ship gameobjects
	public int nModuleSlots = 6;
	public GameObject[] modules;
	GameObject modulePrefab;
	public float modSpriteSize;
	public float modDistanceBuffer;

	//empire overview
	Empire empire;
	public bool isInEmpire = false;

	//resources
	public CurrentResources currentRes;

	// Use this for initialization
	void Start () {
		scanner = GameObject.Find("Scanner").GetComponent<SpriteRenderer> ();
		scannerCol = GameObject.Find("Scanner").GetComponent<CircleCollider2D> ();

		//audio files
		scannerAudio = GameObject.Find("Scanner").GetComponent<AudioSource> ();
		engineAudio = GetComponent<AudioSource> ();

		//module slots
		modulePrefab = (GameObject)Resources.Load("Module Slot");
		modSpriteSize = modulePrefab.GetComponent<SpriteRenderer> ().size.x * transform.localScale.x;
		modDistanceBuffer = modSpriteSize / 4;
		//print ("modSpriteSize = " + modSpriteSize + ", modDistanceBuffer = " + modDistanceBuffer);
		modules = new GameObject[nModuleSlots];

		//get currentResources
		currentRes = GetComponent<CurrentResources>();

		//get empire ref
		empire = GameObject.Find("Empire").GetComponent<Empire>();

		//add ship to empire's ship list
		empire.ships.Add(this);
	}

	// Update is called once per frame
	void Update () {
		//enable scanner sprite if space is pressed
		if (Input.GetButton ("Space")) {
			scanner.enabled = true;
			scannerCol.enabled = true;
			if (!scannerAudio.isPlaying) {
				//scannerAudio.Play ();
			}
		} else {
			scanner.enabled = false;
			scannerCol.enabled = false;
		}

		//actually rotate and then move to target
		RotateShip (target);
		if (doneRotating == true) {
			MoveShip (target);
		}


		/*
		//just to check if OnDestroy works, Fire3 = left shift
		if (Input.GetButton("Fire3")){
			//empire.ships.Remove(this);
			empire.empireUIController.UpdateEmpireUI ();
			//Destroy (this.gameObject);
		}
		*/
	}

	public void RotateShip (Vector2 targetPos){
		//calc vectortotarget, angle and then lerp to desired angle
		Vector2 vectorToTarget = targetPos - (Vector2)transform.position;
		float angle = Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
		Quaternion qt = Quaternion.AngleAxis (angle, Vector3.forward);
		transform.rotation = Quaternion.Lerp (transform.rotation, qt, rotateSpeed * Time.deltaTime);

		//wait for rotation to be almost finished
		if (Vector2.Angle (transform.right, targetPos - (Vector2)transform.position) < angleTreshold) {
			doneRotating = true;
		} else {
			doneRotating = false;
		}
	}

	public void MoveShip (Vector2 targetPos){
		transform.position = Vector2.Lerp (transform.position, targetPos, flightSpeed * Time.deltaTime);
		if (!engineAudio.isPlaying && Vector2.Distance(transform.position,targetPos) > 5) {
			//engineAudio.Play ();
		}
	}

	//deprecated, not gonna be using slot objects anymore, after thinking about it it never made any sense to have those :P
	void CreateModuleSlots (GameObject prefab, int nSlots){
		for (int y = 0; y < nSlots; y++) {
			modules[y] = Instantiate (prefab,transform.position,transform.rotation);
			modules[y].transform.parent = transform;
			modules[y].transform.localPosition = new Vector2(- modSpriteSize - modDistanceBuffer - y * (modSpriteSize + modDistanceBuffer),0);
		}
	}

	void OnCollisionEnter2D(Collision2D otherCol){
		//print ("this = " + this + ", otherCol = " + otherCol.gameObject);
	}

	void OnDestroy(){
		//empire.empireUIController.shipCount--;
		empire.ships.Remove (this);
		empire.empireUIController.UpdateEmpireUI ();
	}
}
