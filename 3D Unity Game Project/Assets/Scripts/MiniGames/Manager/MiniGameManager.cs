using System;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public event Action onAnyGameCompleted;

    private void OnEnable()
    {
        TicketManager.OnAvailableGames += AvailableGameRegistry;
    }

    private void OnDisable()
    {
        TicketManager.OnAvailableGames -= AvailableGameRegistry;
    }

    public void RegisterMiniGame(IGameCompleted miniGame)
    {
        miniGame.OnGameCompleted += HandleMiniGame;
    }

    private void HandleMiniGame()
    {
        //Wrapper to invoke the event
        onAnyGameCompleted?.Invoke();
    }

    private void AvailableGameRegistry(List<IGame> games)
    {
        foreach(var game in games)
        {
            Debug.Log($"{game.GameName} is available to be played!");
        }
    }

}
