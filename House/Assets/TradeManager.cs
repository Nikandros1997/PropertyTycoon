using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeManager : MonoBehaviour {

	public static int PlayerToTrade = -1;
	public static bool IsTrading = false;

	public static List<Property> currentPlayer;
	public Text[] currentPlayerText;

	public static List<Property> otherPlayer;
	public Text[] otherPlayerText;

	public GameObject currentPlayerDisplay;
	public GameObject otherPlayerDisplay;
	public GameObject noSamePlayerWarning;
	public GameObject TradeM;

	public static bool doit = false;

	// Use this for initialization
	void Start () {
		//Debug.Log ("TRADEMANAGER");
	}

	// Update is called once per frame
	void Update () {
		if(doit) {
			foreach(Text t in currentPlayerText) {
				t.text = "";
				t.gameObject.SetActive (false);
			}

			foreach(Text t in otherPlayerText) {
				t.text = "";
				t.gameObject.SetActive (false);
			}

			for(int i = 0; i < currentPlayer.Count; i++) {
				currentPlayerText [i].text = currentPlayer [i].GetName ();
				currentPlayerText [i].gameObject.SetActive (true);
			}

			for(int i = 0; i < otherPlayer.Count; i++) {
				otherPlayerText [i].text = otherPlayer [i].GetName ();
				otherPlayerText [i].gameObject.SetActive (true);
			}

			doit = false;
		}
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

	public void TradeWithPlayer(int number) {
		PlayerToTrade = number;

		currentPlayer = new List<Property> ();
		otherPlayer = new List<Property> ();

		IsTrading = true;

		BuildingManager.needsUpdate = true;
	}

	public void StopTrade() {
		IsTrading = false;
	}
}
