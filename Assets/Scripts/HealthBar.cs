using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

	GameObject myParent;
	public float maxHP;
	public float currentHP;

	float barDisplay = 0;
	Vector2 pos;
	Vector2 size;
	Texture2D progressBarEmpty;
	Texture2D progressBarFull;

	void OnGUI(){
		//draw background
		GUI.BeginGroup (new Rect(pos.x, pos.y, size.x, size.y));
		GUI.Box (new Rect (0, 0, size.x, size.y), progressBarEmpty);

		//draw fill
		GUI.BeginGroup(new Rect(0, 0, size.x * barDisplay, size.y));
		GUI.Box (new Rect (0, 0, size.x, size.y), progressBarFull);

		GUI.EndGroup ();

		GUI.EndGroup ();
	}

	void Awake(){
		pos = new Vector2 (20, 40);
		size = new Vector2 (60, 20);

		progressBarEmpty = new Texture2D ((int)size.x, (int)size.y);
		progressBarFull = new Texture2D ((int)size.x, (int)size.y);

		transform.localPosition = new Vector2 (0,0);
	}

	// Update is called once per frame
	void Update () {
		barDisplay = currentHP / maxHP;
	}
}
