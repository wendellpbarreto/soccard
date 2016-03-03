using UnityEngine;
using System.Collections;

public class PlayButton : MonoBehaviour {

	public GameObject playerPrefab;

	private Player playerScript;

	public void SetPlayer (GameObject playerPrefab) {
		this.playerPrefab = playerPrefab;
	}

	public GameObject GetPlayer () {
		return playerPrefab;
	}

	// Use this for initialization
	void Start () {
		playerScript = (Player) playerPrefab.GetComponent (typeof(Player));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		playerScript.PlayButtonClicked ();	
	}
}
