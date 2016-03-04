using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Util;

public class Deck : MonoBehaviour {

	private int numberOfCards = 0;

	private readonly int cardKickAmount = 20;
	private readonly int cardDefenseAmount = 20;
	private readonly int cardPassAmount = 20;

	public GameObject cardKickPrefab;
	public GameObject cardDefensePrefab;
	public GameObject cardPassPrefab;

	private List<CardSpec> cards;

	private class CardSpec {
		public String cardTypeName;

		public CardSpec (String cardTypeName) {
			this.cardTypeName = cardTypeName;
		}

		public String GetCardTypeName () {
			return this.cardTypeName;
		}
	}

	public GameObject PopCard () {
		CardSpec poppedCard = null;
		while (poppedCard == null) {
			int randomNumber = UnityEngine.Random.Range (0, cards.Count - 1);

			poppedCard = cards [randomNumber];
			cards.RemoveAt (randomNumber);
		}

		GameObject card;
		switch (poppedCard.GetCardTypeName ()) {
			case "kick":
				card = Instantiate (cardKickPrefab);
				break;
			case "defense": 
				card = Instantiate (cardDefensePrefab);
				break;
			case "pass":
				card = Instantiate (cardPassPrefab);
				break;
			default:
				card = Instantiate (cardPassPrefab);
			break;
		}

		card.GetComponent<Card> ().SetCardType (poppedCard.GetCardTypeName ());
		card.transform.position = transform.position;

		return card;
	}

	private void PopulateDeck () {
		cards = new List<CardSpec> ();

		for (int i = 0; i < cardKickAmount; i++) {
			cards.Add( new CardSpec("kick"));
		}

		for (int i = 0; i < cardDefenseAmount; i++) {
			cards.Add( new CardSpec("defense"));
		}

		for (int i = 0; i < cardPassAmount; i++) {
			cards.Add( new CardSpec("pass"));
		}

		cards.Shuffle ();
	}

	void Start () {
		transform.position = new Vector3 (2.3f, 0, 0);

		PopulateDeck ();
	}
	
	void Update () {}
		
}
