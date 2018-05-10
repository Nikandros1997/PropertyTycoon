using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bot : MonoBehaviour
{
	public GameObject TokenScreen;
	public GameObject RollScreen;
	public GameObject BuyScreen;
	public GameObject BidScreen;
	public GameObject EndTurnScreen;
	public GameObject OwnedScreen;
	public GameObject RollButton;
	public Toggle[] Token;
	public Toggle[] Difficulty;

	private bool EarlyGame;
	private static int NumOfPlayers;
	private static int NoP_Val;
	//Val 1 for 1 opponent, 2 for 2 or 3 opponents, 3 for 4+
	private int PurchaseRank;
	private int difficlty;
	private int CurrentPlayer;
	private int[] PropertyRank = new int[10];
	private Toggle[] buy_au_tog;

	private int[] difficulties = { 0, 1, 2 };

	void Start ()
	{
		Debug.Log ("BOT started");
		PurchaseRank = -1;
	}

	void Update ()
	{
		CurrentPlayer = Game.currentPlayer;
		// Token Select
		if (TokenScreen.activeInHierarchy && AssignTokens.isBot) {
			Debug.Log ("Token");

			difficlty = GetDifficulty ();

			int n = Random.Range (0, 6);
			Toggle tok = Token [n];
			if (tok.interactable) {
				ToggleGroup toktg = tok.GetComponentInParent<ToggleGroup> ();
				tok.isOn = true;
				TokenScreen.GetComponentInChildren<Button> ().onClick.Invoke ();
			}

			Debug.Log ("Dif: " + difficlty + " token: " + n);
   		}
		//Check if player value is valid and player is a bot.
		if (checkPlayer (CurrentPlayer) && Game.players [CurrentPlayer].IsBot ()) {
			//Roll Dice
			if (RollScreen.activeInHierarchy) {
				//Debug.Log("rollScreen active");
				//Button newBut = rollScreen.GetComponent<Button>();   //WORKS
				Debug.Log ("Bot auto roll");
				NumOfPlayers = AssignTokens.numberOfPlayers;
				RollScreen.GetComponentInChildren<Button> ().onClick.Invoke ();
			}
            //Buy Property
            else if (BuyScreen.activeInHierarchy) {
				//Debug.Log ("Boo");
				Debug.Log ("Buy/Auction");
				EarlyGame = (Game.TurnCounter < NumOfPlayers*7);
				PropertyRank = createPropertyRank ();

				int pos = Game.players [CurrentPlayer].GetPosition ();
				string group = Game.board [pos].GetGroup ();

				PurchaseRank = GetFinalRank (pos, group); // 1 - 20

				if (PurchaseRank >= 0) {
					//Debug.Log ("Group: " + group + " Rank: " + PurchaseRank);

					double number = Random.Range (0, 100);
					double PR_Perc = ((double)PurchaseRank / 20) * 100;

					buy_au_tog = BuyScreen.GetComponentsInChildren<Toggle> ();

					//Debug.Log (buyTog.name + " and " + auTog.name+" found");
					Debug.Log ("PurchaseRank: "+PurchaseRank+" Number: " + number +" Percentage: "+PR_Perc);

					switch (difficlty) {
					case 0: //easy
						BuyOrAuction (number < PR_Perc - 40);
						break;
					case 1:
						BuyOrAuction (number < PR_Perc - 20);
						break;
					case 2:
						BuyOrAuction (number < PR_Perc);
						break;
					default:
						Debug.Log ("No difficlty found");
						break;
					}
				}
			}
			//Buys or Auctions
			else if (BidScreen.activeInHierarchy) {
//				Debug.Log ("Bid"+CurrentPlayer);
//				InputField[] inpFields = BidScreen.GetComponentsInChildren<InputField>();
//				Debug.Log ("InputFields: " + inpFields);
				//BidScreen.GetComponentInChildren<Button>().onClick.Invoke();
                //Button b = BidScreen.FindWithTag("botAuWarning").GetComponentInChildren<Button>();
                Button warnMsgGO = GameObject.Find("AuctionWarningMessage").GetComponentInChildren<Button>();
                Debug.Log("Msg: "+warnMsgGO.name);
                if (GameObject.Find("AuctionWarningMessage").activeInHierarchy)
                    warnMsgGO.GetComponentInChildren<Button>().onClick.Invoke();
                
                GameObject bidTxtGO = GameObject.Find("InputField Number");
                
                //bidTxtGO.GetComponentInChildren<Text>().text = CalculateBid();
                //Button biBtn = GameObject.Find("Bid").GetComponentInChildren<Button>();
			}
			//Ends Turn
			else if (EndTurnScreen.activeInHierarchy){
				Debug.Log ("EndTurn "+CurrentPlayer);
				EndTurnScreen.GetComponentInChildren<Button> ().onClick.Invoke ();
			}
			//Changes output when bot lands on owned tile.
			else if (OwnedScreen.activeInHierarchy){
				Debug.Log ("Owned");
				string s = "Bot has paid you money";
				string b = "Great. Thanks!";
				OwnedScreen.GetComponentInChildren<Text> ().text = s;
				OwnedScreen.GetComponentInChildren<Button> ().GetComponentInChildren<Text>().text = b;
			}
		}
	}
    
    private string CalculateBid(){
        int pos = Game.players [CurrentPlayer].GetPosition ();
        int price = Game.board [pos].GetPrice ();
        string group = Game.board [pos].GetGroup();
        float min = (float)(price*0.75);
        float max = (float)(price*1.25);
        string bid = Random.Range(min, max)+"";
        Debug.Log("Bid: "+bid);
        return bid;
    }

	private void BuyOrAuction(bool buy){
		int i = buy ? 0 : 1;
		buy_au_tog [i].isOn = !buy_au_tog [i].isOn;
		//Toggle (buy_au_tog [i].isOn);
	}

	private bool Toggle(bool b){
		return !b;
	}

	private int GetFinalRank (int pos, string group)
	{
		int PR = 0;
		if (EarlyGame)
			PR += 2;
		PR += GetPropertyRank (group);

		int money = Game.players [CurrentPlayer].GetMoney ();
		int price = Game.board [pos].GetPrice ();
		PR += GetMoneyRating (money, price);

		return PR;
	}

	private int GetMoneyRating (int money, int price)
	{
		double perc = (price / money) * 100;
		int v = 0;
		if (perc < 80)
			v += 1;
		if (perc < 60)
			v += 1;
		if (perc < 50)
			v += 2;
		if (perc < 40)
			v += 2;
		if (perc < 20)
			v += 3;
		if (perc < 10)
			v += 3;
		return v;
	}

	private bool checkPlayer (int i)
	{
		if (i >= 0 && i < 6) {
			return true;
		}
		return false;
	}

	private int[] createPropertyRank ()
	{
		if (EarlyGame) {
			switch (NoP_Val) {
			case 0:
				return new int[]{ 1, 2, 2, 5, 3, 5, 3, 2, 1, 2 };
			case 1:
				return new int[]{ 0, 1, 2, 5, 3, 5, 5, 3, 2, 2 };
			case 2:
				return new int[]{ 0, 1, 2, 4, 3, 5, 3, 4, 5, 3 };
			default:
				return new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
			}
		} else {
			switch (NoP_Val) {
			case 0:
				return new int[]{ 0, 1, 1, 4, 3, 5, 4, 4, 3, 4 };
			case 1:
				return new int[]{ 0, 1, 1, 3, 2, 4, 5, 3, 3, 4 };
			case 2:
				return new int[]{ 0, 1, 2, 4, 3, 5, 3, 4, 5, 3 };
			default:
				return new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
			}
		}
	}

	private int GetPropertyRank (string group)
	{
		switch (group) {
		case "Brown":
			return PropertyRank [2];
		case "Station":
			return PropertyRank [1];
		case "Blue":
			return PropertyRank [3];
		case "Purple":
			return PropertyRank [4];
		case "Utilities":
			return PropertyRank [0];
		case "Orange":
			return PropertyRank [5];
		case "Red":
			return PropertyRank [6];
		case "Yellow":
			return PropertyRank [7];
		case "Green":
			return PropertyRank [8];
		case "Deep_Blue":
			return PropertyRank [9];
		default:
			return -1;
		}
	}

	private int GetDifficulty ()
	{
		Debug.Log ("Difficulty Length" + Difficulty.Length);
		for (int i = 0; i < Difficulty.Length; i++) {
			Toggle t = Difficulty [i];
			if (t.isOn) {
				Debug.Log ("Number:" + i);
				return i;
			}
		}
		return 3;
	}
}