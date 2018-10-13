using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desert : Planet {
	
	// Runs during inialization, mainly use for internal
	new void Awake () {
		base.Awake ();

		planetType = 2;

		sprite = Resources.Load<Sprite>("PlanetTypes/Desert");
		flavorSprite = Resources.Load<Sprite> ("FlavorSprites/planetdesert");

		//for some reason doesnt work when i put this in the Planet class
		planetSize = GetComponent<CircleCollider2D>().radius * transform.localScale.x;

		typeName = "Desert";

		//set the vars for initResources
		popMin = 0; 
		popMax = 500; 
		foodMin = 0; 
		foodMax = 3000; 
		waterMin = 0;
		waterMax = 200;
		ironMin = 0;
		ironMax = 500;
		oxygenMin = 500;
		oxygenMax = 1000;
		powerMin = 0; 
		powerMax = 500; 
		stoneMin = 500;
		stoneMax = 1000;
		researchMin = 0; 
		researcMax = 0;
		pollutionMin = 0;
		pollutionMax = 0;
		floraMin = 0;
		floraMax = 50;
		faunaMin = 0;
		faunaMax = 50;
		temperatureMin = 0;
		temperatureMax = 0;
		goldMin = 0; 
		goldMax = 0;

		popMultiplier = 0.8f;
		foodMultiplier = 1.2f;
		waterMultiplier = 1.5f;
		ironMultiplier = 1;
		oxygenMultiplier = 1;
		powerMultiplier = 1;
		stoneMultiplier = 1;
		researchMultiplier = 1;
		pollutionMultiplier = 1;
		floraMultiplier = 1f;
		faunaMultiplier = 0.2f;
		temperatureMultiplier = 1.7f;
		goldMultiplier = 1.2f;

		planetNames = new string[3]{"Arrakis", "Dune", "Sahara"};
		//this (apparently?) needs to run here because of saving the name string in Awake(), doesnt work when i put in in planet Start, weird..
		defName = GetRandomName ();
	}

	// Use this for initialization
	new void Start () {
		base.Start ();
	}

}

	/*
	//Desert = new PlanetType();
	Desert.Name = "Desert";
	Desert.popMultiplier = 0.8f;
	Desert.foodMultiplier = 0.5f;
	Desert.waterMultiplier = 0.1f;
	Desert.ironMultiplier = 1f;
	Desert.oxygenMultiplier = 1f;
	Desert.powerMultiplier = 1.1f;
	Desert.stoneMultiplier = 1f;
	Desert.researchMultiplier = 1f;
	Desert.pollutionMultiplier = 1f;
	Desert.floraMultiplier = 0.1f;
	Desert.faunaMultiplier = 0.3f;
	Desert.temperatureMultiplier = 1.7f;
	Desert.buildingSlotMultiplier = 1.5f;
	*/
