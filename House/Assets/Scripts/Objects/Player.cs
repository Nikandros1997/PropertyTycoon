using System.Collections.Generic;
using UnityEngine;

/**
 * 
 * The player keeps all the information for the possible real world players.
 * 
 */
public class Player
{
	private string name;
	private Token token;
	private bool bot;
	private int money;
	private bool isMoving;
	private bool auctionIt;
	private List<Property> owns;

	/**
	 * 
	 * Constructor
	 * 
	 * @name, string, the name of the player
	 * @token, string, the name of the token
	 * @bot, bool, whether the player is a bot or not
	 * 
	 */

	public Player (string name, string token, bool bot) {
		this.name = name;

		this.token = new Token (token);

		this.bot = bot;
		money = 1500;
		isMoving = true;
		auctionIt = false;

		owns = new List<Property> ();
		// here add the query in order to add the player into the database.
	}

	/**
	 * 
	 * Removes player for an auction
	 */
	public void LeaveAuction() {
		auctionIt = false;
	}

	/**
	 * Buys property from the bank.
	 * 
	 * @t, Tile, Is the tile that the player wants to buy.
	 * @ fromAuction, If it zero, the player buys the property with the normal buying process,
	 * 				otherwise, with value greater than 0, it is auction and it is passes how
	 * 				how much was the highest bid.
	 */
	public void BuyProperty(Tile t, int fromAuction) {
		if (fromAuction == 0) {
			money -= t.GetPrice ();
		} else {
			money -= fromAuction;
		}
		owns.Add (t.GetProperty());
		t.GetProperty ().BuyIt (this);
		BuildingManager.needsUpdate = true;
	}
	/**
	 * Let's a player to join an auction.
	 */
	public void JoinAuction() {
		if(money > 0) {
			auctionIt = true;
		}
	}

	/**
	 * Checks if a player is in an Auction or not.
	 */
	public bool InAuction() {
		return auctionIt;
	}

	/**
	 * Moves the player on the board.
	 * 
	 * @positions, int, how many places a player needs to move.
	 */
	public void movePlayer(int positions) {
		if (token.movePiece (positions)) {
			money += 200;
		}

		BuildingManager.needsUpdate = true;
		// here add the query in order to update the player's position
	}

	/**
	 * It removes money from the balance of the player.
	 * 
	 * @outcoming, int, money that need to be removed from the player.
	 */
	public void payAnotherPlayer(int outcoming) {
		money -= outcoming;
		// here add the query in order to update the player's balance
	}

	/**
	 * It add money to the balance of the player.
	 * 
	 * @incoming, int, money that need to be added to the player.
	 */
	public void getPaidByAnotherPlayer(int incoming) {
		money += incoming;
		// here add the query in order to update the player's balance
	}

	/**
	 * Returns all the properties a player owns.
	 */
	public List<Property> GetProperties() {
		return owns;
	}

	/**
	 * Checks whether the player has passed the go or not.
	 */
	public bool PassedGo() {
		return token.HasPassedGo ();
	}

	/**
	 * Return the current balance of the player.
	 */
	public int GetMoney() {
		return money;
	}

	/**
	 * Returns the name of the token.
	 */
	public string GetToken() {
		return token.GetShape ();
	}

	/**
	 * Returns the name of the player.
	 */
	public string GetName() {
		return name;
	}

	/**
	 * Checks if a player is bot or not.
	 */
	public bool isBot() {
		return bot;
	}

	/**
	 * Gets the position of the token.
	 */
	public int GetPosition() {
		return token.GetPosition ();
	}

	/**
	 * Checks if the player is in the moving process (animation implementation variable)
	 */
	public bool IsMoving() {
		return isMoving;
	}
}

