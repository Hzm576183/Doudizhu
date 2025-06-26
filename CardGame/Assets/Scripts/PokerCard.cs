using UnityEngine;

public enum Suit
{
    Hearts,   // 红桃
    Diamonds, // 方块
    Clubs,    // 梅花
    Spades,   // 黑桃
    Joker     // 王牌
}

public enum CardValue
{
    Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10,
    Jack = 11, Queen = 12, King = 13, Ace = 14, Two = 15,
    SmallJoker = 16, BigJoker = 17
}

[System.Serializable]
public class PokerCard
{
    public Suit suit;
    public CardValue value;
    public bool isSelected = false;
    
    public PokerCard(Suit cardSuit, CardValue cardValue)
    {
        suit = cardSuit;
        value = cardValue;
    }
    
    public string GetCardName()
    {
        if (value == CardValue.SmallJoker) return "小王";
        if (value == CardValue.BigJoker) return "大王";
        
        string suitName = suit switch
        {
            Suit.Hearts => "红桃",
            Suit.Diamonds => "方块", 
            Suit.Clubs => "梅花",
            Suit.Spades => "黑桃",
            _ => ""
        };
        
        string valueName = value switch
        {
            CardValue.Jack => "J",
            CardValue.Queen => "Q", 
            CardValue.King => "K",
            CardValue.Ace => "A",
            CardValue.Two => "2",
            _ => ((int)value).ToString()
        };
        
        return suitName + valueName;
    }
    
    public int GetPower()
    {
        return (int)value;
    }
}