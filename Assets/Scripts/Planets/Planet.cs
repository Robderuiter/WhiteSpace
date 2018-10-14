using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

	//classes and such
	Resource res;
	public CurrentResources currentRes;
	public int planetType = 0;
	public string typeName = "Planet";
	public Sprite sprite;
	public Sprite flavorSprite;
	public float planetSize;
	public bool isScanned = false;
	public string[] planetNames;
	public string defName;
	public bool isInEmpire = false;
	public InfoWindow infoW;

	//building slots, should become a ModuleSlots Class
	public int nBuildingSlots;
	public GameObject[] buildingSlots;
	public float planetCircumference;
	public GameObject module;
	//GameObject buildingPlaceholder;
	float moduleSize;
	float buildingOffset;
	public int nModulesAttached;

	//resource tresholds
	public float[] resourceTresholds;
	public int nResourceTresholds = 26;
	public float popMin; 
	public float popMax; 
	public float foodMin; 
	public float foodMax; 
	public float waterMin; 
	public float waterMax; 
	public float ironMin; 
	public float ironMax; 
	public float oxygenMin; 
	public float oxygenMax; 
	public float powerMin; 
	public float powerMax; 
	public float stoneMin; 
	public float stoneMax; 
	public float researchMin; 
	public float researcMax;
	public float pollutionMin;
	public float pollutionMax;
	public float floraMin;
	public float floraMax;
	public float faunaMin;
	public float faunaMax;
	public float temperatureMin;
	public float temperatureMax;
	public float goldMin; 
	public float goldMax;

	//resource multipliers
	//public float[] resourceMultipliers;
	public int nResourceMultipliers = 12;
	public float popMultiplier;
	public float foodMultiplier;
	public float waterMultiplier;
	public float ironMultiplier;
	public float oxygenMultiplier;
	public float powerMultiplier;
	public float stoneMultiplier;
	public float researchMultiplier;
	public float pollutionMultiplier;
	public float floraMultiplier;
	public float faunaMultiplier;
	public float temperatureMultiplier;
	public float goldMultiplier;

	//resource storage
	public float[] storageAmount;
	public float popStorage;
	public float foodStorage;
	public float waterStorage;
	public float ironStorage;
	public float oxygenStorage;
	public float powerStorage;
	public float stoneStorage;
	public float researchStorage;
	public float pollutionStorage;
	public float floraStorage;
	public float faunaStorage;
	public float temperatureStorage;
	public float goldStorage;

	//module slots
	public ModuleSlots modSlots;
	float planetBuildablePercentage;

	// runs during initialization, great for internal settings, not great for external links
	public void Awake () {
		//get the Module resource, used for size calculation later
		module = Resources.Load<GameObject>("Module");

		nModulesAttached = 0;

		//resource multipliers
		//resourceMultipliers = new float[nResourceMultipliers];
	}

	public void Start(){
		//get link to currentresources
		currentRes = GetComponent<CurrentResources>();

		//save resource and treshold multiplier arrays
		SaveResourceTresholds ();
		SaveResourceMultipliers ();
		currentRes.InitResources (resourceTresholds);

		//save resource storage
		SaveStorageAmounts ();
		currentRes.InitStorage (storageAmount);

		//set planet sprite to current type
		GetComponent<SpriteRenderer> ().sprite = sprite;

		//start every planet as scanned and its current resources as active for easy testing, creates lag spikes every ~5s
		currentRes.isActive = true;
		isScanned = true;

		//add module slots, arg 1 = Ship, arg 2 = Planet
		modSlots = new ModuleSlots (this.gameObject);

		planetSize = GetComponent<CircleCollider2D>().radius * transform.localScale.x;
		planetCircumference = 2 * planetSize * Mathf.PI;
		planetBuildablePercentage = 1f;

		modSlots.CalcModuleSlots (planetCircumference, 1f);
	}

	//get current planet type, used in resource subclasses, might prove unnecessary later, depends on Planet Getcomponent calls and children structure	
	public Planet GetPlanetType(){
		if (typeName == "Planet") {
			Planet planet = GetComponent<Planet> ();
			//print ("planet = " + planet + ", planetType = " + planetType);
			return(planet);
		}
		if (typeName == "Earthlike") {
			Earthlike planet = GetComponent<Earthlike> ();
			//print ("planet = " + planet + ", planetType = " + planetType);
			return(planet);
		}
		if (typeName == "Desert") {
			Desert planet = GetComponent<Desert> ();
			//print ("planet = " + planet + ", planetType = " + planetType);
			return(planet);
		}
		if (typeName == "Barren") {
			Barren planet = GetComponent<Barren> ();
			//print ("planet = " + planet + ", planetType = " + planetType);
			return(planet);
		} 
		else {
			return(null);
		}
	}

	//store the resource tresholds for initialization declared on the planet subtypes (earthlike etc)
	public void SaveResourceTresholds(){
		resourceTresholds = new float[] {
			popMin, popMax, foodMin, foodMax, waterMin, waterMax, 
			ironMin, ironMax, oxygenMin, oxygenMax, powerMin, powerMax, stoneMin, 
			stoneMax, researchMin, researcMax, pollutionMin, pollutionMax, floraMin, 
			floraMax, faunaMin, faunaMax, temperatureMin, temperatureMax, goldMin, goldMax
		};
	}

	//store the resource multipliers declared on the planet subtypes (earthlike etc)
	public void SaveResourceMultipliers(){
		currentRes.resourceMultipliers = new float[]{
			popMultiplier, foodMultiplier, waterMultiplier, ironMultiplier, oxygenMultiplier, 
			powerMultiplier, stoneMultiplier, researchMultiplier, pollutionMultiplier, 
			floraMultiplier, faunaMultiplier, temperatureMultiplier, goldMultiplier
		};
	}

	//store the storage variables declared on the planet subtypes (earthlike etc)
	public void SaveStorageAmounts(){
		storageAmount = new float[] {
			popStorage, foodStorage, waterStorage, oxygenStorage, stoneStorage, floraStorage, faunaStorage,
			ironStorage, powerStorage, researchStorage, pollutionStorage, temperatureStorage, goldStorage
		};
	}

	//not used yet, not sure if this is more or less efficient than current InitResources,
	//could potentially be more efficient if i only call this for each specific resource on planettype init.. (since not all planets init all resources)
	public float RollResource(float minRes, float maxRes){
		float res = Random.Range(minRes,maxRes);
		return (res);
	}

	//interaction with scanner
	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.name == "Scanner") {
			isScanned = true;
			//set the ticker on currentResources on
			currentRes.isActive = true;

		}
	}
		
	//choose random name from string[] planetNames
	public string GetRandomName(){
		return planetNames [Random.Range (0, planetNames.Length)];
	}

}
