using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInfo : MonoBehaviour {

	GameObject planetInfoPanel;
	public float planetInfoPanelWidth;

	void Awake () {
		planetInfoPanel = this.gameObject;

		planetInfoPanelWidth = planetInfoPanel.GetComponent<RectTransform> ().sizeDelta.x;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
