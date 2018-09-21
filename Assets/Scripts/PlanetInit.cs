using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInit : MonoBehaviour {

	public Sprite sprite;
	//will be used for planet initialization, such as RollPlanetType(), CalcBuildingSlots() so that Planet can truly become a "superclass"

	//
	void Awake () {
		sprite = Resources.Load<Sprite>("PlanetTypes/Planet");

		RollPlanetType ();
	}
		
	//determine planet type
	private void RollPlanetType(){
		//roll 1-1000 (so we can customize spawn change per planet a bit more than 1-11:P
		float roll = Random.Range (0, 1000);
		//chances for each planet type
		if (roll < 400) {
			gameObject.AddComponent<Earthlike> ();
		}
		if (roll > 399 && roll < 700) {
			gameObject.AddComponent<Desert> ();
		}
		if (roll >699) {
			gameObject.AddComponent<Barren> ();
		}
	}
}
