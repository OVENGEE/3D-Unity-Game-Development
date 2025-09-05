using UnityEngine;

public interface IGame
{
    public int RequiredTickets { get; }
    public string GameName { get; }
    bool isUnlocked { get; }

    public void OnTicketChanged(int availableTickets);
    public bool TryPlay();
}
