using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeManager : MonoBehaviour {

	public static int PlayerToTrade = -1;
	public static bool IsTrading = false;

	public static List<Property> currentPlayer;
	public Text[] currentPlayerText1;
	public Text[] currentPlayerText2;
	public Text[] currentPlayerText3;

	public static List<Property> otherPlayer;
	public Text[] otherPlayerText1;
	public Text[] otherPlayerText2;
	public Text[] otherPlayerText3;

	public GameObject currentPlayerDisplay;
	public GameObject otherPlayerDisplay;
	public GameObject noSamePlayerWarning;
	public GameObject TradeM;

	public GameObject[] trades;

	public GameObject[] contents;

	public static bool doit = false;

	public int TradeOption = -1;

	// Use this for initialization
	void Start () {
		//Debug.Log ("TRADEMANAGER");
	}

	// Update is called once per frame
	void Update () {
		if(doit) {
			if (TradeOption == -1) {

				foreach (Text t in currentPlayerText1) {
					t.text = "";
					t.gameObject.SetActive (false);
				}

				foreach (Text t in otherPlayerText1) {
					t.text = "";
					t.gameObject.SetActive (false);
				}


				for (int i = 0; i < currentPlayer.Count; i++) {
					currentPlayerText1 [i].text = currentPlayer [i].GetName ();
					currentPlayerText1 [i].gameObject.SetActive (true);
				}

				for (int i = 0; i < otherPlayer.Count; i++) {
					otherPlayerText1 [i].text = otherPlayer [i].GetName ();
					otherPlayerText1 [i].gameObject.SetActive (true);
				}
			} else if (TradeOption == 0) {

				foreach (Text t in currentPlayerText2) {
					t.text = "";
					t.gameObject.SetActive (false);
				}

				foreach (Text t in otherPlayerText2) {
					t.text = "";
					t.gameObject.SetActive (false);
				}

				for (int i = 0; i < otherPlayer.Count; i++) {
					currentPlayerText2 [i].text = otherPlayer [i].GetName ();
					currentPlayerText2 [i].gameObject.SetActive (true);
				}

				for (int i = 0; i < currentPlayer.Count; i++) {
					otherPlayerText2 [i].text = currentPlayer [i].GetName ();
					otherPlayerText2 [i].gameObject.SetActive (true);
				}

			} else {
				foreach (Text t in currentPlayerText3) {
					t.text = "";
					t.gameObject.SetActive (false);
				}

				foreach (Text t in otherPlayerText3) {
					t.text = "";
					t.gameObject.SetActive (false);
				}

				for (int i = 0; i < currentPlayer.Count; i++) {
					currentPlayerText3 [i].text = currentPlayer [i].GetName ();
					currentPlayerText3 [i].gameObject.SetActive (true);
				}

				for (int i = 0; i < otherPlayer.Count; i++) {
					otherPlayerText3 [i].text = otherPlayer [i].GetName ();
					otherPlayerText3 [i].gameObject.SetActive (true);
				}
			}
			doit = false;
		}
	}

	public void Trade() {
		IsTrading = true;

	}

	public void Trade(int number) {
		
		if(Game.rolled) {
			if (number != Game.currentPlayer) {
				TradeWithPlayer (number);
				otherPlayerDisplay.SetActive(true);
				TradeM.SetActive (true);
			} else {
				//Debug.Log ("You cannot trade with yourself.");
				noSamePlayerWarning.SetActive(true);
			}
			currentPlayerDisplay.SetActive (true);
		}
	}

	public void Offer(int option) {

		if(currentPlayer.Count == 0 || otherPlayer.Count == 0) {
			return;
		}
		trades [option].SetActive (true);
		TradeOption = option;

		if(option == 0) {
			Debug.Log ("Counter offer 1");
			TradeM.SetActive (false);
			trades [option].SetActive (true);
			trades [option + 1].SetActive (false);
		} else if(option == 1) {
			Debug.Log ("Counter offer 2");
			TradeM.SetActive (false);
			trades [option].SetActive (true);
			trades [option - 1].SetActive (false);
		}

		doit = true;
	}

	public void TradeWithPlayer(int number) {
		PlayerToTrade = number;

		currentPlayer = new List<Property> ();
		otherPlayer = new List<Property> ();

		BuildingManager.needsUpdate = true;
	}

	public void AcceptOffer() {
		foreach (Text t in currentPlayerText1) {
			t.text = "";
			t.gameObject.SetActive (false);
		}

		foreach (Text t in otherPlayerText1) {
			t.text = "";
			t.gameObject.SetActive (false);
		}

		foreach (Text t in currentPlayerText2) {
			t.text = "";
			t.gameObject.SetActive (false);
		}

		foreach (Text t in otherPlayerText2) {
			t.text = "";
			t.gameObject.SetActive (false);
		}

		foreach (Text t in currentPlayerText3) {
			t.text = "";
			t.gameObject.SetActive (false);
		}

		foreach (Text t in otherPlayerText3) {
			t.text = "";
			t.gameObject.SetActive (false);
		}


		Player p1 = Game.players [Game.currentPlayer];
		Player p2 = Game.players [PlayerToTrade];

		List<Property> owned1 = p1.GetProperties ();
		List<Property> owned2 = p2.GetProperties ();

		foreach(Property p in currentPlayer) {
			owned1.Remove (p);
			owned2.Add (p);
		}

		foreach(Property p in otherPlayer) {
			owned2.Remove (p);
			owned1.Add (p);
		}

		currentPlayer = new List<Property> ();
		otherPlayer = new List<Property> ();

		doit = true;

		otherPlayerDisplay.SetActive (false);

		BuildingManager.needsUpdate = true;
	}

	public void StopTrade() {
		IsTrading = false;
	}
}
