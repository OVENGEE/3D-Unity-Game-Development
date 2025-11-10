using System;
using UnityEngine;

public class DuckGame : MonoBehaviour, IGame,IGameCompleted
{
    [Header("Game data")]
    [SerializeField] BaseGame gameinfo;

    public int RequiredTickets { get; set; }
    public string GameName { get; set; }
    public bool isUnlocked { get; set; }
    public GameType gameType { get; set;}

    Interaction interaction;

    public static event Action OnGameCompleted;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        interaction = GetComponent<Interaction>();
        InitialiseGame(gameinfo);
    }

    void OnEnable()
    {
        PlayerShootState.OnTargetShot += CompleteGameObj;
    }

    void OnDisable()
    {
        PlayerShootState.OnTargetShot -= CompleteGameObj;
    }

    public void InitialiseGame(BaseGame game)
    {
        RequiredTickets = game.RequiredTickets;
        GameName = game.GameName;
        isUnlocked = false;
        gameType = GameType.DuckShootingType;
    }


    public bool TryPlay()
    {
        //if game is able to be played and player tries to play game.
        if (isUnlocked) TicketManager.AvailableTickets -= RequiredTickets;
        return isUnlocked;
    }

    public void DetermineGameAvailability(int ticket)
    {
        isUnlocked = ticket >= RequiredTickets;
    }

    private void CompleteGameObj(int score)
    {
        if (score >= 10) OnGameCompleted?.Invoke();
    }


}
