﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ValidateSetup : MonoBehaviour {

	// Input Name Fields
	public InputField[] fields;

	public InputField timer;

	public GameObject SetupGame;
	public GameObject ChooseToken;

	// Warnings
	public GameObject playersWarningMessage;
	public GameObject timerWarningMessage;

	private Toggle tog;

	public static string[] names = new string[6];
	public static bool[] bot = new bool[6];

	/**
	 * Validate all the fields on the setup game page.
	 */
	public bool validateStart() {
		//Debug.Log ("validateStart()");

		int player = 0;

		// Get the names from fields and save then in an array.
		for(int i = 0; i < fields.Length; i++) {
			string content = fields [i].text;

			if(!content.Equals("")) {

				Toggle a = fields [i].FindSelectableOnRight () as Toggle;

				if (a.isOn) {
					bot[player] = true;
				} else {
					bot[player] = false;
				}

				//Debug.Log ("Is player no " + (player + 1) + " a bot? " + bot[player]);

				names [player] = content;
				player++;
			}
		}

		if (player >= 2) {
			int minutes = 0;

			if (timer.IsActive () && !timer.text.Equals ("") && int.Parse (timer.text) >= 5) {
				minutes = int.Parse (timer.text);

				//Debug.Log ("The timer is " + minutes);

				//Debug.Log ("Start Timed Game");
				return true;
			} else if (timer.IsActive()) {
				//Debug.Log ("No game");
				timerWarningMessage.SetActive (true);
				return false;
			} else if (!timer.IsActive()){
				//Debug.Log ("Start Normal Game");
				return true;
			}
		} else {
			//Debug.Log ("No game");
			playersWarningMessage.SetActive (true);
		}
		return false;
	}

	/**
	 * Show next page after setup.
	 */
	public void startGame () {
		//Debug.Log ("startGame()");

		if(validateStart()) {
			SetupGame.SetActive (false);
			ChooseToken.SetActive (true);
		}
	}

	/**
	 * Add name when bot is added and doesn't contain name.
	 */
	public void addBotName(InputField field) {
		//Debug.Log ("addBotName()");

		if (tog.isOn && field.text.Equals ("")) {
			field.text = "BOT";
		} else if (field.text.Equals ("BOT")) {
			field.text = "";
		}
	}

	// this is the AI toggle button
	public void getToggle(Toggle tog) {
		this.tog = tog;
	}
}
