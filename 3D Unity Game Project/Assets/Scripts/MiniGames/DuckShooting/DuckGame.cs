using UnityEngine;

public class DuckGame : MonoBehaviour, IGame
{
    [Header("Game data")]
    [SerializeField] BaseGame gameinfo;
           

    public int RequiredTickets { get; set; }
    public string GameName { get; set; }
    public bool isUnlocked { get; set; }


    Interaction interaction;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        interaction = GetComponent<Interaction>();
        InitializeGame(gameinfo);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeGame(BaseGame game)
    {
        RequiredTickets = game.RequiredTickets;
        GameName = game.GameName;
        isUnlocked = false;
    }
    

    public bool TryPlay()
    {

        return isUnlocked;
    }
}
