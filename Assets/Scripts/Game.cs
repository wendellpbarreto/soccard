using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	private int numberOfPlayers;

	public GameObject redPlayer;
	public GameObject bluePlayer;

	private List<GameObject> redPlayers;
	private List<GameObject> bluePlayers;

	// Use this for initialization
	void Start () {
		numberOfPlayers = 6;	

		redPlayers = new List<GameObject> ();
		bluePlayers = new List<GameObject> ();

		int[,] posB = new int[,]{ {-5, 2}, {0, -4}, {5, 2} };
		int[,] posR = new int[,]{ {-5, -2}, {0, 4}, {5, -2} };
			
		for (int i = 0; i < numberOfPlayers/2; i++) {
			redPlayer = Instantiate (redPlayer);
			Player redPlayerScript = (Player)redPlayer.GetComponent (typeof(Player));
			redPlayerScript.Play ();


			redPlayer.transform.position = transform.position + new Vector3(posR[i, 0], posR[i, 1], 0);
			redPlayers.Add (redPlayer);


			bluePlayer = Instantiate (bluePlayer);
			bluePlayer.transform.position = transform.position + new Vector3(posB[i, 0], posB[i, 1], 0);
			bluePlayers.Add (bluePlayer);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
