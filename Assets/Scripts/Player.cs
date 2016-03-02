﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public GameObject playButtonPrefab = null;

	private GameObject gameGameObject = null;
	private Game game = null;

	private GameObject deckGameObject = null;
	private Deck deck = null;

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

	public void SetHand(List<GameObject> hand) {
		hand = hand;
	}

	public List<GameObject> GetHand () {
		return hand;
	}

	public bool IsBot() {
		return isBot;
	}

	public bool CanPlay() {
		return canPlay;	
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
//				foreach (GameObject handCard in hand) {
//					Card handCardScript = (Card)handCard.GetComponent (typeof(Card));
//					handCardScript.UnselectCard ();
//				}

				cardScript.SelectCard ();
			}
		}
	}

	private void TogglePlayButton() {
		if (playButtonPrefab.GetComponent<Renderer> ().enabled) {
			playButtonPrefab.GetComponent<Renderer> ().enabled = false;
		} else {
			playButtonPrefab.GetComponent<Renderer> ().enabled = true;
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
			TogglePlayButton ();
		}
	}

	public void StopPlaying () {
		canPlay = false;

		if (IsBot ()) {
			
		} else {
			TogglePlayButton ();
		}
	}

	private bool IsAnyCardSelected() {
		foreach (GameObject handCard in hand) {
			if (handCard.GetComponent<Card> ().IsSelected ()) {
				return true;
			}
		}

		return false;
	}

	private void EndPlayerTurn (List<GameObject> usedCards) {
		canPlay = false;
		TogglePlayButton ();

	}

	public void PlayButtonClicked () {
		if (CanPlay()) {
			canPlay = false;
			TogglePlayButton ();

			List<GameObject> usedCards = game.EnginePlayerTurn ();

			if (usedCards != null) {
				EndPlayerTurn (usedCards);
			} else {
				canPlay = true;
				TogglePlayButton ();

				Debug.Log ("Move isn't possible!");
			}
		}
	}

	// This function is always called before any Start functions and also just after a prefab is instantiated
	void Awake () {
		if (deck == null) {
			deckGameObject = GameObject.Find ("Deck");
			deck = (Deck) deckGameObject.GetComponent (typeof(Deck));
		}

		if (game == null) {
			gameGameObject = GameObject.Find ("Game");
			game = (Game) gameGameObject.GetComponent (typeof(Game));
		}
	}

	// Use this for initialization
	void Start () {
		if (!IsBot ()) {
			playButtonPrefab = Instantiate (playButtonPrefab);
			playButtonPrefab.GetComponent<Renderer> ().enabled = false;

			PlayButton playButton = (PlayButton)playButtonPrefab.GetComponent (typeof(PlayButton));
			playButton.transform.position = new Vector3 (0, -1, 0);
			playButton.SetPlayer (gameObject);

		}
	}

	// Update is called once per frame
	void Update () {

	}
}
