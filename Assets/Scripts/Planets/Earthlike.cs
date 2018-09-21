using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthlike : Planet {

	// Runs during inialization, mainly use for internal
	new void Awake () {
		base.Awake ();

		planetType = 1;

		sprite = Resources.Load<Sprite> ("PlanetTypes/Earthlike");

		//for some reason doesnt work when i put this in the Planet class
		planetSize = GetComponent<CircleCollider2D>().radius * transform.localScale.x;

		typeName = "Earthlike";

		//set the vars for initResources
		popMin = 0; 
		popMax = 1000 * planetSize; 
		foodMin = 1000; 
		foodMax = 10000; 
		waterMin = 2000 * planetSize;
		waterMax = 5000 * planetSize;
		ironMin = 0;
		ironMax = 500 * planetSize;
		oxygenMin = 500 * planetSize;
		oxygenMax = 1000 * planetSize;
		powerMin = 0; 
		powerMax = 1000; 
		stoneMin = 500 * planetSize;
		stoneMax = 1000 * planetSize;
		researchMin = 0; 
		researcMax = 0;
		pollutionMin = 0;
		pollutionMax = 0;
		floraMin = 0;
		floraMax = 1000;
		faunaMin = 0;
		faunaMax = 500;
		temperatureMin = 0;
		temperatureMax = 0;
		goldMin = 0; 
		goldMax = 0;

		popMultiplier = 2;
		foodMultiplier = 1;
		waterMultiplier = 1;
		ironMultiplier = 1;
		oxygenMultiplier = 1;
		powerMultiplier = 1;
		stoneMultiplier = 1;
		researchMultiplier = 1;
		pollutionMultiplier = 1;
		floraMultiplier = 1;
		faunaMultiplier = 1;
		temperatureMultiplier = 1;
		goldMultiplier = 1;

		popStorage = 10000;
		foodStorage = 10000;
		waterStorage = 10000;
		ironStorage = 10000;
		oxygenStorage = 10000;
		powerStorage = 10000;
		stoneStorage = 10000;
		researchStorage = 10000;
		pollutionStorage = 10000;
		floraStorage = 10000;
		faunaStorage = 10000;
		temperatureStorage = 10000;
		goldStorage = 10000;

		planetNames = new string[6]{ "Earth", "Gaia", "Primus", "Atlas", "Dirt", "Calradia" };
		//this (apparently?) needs to run here because of saving the name string in Awake(), doesnt work when i put in in planet Start, weird..
		planetName = GetRandomName ();
	}

	// Use this for initialization
	new void Start () {
		base.Start ();
	}

}
