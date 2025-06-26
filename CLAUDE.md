# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a Unity-based 斗地主 (Dou Di Zhu / Chinese poker) card game implementation with AI opponents. The game features a complete card game engine with pattern recognition, intelligent AI, and a custom GUI-based interface.

## Unity Project Structure

- **Project Location**: `/CardGame/` - Main Unity project directory
- **Unity Version**: 6000.0.51f1 
- **Target Platform**: Cross-platform (Windows, Mac, Linux)

## Core Architecture

### Game Components (Scripts in `Assets/Scripts/`)

1. **Card System**:
   - `PokerCard.cs` - Core card data structure with Suit/CardValue enums and card naming/power logic
   - `CardPattern.cs` - Pattern recognition system for card combinations (单牌, 对子, 三张, 炸弹, 王炸, etc.)
   - Pattern recognition handles all standard 斗地主 card types and comparison logic

2. **Game Logic**:
   - `GameController.cs` - Main game state machine with player turn management and AI logic
   - `DeckManager.cs` - 54-card deck creation, shuffling, dealing (17 cards per player + 3 landlord cards)
   - Implements proper 斗地主 rules: consecutive passes clear the table, allowing new card patterns

3. **AI System**:
   - Intelligent card analysis that finds optimal plays based on table state
   - AI searches for minimal cards to beat current pattern, uses bombs strategically
   - Only passes when no valid plays available

4. **UI System**:
   - `DouDiZhuUI.cs` - Custom OnGUI() rendering for complete game interface
   - `UIBackground.cs` - Helper for colored UI area backgrounds
   - Handles card selection, visual feedback, and game state display

### Key Game Flow

1. **Initialization**: Deck creation → Shuffle → Deal 17 cards per player + 3 landlord cards
2. **Game Loop**: Player turns → Card pattern validation → AI analysis → Win condition check
3. **Rules Engine**: Proper 斗地主 logic with table clearing after consecutive passes

## Development Commands

### Running the Game
```bash
# Open Unity project
unity -projectPath /path/to/Doudizhu/CardGame

# In Unity Editor, open scene and press Play button
# Game starts automatically with card dealing and AI setup
```

### Game Controls (In Play Mode)
- **Mouse Click**: Select/deselect cards (selected cards highlighted and raised)
- **Space Bar**: Play selected cards or test random cards
- **T Key**: Test pattern recognition on hand combinations
- **出牌 Button**: Play selected cards through UI
- **不出 Button**: Pass turn through UI

### Code Architecture Patterns

- **Component-based**: Each major system is a MonoBehaviour component
- **Event-driven**: Game state changes trigger UI updates and AI responses
- **Data-driven**: Card patterns defined by enums and recognized algorithmically
- **Modular AI**: Separate methods for finding optimal plays per card pattern type

## Core Game Rules Implementation

- **54 Card Deck**: Standard poker + 2 jokers, proper 斗地主 card power ranking
- **Pattern Recognition**: Handles 单牌, 对子, 三张, 三带一, 三带二, 顺子, 连对, 炸弹, 王炸
- **Table State**: Tracks last played pattern, consecutive pass count, automatic table clearing
- **Win Conditions**: First player to empty hand wins

## Unity Specifics

- Uses OnGUI() for immediate mode UI rendering (not UGUI)
- No external dependencies beyond standard Unity packages
- Compatible with Unity 6000.x series
- Cross-platform build support configured