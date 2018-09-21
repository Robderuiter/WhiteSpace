using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMovement : MonoBehaviour {

	//physics
	private float rotationSpeed = 0.02f;
	private float minRotationSpeed = 0.02f;
	private float maxRotationSpeed = 0.04f;
	private float orbitRadius;
	private Vector2 center;
	private float angle;
	float rotationsPerMinute;

	//Use this for initialization
	void Start () {
		rotationSpeed = Random.Range(minRotationSpeed,maxRotationSpeed);

		//randomize rotation direction
		if (Random.value < 0.5f) {
			rotationSpeed = -rotationSpeed;
		}

		//determine rotation direction
		int roll = Mathf.RoundToInt(Random.value);
		if (roll == 0) {
			rotationsPerMinute = 1;
		}
		if (roll == 1) {
			rotationsPerMinute = -1;
		}

	}
	
	// Update is called once per frame
	void Update () {
		MovePlanet ();
		RotatePlanet ();
	}

	public void MovePlanet(){
		angle += rotationSpeed * Time.deltaTime;

		var offset = new Vector2 (Mathf.Sin (angle), Mathf.Cos (angle)) * orbitRadius;
		transform.position = center + offset;
	}

	public void RotatePlanet(){
		transform.Rotate (0,0,6 * rotationsPerMinute * Time.deltaTime);
	}

	public void SetOrbit(float or){
		orbitRadius = or;
	}

	public void SetCenter(Vector2 centre){
		center = centre;
	}
}
