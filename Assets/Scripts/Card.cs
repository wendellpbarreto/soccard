using UnityEngine;
using System.Collections;
using System;

public class Card : MonoBehaviour {

	private String cardType;
		
	public void SetCardType (String cardType) {
		this.cardType = cardType;
	}

	public String GetCardType () {
		return this.cardType;
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
