using UnityEngine;

public class Property : Card {

	private string group;
	private Player owner;
	private int numberOfHouses;

	/**
	 * It is a property card.
	 * @text, string, the name of the property
	 * @group, string, the group the property owns (i.e. Stations, Blue, Orange)
	 */
	public Property(string text, string group) : base(text) {
		this.group = group;
		numberOfHouses = 0;
		owner = null;
	}

	/**
	 * Assign the property to a player
	 * 
	 * @owner, Player, the player that will own this property.
	 */
	public void BuyIt(Player owner) {
		this.owner = owner;
	}

	/**
	 * Build a house on the property.
	 */
	public void Build() {
		if(numberOfHouses < 5) {
			numberOfHouses++;
		}
	}

	/**
	 * Returns the group of the property.
	 */
	public string GetGroup() {
		return group;
	}

	/**
	 * Returns the owner of the property.
	 */
	public Player GetOwner() {
		return owner;
	}

	/**
	 * Returns the number of houses constructed on the property.
	 */
	public int GetNumberOfHouses() {
		return numberOfHouses;
	}
}
