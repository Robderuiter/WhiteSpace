using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : Module {

	Resource extractedRes;

	new void Awake(){
		base.Awake();

		nExtracted = 100;

		//infowindow stuff
		defName = "Kelvinator";
		type = "Miner";
		flavorSprite = Resources.Load<Sprite> ("FlavorSprites/planetempty");

		//used by SelectionMaster
		hasResources = true;

		//init resources
		//stone = new Stone ();
		//res = new Resource[1](stone);
	}

	new void Start(){
		base.Start();

		//example on how to get iron storage to work
		//currentRes.food = new Food (this);
	}

	// Update is called once per frame
	public override void DoYourThang () {
		extractedRes = currentRes.stone;

		if (myParent.tag == "Planet"){
		//if there is enough resource left to grab a full extraction, else 
		if (extractedRes.amount > nExtracted){
			storage = storage + nExtracted;
			AddToChange (extractedRes, -nExtracted);
		} else if (extractedRes.amount > 0){
			storage = storage + extractedRes.amount;
			AddToChange (extractedRes, -extractedRes.amount);
		}
		}

		print ("currentRes = " + currentRes + ", extractedRes = " + extractedRes + ", nExtracted = " + nExtracted + ", storage = " + storage);
	}
}
