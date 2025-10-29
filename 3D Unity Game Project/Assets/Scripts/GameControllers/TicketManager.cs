using System;
using System.Collections.Generic;
using UnityEngine;

public class TicketManager : MonoBehaviour
{
    //Event declarations
    public static event Action<int> OnTicketChanged;
    public static event Action<List<IGame>> OnAvailableGames;
    Action<int> ticketHandler;
    public static int AvailableTickets;
    List<IGame> gamesList = new List<IGame>();



    void Awake()
    {
        AvailableTickets = 0;
        SearchGames();
        DisplayGame();
    }

    void OnEnable()
    {
        ticketHandler = (x) =>
        {
            AvailableTickets++;
            OnTicketChanged?.Invoke(AvailableTickets);
            Debug.Log($"{AvailableTickets} tickets collected and available!");
        };
        Ticket.OnTicketCollect += ticketHandler;
        OnTicketChanged += CheckGameAvailabiltiy;
    }

    void OnDisable()
    {
        Ticket.OnTicketCollect -= ticketHandler;
        OnTicketChanged-= CheckGameAvailabiltiy;
    }

    void SearchGames()
    {
        //Searches for all Gameobjects with a Monobehaviour script
        var objs = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var obj in objs)
        {
            //Searchs for which object is implements the interface IGame
            if (obj is IGame)
            {
                IGame game = (IGame)obj;
                gamesList.Add(game);
            }
        }
    }

    void DisplayGame()
    {
        foreach (var game in gamesList)
        {
            Debug.Log($"{game.GameName} is a Game and requires {game.RequiredTickets} tickets to be played!");
        }

        // Debug.Log($"Number of miniGames is:{gamesList.Count}");
    }

    void CheckGameAvailabiltiy(int tickets)
    {
        List<IGame> availableGames = new List<IGame>();
        foreach (var game in gamesList)
        {
            if (tickets >= game.RequiredTickets)
            {
                //Debug.Log($"Enough tickets collected to play {game.GameName}");
                availableGames.Add(game);
            }
        }

        //Invoke event on available games and notify the miniGameManager
        OnAvailableGames.Invoke(new List<IGame>(availableGames));
    }

    
}

//Code references:
// 1)Title: C# Pattern Matching - Improve your C# skills in 6 minutes
//  Author: tutorialsEU - C#
//  Date accessed:  21/09/2025
//  Availability: https://www.youtube.com/watch?v=ySd_-h_Dapc

// 2)Title: The is keyword (C# .NET)
//  Author: Brian (Able Opus)
//  Date accessed:  21/09/2025
//  Availability: https://www.youtube.com/watch?v=3ga4rOKgnnU

// This helped me with the the logic for pattern matching and types
