using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BidManager : MonoBehaviour {

	public InputField bidDisplay;
	public Button[] bids;

	/**
	 * Sets the bid on the hidden field.
	 */
	public void SetBid(string bid) {
		bidDisplay.text = bid;
	}
}
