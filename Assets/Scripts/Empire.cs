using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empire : MonoBehaviour {

	public List<Planet> planets = new List<Planet>();
	public List<Ship> ships = new List<Ship>();
	public List<Resource> resource = new List<Resource>();

	public EmpireUIController empireUIController;

	public static Empire instance;

	void Awake(){
		instance = this;

		empireUIController = gameObject.AddComponent<EmpireUIController>();
	}

	public void AddPlanet(Planet planet){
		if (!planets.Contains (planet)) {
			planet.isInEmpire = true;
			planets.Add (planet);
			empireUIController.AddToPlanetUI (planet);
		}
	}

	public void RemovePlanet(Planet planet){
		if (planets.Contains (planet)) {
			planet.isInEmpire = false;
			planets.Remove (planet);
			empireUIController.RemoveFromPlanetUI (planet);
		}
	}

	public void AddShip(Ship ship){
		if (!ships.Contains (ship)) {
			ship.isInEmpire = true;
			ships.Add (ship);
			empireUIController.AddToShipUI (ship);
		}
	}

	public void RemoveShip(Ship ship){
		if (ships.Contains (ship)) {
			ship.isInEmpire = false;
			ships.Remove (ship);
			empireUIController.RemoveFromShipUI (ship);
		}
	}
}