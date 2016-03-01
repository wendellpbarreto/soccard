using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	// TODO: set times with variables
	private float turnTime = 16F;

	private int numberOfPlayers;

	public GameObject countdownPrefab;
	public GameObject ball;
	public GameObject redPlayer;
	public GameObject bluePlayer;
	private List<GameObject> redPlayers;
	private List<GameObject> bluePlayers;

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

			redPlayer = Instantiate (redPlayer);
			redPlayer.transform.position = transform.position + new Vector3(redPlayersPositions[i, 0], redPlayersPositions[i, 1], 0);
			redPlayers.Add (redPlayer);
		}

		Player playerScript = (Player)bluePlayers[0].GetComponent (typeof(Player));
		playerScript.SetPlayerName ("Zé da Burra");
	}

	public void StartCountdown(int seconds) {
		StartCoroutine (CORStartCountdown (seconds));
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
		yield return new WaitForSeconds (6);

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
		yield return new WaitForSeconds (16);

		// 4º, stop player turn
		playerScript.StopPlaying ();
	}

	// Use this for initialization
	void Start () {
		SummonPlayers ();

		SummonBall ();

		StartCoroutine(StartMatch ());
	}

	// Update is called once per frame
	void Update () {}
}
