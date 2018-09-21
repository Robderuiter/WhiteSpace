using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour {

	public static TimeController instance;

	//Time panel
	public float timer;
	public float tickLength = 5;
	GameObject timePanel;
	Text[] timeTextHolder;
	int nYears = 0;
	int nTicks = 0;
	int daysInAYear = 365;

	void Awake () {
		instance = this;
	}

	void Start () {
		timePanel = GameObject.Find("TimePanel");
		timeTextHolder = timePanel.GetComponentsInChildren<Text> ();
	}

	// Update is called once per frame
	void Update () {
		//timer used for resource
		timer -= Time.deltaTime;
		if (timer < 0) {
			UpdateTimePanel ();
			timer = tickLength;
		}
	}

	void UpdateTimePanel (){
		nTicks++;
		nYears = nTicks / daysInAYear;
		timeTextHolder [3].text = nTicks.ToString ("0");
		timeTextHolder [2].text = nYears.ToString ("0");
	}

	public void Pause(){
		Time.timeScale = 0;
	}

	public void Play(){
		Time.timeScale = 1;
	}

	public void DoubleSpeed(){
		Time.timeScale = 2;
	}

	public void TripleSpeed(){
		Time.timeScale = 3;
	}
}
