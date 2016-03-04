using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	// TODO: set times with variables
	private float turnTime = 16F;

	private int blueTeamScore = 0;
	private int redTeamScore = 0;

	private int numberOfPlayers;
	private Stack<GameObject> deskCards;

	public GameObject countdownPrefab;
	public GameObject ball;
	public GameObject redPlayer;
	public GameObject bluePlayer;

	private List<GameObject> redPlayers;
	private List<GameObject> bluePlayers;

	private Stack<GameAction> gameActions;

	enum GameActionState {
		Kick,
		Pass,
		Defense,
		None
	}

	private class GameAction {
		private GameObject player;
		private DateTime time;
		private List<GameObject> usedCards;
		private GameActionState prevActionState;
		private GameActionState currentActionState;
		private int prevActionPoints;
		private int currentActionPoints;

		public GameAction(GameObject player, DateTime time, List<GameObject> usedCards, GameActionState prevActionState, GameActionState currentActionState, int prevActionPoints, int currentActionPoints) {
			this.player = player;
			this.time = time;
			this.usedCards = usedCards;
			this.prevActionState = prevActionState;
			this.currentActionState = currentActionState;
			this.prevActionPoints = prevActionPoints;
			this.currentActionPoints = currentActionPoints;
		}

		public int GetPrevActionPoints() {
			return currentActionPoints;
		}

		public GameActionState GetCurrentActionState() {
			return currentActionState;
		}
	}

	private GameAction GetLatestGameAction () {
		return gameActions.Peek ();
	}

	private void UpRedTeamScore () {
		Debug.Log ("Red team made a GOAL");
		this.redTeamScore += 1;
	}

	private void UpBlueTeamScore () {
		Debug.Log ("Blue team made a GOAL");
		this.blueTeamScore += 1;
	}

	// Summon ball on the field
	private void SummonBall () {
		ball = Instantiate (ball);
	}

	// Move ball to current player position
	private void MoveBall (GameObject ball, Vector3 playerPosition) {
		float newX = playerPosition.x;
		float newY = playerPosition.y;

		if (playerPosition.x > 0) {
			newX -= 1f;
		} else if (playerPosition.x < 0) {
			newX += 1f;
		}

		if (playerPosition.y > 0) {
			newY -= 1f;
		} else if (playerPosition.y < 0) {
			newY += 1f;
		}

		ball.transform.position = new Vector3 (newX, newY, 0);
	}

	// Summon player on the field
	private void SummonPlayers () {
		numberOfPlayers = 6;	

		redPlayers = new List<GameObject> ();
		bluePlayers = new List<GameObject> ();

		int[,] bluePlayersPositions = new int[,]{ {0, -4}, {-5, 2}, {5, 2} };
		int[,] redPlayersPositions = new int[,]{ {-5, -2}, {0, 4}, {5, -2} };

		for (int i = 0; i < numberOfPlayers / 2; i++) {
			bluePlayer = Instantiate (bluePlayer);
			bluePlayer.transform.position = transform.position + new Vector3(bluePlayersPositions[i, 0], bluePlayersPositions[i, 1], 0);
			bluePlayers.Add (bluePlayer);
			bluePlayer.GetComponent<Player> ().IsFromRedTeam (false);

			redPlayer = Instantiate (redPlayer);
			redPlayer.transform.position = transform.position + new Vector3(redPlayersPositions[i, 0], redPlayersPositions[i, 1], 0);
			redPlayers.Add (redPlayer);
		}

		Player playerScript = (Player)bluePlayers[0].GetComponent (typeof(Player));
		playerScript.SetPlayerName ("Zé da Burra");
	}
		
	// Start countdown
	IEnumerator CORStartCountdown(int seconds) {
		GameObject countdown = Instantiate (countdownPrefab);
		countdown.transform.position = new Vector3 (0, 0, 0);
		Animator countdownAnimation = countdown.GetComponent<Animator> ();
		countdownAnimation.speed = Time.deltaTime * 4;

		yield return new WaitForSeconds (seconds);

		Destroy (countdown);
	}

	public void StartCountdown(int seconds) {
		StartCoroutine (CORStartCountdown (seconds));
	}


	// Start game match
	IEnumerator StartMatch () {
		bool matchIsOver = false;

		Debug.Log("Delivering first five cards to each player at: " + Time.time + " seconds");
		for (int i = 0; i < numberOfPlayers / 2; i++) {
			Player bluePlayerScript = (Player)bluePlayers[i].GetComponent (typeof(Player));
			bluePlayerScript.PopCardsFromDeck (5);

			Player redPlayerScript = (Player)redPlayers[i].GetComponent (typeof(Player));
			redPlayerScript.PopCardsFromDeck (5);
		}

		StartCountdown (5);
		yield return new WaitForSeconds (5);

		Debug.Log("Starting match at: " + Time.time + " seconds");
		while (!matchIsOver) {
			for (int i = 0; i < numberOfPlayers / 2; i++) {
				yield return StartCoroutine (StartPlayerTurn (bluePlayers [i]));
				yield return StartCoroutine (StartPlayerTurn (redPlayers [i]));
			}
		}
	}

	// Start player turn
	IEnumerator StartPlayerTurn (GameObject player) {
		Player playerScript = (Player)player.GetComponent (typeof(Player));

		// 0º, start countdown
		StartCountdown (16);

		// 1º, change ball position to match current player's position
		MoveBall (this.ball, player.transform.position);

		// 2º, tell player to play
		playerScript.StartPlaying ();

		// 3º, wait turn time
		yield return new WaitForSeconds (10);

		// 4º, stop player turn
		playerScript.StopPlaying ();
	}

	public void SelectAPossibleCardForMe(GameObject player, List<GameObject> hand) {
		foreach (GameObject handCard in hand) {
			if (IsMovePossible(handCard.GetComponent<Card>().GetCardType())) {
				handCard.GetComponent<Card>().SelectCard();
				player.GetComponent<Player>().PlayButtonClicked();
				break;
			}
		}
	}

	public bool IsMovePossible(String actionCardType) {
		Debug.Log ("Checking if " + GetLatestGameAction ().GetCurrentActionState ().ToString() + " let move " + actionCardType);

		switch (GetLatestGameAction ().GetCurrentActionState ()) {
		case GameActionState.Kick:
			return actionCardType.Equals ("defense");
		case GameActionState.Pass:
			return actionCardType.Equals ("defense");
		case GameActionState.Defense:
			return actionCardType.Equals ("pass") || actionCardType.Equals ("kick");
		case GameActionState.None:
			return actionCardType.Equals ("pass") || actionCardType.Equals ("kick");
		default:
			return false;
		}
	}


	public bool ActionToGameStateVerification(GameObject player, List<GameObject> handSelectedCards) {
		GameObject actionCard = handSelectedCards[0];
		GameActionState gameCurrentActionState = GetLatestGameAction ().GetCurrentActionState ();
		String actionCardType = actionCard.GetComponent<Card> ().GetCardType ();
		int prevActionPoints = GetLatestGameAction ().GetPrevActionPoints ();
		int newActionPoints = -1;
		bool isMovePossible = IsMovePossible(actionCardType);

		if (isMovePossible) {
			GameActionState gameNewActionState = GameActionState.None;

			switch (actionCardType) {
			case "kick": // I'm kicking, nothing to calculate
				prevActionPoints = -1;

				newActionPoints = UnityEngine.Random.Range (0, 10);

				gameNewActionState = GameActionState.Kick;

				Debug.Log ("Player " + player.GetComponent<Player>().GetPlayerName() + " made a " + actionCardType + " with " + newActionPoints + " force.");
				break;
			case "pass": // I'm passind, nothing to calculate
				prevActionPoints = -1;

				newActionPoints = UnityEngine.Random.Range (0, 10);

				gameNewActionState = GameActionState.Pass;

				Debug.Log ("Player " + player.GetComponent<Player>().GetPlayerName() + " made a " + actionCardType + " with " + newActionPoints + " force.");
				break;
			case "defense": // I'm defending, if there is a kick, calculate score, if there is a pass, calculate turn
				newActionPoints = UnityEngine.Random.Range (0, 10);

				gameNewActionState = GameActionState.Defense;

				if (gameCurrentActionState == GameActionState.Kick) { // My oponent has kicked
					Debug.Log ("Player " + player.GetComponent<Player>().GetPlayerName() + " made a " + newActionPoints + " " + actionCardType + " from a kick with " + prevActionPoints + " force.");

					if (newActionPoints >= prevActionPoints) { // I defended
						
					} else { // I didn't defend
						if (player.GetComponent<Player> ().IsFromRedTeam ()) {
							UpBlueTeamScore ();
						} else {
							UpRedTeamScore ();
						}
					}
				} else if (gameCurrentActionState == GameActionState.Pass) { // My oponent has made a pass
					Debug.Log ("Player " + player.GetComponent<Player>().GetPlayerName() + " made a " + newActionPoints + " " + actionCardType + " from a pass with " + prevActionPoints + " force.");

					if (newActionPoints >= prevActionPoints) { // I defended

					} else { // I didn't defend
						
					}
				}
				break;
			case "none": // I'm doing none, if there is a kick, calculate score
				newActionPoints = UnityEngine.Random.Range (0, 10);

				gameNewActionState = GameActionState.None;

				if (gameCurrentActionState == GameActionState.Kick) { // My oponent has kicked
					Debug.Log ("Player " + player.GetComponent<Player>().GetPlayerName() + " passed a kick with " + prevActionPoints + " force.");

					if (player.GetComponent<Player> ().IsFromRedTeam ()) {
						UpBlueTeamScore ();
					} else {
						UpRedTeamScore ();
					}
				}
				break;
			}

			gameActions.Push (new GameAction (player, DateTime.Now, handSelectedCards, gameCurrentActionState, gameNewActionState, prevActionPoints, newActionPoints));
		}

		return isMovePossible;
	}

	// Engine player turn (this function is called by player object when he make a move)
	public bool EnginePlayerTurn(GameObject player) {
		List<GameObject> handSelectedCards = player.GetComponent<Player> ().GetHandSelectedCards ();
		bool isMovePossible = false;

		if (handSelectedCards.Count == 1) {
			isMovePossible = ActionToGameStateVerification(player, handSelectedCards);
		} else if (handSelectedCards.Count > 1) {
			Debug.Log ("Move isn't possible! More than one card selected on player's hand.");

			isMovePossible = false;
		} 

		return isMovePossible;
	}

	// Use this for initialization
	void Start () {
		gameActions = new Stack<GameAction> ();
		gameActions.Push( new GameAction(null, DateTime.Now, null, GameActionState.None, GameActionState.None, -1, -1) );

		deskCards = new Stack<GameObject> ();
		
		SummonPlayers ();

		SummonBall ();

		StartCoroutine(StartMatch ());
	}

	// Update is called once per frame
	void Update () {
		transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);
	}
}
