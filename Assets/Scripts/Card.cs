using UnityEngine;
using System.Collections;
using System;

public class Card : MonoBehaviour {

	public GameObject playerPrefab;

	private Player playerScript;
	private String cardType;
	private bool isSelected = false;

	public void SetCardType (String cardType) {
		this.cardType = cardType;
	}

	public String GetCardType () {
		return this.cardType;
	}

	public void SetPlayer (GameObject playerPrefab) {
		this.playerPrefab = playerPrefab;
	}

	public GameObject GetPlayer () {
		return this.playerPrefab;
	}

	public bool IsSelected() {
		return isSelected;
	}

	public void SelectCard () {
		if (!IsSelected ()) {
			transform.position += new Vector3 (0, 1, 0);
			isSelected = true;
		}
	}

	public void UnselectCard () {
		if (IsSelected ()) {
			transform.position -= new Vector3 (0, 1, 0);
			isSelected = false;
		}
	}
		
	// Use this for initialization
	void Start () {
		playerScript = (Player) playerPrefab.GetComponent (typeof(Player));
	}

	// Update is called once per frame
	void Update() {
		
	}

	void OnMouseDown() {
		playerScript.CardClicked (gameObject);	
	}
}
