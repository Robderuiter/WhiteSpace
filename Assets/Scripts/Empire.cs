using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empire : MonoBehaviour {

	public List<Planet> planets = new List<Planet>();
	public List<Ship> ships = new List<Ship>();
	public List<Module> modules = new List<Module>();
	public List<Resource> resource = new List<Resource>();

	public EmpireUIController empireUIController;

	void Awake(){
		empireUIController = GetComponent<EmpireUIController>();
	}
}