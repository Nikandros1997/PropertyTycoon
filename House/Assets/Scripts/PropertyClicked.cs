using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Required when using Event data.
using UnityEngine.UI;

public class PropertyClicked : MonoBehaviour, IPointerDownHandler {

	public GameObject Parent;
	public int click = 0;
	public int player = 0;
	public static int PlayerChosen = 0;

	public static string propertyName = "";
	public static int no = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/**
	 * When the player left clicks, then shows the property card on the screen.
	 * When the player right clicks, and it is on trade it adds the card in list to be traded.
	 */
	public void OnPointerDown (PointerEventData eventData) {
		PlayerChosen = player;

		if (Input.GetMouseButtonDown (0)) {
			//Debug.Log (Parent.name + "\t" + click);
			List<Property> properties;
			if (player == 0) {
				properties = Game.players [Game.currentPlayer].GetProperties ();
			} else {
				properties = Game.players [TradeManager.PlayerToTrade].GetProperties ();
			}

			int index = 0;

			foreach (Property p in properties) {
				if (p.GetGroup ().Equals (Parent.name)) {
					if (index == click) {
						propertyName = p.GetName();
						//Debug.Log (propertyName + "\t");

						PropertyDisplay.SetSprite (propertyName);


						break;
					}
					index++;
				}
			}
		} else if(Input.GetMouseButtonDown (1) && TradeManager.IsTrading) {
			List<Property> properties;
			if (player == 0) {
				properties = Game.players [Game.currentPlayer].GetProperties ();
			} else {
				properties = Game.players [TradeManager.PlayerToTrade].GetProperties ();
			}

			int index = 0;

			foreach (Property p in properties) {
				if (p.GetGroup ().Equals (Parent.name)) {
					if (index == click) {
						propertyName = p.GetName();
						Debug.Log (propertyName);

						if (player == 0) {
							if (!TradeManager.currentPlayer.Contains (p)) {
								TradeManager.currentPlayer.Add (p);
							} else {
								TradeManager.currentPlayer.Remove (p);
							}
						} else {
							if (!TradeManager.otherPlayer.Contains (p)) {
								TradeManager.otherPlayer.Add (p);
							} else {
								TradeManager.otherPlayer.Remove (p);
							}
						}
						TradeManager.doit = true;
						break;
					}
					index++;
				}
			}

		}
	}
}
