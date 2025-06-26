using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("斗地主牌库管理")]
    public List<PokerCard> fullDeck = new List<PokerCard>();
    public List<PokerCard> playerHand = new List<PokerCard>();
    public List<PokerCard> leftPlayerHand = new List<PokerCard>();
    public List<PokerCard> rightPlayerHand = new List<PokerCard>();
    public List<PokerCard> landlordCards = new List<PokerCard>(); // 地主牌(3张)
    
    void Start()
    {
        CreateFullDeck();
        ShuffleDeck();
        DealCards();
        DisplayPlayerCards();
    }
    
    void CreateFullDeck()
    {
        fullDeck.Clear();
        
        // 创建52张普通牌
        foreach (Suit suit in new Suit[] { Suit.Hearts, Suit.Diamonds, Suit.Clubs, Suit.Spades })
        {
            for (int value = 3; value <= 15; value++) // 3到A，再到2
            {
                if (value == 15) // 2
                {
                    fullDeck.Add(new PokerCard(suit, CardValue.Two));
                }
                else if (value == 14) // A
                {
                    fullDeck.Add(new PokerCard(suit, CardValue.Ace));
                }
                else
                {
                    fullDeck.Add(new PokerCard(suit, (CardValue)value));
                }
            }
        }
        
        // 添加大小王
        fullDeck.Add(new PokerCard(Suit.Joker, CardValue.SmallJoker));
        fullDeck.Add(new PokerCard(Suit.Joker, CardValue.BigJoker));
        
        Debug.Log($"创建完整牌库，共{fullDeck.Count}张牌");
    }
    
    void ShuffleDeck()
    {
        for (int i = 0; i < fullDeck.Count; i++)
        {
            int randomIndex = Random.Range(i, fullDeck.Count);
            PokerCard temp = fullDeck[i];
            fullDeck[i] = fullDeck[randomIndex];
            fullDeck[randomIndex] = temp;
        }
        Debug.Log("牌库洗牌完成");
    }
    
    void DealCards()
    {
        playerHand.Clear();
        leftPlayerHand.Clear();
        rightPlayerHand.Clear();
        landlordCards.Clear();
        
        // 发17张牌给每个玩家
        for (int i = 0; i < 17; i++)
        {
            playerHand.Add(fullDeck[i]);
            leftPlayerHand.Add(fullDeck[i + 17]);
            rightPlayerHand.Add(fullDeck[i + 34]);
        }
        
        // 剩余3张作为地主牌
        for (int i = 51; i < 54; i++)
        {
            landlordCards.Add(fullDeck[i]);
        }
        
        // 排序手牌
        SortHand(playerHand);
        SortHand(leftPlayerHand);
        SortHand(rightPlayerHand);
        
        Debug.Log($"发牌完成 - 玩家:{playerHand.Count}张, 左:{leftPlayerHand.Count}张, 右:{rightPlayerHand.Count}张, 地主牌:{landlordCards.Count}张");
    }
    
    void SortHand(List<PokerCard> hand)
    {
        hand.Sort((card1, card2) => card1.GetPower().CompareTo(card2.GetPower()));
    }
    
    void DisplayPlayerCards()
    {
        Debug.Log("=== 玩家手牌 ===");
        foreach (var card in playerHand)
        {
            Debug.Log($"{card.GetCardName()} (威力:{card.GetPower()})");
        }
        
        Debug.Log("=== 地主牌 ===");
        foreach (var card in landlordCards)
        {
            Debug.Log($"{card.GetCardName()} (威力:{card.GetPower()})");
        }
    }
}