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
	}

	new void Start(){
		base.Start();

		//example on how to get iron storage to work
		//currentRes.food = new Food (this);
	}

	// Update is called once per frame
	public override void DoYourThang () {
		extractedRes = currentRes.water;
		AddToChange (extractedRes, -nExtracted);
	}
}
