using UnityEngine;

public class TicketManager : MonoBehaviour
{
    public int AvailableTickets;



    void Awake()
    {
        AvailableTickets=0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        Ticket.OnTicketCollect += TicketCollected;
    }

    void OnDisable()
    {
        Ticket.OnTicketCollect -= TicketCollected;
    }

    void TicketCollected(int ticket)
    {
        AvailableTickets++;
        Debug.Log($"{AvailableTickets} tickets collected and available!");
    }
}
