using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	private GameObject deckGameObject;
	private Deck deck;

	private List<GameObject> hand = new List<GameObject> ();

	private bool canPlay = false;
	private bool isBot = true;
	private String playerName = "Bot";

	public void SetPlayerName (String playerName) {
		this.playerName = playerName;	
		this.isBot = false;
	}

	public String GetPlayerName () {
		if (this.IsBot()) {
			return this.playerName + " " + gameObject.GetInstanceID ().ToString().Replace("-", "");
		} else {
			return this.playerName;
		}
	}

	public bool IsBot() {
		return this.isBot;
	}

//	IEnumerator MoveObject(GameObject obj, Vector3 startPosition, Vector3 endPosition, float rate) {
//		bool done = false;
//		float delay = 0;
//
//		obj.transform.position = startPosition;
//
//		while (true) {
//			delay += Time.deltaTime;
//
//			while(delay > rate) {
//				delay = 0;
//
//				obj.transform.position = Vector3.MoveTowards(obj.transform.position, endPosition, rate);
//			}
//		}
//	}

	private void PutCardOnPlayersHand (GameObject card) {
		float x = (gameObject.transform.position.x - 0.5f) + ((float) hand.Count / 6);
		float y = gameObject.transform.position.y - 0.5f;

		card.transform.position = new Vector3 (x, y, 0);
		card.layer = hand.Count;
	}
		
	private void PopCardFromDeck () {
		if (deck == null) {
			deckGameObject = GameObject.Find ("Deck");
			deck = (Deck) deckGameObject.GetComponent (typeof(Deck));
		}
		GameObject poppedCard = deck.PopCard ();

		this.PutCardOnPlayersHand(poppedCard);

		hand.Add (poppedCard);
	}

	IEnumerator CORPopCardsFromDeck(int times) {
		for (int i = 0; i < times; i++) {
			PopCardFromDeck ();

			yield return new WaitForSeconds(1);
		}
	}

	public void PopCardsFromDeck (int times) {
		StartCoroutine (CORPopCardsFromDeck (times));
	}

	public void StartPlaying () {
		Debug.Log ("Player " + this.GetPlayerName() +  " is playing at: " + Time.time + " seconds");

		// Tell the player that he can play
		this.canPlay = true;

		// Pop one card from deck
		this.PopCardFromDeck ();


	}

	public void StopPlaying () {
		this.canPlay = false;
	}

	// Use this for initialization
	void Start () {
		deckGameObject = GameObject.Find ("Deck");
		deck = (Deck) deckGameObject.GetComponent (typeof(Deck));
	}

	// Update is called once per frame
	void Update () {

	}
}
