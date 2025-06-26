using UnityEngine;

[System.Serializable]
public class Card
{
    [Header("卡牌基础信息")]
    public int id;
    public string cardName;
    public string description;
    public int cost;
    public int attack;
    public int health;
    
    [Header("卡牌类型")]
    public CardType cardType;
    public CardRarity rarity;
    
    [Header("视觉资源")]
    public Sprite cardArt;
    public Sprite frameSprite;
    
    public Card(int id, string name, string desc, int cost, int attack, int health, CardType type, CardRarity rarity)
    {
        this.id = id;
        this.cardName = name;
        this.description = desc;
        this.cost = cost;
        this.attack = attack;
        this.health = health;
        this.cardType = type;
        this.rarity = rarity;
    }
    
    public Card(Card original)
    {
        this.id = original.id;
        this.cardName = original.cardName;
        this.description = original.description;
        this.cost = original.cost;
        this.attack = original.attack;
        this.health = original.health;
        this.cardType = original.cardType;
        this.rarity = original.rarity;
        this.cardArt = original.cardArt;
        this.frameSprite = original.frameSprite;
    }
}

public enum CardType
{
    生物,
    法术,
    装备
}

public enum CardRarity
{
    普通,
    稀有,
    史诗,
    传说
}