using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Deck : MonoBehaviour {

	public GameObject cardPrefab;

	private List<CCard> cards;

	private List<CardType> deckSpecs;

	private class CCard {
		public String cardTypeName;

		public CCard (String cardTypeName) {
			this.cardTypeName = cardTypeName;
		}

		public String GetCardTypeName () {
			return this.cardTypeName;
		}
	}

	private class CardType {
		public String cardTypeName;
		public int numberOfCardsOfType;

		public CardType(String cardTypeName, int numberOfCardsOfType) {
			this.cardTypeName = cardTypeName;
			this.numberOfCardsOfType = numberOfCardsOfType;
		}


	}

	public GameObject PopCard () {
		CCard poppedCard = null;

		while (poppedCard == null) {
			int randomNumber = UnityEngine.Random.Range (0, cards.Count);

			poppedCard = cards [randomNumber];
			cards.RemoveAt (randomNumber);
		}

		GameObject card = Instantiate (cardPrefab);
		card.transform.position = transform.position;

		Card cardScript = (Card)card.GetComponent (typeof(Card));
		cardScript.SetCardType (poppedCard.GetCardTypeName ());

		return card;
	}

	void Start () {
		cards = new List<CCard> ();
		deckSpecs = new List<CardType>() { 
			new CardType("kick", 20),
			new CardType("defense", 20),
			new CardType("pass", 20),
		};

		transform.position = new Vector3 (1, 0, 0);

		foreach ( CardType deckSpec in deckSpecs ) {
			for (int i = 0; i < deckSpec.numberOfCardsOfType; i++) {
				CCard card = new CCard (deckSpec.cardTypeName);
				cards.Add (card);
			}
		}
	}
	
	void Update () {}
		
}
