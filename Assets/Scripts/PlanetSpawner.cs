using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour {

	//calculation variables
	int minPlanets = 1;
	int maxPlanets = 9;
	int nPlanets; //number of planets
	float orbitRadius;
	float orbitSection;
	float sunWidth;
	float sunOffset;
	float planetWidth;
	float planetOffset;
	private float planetSize;
	float planetSizeMin = 0.2f;
	float planetSizeMax = 0.7f;
	float planetWidthMultiplier = 1.2f;
	float systemWidth;
	float systemWidthMax;

	//gameobjects etc
	GameObject planetPrefab;
	GameObject planet;
	GameObject[] planets;
	Vector2 spawnPos;

	//list of all planets spawned in this system, public for easy checking
	public List<Planet> spawnedPlanets = new List<Planet>();

	// Use this for initialization
	void Start () {
		//get resources
		planetPrefab = (GameObject) Resources.Load("Planet");

		//randomize number of planets in system
		nPlanets = Random.Range(minPlanets,maxPlanets);
		//print ("nPlanets = " + nPlanets);

		//check sun width
		sunWidth = GetComponent<Renderer>().bounds.size.x;
		sunOffset = sunWidth / 2;

		//check planet width
		planetWidth = planetPrefab.GetComponent<CircleCollider2D>().radius * planetPrefab.transform.localScale.x;
		planetWidth = planetWidth * planetWidthMultiplier;
		planetOffset = planetWidth / 2;
		//print ("planetWidth = " + planetWidth + ", planetOffset = " + planetOffset + ", radius = " + planetPrefab.GetComponent<CircleCollider2D>().radius + ", scale = " + planetPrefab.transform.localScale.x);

		//check system width
		systemWidthMax = GetComponentInParent<CircleCollider2D>().radius * transform.parent.localScale.x;
		systemWidth = systemWidthMax - sunOffset;
		//print ("systemWidth = " + systemWidth + ", systemWidthMax = " + systemWidthMax + ", radius = " + GetComponentInParent<CircleCollider2D>().radius + ", scale = " + transform.parent.localScale.x);

		//loop through nPlanets, spawn planet and set variables where needed
		for (int i = 0; i < nPlanets; i++) {
			//calculate spawnable section: width of system / amount of planets
			orbitSection = systemWidth / nPlanets;

			//calculate orbit radius for planet to be spawned (currently only spawning on y=0 because easy)
			orbitRadius = sunOffset + i * orbitSection + Random.Range (0 + planetOffset, orbitSection - planetOffset);
			spawnPos = new Vector2 (orbitRadius, 0);

			//spawn planet, set parent, set orbit and set center
			planet = GameObject.Instantiate (planetPrefab, spawnPos, transform.rotation);
			planetSize = Random.Range (planetSizeMin, planetSizeMax);
			planet.transform.localScale = new Vector3 (planetSize, planetSize);
			planet.GetComponent<PlanetMovement> ().SetOrbit (orbitRadius);
			planet.transform.parent = gameObject.transform;
			planet.GetComponent<PlanetMovement> ().SetCenter (planet.transform.parent.position);

			//save to spawnedPlanets list:
			spawnedPlanets.Add(planet.GetComponent<Planet>());
			print (planet.GetComponent<Planet>() + " succesfully added to spawnPlanet list, for a total of " + spawnedPlanets.Count + " planets");
		}
	}
		
	/*
	public struct PlanetType{
		public string Name;
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
		public float buildingSlotMultiplier;
	}

	public PlanetType Earthlike;
	public PlanetType Desert;
	public PlanetType Vulcanic;
	public PlanetType Ice;
	public PlanetType Jungle;
	public PlanetType Water;
	public PlanetType Moon;
	public PlanetType Gas;
	public PlanetType Ruin;
	public PlanetType Alien;
	public PlanetType Radioactive;
	*/

	/*
	void initPlanetTypes(){
		//PlanetType declarations for days
		Earthlike = new PlanetType();
		Earthlike.Name = "Earthlike";
		Earthlike.popMultiplier = 1f;
		Earthlike.foodMultiplier = 1f;
		Earthlike.waterMultiplier = 1f;
		Earthlike.ironMultiplier = 1f;
		Earthlike.oxygenMultiplier = 1f;
		Earthlike.powerMultiplier = 1f;
		Earthlike.stoneMultiplier = 1f;
		Earthlike.researchMultiplier = 1f;
		Earthlike.pollutionMultiplier = 1f;
		Earthlike.floraMultiplier = 1f;
		Earthlike.faunaMultiplier = 1f;
		Earthlike.temperatureMultiplier = 1f;
		Earthlike.buildingSlotMultiplier = 1f;
		

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

		//Vulcanic = new PlanetType();
		Vulcanic.Name = "Vulcanic";
		Vulcanic.popMultiplier = 0.3f;
		Vulcanic.foodMultiplier = 0f;
		Vulcanic.waterMultiplier = 0f;
		Vulcanic.ironMultiplier = 5f;
		Vulcanic.oxygenMultiplier = 3f;
		Vulcanic.powerMultiplier = 4f;
		Vulcanic.stoneMultiplier = 2f;
		Vulcanic.researchMultiplier = 0.4f;
		Vulcanic.pollutionMultiplier = 4f;
		Vulcanic.floraMultiplier = 0f;
		Vulcanic.faunaMultiplier = 0f;
		Vulcanic.temperatureMultiplier = 10f;
		Vulcanic.buildingSlotMultiplier = 0.4f;

		//Ice = new PlanetType();
		Ice.Name = "Ice";
		Ice.popMultiplier = 0.5f;
		Ice.foodMultiplier = 0.3f;
		Ice.waterMultiplier = 5f;
		Ice.ironMultiplier = 0.5f;
		Ice.oxygenMultiplier = 2f;
		Ice.powerMultiplier = 0.5f;
		Ice.stoneMultiplier = 1f;
		Ice.researchMultiplier = 1f;
		Ice.pollutionMultiplier = 1f;
		Ice.floraMultiplier = 0.1f;
		Ice.faunaMultiplier = 0.3f;
		Ice.temperatureMultiplier = 0.1f;
		Ice.buildingSlotMultiplier = 0.3f;

		//Jungle = new PlanetType();
		Jungle.Name = "Jungle";
		Jungle.popMultiplier = 1.2f;
		Jungle.foodMultiplier = 2f;
		Jungle.waterMultiplier = 2f;
		Jungle.ironMultiplier = 0.5f;
		Jungle.oxygenMultiplier = 1.3f;
		Jungle.powerMultiplier = 1f;
		Jungle.stoneMultiplier = 1f;
		Jungle.researchMultiplier = 1f;
		Jungle.pollutionMultiplier = 0.5f;
		Jungle.floraMultiplier = 5f;
		Jungle.faunaMultiplier = 4f;
		Jungle.temperatureMultiplier = 2.5f;
		Jungle.buildingSlotMultiplier = 0.2f;

		//Water = new PlanetType();
		Water.Name = "Water";
		Water.popMultiplier = 1f;
		Water.foodMultiplier = 4f;
		Water.waterMultiplier = 8f;
		Water.ironMultiplier = 0f;
		Water.oxygenMultiplier = 1f;
		Water.powerMultiplier = 1f;
		Water.stoneMultiplier = 0f;
		Water.researchMultiplier = 1f;
		Water.pollutionMultiplier = 1f;
		Water.floraMultiplier = 1f;
		Water.faunaMultiplier = 4f;
		Water.temperatureMultiplier = 0.4f;
		Water.buildingSlotMultiplier = 0.2f;

		//Moon = new PlanetType();
		Moon.Name = "Moon";
		Moon.popMultiplier = 0.5f;
		Moon.foodMultiplier = 0f;
		Moon.waterMultiplier = 0f;
		Moon.ironMultiplier = 4f;
		Moon.oxygenMultiplier = 0f;
		Moon.powerMultiplier = 1f;
		Moon.stoneMultiplier = 1f;
		Moon.researchMultiplier = 4f;
		Moon.pollutionMultiplier = 0f;
		Moon.floraMultiplier = 0f;
		Moon.faunaMultiplier = 0f;
		Moon.temperatureMultiplier = 0f;
		Moon.buildingSlotMultiplier = 1f;

		//Gas = new PlanetType();
		Gas.Name = "Gas";
		Gas.popMultiplier = 1f;
		Gas.foodMultiplier = 0f;
		Gas.waterMultiplier = 0f;
		Gas.ironMultiplier = 0f;
		Gas.oxygenMultiplier = 3f;
		Gas.powerMultiplier = 1f;
		Gas.stoneMultiplier = 0f;
		Gas.researchMultiplier = 1f;
		Gas.pollutionMultiplier = 2f;
		Gas.floraMultiplier = 0f;
		Gas.faunaMultiplier = 0f;
		Gas.temperatureMultiplier = 0f;
		Gas.buildingSlotMultiplier = 0.1f;


		//Ruin = new PlanetType();
		Ruin.Name = "Ruin";
		Ruin.popMultiplier = 1f;
		Ruin.foodMultiplier = 1f;
		Ruin.waterMultiplier = 1f;
		Ruin.ironMultiplier = 0.5f;
		Ruin.oxygenMultiplier = 1f;
		Ruin.powerMultiplier = 1f;
		Ruin.stoneMultiplier = 3f;
		Ruin.researchMultiplier = 2f;
		Ruin.pollutionMultiplier = 5f;
		Ruin.floraMultiplier = 0.3f;
		Ruin.faunaMultiplier = 0.7f;
		Ruin.temperatureMultiplier = 1f;
		Ruin.buildingSlotMultiplier = 3f;


		//Alien = new PlanetType();
		Alien.Name = "Alien";
		Alien.popMultiplier = 0.1f;
		Alien.foodMultiplier = 4f;
		Alien.waterMultiplier = 1f;
		Alien.ironMultiplier = 1f;
		Alien.oxygenMultiplier = 1f;
		Alien.powerMultiplier = 1f;
		Alien.stoneMultiplier = 1f;
		Alien.researchMultiplier = 1f;
		Alien.pollutionMultiplier = 1f;
		Alien.floraMultiplier = 5f;
		Alien.faunaMultiplier = 8f;
		Alien.temperatureMultiplier = 1f;
		Alien.buildingSlotMultiplier = 0.2f;


		//Radioactive = new PlanetType();
		Radioactive.Name = "Radioactive";
		Radioactive.popMultiplier = 0.5f;
		Radioactive.foodMultiplier = 0.1f;
		Radioactive.waterMultiplier = 0.1f;
		Radioactive.ironMultiplier = 1f;
		Radioactive.oxygenMultiplier = 1f;
		Radioactive.powerMultiplier = 10f;
		Radioactive.stoneMultiplier = 1f;
		Radioactive.researchMultiplier = 1f;
		Radioactive.pollutionMultiplier = 100f;
		Radioactive.floraMultiplier = 0.1f;
		Radioactive.faunaMultiplier = 0f;
		Radioactive.temperatureMultiplier = 4f;
		Radioactive.buildingSlotMultiplier = 1f;

	}*/

}
