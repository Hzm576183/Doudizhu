using UnityEngine;
using System.Collections.Generic;

public class DouDiZhuUI : MonoBehaviour
{
    [Header("斗地主UI布局")]
    public Color playerAreaColor = Color.blue;      // 玩家区域 - 蓝色
    public Color leftPlayerColor = Color.green;     // 左侧玩家 - 绿色  
    public Color rightPlayerColor = Color.yellow;   // 右侧玩家 - 黄色
    public Color landlordAreaColor = Color.red;     // 地主牌区域 - 红色
    public Color playAreaColor = Color.gray;        // 出牌区域 - 灰色
    
    [Header("卡牌显示设置")]
    public int cardWidth = 60;
    public int cardHeight = 80;
    public int cardSpacing = 65;
    
    private DeckManager deckManager;
    private GameController gameController;
    
    void Start()
    {
        deckManager = GetComponent<DeckManager>();
        gameController = GetComponent<GameController>();
    }
    
    private void OnGUI()
    {
        DrawGameLayout();
        DrawCards();
        DrawGameInfo();
    }
    
    void DrawGameLayout()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        
        // 设置UI样式
        Color oldColor = GUI.color;
        
        // 玩家手牌区域 (底部)
        GUI.color = playerAreaColor;
        Rect playerArea = new Rect(50, screenHeight - 150, screenWidth - 100, 100);
        GUI.DrawTexture(playerArea, Texture2D.whiteTexture);
        GUI.color = Color.white;
        GUI.Label(new Rect(playerArea.x + 10, playerArea.y + 10, 200, 30), "玩家手牌区域", GetLabelStyle());
        
        // 左侧玩家区域
        GUI.color = leftPlayerColor;
        Rect leftArea = new Rect(20, 50, 120, screenHeight - 250);
        GUI.DrawTexture(leftArea, Texture2D.whiteTexture);
        GUI.color = Color.white;
        GUI.Label(new Rect(leftArea.x + 10, leftArea.y + 10, 100, 30), "左侧玩家", GetLabelStyle());
        
        // 右侧玩家区域
        GUI.color = rightPlayerColor;
        Rect rightArea = new Rect(screenWidth - 140, 50, 120, screenHeight - 250);
        GUI.DrawTexture(rightArea, Texture2D.whiteTexture);
        GUI.color = Color.white;
        GUI.Label(new Rect(rightArea.x + 10, rightArea.y + 10, 100, 30), "右侧玩家", GetLabelStyle());
        
        // 地主牌区域 (顶部中央)
        GUI.color = landlordAreaColor;
        Rect landlordArea = new Rect(screenWidth/2 - 100, 20, 200, 80);
        GUI.DrawTexture(landlordArea, Texture2D.whiteTexture);
        GUI.color = Color.white;
        GUI.Label(new Rect(landlordArea.x + 10, landlordArea.y + 10, 180, 30), "地主牌区域", GetLabelStyle());
        
        // 中央出牌区域
        GUI.color = playAreaColor;
        Rect playArea = new Rect(160, screenHeight/2 - 100, screenWidth - 320, 200);
        GUI.DrawTexture(playArea, Texture2D.whiteTexture);
        GUI.color = Color.white;
        GUI.Label(new Rect(playArea.x + 10, playArea.y + 10, 200, 30), "出牌区域", GetLabelStyle());
        
        // 游戏信息区域 (右上角)
        GUI.color = Color.cyan;
        Rect infoArea = new Rect(screenWidth - 200, 120, 180, 100);
        GUI.DrawTexture(infoArea, Texture2D.whiteTexture);
        GUI.color = Color.white;
        GUI.Label(new Rect(infoArea.x + 10, infoArea.y + 10, 160, 30), "游戏信息", GetLabelStyle());
        
        GUI.color = oldColor;
    }
    
    void DrawCards()
    {
        if (deckManager == null) return;
        
        DrawPlayerCards();
        DrawLandlordCards();
        DrawOtherPlayersCards();
        DrawPlayedCards();
    }
    
    void DrawPlayerCards()
    {
        if (deckManager.playerHand == null) return;
        
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        
        // 计算玩家手牌的起始位置（底部居中）
        int totalCards = deckManager.playerHand.Count;
        float totalWidth = totalCards * cardSpacing;
        float startX = (screenWidth - totalWidth) / 2;
        float startY = screenHeight - 140;
        
        Color oldColor = GUI.color;
        
        for (int i = 0; i < deckManager.playerHand.Count; i++)
        {
            var card = deckManager.playerHand[i];
            float cardX = startX + i * cardSpacing;
            
            // 选中的牌向上移动
            float cardY = card.isSelected ? startY - 20 : startY;
            
            Rect cardRect = new Rect(cardX, cardY, cardWidth, cardHeight);
            
            // 绘制卡牌背景
            GUI.color = GetCardColor(card);
            GUI.DrawTexture(cardRect, Texture2D.whiteTexture);
            
            // 绘制卡牌文字
            GUI.color = Color.white;
            GUIStyle cardStyle = GetCardStyle();
            GUI.Label(cardRect, card.GetCardName(), cardStyle);
            
            // 检测点击
            if (Event.current.type == EventType.MouseDown && cardRect.Contains(Event.current.mousePosition))
            {
                card.isSelected = !card.isSelected;
                Event.current.Use();
            }
        }
        
        GUI.color = oldColor;
        
        // 显示操作提示和按钮
        GUI.Label(new Rect(50, screenHeight - 40, 300, 30), 
                 "点击选牌，空格出牌，T测试", GetLabelStyle());
        
        // 出牌按钮
        if (GUI.Button(new Rect(400, screenHeight - 40, 80, 30), "出牌"))
        {
            if (gameController != null)
            {
                gameController.SendMessage("TestPlayCards");
            }
        }
        
        // 不出按钮
        if (GUI.Button(new Rect(490, screenHeight - 40, 80, 30), "不出"))
        {
            if (gameController != null)
            {
                gameController.PlayCards(null, gameController.currentPlayer);
            }
        }
    }
    
    void DrawLandlordCards()
    {
        if (deckManager.landlordCards == null) return;
        
        float screenWidth = Screen.width;
        float startX = screenWidth/2 - 90;
        float startY = 25;
        
        Color oldColor = GUI.color;
        
        for (int i = 0; i < deckManager.landlordCards.Count; i++)
        {
            var card = deckManager.landlordCards[i];
            float cardX = startX + i * cardSpacing;
            
            Rect cardRect = new Rect(cardX, startY, cardWidth, cardHeight);
            
            GUI.color = Color.red;
            GUI.DrawTexture(cardRect, Texture2D.whiteTexture);
            
            GUI.color = Color.white;
            GUI.Label(cardRect, card.GetCardName(), GetCardStyle());
        }
        
        GUI.color = oldColor;
    }
    
    void DrawOtherPlayersCards()
    {
        float screenHeight = Screen.height;
        Color oldColor = GUI.color;
        
        // 左侧玩家手牌数量
        if (deckManager.leftPlayerHand != null)
        {
            GUI.color = Color.white;
            string leftInfo = $"左侧玩家\n{deckManager.leftPlayerHand.Count}张牌";
            GUI.Label(new Rect(30, screenHeight/2, 100, 60), leftInfo, GetLabelStyle());
        }
        
        // 右侧玩家手牌数量
        if (deckManager.rightPlayerHand != null)
        {
            GUI.color = Color.white;
            string rightInfo = $"右侧玩家\n{deckManager.rightPlayerHand.Count}张牌";
            GUI.Label(new Rect(Screen.width - 130, screenHeight/2, 100, 60), rightInfo, GetLabelStyle());
        }
        
        GUI.color = oldColor;
    }
    
    void DrawGameInfo()
    {
        if (gameController == null) return;
        
        float screenWidth = Screen.width;
        
        Color oldColor = GUI.color;
        GUI.color = Color.white;
        
        string gameInfo = $"当前状态: {gameController.currentState}\n";
        gameInfo += $"当前玩家: {gameController.currentPlayer}\n";
        gameInfo += $"地主: {gameController.landlord}";
        
        if (gameController.lastPlayedPattern != null)
        {
            gameInfo += $"\n上家出牌: {gameController.lastPlayedPattern.type}";
            gameInfo += $"\n出牌者: {gameController.lastPlayedBy}";
        }
        else
        {
            gameInfo += "\n桌面无牌，可出任何牌型";
        }
        
        gameInfo += $"\n连续不出: {gameController.passCount}/2";
        
        GUI.Label(new Rect(screenWidth - 200, 130, 180, 120), gameInfo, GetLabelStyle());
        
        GUI.color = oldColor;
    }
    
    void DrawPlayedCards()
    {
        if (gameController == null) return;
        
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float centerX = screenWidth / 2;
        float centerY = screenHeight / 2;
        
        Color oldColor = GUI.color;
        
        // 如果桌面无牌，显示提示
        if (gameController.lastPlayedPattern == null)
        {
            GUI.color = Color.yellow;
            GUIStyle style = GetLabelStyle();
            style.alignment = TextAnchor.MiddleCenter;
            GUI.Label(new Rect(centerX - 150, centerY - 15, 300, 30), 
                     "桌面无牌，可以出任何牌型", style);
            GUI.color = oldColor;
            return;
        }
        
        var playedCards = gameController.lastPlayedPattern.cards;
        if (playedCards == null || playedCards.Count == 0) return;
        
        // 计算牌的起始位置
        float totalWidth = playedCards.Count * cardSpacing;
        float startX = centerX - totalWidth / 2;
        float startY = centerY - cardHeight / 2;
        
        // 绘制出牌区域背景
        Rect playAreaRect = new Rect(startX - 20, startY - 20, totalWidth + 40, cardHeight + 40);
        GUI.color = new Color(0.3f, 0.3f, 0.3f, 0.8f); // 半透明灰色背景
        GUI.DrawTexture(playAreaRect, Texture2D.whiteTexture);
        
        // 绘制已出的牌
        for (int i = 0; i < playedCards.Count; i++)
        {
            var card = playedCards[i];
            float cardX = startX + i * cardSpacing;
            
            Rect cardRect = new Rect(cardX, startY, cardWidth, cardHeight);
            
            // 绘制卡牌背景
            GUI.color = GetCardColor(card);
            GUI.DrawTexture(cardRect, Texture2D.whiteTexture);
            
            // 绘制卡牌文字
            GUI.color = Color.white;
            GUI.Label(cardRect, card.GetCardName(), GetCardStyle());
        }
        
        // 显示牌型信息
        GUI.color = Color.yellow;
        GUIStyle patternStyle = GetLabelStyle();
        patternStyle.alignment = TextAnchor.MiddleCenter;
        string patternInfo = $"{gameController.lastPlayedPattern.type} (威力:{gameController.lastPlayedPattern.power})";
        GUI.Label(new Rect(centerX - 150, startY + cardHeight + 10, 300, 30), patternInfo, patternStyle);
        
        GUI.color = oldColor;
    }
    
    Color GetCardColor(PokerCard card)
    {
        if (card.isSelected) return Color.cyan;
        
        switch (card.suit)
        {
            case Suit.Hearts:
            case Suit.Diamonds:
                return Color.red;
            case Suit.Clubs:
            case Suit.Spades:
                return Color.black;
            case Suit.Joker:
                return card.value == CardValue.BigJoker ? Color.red : Color.black;
            default:
                return Color.white;
        }
    }
    
    GUIStyle GetLabelStyle()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 16;
        style.fontStyle = FontStyle.Bold;
        return style;
    }
    
    GUIStyle GetCardStyle()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 12;
        style.fontStyle = FontStyle.Bold;
        style.alignment = TextAnchor.MiddleCenter;
        return style;
    }
}