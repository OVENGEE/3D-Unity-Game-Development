using System;
using System.Collections.Generic;
using UnityEngine;

public class TicketManager : MonoBehaviour
{

    public static event Action<int> OnTicketChanged;
    Action<int> ticketHandler;
    public int AvailableTickets;
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
    }

    void OnDisable()
    {
        Ticket.OnTicketCollect -= ticketHandler;
    }

    void SearchGames()
    {
        var objs = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var obj in objs)
        {
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
            Debug.Log($"{game.GameName} is a Game!");
        }

        Debug.Log($"Number of miniGames is:{gamesList.Count}");
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
