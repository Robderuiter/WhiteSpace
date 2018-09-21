using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour {

	float size;
	float spriteSize;
	float sizeBuffer;
	public Planet homePlanet;
	public GameObject planet;
	//Rigidbody2D rb;
	public bool hasLanded = false;
	private bool extractsResources = true;
	private bool extractsWater = false;
	private bool extractsFood = false;
	public float waterOnModule;
	public float nWaterExtracted;

	//resources
	public CurrentResources currentRes;
	public CurrentResources targetRes;
	public float nExtracted;
	public float storage;

	//Empire overview
	Empire empire;

	public void Awake () {
		size = GetComponent<CircleCollider2D>().radius;
		spriteSize = GetComponent<SpriteRenderer>().size.x * transform.localScale.x;
		sizeBuffer = size/5;
		//rb = GetComponent<Rigidbody2D> ();
	}

	// Use this for initialization
	public void Start () {
		extractsWater = true;
		nWaterExtracted = 100000;

		empire = GameObject.Find("Empire").GetComponent<Empire>();
	}

	// Update is called once per frame
	void Update () {
		//this is here for testing, needs to be on a moduletype, not Module
		if (hasLanded && extractsResources) {
			if (extractsWater){
				//ExtractResources (homePlanet.GetComponent<Water>());
			}
			if (extractsFood){
				//ExtractResources (homePlanet.GetComponent<Food>());
			}
		}
	}

	public void AttachModule (GameObject target){
		print (target);
		if (target.tag == "Planet") {
			//save Planet component on target
			planet = target;
			homePlanet = target.GetComponent<Planet>();
			print ("homePlanet = " + homePlanet + ", homePlanet.isScanned = " + homePlanet.isScanned + ", homePlanet.nBuildingSlots = " + homePlanet.nBuildingSlots);

			//check if targetplanet has been scanned
			if (homePlanet.isScanned) {
				for (int n = 0; n < homePlanet.nBuildingSlots; n++) {
					//if less modules have been attached then there have been iterations of the loop
					if (homePlanet.nModulesAttached <= n) {
						//save module on planet
						homePlanet.buildingSlots [n] = gameObject;

						//set planet as parent
						transform.parent = target.transform;

						//determine new position and place, not exactly working perfectly yet
						float angle = n * Mathf.PI * 2f / homePlanet.nBuildingSlots;
						float newRadius = homePlanet.planetSize * homePlanet.transform.localScale.x * 2 + spriteSize * transform.localScale.x * 2;
						float x = newRadius * Mathf.Cos (angle);
						float y = newRadius * Mathf.Sin (angle);
						//gaat niet goed omdat module collider tegen planet botst bij plaatsing, aka berekening klopt niet.. getest door collider trigger te maken
						transform.localPosition = new Vector2 (x, y);
						print ("angle = " + angle + ", newRadius = " + newRadius + ", transform.localPosition = " + transform.localPosition + ", homePlanet.nModulesAttached = " + homePlanet.nModulesAttached);
						print ("homePlanet.planetSize = " + homePlanet.planetSize + ", spriteSize = " + spriteSize);

						//set rotation relative to parent
						transform.localRotation = Quaternion.Euler (0f, 0f, angle * Mathf.Rad2Deg);

						//update module count on planet
						homePlanet.nModulesAttached++;
						currentRes = homePlanet.currentRes;

						//add this module to the currentResources modList
						homePlanet.currentRes.modList.Add (this);
						empire.modules.Add (this);
						empire.planets.Add (homePlanet);
						empire.empireUIController.UpdateEmpireUI ();

						break;
					}
				}
				hasLanded = true;
			}
		}
		if (target.tag == "Ship") {
			//need to check if slot is available, if not, check next one
			Ship ship = target.GetComponent<Ship> ();
			int nSlots = ship.nModuleSlots;
			for (int m = 0; m < nSlots; m++) {
				if (ship.modules [m] == null) { 
					//print ("!ship.modules [m]" + !ship.modules [m] + ", m = " + m);
					//save Module on ship
					ship.modules [m] = gameObject;

					//set ship as parent, determine position and place
					transform.parent = target.transform;
					transform.localPosition = new Vector2 (-spriteSize - sizeBuffer - (m) * (spriteSize + sizeBuffer), 0);
					transform.rotation = new Quaternion (0, 0, 0, 0);

					//update module count on planet, if module was landed before
					if (hasLanded) {
						homePlanet.nModulesAttached--;
					}

					//set
					currentRes = ship.currentRes;

					break;
				}
			}

			hasLanded = false;
			//print ("Succesfully attached to ship, hasLanded = " + hasLanded);
		}
	}

	public void AddToChange(Resource res, float n){
		//still need to add a currentResources to Module
		res.change += n;
		//moduleRes += n;
		print ("res.amount = " + res.amount + ", currentRes = " + currentRes);
	}

	public virtual void DoYourThang(){

	}
}
