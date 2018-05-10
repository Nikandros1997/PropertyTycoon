using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileListener : MonoBehaviour {

	public GameObject auctionORbuy;
	public static GameObject auction_buy;

	public GameObject ownedText;
	public static GameObject owned;

	public GameObject endTurn;
	public static GameObject endTurn2;

	public InputField bidInteger;
	public static InputField bidInteger2;

	public InputField bidPassword;
	public static InputField bidPassword2;

	public GameObject auction;
	public static GameObject auction2;

	private static Tile currentTile;
	private static Player p;


	public static int localPlayer = 0;
	public static int highestBid = 0;
	public static int highestBidder = -1;
	public static int continuePlayer = 0;

	public List<KeyValuePair<Player,int>> bids;

	// initialize some values.
	void Start() {
		auction_buy = auctionORbuy;
		owned = ownedText;
		endTurn2 = endTurn;
		bidInteger2 = bidInteger;
		bidPassword2 = bidPassword;
		auction2 = auction;
	}

	/**
	 * Is the action method that determines the state of a property.
	 */
	public static void action(Tile currentT, Player p1) {
		currentTile = currentT;
		p = p1;

		if(currentTile.GetProperty() == null) {
			Debug.Log ("It is a card or tax");
			endTurn2.SetActive (true);

		} else if(currentTile.GetProperty().GetOwner() != null || p.Equals(currentTile.GetProperty().GetOwner())) {
			Debug.Log ("It is owned by someone else or yourself.");
			owned.SetActive (true);
			//Remove the money from  the player that the property costs
			//Add the money from  the player that the property costs

			//p.PayRent(currentTile.GetProperty());
		} else if(currentTile.GetProperty ().GetOwner () == null && p.PassedGo() && currentTile.IsSellable()) {
			if (p.GetMoney () < currentTile.GetPrice ()) {
				Debug.Log ("You don't have enough money to buy the property. It will automatically be auctioned.");
				// Message for player not having enough money to buy the property that landed on.
				endTurn2.SetActive (true);
			} else {
				//Debug.Log ("Buy or not panel");
				// Ask action for auction or buy.
				auction_buy.SetActive (true);
			}
		} else {
			Debug.Log ("Not a possible thing to do!!!");
			endTurn2.SetActive (true);
			// SUGGESTIONS: call the build methd.
		}
	}

	/**
	 * Buy a property.
	 */
	public void Buy() {
		//Debug.Log ("Property Bought!!!");
		p.BuyProperty (currentTile, 0);
		endTurn2.SetActive (true);
	}

	/**
	 * Make all the player join the auction, except from 
	 */
	public void PlayersJoinAuction() {
		Debug.Log ("PlayersJoinAuction("+Game.currentPlayer+")");
		continuePlayer = Game.currentPlayer;
		bids = new List<KeyValuePair<Player, int>> ();

		foreach(Player p in Game.players) {
			p.LeaveAuction ();
			if (!p.Equals (Game.players [Game.currentPlayer])) {
				p.JoinAuction ();
			}
		}

		Game.nextPlayer2 ();

		if (MinimumPlayersForAuction (Game.players)) {
			Debug.Log ("MinimumPlayersForAuction (Game.players)");
			auction2.SetActive (true);
		} else {
			// say that there are not enough players to auction properties.
			Debug.Log ("there are not enough players to auction properties.");
		}
	}

	/**
	 * Update fields.
	 */
	public void UpdateInputFieldPassword() {
		bidInteger2.text = bidPassword2.text;
	}

	/**
	 * The player bids money to the auction.
	 */
	public void Bid() {
		if(bidInteger2.text.Equals("")) {
			return;
		}
		int bid = Int32.Parse(bidInteger2.text);

		if (HasTheMoney (Game.players [Game.currentPlayer], bid)) {
			
			bidPassword2.text = "";
			bidInteger2.text = "";

			bids.Add (new KeyValuePair<Player, int> (Game.players [Game.currentPlayer], bid));

			Game.nextPlayer2 ();

			while(!Game.players[Game.currentPlayer].InAuction()) {
				Game.nextPlayer2 ();
			}
		} else {
			Debug.Log ("Sorry, "
				+ Game.players[Game.currentPlayer].GetName() + "! "
				+ "You have insufficient funds for this "
				+ "bid. Check if you can bid something "
				+ "smaller. Otherwise, withdraw.");
		}
	}

	/**
	 * Checks if the player passes has money.
	 * 
	 * @p, Player, a player from the game.
	 * @bidOrigin, int, 
	 */
	private bool HasTheMoney(Player p, int bidOrigin) {
		return BidSum(bidOrigin) <= p.GetMoney();
	}

	/**
	 * Calculates the sum of all the bids that are owned by the player who bids right now.
	 * 
	 * @bidOrigin, int, the current bid of the player.
	 */
	private int BidSum(int bidOrigin) {
		int bid = 0;

		foreach(KeyValuePair<Player, int> entry in bids) {
			bid += entry.Value;
		}

		return (bid + bidOrigin);
	}

	/**
	 * Remove a player from the auction.
	 */
	public void LeaveAuction() {
		Debug.Log (Game.currentPlayer);
		Debug.Log("Hey,"
			+ Game.players[Game.currentPlayer].GetName() + "! You "
			+ "just left the Auction.");

		Game.players[Game.currentPlayer].LeaveAuction();

		while(!Game.players[Game.currentPlayer].InAuction()) {
			Debug.Log (Game.currentPlayer);
			Game.nextPlayer2 ();
		}

		if (!MinimumPlayersForAuction (Game.players)) {
			if(bids.Count > 0) {
				Debug.Log ("Auction is done. " + Game.players [Game.currentPlayer].GetName () + " got the property in the price of " + BidSum (0));

				// player buy property
				Game.players [Game.currentPlayer].BuyProperty (Game.board [Game.players [continuePlayer].GetPosition ()], BidSum (0));
			} else {
				Debug.Log ("Auction is done, but noone bought anything.");
			}
			// continue from last player stored in continuePlayer
			Game.currentPlayer = continuePlayer;
			BuildingManager.needsUpdate = true;
			// hide auction panel
			auction.SetActive (false);
			// show option that a player has before its go finishes.
			endTurn2.SetActive (true);
		} 
	}

	/**
	 * Returns true if there at least two players in the auction, false otherwise.
	 * 
	 * @players, List<Player>, the list of all the players that play the game.
	 */
    public static bool MinimumPlayersForAuction(List<Player> players) {
        int counter = 0;

		foreach (Player p in players) {
			Debug.Log (p.InAuction());
			if(p.InAuction()) {
				counter++;
			}
		}
		Debug.Log ("Counter: " + counter);
		return counter > 1;
    }
}


