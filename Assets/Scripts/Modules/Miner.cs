﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : Module {

	Resource extractedRes;

	new void Awake(){
		base.Awake();

		nExtracted = 100;
	}

	new void Start(){
		base.Start();
	}

	// Update is called once per frame
	public override void DoYourThang () {
		extractedRes = currentRes.water;
		AddToChange (extractedRes, -nExtracted);
	}
}
