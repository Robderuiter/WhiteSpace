using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barren : Planet {
		
	// Runs during inialization, mainly use for internal
	new void Awake () {
		base.Awake ();

		planetType = 3;

		sprite = Resources.Load<Sprite> ("PlanetTypes/Barren");
		flavorSprite = Resources.Load<Sprite> ("FlavorSprites/planetbarren");

		//for some reason doesnt work when i put this in the Planet class
		planetSize = GetComponent<CircleCollider2D>().radius * transform.localScale.x;

		typeName = "Barren";

		//set the vars for initResources
		popMin = 0; 
		popMax = 0; 
		foodMin = 0; 
		foodMax = 0; 
		waterMin = 0;
		waterMax = 0;
		ironMin = 0;
		ironMax = 0;
		oxygenMin = 0;
		oxygenMax = 0;
		powerMin = 0; 
		powerMax = 0; 
		stoneMin = 0;
		stoneMax = 0;
		researchMin = 100; 
		researcMax = 100;
		pollutionMin = 0;
		pollutionMax = 0;
		floraMin = 0;
		floraMax = 0;
		faunaMin = 0;
		faunaMax = 0;
		temperatureMin = 0;
		temperatureMax = 0;
		goldMin = 0; 
		goldMax = 0;

		popMultiplier = 1;
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

		planetNames = new string[9]{"Little Eye", "Woon", "Badar", "Aku", "Yareach", "Ramachandra", "Chan", "Iah", "Arche"};
		//this (apparently?) needs to run here because of saving the name string in Awake(), doesnt work when i put in in planet Start, weird..
		planetName = GetRandomName ();
	}

	// Use this for initialization
	new	void Start () {
		base.Start ();
	}
}
