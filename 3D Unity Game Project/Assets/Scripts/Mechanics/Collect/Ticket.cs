using System;
using UnityEngine;
public class Ticket : MonoBehaviour, IItem
{
    public static event Action<int> OnTicketCollect; // Event to notify when a gem is collected
    public int worth = 5;
    public void Collect()
    {
        OnTicketCollect.Invoke(worth); // Invoke the event with the gem's worth
        SoundEffectManager.Play("Coin");
        Destroy(gameObject); // Destroy the gem GameObject when collected
    }
}
