using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameState
{
    Dealing,      // 发牌阶段
    Bidding,      // 叫地主阶段
    Playing,      // 出牌阶段
    GameOver      // 游戏结束
}

public enum PlayerType
{
    Human,        // 人类玩家
    LeftAI,       // 左侧AI
    RightAI       // 右侧AI
}

public class GameController : MonoBehaviour
{
    [Header("游戏控制")]
    public GameState currentState = GameState.Dealing;
    public PlayerType currentPlayer = PlayerType.Human;
    public PlayerType landlord = PlayerType.Human;
    public CardPattern lastPlayedPattern;
    public PlayerType lastPlayedBy;     // 上次出牌的玩家
    public int passCount = 0;           // 连续不出牌的次数
    
    [Header("出牌测试")]
    public List<int> testCardIndices = new List<int>(); // 用于测试的牌索引
    
    private DeckManager deckManager;
    
    void Start()
    {
        deckManager = GetComponent<DeckManager>();
        if (deckManager != null)
        {
            Invoke("StartGame", 1f); // 等待发牌完成
        }
    }
    
    void StartGame()
    {
        currentState = GameState.Playing;
        currentPlayer = landlord;
        Debug.Log("=== 斗地主游戏开始 ===");
        Debug.Log($"地主: {landlord}");
        Debug.Log("按空格键测试出牌，按T键测试牌型识别");
    }
    
    void Update()
    {
        if (currentState != GameState.Playing) return;
        
        // 测试出牌 - 空格键
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestPlayCards();
        }
        
        // 测试牌型识别 - T键
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestPatternRecognition();
        }
        
        // 切换到下一个玩家 - N键
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextPlayer();
        }
    }
    
    void TestPlayCards()
    {
        if (deckManager == null || deckManager.playerHand.Count == 0) return;
        
        List<PokerCard> selectedCards = new List<PokerCard>();
        
        // 获取玩家选中的牌
        foreach (var card in deckManager.playerHand)
        {
            if (card.isSelected)
            {
                selectedCards.Add(card);
            }
        }
        
        // 如果没有选中的牌，使用测试索引或随机选择
        if (selectedCards.Count == 0)
        {
            if (testCardIndices.Count > 0)
            {
                foreach (int index in testCardIndices)
                {
                    if (index >= 0 && index < deckManager.playerHand.Count)
                    {
                        selectedCards.Add(deckManager.playerHand[index]);
                    }
                }
            }
            else
            {
                // 随机选择几张牌测试
                int numCards = Random.Range(1, 4);
                for (int i = 0; i < numCards && i < deckManager.playerHand.Count; i++)
                {
                    selectedCards.Add(deckManager.playerHand[i]);
                }
            }
        }
        
        PlayCards(selectedCards, currentPlayer);
    }
    
    void TestPatternRecognition()
    {
        if (deckManager == null) return;
        
        Debug.Log("=== 牌型识别测试 ===");
        
        // 测试玩家手牌中的各种组合
        var hand = deckManager.playerHand;
        if (hand.Count >= 2)
        {
            // 测试前两张牌
            var testCards = new List<PokerCard> { hand[0], hand[1] };
            var pattern = CardPatternRecognizer.RecognizePattern(testCards);
            Debug.Log($"测试 {hand[0].GetCardName()} + {hand[1].GetCardName()} = {pattern.type}");
        }
        
        if (hand.Count >= 3)
        {
            // 测试前三张牌
            var testCards = new List<PokerCard> { hand[0], hand[1], hand[2] };
            var pattern = CardPatternRecognizer.RecognizePattern(testCards);
            Debug.Log($"测试前三张牌 = {pattern.type}");
        }
    }
    
    public bool PlayCards(List<PokerCard> cards, PlayerType player)
    {
        if (cards == null || cards.Count == 0)
        {
            Debug.Log($"{player} 选择不出牌");
            passCount++;
            
            // 如果连续两个玩家都不出牌，则清空桌面，当前玩家重新开始
            if (passCount >= 2)
            {
                Debug.Log("=== 连续两家不出，清空桌面，重新开始 ===");
                lastPlayedPattern = null;
                lastPlayedBy = player;
                passCount = 0;
                Debug.Log($"{player} 重新开始，可以出任何牌型");
            }
            
            NextPlayer();
            return true;
        }
        
        // 识别牌型
        CardPattern pattern = CardPatternRecognizer.RecognizePattern(cards);
        
        if (pattern.type == CardPatternType.Invalid)
        {
            Debug.Log($"{player} 出牌失败: 无效牌型");
            return false;
        }
        
        // 检查是否能压过上家（如果桌面有牌的话）
        if (lastPlayedPattern != null && !pattern.CanBeat(lastPlayedPattern))
        {
            Debug.Log($"{player} 出牌失败: 无法压过上家的 {lastPlayedPattern.type}");
            return false;
        }
        
        // 出牌成功
        string cardsStr = "";
        foreach (var card in cards)
        {
            cardsStr += card.GetCardName() + " ";
        }
        
        Debug.Log($"=== {player} 出牌成功 ===");
        Debug.Log($"牌型: {pattern.type}");
        Debug.Log($"牌面: {cardsStr}");
        Debug.Log($"威力: {pattern.power}");
        
        lastPlayedPattern = pattern;
        lastPlayedBy = player;
        passCount = 0; // 重置不出牌计数
        
        // 从手牌中移除已出的牌
        if (player == PlayerType.Human)
        {
            foreach (var card in cards)
            {
                card.isSelected = false; // 取消选中状态
                deckManager.playerHand.Remove(card);
            }
            
            if (deckManager.playerHand.Count == 0)
            {
                Debug.Log("=== 玩家获胜! ===");
                currentState = GameState.GameOver;
                return true;
            }
        }
        
        NextPlayer();
        return true;
    }
    
    void NextPlayer()
    {
        switch (currentPlayer)
        {
            case PlayerType.Human:
                currentPlayer = PlayerType.LeftAI;
                break;
            case PlayerType.LeftAI:
                currentPlayer = PlayerType.RightAI;
                break;
            case PlayerType.RightAI:
                currentPlayer = PlayerType.Human;
                break;
        }
        
        Debug.Log($"轮到 {currentPlayer} 出牌");
        
        // 简单AI逻辑 - 随机不出牌或出最小的牌
        if (currentPlayer != PlayerType.Human)
        {
            Invoke("AIPlay", 1f);
        }
    }
    
    void AIPlay()
    {
        var aiHand = currentPlayer == PlayerType.LeftAI ? 
                    deckManager.leftPlayerHand : deckManager.rightPlayerHand;
        
        if (aiHand.Count == 0) return;
        
        // 寻找AI能出的最佳牌型
        List<PokerCard> bestPlay = FindBestAIPlay(aiHand);
        
        if (bestPlay != null && bestPlay.Count > 0)
        {
            // 移除AI出的牌
            foreach (var card in bestPlay)
            {
                aiHand.Remove(card);
            }
            PlayCards(bestPlay, currentPlayer);
        }
        else
        {
            // 没有可出的牌，选择不出
            PlayCards(null, currentPlayer);
        }
    }
    
    List<PokerCard> FindBestAIPlay(List<PokerCard> hand)
    {
        // 如果桌面无牌，AI可以出任何牌型，选择最小的单牌
        if (lastPlayedPattern == null)
        {
            var sortedHand = hand.OrderBy(c => c.GetPower()).ToList();
            return new List<PokerCard> { sortedHand[0] };
        }
        
        // 桌面有牌，需要找到能压过的最小牌型
        List<PokerCard> bestPlay = null;
        int lowestPower = int.MaxValue;
        
        // 根据桌面牌型寻找对应类型的牌
        switch (lastPlayedPattern.type)
        {
            case CardPatternType.Single:
                bestPlay = FindBestSingle(hand, lastPlayedPattern.power);
                break;
            case CardPatternType.Pair:
                bestPlay = FindBestPair(hand, lastPlayedPattern.power);
                break;
            case CardPatternType.Triple:
                bestPlay = FindBestTriple(hand, lastPlayedPattern.power);
                break;
            case CardPatternType.Bomb:
                bestPlay = FindBestBomb(hand, lastPlayedPattern.power);
                break;
        }
        
        // 如果找不到同类型的牌，尝试用炸弹压制（除非桌面已经是王炸）
        if (bestPlay == null && lastPlayedPattern.type != CardPatternType.JokerBomb)
        {
            bestPlay = FindBestBomb(hand, 0); // 任何炸弹都可以
            if (bestPlay == null)
            {
                bestPlay = FindJokerBomb(hand); // 尝试王炸
            }
        }
        
        return bestPlay;
    }
    
    List<PokerCard> FindBestSingle(List<PokerCard> hand, int minPower)
    {
        var sortedCards = hand.OrderBy(c => c.GetPower()).ToList();
        foreach (var card in sortedCards)
        {
            if (card.GetPower() > minPower)
            {
                return new List<PokerCard> { card };
            }
        }
        return null;
    }
    
    List<PokerCard> FindBestPair(List<PokerCard> hand, int minPower)
    {
        var groups = hand.GroupBy(c => c.value).Where(g => g.Count() >= 2);
        var sortedGroups = groups.OrderBy(g => (int)g.Key);
        
        foreach (var group in sortedGroups)
        {
            if ((int)group.Key > minPower)
            {
                return group.Take(2).ToList();
            }
        }
        return null;
    }
    
    List<PokerCard> FindBestTriple(List<PokerCard> hand, int minPower)
    {
        var groups = hand.GroupBy(c => c.value).Where(g => g.Count() >= 3);
        var sortedGroups = groups.OrderBy(g => (int)g.Key);
        
        foreach (var group in sortedGroups)
        {
            if ((int)group.Key > minPower)
            {
                return group.Take(3).ToList();
            }
        }
        return null;
    }
    
    List<PokerCard> FindBestBomb(List<PokerCard> hand, int minPower)
    {
        var groups = hand.GroupBy(c => c.value).Where(g => g.Count() >= 4);
        var sortedGroups = groups.OrderBy(g => (int)g.Key);
        
        foreach (var group in sortedGroups)
        {
            if ((int)group.Key > minPower)
            {
                return group.Take(4).ToList();
            }
        }
        return null;
    }
    
    List<PokerCard> FindJokerBomb(List<PokerCard> hand)
    {
        var jokers = hand.Where(c => c.value == CardValue.SmallJoker || c.value == CardValue.BigJoker).ToList();
        if (jokers.Count >= 2)
        {
            return jokers.Take(2).ToList();
        }
        return null;
    }
}