using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour {

	public bool hasLanded = false;

	//resources
	public CurrentResources currentRes;
	public bool extractsResources;
	public float nExtracted;
	public float storage;

	//Empire overview
	Empire empire;

	//stuff for module slots on ships and planets
	public static float modSize;
	public bool wasPlaced;
	int wasPlacedAtSpot;
	public GameObject myParent;

	//infowindow stuff
	public string defName;
	public string type;
	public Sprite flavorSprite;
	public bool hasResources;

	//resource stuff
	public Resource[] res;
	public Population pop;
	public Food food;
	public Water water;
	public Oxygen oxygen;
	public Flora flora;
	public Fauna fauna;
	public Stone stone;

	public void Awake () {
		modSize = GetComponent<SpriteRenderer>().size.x * transform.localScale.x;
	}

	// Use this for initialization
	public void Start () {
		empire = GameObject.Find("Empire").GetComponent<Empire>();
	}

	public void AddToChange(Resource res, float n){
		//still need to add a currentResources to Module
		res.change += n;
		//print ("res.amount = " + res.amount + ", currentRes = " + currentRes);
	}

	//meant as a generic function that runs on every submodule
	public virtual void DoYourThang(){

	}

	//run this on attach/detach module
	public void GetCurrentRes (GameObject parent){
		currentRes = parent.GetComponent<CurrentResources>();
	}

	//called from Selectionmaster for each selected module, attaches this module to the targeted gameobject
	public void AttachModule (GameObject targetObject){
		//if ship
		if (targetObject.tag == "Ship") {
			//get ref to the ships modslots
			ModuleSlots slots = targetObject.GetComponent<Ship>().modSlots;

			//for each selectedobject (= selectedModules)
			for (int n = 0;n < slots.nSlots;n++){
				//check if the n'th addedmodules in the array is empty
				if (slots.addedModules[n] == null && !wasPlaced){
					Ship targetShip = targetObject.GetComponent<Ship> ();
					transform.parent = targetObject.transform;

					//move to the right position
					transform.localPosition = new Vector2 (0 - modSize * 1.2f * (n + 1), 0);
					transform.localRotation = Quaternion.Euler (0,0,0);

					//save ref of this module on ship's ModuleSlots
					slots.addedModules[n] = this;
					wasPlacedAtSpot = n;
					myParent = targetObject.gameObject;

					//increase the amount of modules attached
					targetShip.nModulesAttached++;

					//add it to the currentResources to be added to calculations
					targetShip.currentRes.modList.Add (this);
					GetCurrentRes(targetObject);

					if (hasResources){
						//targetShip.currentRes.res = targetShip.currentRes.res + res;
					}

					wasPlaced = true;
				}
			}
		}

		//if planet
		if (targetObject.tag == "Planet") {
			//get ref to the planets modslots
			Planet targetPlanet = targetObject.GetComponent<Planet>();
			ModuleSlots slots = targetPlanet.modSlots;

			//add this planet to the empire
			Empire.instance.AddPlanet (targetPlanet);

			//for each selectedobject (= selectedModules)
			for (int n = 0;n < slots.nSlots;n++){
				//check if the n'th addedmodules in the array is empty
				if (slots.addedModules [n] == null && !wasPlaced) {
					transform.parent = targetObject.transform;

					//determine new position and place, not exactly working perfectly yet
					float angle = n * Mathf.PI * 2f / slots.nSlots;
					float newRadius = targetPlanet.GetComponent<CircleCollider2D> ().radius + modSize;
					float x = newRadius * Mathf.Cos (angle);
					float y = newRadius * Mathf.Sin (angle);

					//gaat niet goed omdat module collider tegen planet botst bij plaatsing, aka berekening klopt niet.. getest door collider trigger te maken
					transform.localPosition = new Vector2 (x, y);
					//print ("angle = " + angle + ", newRadius = " + newRadius + ", transform.localPosition = " + transform.localPosition + ", targetPlanet.nModulesAttached = " + targetPlanet.nModulesAttached);
					//print ("targetPlanet.planetSize = " + targetPlanet.planetSize + ", modsize = " + modSize);

					//set rotation relative to parent
					transform.localRotation = Quaternion.Euler (0f, 0f, angle * Mathf.Rad2Deg);

					//save ref of this module on ship's ModuleSlots
					slots.addedModules [n] = this;
					wasPlacedAtSpot = n;
					myParent = targetObject.gameObject;

					//update the planets number of modules attached
					targetPlanet.nModulesAttached++;

					//add it to the currentResources to be added to calculations
					targetPlanet.currentRes.modList.Add (this);
					GetCurrentRes(targetObject);

					//
					wasPlaced = true;
				}
			}
		}
	}

	//called from Selectionmaster if moved from an object
	public void DetachModule(){
		wasPlaced = false;

		if (myParent.GetComponent<Ship>()){
			Ship myparentShip = myParent.GetComponent<Ship>();

			myparentShip.modSlots.addedModules.SetValue(null,wasPlacedAtSpot);

			myparentShip.nModulesAttached--;

			if (hasResources){
				myparentShip.currentRes.stone.amount = myparentShip.currentRes.stone.amount - storage;
			}
		}
		if (myParent.GetComponent<Planet>()){
			Planet myParentPlanet = myParent.GetComponent<Planet> ();

			myParentPlanet.nModulesAttached--;

			if (myParentPlanet.nModulesAttached == 0){
				Empire.instance.RemovePlanet(myParentPlanet);
			}

			myParentPlanet.modSlots.addedModules.SetValue(null,wasPlacedAtSpot);
		}
	}

	//@@ does nog work YET!
	//runs in selectionmaster, if myparent is ship and right after detach module has run
	public void ReOrder(){
		int n = 0;
		ModuleSlots slots = myParent.gameObject.GetComponent<Ship>().modSlots;
		foreach (Module mod in slots.addedModules){
			print ("mod =" + mod + ", n = " + n);
			//mod.DetachModule ();
			//mod.AttachModule (myParent);

			//mod.gameObject.transform.localPosition = new Vector2 (0 - modSize * 1.2f * (n + 1), 0);
			n++;
		}
	}

}
