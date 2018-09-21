using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSpawner : MonoBehaviour {

	float universeSize = 1000;
	float systemWidth;
	float systemScarsity = 1; //higher value means less planets
	int nSystems;
	bool checkResult;

	GameObject systemPrefab;
	GameObject[] systems;
	GameObject system;
	Vector2 spawnPos;

	// Use this for initialization
	void Start () {
		systemPrefab = (GameObject) Resources.Load ("System");

		//determine number of systems based on universeSize (lets say it's a square)
		systemWidth = systemPrefab.GetComponent<CircleCollider2D> ().radius * systemScarsity;
		nSystems = Mathf.RoundToInt(universeSize / systemWidth * universeSize / systemWidth);
		//print ("nsystems = " + nSystems);

		systems = new GameObject[nSystems];

		//max suggestion: while loop
		for (int j = 0; j < nSystems; j++) {
			spawnPos.x = Random.Range(-universeSize/2,universeSize/2);
			spawnPos.y = Random.Range(-universeSize/2,universeSize/2);
			//print ("spawnPos = " + spawnPos);

			checkResult = Physics2D.OverlapCircle (spawnPos,systemWidth);
			//print ("checkResult = " + checkResult);

			if (checkResult == false) {
				//leaves increasing gaps as more and more systems spawns, it becomes harder to find a new spawn spot
				systems[j] = GameObject.Instantiate (systemPrefab, spawnPos, transform.rotation);
				//print("system["+j+"] = " + systems[j]);
			}

		}
	}
}
