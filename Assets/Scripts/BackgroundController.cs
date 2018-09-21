using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

	//Part of backgroundController (the "paralex" part: slower scrolling bg then camera) is done on cameracontroller

	Camera cam;
	float cameraDistance;
	//float cameraSize;
	//Vector2 cameraPos;
	float leftBorder;
	float rightBorder;
	float topBorder;
	float bottomBorder;

	GameObject[] bgChildren;
	SpriteRenderer spriteR;
	GameObject bgPrefab;
	//Background[] bg;
	int nBackgrounds;

	float spriteWidth;
	float spriteHeight; //not strictly necessary since bg will most likely always be exactly even sided, but still..
	Vector2 spriteBounds;
	Vector2 currentPos;
	float bgOffset = 10;

	void Awake () {
		nBackgrounds = 9;
		bgChildren = new GameObject[nBackgrounds];
		//bg = new Background[nBackgrounds];

		
	}

	// Use this for initialization
	void Start () {
		//find the camera and save its camera component, just to be safe move this controller to exactly where the camera starts
		cam = GameObject.Find("Main Camera").GetComponent<Camera> ();
		transform.position = (Vector2)cam.transform.position;

		//get background Prefab and the attached sprite renderer
		bgPrefab = (GameObject)Resources.Load("Background");
		spriteR = bgPrefab.GetComponent<SpriteRenderer> ();

		//determine sprite Width and Height
		spriteWidth = spriteR.size.x * bgPrefab.transform.localScale.x;
		spriteHeight = spriteR.size.y * bgPrefab.transform.localScale.y;

		//spawn background gameobjects
		for (int t = 0; t < nBackgrounds; t++) {
			SpawnBG (t);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		CheckCameraBounds ();
		CompareBounds ();
	}

	//keep an eye on camera borders, runs in update
	void CheckCameraBounds(){
		// Camera borders
		cameraDistance = (transform.position - Camera.main.transform.position).z;
		leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraDistance)).x;
		rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, cameraDistance)).x;
		topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, cameraDistance)).y;
		bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraDistance)).y;

		//get current zoom level, based on orthographic size and current camera position
		//cameraSize = cam.orthographicSize;
		//cameraPos = cam.transform.position;

		//print ("screenHeight = " + screenHeight + ", screenWidth = " + screenWidth + ", cameraPos = " + cameraPos);
		//print ("bottom = " + bottomBorder + ", topBorder = " + topBorder + ", leftBorder = " + leftBorder + ", rightBorder = " + rightBorder + ", cameraDistance = " + cameraDistance + ", cameraSize = " + cameraSize); 
	}

	//keep an eye on sprite borders, runs in update
	void CheckBackgroundBounds(){
		spriteBounds.x = bgPrefab.transform.position.x + spriteWidth / 2;
		spriteBounds.y = bgPrefab.transform.position.y + spriteWidth / 2;
	}

	//compares camera borders with sprite borders and moves bgcontroller accordingly, runs in update
	void CompareBounds(){
		//right
		if (rightBorder > transform.position.x + spriteBounds.x - bgOffset + spriteWidth){
			MoveBG(bgChildren [3].transform.position);
			CheckBackgroundBounds ();
			//print ("detected someFYNN right border");
		}
		//left
		if (leftBorder < transform.position.x - spriteBounds.x + bgOffset - spriteWidth){
			MoveBG(bgChildren [7].transform.position);
			CheckBackgroundBounds ();
			//print ("detected someFYNN left border");
		}
		//top
		if (topBorder > transform.position.y + spriteBounds.y - bgOffset + spriteWidth){
			MoveBG(bgChildren [1].transform.position);
			CheckBackgroundBounds ();
			//print ("detected someFYNN top border");
		}
		//bottom
		if (bottomBorder < transform.position.y - spriteBounds.y + bgOffset - spriteWidth){
			MoveBG(bgChildren [5].transform.position);
			CheckBackgroundBounds ();
			//print ("detected someFYNN bottom border");
		}
		else{
		}
	}

	//move to given position
	void MoveBG(Vector2 pos){
		transform.position = pos;
	}

	//spawn a background instance, runs nBackgrounds (9) amount of times
	void SpawnBG(int n){
		bgChildren[n] = GameObject.Instantiate (bgPrefab, transform.position, transform.rotation);
		bgChildren[n].transform.parent = gameObject.transform;

		currentPos = bgChildren[n].transform.localPosition;
		InitBGPositions (n);
	}

	//move spawned background instances
	void InitBGPositions(int r){
		//top
		if (r == 1) {
			bgChildren[r].transform.localPosition = new Vector2(currentPos.x,currentPos.y + spriteHeight);
		}
		//top right
		if (r == 2) {
			bgChildren[r].transform.localPosition = new Vector2(currentPos.x + spriteWidth,currentPos.y + spriteHeight);
		}
		//right
		if (r == 3) {
			bgChildren[r].transform.localPosition = new Vector2(currentPos.x + spriteWidth,currentPos.y);
		}
		//bottom right
		if (r == 4) {
			bgChildren[r].transform.localPosition = new Vector2(currentPos.x + spriteWidth,currentPos.y - spriteHeight);
		}
		//bottom
		if (r == 5) {
			bgChildren[r].transform.localPosition = new Vector2(currentPos.x,currentPos.y - spriteHeight);
		}
		//bottom left
		if (r == 6) {
			bgChildren[r].transform.localPosition = new Vector2(currentPos.x - spriteWidth,currentPos.y - spriteHeight);
		}
		//left
		if (r == 7) {
			bgChildren[r].transform.localPosition = new Vector2(currentPos.x - spriteWidth,currentPos.y);
		}
		//top left
		if (r == 8) {
			bgChildren[r].transform.localPosition = new Vector2(currentPos.x - spriteWidth,currentPos.y + spriteHeight);
		}
	}
}
