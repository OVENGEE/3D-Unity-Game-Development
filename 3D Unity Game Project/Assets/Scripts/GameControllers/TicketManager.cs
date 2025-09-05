using System;
using UnityEngine;

public class TicketManager : MonoBehaviour
{
    
    public static event Action<int> OnTicketChanged;
    Action<int> ticketHandler;
    public int AvailableTickets;

    void Awake()
    {
        AvailableTickets=0;
    }

    void OnEnable()
    {
        ticketHandler = (int x) =>
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


}
