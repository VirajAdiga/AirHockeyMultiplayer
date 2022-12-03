using UnityEngine;

public enum GameType
{
    DefaultGame,
    MultiplayerGame,
    Gold,
    Silver,
    Bronze
};

public class GameData : MonoBehaviour
{

    public static GameType gameType;

    public static readonly int multiPlayerMileStone = 9, goldMileStone = 5, silverMileStone = 3, bronzeMileStone = 1;

    private void Awake()
    {
        DefaultGamePlay();
    }

    public static void MutliPlayerGame()
    {
        gameType = GameType.MultiplayerGame;
    }

    public static void GoldUnlock()
    {
        gameType = GameType.Gold;
    }

    public static void SilverUnlock()
    {
        gameType = GameType.Silver;
    }

    public static void BronzeUnlock()
    {
        gameType = GameType.Bronze;
    }
    
    public static void DefaultGamePlay()
    {
        gameType = GameType.DefaultGame;
    }
}
