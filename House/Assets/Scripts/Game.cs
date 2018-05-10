using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {
	public GameObject[] boardGraphics;
	public static Tile[] board = new Tile[40];
	public Text RollField;
	public Text Buy_Auction_Property;

	public GameObject[] tokens;
	public static Dictionary<string, GameObject> Tokens;
	//public static Player[] players = new Player[6];
	public static List<Player> players = new List<Player> ();

	public static int currentPlayer = -1;
	public static Dice dice = new Dice();

	private int DoublesCounter = 0;

	public GameObject Buy_Auction;

	public bool HasStarted = false;

	public static databaseConnection Database = new databaseConnection();

	public static bool rolled = false;


	// Use this for initialization
	void Start () {
		//Debug.Log ("Game Object");
	}

	/**
	 * Starts the game.
	 */
	public void StartGame() {
		HasStarted = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(dice.IsDoneRolling()) {
			RollField.text = dice.GetDie (0) + "+" + dice.GetDie (1);
			RollField.gameObject.SetActive (true);
		}
		if(HasStarted && currentPlayer > -1) {
			Buy_Auction_Property.text = players [currentPlayer].GetName ();
		}
	}

	/**
	 * Checks if it rolled.
	 */
	public void initializeRoll() {
		rolled = false;
	}

	/**
	 * Does all the rolling procedure.
	 */
	public void RollDice() {

		int roll = dice.Roll ();

		rolled = true;

		if (dice.IsDoubles ()) {
			DoublesCounter++;

			if(DoublesCounter == 3) {
				DoublesCounter = 0;

				// Send player in jail.
				Debug.Log ("In jail");
				// Add the code for the player to go to the jail.
			}
		} else {

			DoublesCounter = 0;
		}

		Player p = players [currentPlayer];

		p.movePlayer (roll);

		board [p.GetPosition ()].LocatePlayer (p);

		Vector3 pos = boardGraphics [p.GetPosition ()].transform.position;

		Tokens [p.GetToken ()].transform.position = pos;
	}

	/**
	 * Add all the tokens in a dictionary.
	 */
	public void AddTokens() {
		Tokens = new Dictionary<string, GameObject> ();

		for(int i = 0; i < 6; i++) {
			Tokens.Add (tokens[i].name, tokens[i]);
		}

		tokens = null;
	}

	/**
	 * Call next player.
	 */
	public void nextPlayerSimple() {
		currentPlayer++;
		//Debug.Log ("Next player is player no. " + currentPlayer);

		if(currentPlayer >= AssignTokens.numberOfPlayers) {
			currentPlayer = 0;
		}

		BuildingManager.needsUpdate = true;
	}

	/**
	 * Call next player.
	 */
	public static void nextPlayer2() {
		Debug.Log ("Next player is player no. " + currentPlayer);
		currentPlayer++;

		if(currentPlayer >= AssignTokens.numberOfPlayers) {
			currentPlayer = 0;
		}

		BuildingManager.needsUpdate = true;
	}
}
