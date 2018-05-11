﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillNames : MonoBehaviour {

	public Text[] players;

	// Use this for initialization
	void Start () {



	}
	
	// Update is called once per frame
	void Update () {
        foreach(Text t in players) {
            if (t.text.Equals (Game.players [Game.currentPlayer].GetName ())) {
                t.color = Color.yellow;
            } else {
                t.color = Color.black;
            }
        }
	}

	public void addNames() {
		//Debug.Log ("addNames()");
		for (int i = 0; i < AssignTokens.numberOfPlayers; i++) {
			players [i].text = Game.players [i].GetName ();
		}
	}
}
