using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentResources : MonoBehaviour {

	//resource classes
	//public float[] planetModifiers;
	public float[] resourceMultipliers;
	public GameObject parentGameObject;
	public Planet planet;
	public Population pop;
	public Food food;
	public Water water;
	public Oxygen oxygen;
	public Power power;
	public Flora flora;
	public Fauna fauna;

	//timer
	public bool isActive = false;

	//module array
	public Module[] mod;
	public List<Module> modList = new List<Module>();

	//resource array
	public Resource[] res;

	//infoWindow
	InfoWindow infoW;

	// Use this for initialization
	void Start () {
		planet = GetComponentInParent<Planet>();
		//print ("planetModifiers on currentresources = " + planetModifiers[0] + ", also nresMultipliers = " + planet.nResourceMultipliers + ", planet.resourcemult[0] = " + planet.resourceMultipliers[0]);

		//add resources
		pop = new Population (this);
		food = new Food (this);
		water = new Water (this);
		oxygen = new Oxygen (this);
		power = new Power (this);
		flora = new Flora (this);
		fauna = new Fauna (this);
		//@@ volgorde moet later aangepast..
		res = new Resource[7]{pop, food, water, oxygen, power, flora, fauna};
	}

	void Update(){
		if (isActive == true) {
			if (TimeController.instance.timer == 5) {
				//Debug.Log ("resourceMultipliers = " + resourceMultipliers[0]);

				//for each resource, calc changes
				for (int q = 0; q < res.Length; q++) {
					res[q].Calc (resourceMultipliers[q]);
				}

				//for each mod, adjust change in resource subclasses
				foreach (Module mod in modList){
					mod.DoYourThang ();
					//Debug.Log (mod);
				}

				//for each resource, apply change
				for (int q = 0; q < res.Length; q++) {
					res [q].ApplyChange ();
				}

				/*
				//update empire based on new amount
				for (int q = 0; q < res.Length; q++) {
					Empire.instance.currentRes.res[q].amount += res[q].amount;
					print ("Empire.instance.currentRes.res[q] = " + Empire.instance.currentRes.res[q].amount + ", res[q].amount = " +  res[q].amount);
				}
				*/

				//check for InfoWindow and update that
				//CheckForInfoWindowAndUpdateThat ();
				//parentGameObject.GetComponentInChildren<InfoWindow>().UpdateFloatingInfo();
			}
		}
	}

	//called on planet type subclasses (Earthlike, Desert..)
	public void InitResources(float[] resourceTresholds){
		//for (int k = 0;k < resourceTresholds.Length;k++){
		//	print("resourceTresholds [" + k + "] = " + resourceTresholds [k]);
		//}

		pop.amount = Random.Range(resourceTresholds[0],resourceTresholds[1]);
		food.amount = Random.Range(resourceTresholds[2],resourceTresholds[3]);
		water.amount = Random.Range(resourceTresholds[4],resourceTresholds[5]);
		water.max = water.amount;
		//so that water is already set to account for current population
		water.Calc (resourceMultipliers[2]);

		oxygen.amount = Random.Range(resourceTresholds[8], resourceTresholds[9]);
		power.amount = Random.Range(resourceTresholds[10], resourceTresholds[11]);
		flora.amount = Random.Range (resourceTresholds[18], resourceTresholds[19]);
		fauna.amount = Random.Range(resourceTresholds[20], resourceTresholds[21]);
		/*
		iron = Random.Range(resourceTresholds[6],resourceTresholds[7]);
		oxygen = Random.Range(resourceTresholds[8],resourceTresholds[9]);
		power = Random.Range(resourceTresholds[10],resourceTresholds[11]);
		stone = Random.Range(resourceTresholds[12],resourceTresholds[13]);
		research = Random.Range(resourceTresholds[14],resourceTresholds[15]);
		pollution = Random.Range(resourceTresholds[16],resourceTresholds[17]);
		flora = Random.Range(resourceTresholds[18],resourceTresholds[19]);
		fauna = Random.Range(resourceTresholds[20],resourceTresholds[21]);
		temperature = Random.Range(resourceTresholds[22],resourceTresholds[23]);
		gold = Random.Range(resourceTresholds[24],resourceTresholds[25]);
		*/
	}

	//called on planet and ships
	public void InitStorage(float[] st){
		pop.storage = st[0];
		food.storage = st[1];
		water.storage = st[2];
		oxygen.storage = st[3];
		power.storage = st[4];
		flora.storage = st[5];
		fauna.storage = st[6];
	}

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
}
