using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bot : MonoBehaviour{
    
    public GameObject TokenScreen;
    public GameObject RollScreen;
    public GameObject BuyScreen;
    public GameObject BidScreen;
    public GameObject RollButton;
    public Toggle[] Token;
    public Toggle[] Difficulty;
    
    private bool EarlyGame;
    private static int NumOfPlayers;
    private static int NoP_Val; //Val 1 for 1 opponent, 2 for 2 or 3 opponents, 3 for 4+
    private int PurchaseRank;
    private int[] PropertyRank = new int[10];
    private int difficlty;
    private int CurrentPlayer;
    
    private int[] difficulties = {0, 1, 2};
    
    void Start(){
        Debug.Log("BOT started");
        PurchaseRank = 0;
    }
    
    void Update(){
        CurrentPlayer = Game.currentPlayer;
        
        /*
            Token Select:   Done
            Roll Dice:      Done
            Buy:            Processing
            Auction:        -
        */
        
        // Token Select
        if(TokenScreen.activeInHierarchy && AssignTokens.isBot){
            Debug.Log("Bot Ting");
            
            difficlty = GetDifficulty();
            
            int n = Random.Range(0,6);
            Toggle tok = Token[n];
            if(tok.interactable){
                ToggleGroup toktg = tok.GetComponentInParent<ToggleGroup>();
                tok.isOn = true;
                TokenScreen.GetComponentInChildren<Button>().onClick.Invoke();
            }
            
            Debug.Log("Dif: "+difficlty+" n: "+n);
        }
        
        if(checkPlayer(CurrentPlayer) && Game.players[CurrentPlayer].IsBot()){
            //Roll Dice
            if(RollScreen.activeInHierarchy){
                //Debug.Log("rollScreen active");
                //Button newBut = rollScreen.GetComponent<Button>();   //WORKS
                Debug.Log("Bot auto roll");
                NumOfPlayers = AssignTokens.numberOfPlayers;
                RollScreen.GetComponentInChildren<Button>().onClick.Invoke();
            }
            //Buy Property
            else if(BuyScreen.activeInHierarchy){
                EarlyGame = (Game.TurnCounter<GetEarlyGame());
                PropertyRank = createPropertyRank();
                int pos = Game.players[CurrentPlayer].GetPosition();
                string group = Game.board[pos].GetGroup();
                PurchaseRank = GetFinalRank(pos, group); // 1 - 20
                
                int number = Random.Range(0,100);
                int PR_Perc = (PurchaseRank/20)*100;
                
                Toggle[] buy_au_tog = BuyScreen.GetComponentsInChildren<Toggle>();
                Toggle buyTog = buy_au_tog[0];
                Toggle auTog = buy_au_tog[1];
                
                Debug.Log(buyTog+" "+auTog);
                
                switch(difficlty){
                    case 0: //easy
                        break;
                    case 1:
                        break;
                    case 2:
                        if(number<PR_Perc){
                            //Buy prop
                            Game.players[CurrentPlayer].BuyProperty(Game.board[pos], 0);
                            BuyScreen.SetActive(false);
                        }
                        else{
                            //Auction
                            BuyScreen.SetActive(false);
                            BidScreen.SetActive(true);
                            
                        }
                        break;
                    default:
                        Debug.Log("No difficlty found");
                        break;
                }
            }
            else if(BidScreen.activeInHierarchy){
                Debug.Log("bidScreen active");
            }
        }
    }
    
    private int GetEarlyGame(){
        switch (NumOfPlayers){
            case 2: 
                NoP_Val = 1;
                return 20;
            case 3:
            case 4: {
                NoP_Val = 2;
                return 30;}
            case 5:
            case 6: {
                NoP_Val = 3;
                return 45;}
            default: 
                NoP_Val = 0;
                return 25;
        }
    }
    
    private int GetFinalRank(int pos, string group){
        int PR = 0;
        if(EarlyGame) PR+=2;
        PR += GetPropertyRank(group);
        
        int money = Game.players[CurrentPlayer].GetMoney();
        int price = Game.board[pos].GetPrice();
        PR += GetMoneyRating(money, price);
        
        Debug.Log("Group: "+group+" Rank: "+PR);
        
        return PR;
    }
    
    private bool Purchase(int rank){
        return false;
    }
    
    private int GetMoneyRating(int money, int price){
        double perc = (price/money)*100;
        int v = 0;
        if(perc < 80)
            v+=1;
        if(perc < 60)
            v+=1;
        if(perc < 50)
            v+=2;
        if(perc < 40)
            v+=2;
        if(perc < 20)
            v+=3;
        if(perc < 10)
            v+=3;
        return v;
    }
    
    private bool checkPlayer(int i){
        if(i>=0 && i<6){
            return true;
        }
        return false;
    }
    
    private int[] createPropertyRank(){
        if(EarlyGame){
            switch(NoP_Val){
                case 0:
                    return new int[]{1,2,2,5,3,5,3,2,1,2};
                    break;
                case 1:
                    return new int[]{0,1,2,5,3,5,5,3,2,2};
                    break;
                case 2:
                    return new int[]{0,1,2,4,3,5,3,4,5,3};
                    break;
                default:
                    return new int[]{0,0,0,0,0,0,0,0,0,0};
                    break;
            }
        }
        else{
            switch(NoP_Val){
                case 0:
                    return new int[]{0,1,1,4,3,5,4,4,3,4};
                    break;
                case 1:
                    return new int[]{0,1,1,3,2,4,5,3,3,4};
                    break;
                case 2:
                    return new int[]{0,1,2,4,3,5,3,4,5,3};
                    break;
                default:
                    return new int[]{0,0,0,0,0,0,0,0,0,0};
                    break;
            }
        }
    }
    
    private int GetPropertyRank(string group){
        switch(group){
            case "Brown": return PropertyRank[2];
            case "Station": return PropertyRank[1];
            case "Blue": return PropertyRank[3];
            case "Purple": return PropertyRank[4];
            case "Utilities": return PropertyRank[0];
            case "Orange": return PropertyRank[5];
            case "Red": return PropertyRank[6];
            case "Yellow": return PropertyRank[7];
            case "Green": return PropertyRank[8];
            case "Deep_Blue": return PropertyRank[9];
            default: return -1;
        }
    }
    
    private int GetDifficulty(){
        Debug.Log("Difficulty Length"+Difficulty.Length);
        for(int i = 0; i < Difficulty.Length; i++){
            Toggle t = Difficulty[i];
            if(t.isOn){
                Debug.Log("Number:"+i);
                return i;
            }
        }
        return 3;
    }
}