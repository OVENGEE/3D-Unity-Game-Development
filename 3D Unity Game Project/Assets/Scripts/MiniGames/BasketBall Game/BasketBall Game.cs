using System;
using UnityEngine;

public class BasketBallGame : MonoBehaviour,IGame,IGameCompleted
{
    [SerializeField] BaseGame gameinfo;

    public int RequiredTickets { get ; set; }
    public string GameName { get ; set;  }
    public bool isUnlocked { get ; set; }
    public GameType gameType { get; set; }

    public static event Action OnGameCompleted;

    private void Awake()
    {
        InitialiseGame(gameinfo);
    }

    private void OnEnable()
    {
        HoopTrigger.OnHoopScored += CompleteGameObj;
    }

    private void OnDisable()
    {
        HoopTrigger.OnHoopScored -= CompleteGameObj;
    }

    public void DetermineGameAvailability(int ticket)
    {
        isUnlocked = ticket >= RequiredTickets;
    }

    public void InitialiseGame(BaseGame game)
    {
        RequiredTickets = game.RequiredTickets;
        GameName = game.GameName;
        isUnlocked = false;
        gameType = GameType.BasketBallType;
    }

    public bool TryPlay()
    {
        if (isUnlocked)
        {
            TicketManager.AvailableTickets = -RequiredTickets;
            Debug.Log($"{gameinfo.name} can be played!");
        }
        return isUnlocked;
    }
    

    private void CompleteGameObj(int score)
    {
        if (score >= 5) OnGameCompleted?.Invoke();
    }
}
