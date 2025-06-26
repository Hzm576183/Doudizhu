using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CardPatternType
{
    Invalid,        // 无效牌型
    Single,         // 单牌
    Pair,           // 对子
    Triple,         // 三张
    TripleWithOne,  // 三带一
    TripleWithPair, // 三带二
    Straight,       // 顺子(5张以上连续单牌)
    PairStraight,   // 连对(3对以上连续对子)
    TripleStraight, // 飞机(2个以上连续三张)
    TripleStraightWithOnes, // 飞机带单牌
    TripleStraightWithPairs, // 飞机带对子
    Bomb,           // 四张炸弹
    JokerBomb       // 王炸
}

[System.Serializable]
public class CardPattern
{
    public CardPatternType type;
    public List<PokerCard> cards;
    public int mainValue; // 主要牌的点数
    public int power;     // 牌型威力
    
    public CardPattern(CardPatternType patternType, List<PokerCard> patternCards)
    {
        type = patternType;
        cards = new List<PokerCard>(patternCards);
        CalculateMainValue();
        CalculatePower();
    }
    
    void CalculateMainValue()
    {
        if (cards.Count == 0) return;
        
        switch (type)
        {
            case CardPatternType.Single:
            case CardPatternType.Pair:
            case CardPatternType.Triple:
                mainValue = (int)cards[0].value;
                break;
            case CardPatternType.TripleWithOne:
            case CardPatternType.TripleWithPair:
                // 找到三张相同的牌
                var groups = cards.GroupBy(c => c.value).Where(g => g.Count() >= 3);
                mainValue = (int)groups.First().Key;
                break;
            case CardPatternType.Straight:
            case CardPatternType.PairStraight:
            case CardPatternType.TripleStraight:
                // 顺子类型用最小的牌作为主值
                mainValue = cards.Min(c => (int)c.value);
                break;
            case CardPatternType.Bomb:
                mainValue = (int)cards[0].value;
                break;
            case CardPatternType.JokerBomb:
                mainValue = 100; // 王炸最大
                break;
        }
    }
    
    void CalculatePower()
    {
        switch (type)
        {
            case CardPatternType.JokerBomb:
                power = 1000;
                break;
            case CardPatternType.Bomb:
                power = 500 + mainValue;
                break;
            default:
                power = mainValue;
                break;
        }
    }
    
    public bool CanBeat(CardPattern other)
    {
        if (other == null || other.type == CardPatternType.Invalid) return true;
        
        // 王炸最大
        if (type == CardPatternType.JokerBomb) return true;
        if (other.type == CardPatternType.JokerBomb) return false;
        
        // 炸弹可以压其他非炸弹牌型
        if (type == CardPatternType.Bomb && other.type != CardPatternType.Bomb) return true;
        if (other.type == CardPatternType.Bomb && type != CardPatternType.Bomb) return false;
        
        // 相同牌型比较威力
        if (type == other.type && cards.Count == other.cards.Count)
        {
            return power > other.power;
        }
        
        return false;
    }
}

public static class CardPatternRecognizer
{
    public static CardPattern RecognizePattern(List<PokerCard> cards)
    {
        if (cards == null || cards.Count == 0)
            return new CardPattern(CardPatternType.Invalid, new List<PokerCard>());
        
        // 按数值排序
        var sortedCards = cards.OrderBy(c => (int)c.value).ToList();
        
        // 检查各种牌型
        if (IsJokerBomb(sortedCards)) return new CardPattern(CardPatternType.JokerBomb, sortedCards);
        if (IsBomb(sortedCards)) return new CardPattern(CardPatternType.Bomb, sortedCards);
        if (IsSingle(sortedCards)) return new CardPattern(CardPatternType.Single, sortedCards);
        if (IsPair(sortedCards)) return new CardPattern(CardPatternType.Pair, sortedCards);
        if (IsTriple(sortedCards)) return new CardPattern(CardPatternType.Triple, sortedCards);
        if (IsTripleWithOne(sortedCards)) return new CardPattern(CardPatternType.TripleWithOne, sortedCards);
        if (IsTripleWithPair(sortedCards)) return new CardPattern(CardPatternType.TripleWithPair, sortedCards);
        if (IsStraight(sortedCards)) return new CardPattern(CardPatternType.Straight, sortedCards);
        if (IsPairStraight(sortedCards)) return new CardPattern(CardPatternType.PairStraight, sortedCards);
        
        return new CardPattern(CardPatternType.Invalid, new List<PokerCard>());
    }
    
    static bool IsJokerBomb(List<PokerCard> cards)
    {
        return cards.Count == 2 && 
               cards.All(c => c.value == CardValue.SmallJoker || c.value == CardValue.BigJoker);
    }
    
    static bool IsBomb(List<PokerCard> cards)
    {
        return cards.Count == 4 && cards.All(c => c.value == cards[0].value);
    }
    
    static bool IsSingle(List<PokerCard> cards)
    {
        return cards.Count == 1;
    }
    
    static bool IsPair(List<PokerCard> cards)
    {
        return cards.Count == 2 && cards[0].value == cards[1].value;
    }
    
    static bool IsTriple(List<PokerCard> cards)
    {
        return cards.Count == 3 && cards.All(c => c.value == cards[0].value);
    }
    
    static bool IsTripleWithOne(List<PokerCard> cards)
    {
        if (cards.Count != 4) return false;
        var groups = cards.GroupBy(c => c.value);
        return groups.Any(g => g.Count() == 3) && groups.Any(g => g.Count() == 1);
    }
    
    static bool IsTripleWithPair(List<PokerCard> cards)
    {
        if (cards.Count != 5) return false;
        var groups = cards.GroupBy(c => c.value);
        return groups.Any(g => g.Count() == 3) && groups.Any(g => g.Count() == 2);
    }
    
    static bool IsStraight(List<PokerCard> cards)
    {
        if (cards.Count < 5) return false;
        
        // 检查是否连续
        for (int i = 1; i < cards.Count; i++)
        {
            if ((int)cards[i].value - (int)cards[i-1].value != 1)
                return false;
        }
        
        // 顺子不能包含2和王
        return !cards.Any(c => c.value >= CardValue.Two);
    }
    
    static bool IsPairStraight(List<PokerCard> cards)
    {
        if (cards.Count < 6 || cards.Count % 2 != 0) return false;
        
        var groups = cards.GroupBy(c => c.value).OrderBy(g => (int)g.Key);
        if (groups.Any(g => g.Count() != 2)) return false;
        
        var values = groups.Select(g => (int)g.Key).ToList();
        for (int i = 1; i < values.Count; i++)
        {
            if (values[i] - values[i-1] != 1) return false;
        }
        
        return !cards.Any(c => c.value >= CardValue.Two);
    }
}