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
		playerName = playerName;	
		isBot = false;
	}

	public String GetPlayerName () {
		if (IsBot()) {
			return playerName + " " + gameObject.GetInstanceID ().ToString().Replace("-", "");
		} else {
			return playerName;
		}
	}

	public List<GameObject> GetHand () {
		return hand;
	}

	public bool IsBot() {
		return isBot;
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

	private void PutCardOnOtherPlayersHand (GameObject card) {
		float x = (gameObject.transform.position.x - 0.5f) + ((float) hand.Count / 6);
		float y = gameObject.transform.position.y - 0.5f;

		card.transform.position = new Vector3 (x, y, 0);
	}

	private void PutCardOnPlayersHand (GameObject card) {
		float x = (gameObject.transform.position.x - 2f) + (float) hand.Count;
		float y = gameObject.transform.position.y + 1f;

		card.transform.localScale += new Vector3 (0.3f, 0.3f, 0);
		card.transform.position = new Vector3 (x, y, 0);

		Card cardScript = (Card)card.GetComponent (typeof(Card));
		cardScript.SetPlayer (gameObject);
	}

	private void PopCardFromDeck () {
		GameObject poppedCard = deck.PopCard ();

		if (IsBot ()) {
			PutCardOnOtherPlayersHand (poppedCard);
		} else { 
			PutCardOnPlayersHand (poppedCard);
		}

		hand.Add (poppedCard);
	}

	IEnumerator CORPopCardsFromDeck(int times) {
		for (int i = 0; i < times; i++) {
			PopCardFromDeck ();

			yield return new WaitForSeconds(0.5f);
		}
	}

	public void PopCardsFromDeck (int times) {
		StartCoroutine (CORPopCardsFromDeck (times));
	}

	public void CardClicked (GameObject card) {
		if (!IsBot ()) {
			Card cardScript = (Card)card.GetComponent (typeof(Card));

			if (cardScript.IsSelected ()) {
				cardScript.UnselectCard ();
			} else {
				foreach (GameObject handCard in hand) {
					Card handCardScript = (Card)handCard.GetComponent (typeof(Card));
					handCardScript.UnselectCard ();
				}

				cardScript.SelectCard ();
			}
		}
	}

	public void StartPlaying () {
		Debug.Log ("Player " + GetPlayerName() +  " is playing at: " + Time.time + " seconds");

		// 0º, tell the player that he can play
		canPlay = true;

		// 1º, pop one card from deck
		PopCardFromDeck ();

		// 2º, is player is bot, play for him
		if (IsBot ()) {

		} else {
			
		}
	}

	public void StopPlaying () {
		canPlay = false;
	}

	// This function is always called before any Start functions and also just after a prefab is instantiated
	void Awake () {
		if (deck == null) {
			deckGameObject = GameObject.Find ("Deck");
			deck = (Deck) deckGameObject.GetComponent (typeof(Deck));
		}
	}

	// Use this for initialization
	void Start () {}

	// Update is called once per frame
	void Update () {

	}
}
