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
	public bool hasToMove;
	SpriteRenderer scanner;
	CircleCollider2D scannerCol;

	//audio
	AudioSource scannerAudio;
	AudioSource engineAudio;

	//empire overview
	public bool isInEmpire = false;

	//resources
	public CurrentResources currentRes;

	//module slots
	public ModuleSlots modSlots;
	float moduleSpineLength; //how much space there is for placing modules
	float usableSpineLengthPercent;
	public int nModulesAttached;

	//infowindow stuff
	public string defName;
	public string type;
	public Sprite flavorSprite;

	void Awake(){
		defName = "SS Hammer";
		type = "Cargo Ship";
		flavorSprite = Resources.Load<Sprite> ("FlavorSprites/shipflavor");
	}

	// Use this for initialization
	void Start () {
		//scanner = GameObject.Find("Scanner").GetComponent<SpriteRenderer> ();
		//scannerCol = GameObject.Find("Scanner").GetComponent<CircleCollider2D> ();

		//audio files
		//scannerAudio = GameObject.Find("Scanner").GetComponent<AudioSource> ();
		engineAudio = GetComponent<AudioSource> ();


		//get currentResources
		currentRes = GetComponent<CurrentResources>();

		//add module slots, arg 1 = Ship, arg 2 = Planet
		modSlots = new ModuleSlots (this.gameObject);
		moduleSpineLength = 10f;
		usableSpineLengthPercent = 1f;

		modSlots.CalcModuleSlots (moduleSpineLength, usableSpineLengthPercent);

		//add ship to empire's ship list
		Empire.instance.AddShip(this);
	}

	// Update is called once per frame
	void Update () {
		/*
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
		*/

		if (Input.GetButton ("Space")) {
			Destroy (this.gameObject);
		}
			
		//actually rotate and then move to target
		if (hasToMove) {
			RotateShip (target);
			if (doneRotating == true) {
				MoveShip (target);
			}
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

	//ship rotation
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

	//ship movement
	public void MoveShip (Vector2 targetPos){
		transform.position = Vector2.Lerp (transform.position, targetPos, flightSpeed * Time.deltaTime);
		if (!engineAudio.isPlaying && Vector2.Distance(transform.position,targetPos) > 5) {
			//engineAudio.Play ();
		}
	}

	void OnCollisionEnter2D(Collision2D otherCol){
		//print ("this = " + this + ", otherCol = " + otherCol.gameObject);
	}

	//default unity OnDestroy()
	void OnDestroy(){
		Empire.instance.RemoveShip (this);
	}
}
