using System.Collections;
using System;
using UnityEngine;

public delegate void TileAddDelegate(TileArgs e);

/**
 * The argument object that is passed in the Tile Listeners.
 */
public class TileArgs : EventArgs {
	public Player _player;
	public Tile _tile;

	public TileArgs(Tile t, Player p) {
		this._player = p;
		this._tile = t;
	}
}

public delegate void OnTop(TileArgs e);

public class Tile {

	public event TileAddDelegate _tileEventHandler;
	private int price;
	private Property property;
	private string name;

	/**
	 * Represnts a Tile on the board.
	 * 
	 * @name, string, the name of the property.
	 * @group, string, the group of the property.
	 * @price, int, the price of the property to buy.
	 */
	public Tile(string name, string group, int price) :base() {

		//Debug.Log ("Tile(string name, string group, int price)");
		this.price = price;
		this.name = name;
		property = new Property (name, group);
	}

	/**
	 * @name, string, the name of the Tile.
	 */
	public Tile(string name) :base() {
		//Debug.Log ("Tile(string " + name + ")");
		this.name = name;
		property = null;
	}

	/**
     * Returns whether this property is sellable or not.
     * 
     * @return Whether this property is sellable or not.
     */
	public bool IsSellable() {
		// checks if it is an item that is not on sale, like the taxes and other staff
		return !(property != null && property.GetOwner () != null);
	}
	/**
	 * Locates the player on the board.
	 * 
	 * @p, Player, the player that moved on the Tile.
	 */
	public void LocatePlayer(Player p) {
		if(p != null) {
			TileArgs e = new TileArgs (this, p);
			OnPlayerLocation (e);
		}
	}

	/**
	 * This is the location event.
	 * @e, TileArgs, Arguments that are related to the player.
	 */
	public void OnPlayerLocation(TileArgs e) {
		if (_tileEventHandler != null) {
			_tileEventHandler (e);
		}
	}

	/**
	 * Returns the property card.
	 */
	public Property GetProperty() {
		return property;
	}

	/**
	 * Builds a house on the property.
	 */
	public void Build() {
		property.Build();
	}

	/**
	 * Returns the price of the Tile.
	 */
	public int GetPrice() {
		return price;
	}

	/**
	 * Returns the name of the property.
	 */
	public string GetName() {
		return name;
	}

	/**
	 * Return the group that the property owes.
	 */
	public string GetGroup() {
		return property.GetGroup();
	}
}
