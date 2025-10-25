using UnityEngine;

public interface IGame
{
    public int RequiredTickets { get; set; }
    public string GameName { get; set; }
    bool isUnlocked { get; set; }

    public bool TryPlay();
    public void InitialiseGame(BaseGame game);
    public void DetermineGameAvailability(int ticket);

    public GameType gameType { get; set; }
}

public enum GameType
{
    DuckShootingType,
    BasketBallType,
    MazeType
}