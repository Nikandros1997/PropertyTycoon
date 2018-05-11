using System;

/**
 * This is the base class for all the cards of the game.
 */

public class Card {

	private string name;

	public Card (string name) {
		this.name = name;
	}

	public string GetName() {
		return name;
	}

	/**
	 * This is going to be a listener which waits for the player to access it and have the effects on him.
	 */
	public void UseCard(Player p) {

	}
}
