using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentResources : MonoBehaviour {

	//resource classes
	//public float[] planetModifiers;
	public float[] resourceMultipliers;
	public GameObject parentGameObject;
	public Planet planet;

	//resource array
	public Resource[] res;
	public Population pop;
	public Food food;
	public Water water;
	public Oxygen oxygen;
	public Flora flora;
	public Fauna fauna;
	public Stone stone;

	//timer
	public bool isActive = false;

	//module array
	public Module[] mod;
	public List<Module> modList = new List<Module>();

	//infoWindow
	InfoWindow infoW;

	public void Update(){
		if (isActive == true) {
			if (TimeController.instance.timer == 5) {
				//Debug.Log ("resourceMultipliers = " + resourceMultipliers[0]);

				//for each resource, reset change
				for (int q = 0; q < res.Length; q++) {
					res[q].change = 0;
				}


				//for each resource, calc changes
				for (int q = 0; q < res.Length; q++) {
					res[q].Calc (resourceMultipliers[q]);
				}

				//for each mod, adjust change in resource subclasses
				foreach (Module mod in modList){
					mod.DoYourThang ();
				}

				//for each resource, apply change
				for (int q = 0; q < res.Length; q++) {
					res [q].ApplyChange ();
				}


				//update empire based on new amount
				if (this.gameObject.tag == "Ship" || this.gameObject.tag == "Module"){
					for (int q = 0; q < res.Length; q++) {
						Empire.instance.currentRes.res[q].amount += res[q].amount;
						print ("Empire.instance.currentRes.res[q] = " + Empire.instance.currentRes.res[q].amount + ", res[q].amount = " +  res[q].amount);
					}
				}

				//check for InfoWindow and update that
				//CheckForInfoWindowAndUpdateThat ();
				//parentGameObject.GetComponentInChildren<InfoWindow>().UpdateFloatingInfo();
			}
		}
	}

	/*
	//called on planet and ships
	public void InitStorage(float[] st){
		pop.storage = st[0];
		food.storage = st[1];
		water.storage = st[2];
		oxygen.storage = st[3];
		stone.storage = st [4];
		flora.storage = st[5];
		fauna.storage = st[6];
	}
	*/

	/* deprecated since floating > static info window
	void CheckForInfoWindowAndUpdateThat(){
		print ("check");
		if (GetComponentInParent<InfoWindow>()){
			print ("check 2");
			infoW = gameObject.GetComponentInParent<InfoWindow> ();

			//infoW.UpdateFloatingInfo ();
		}
	}
	*/

	public virtual void InitResources(){

	}
}
