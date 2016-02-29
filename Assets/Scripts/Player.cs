using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour {

	private bool canPlay = false;
	private String name = "AI";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetName (String name) {
		this.name = name;
	}

	public String GetName () {
		return name;
	}

	public void StartPlaying () {
		canPlay = true;


		Debug.Log ("Player " + this.GetName() +  " is playing!");
	}

	public void StopPlaying () {
		canPlay = false;
	}
}
