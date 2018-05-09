using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignTokens : MonoBehaviour {

	public Toggle[] toggles;
	public GameObject chooseTokenGameObj;
	public GameObject startGame;
	public Text header;

	//public static Player[] players = new Player[6];
	public static int numberOfPlayers;

	private int index = 0;
    
    //public for bot
    public static bool isBot;

	private string[] tokens = {"Boot", "Phone", "Goblet", "Spoon", "Cat", "Hatstand"};

	void Start() {
        isBot = false;
	}

	public void setHeader() {
		if(index < 6) {
			header.text = ValidateSetup.names[index] + " Choose Token:";
            if(ValidateSetup.bot[index]){
                isBot = true;
            }
		}
	}

	public void chooseToken() {
		//Debug.Log ("chooseToken()");

		for(int i = 0; i < toggles.Length; i++) {
			Toggle t = toggles [i];

			if(t.isOn) {
				string token = tokens[i];

				//Game.players [index] = new Player (ValidateSetup.names[index], token, ValidateSetup.bot[index]);
				//Debug.Log (tokens[i]);
				Game.players.Add (new Player(ValidateSetup.names[index], token, ValidateSetup.bot[index]));
				ToggleGroup tg = t.GetComponentInParent<ToggleGroup> ();
				tg.SetAllTogglesOff ();
				t.interactable = false;
				t.isOn = false;
                isBot = false;
				index++;
				setHeader ();
			}
		}

		if (index > 5 || ValidateSetup.names [index] == null) {
			numberOfPlayers = index;
			chooseTokenGameObj.SetActive (false);
			startGame.SetActive (true);
		}
	}
}
