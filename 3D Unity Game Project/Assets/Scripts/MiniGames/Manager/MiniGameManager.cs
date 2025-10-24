using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    //Event declaration
    public event Action onAnyGameCompleted;
    private event Action<List<GameObject>> OnSetAvailableGame;
    private List<GameObject> miniGameObjects;
    private List<IGame> availableGames;

    //Constants
    const int InteractableLayer = 7;
    const int UnavailableLayer = 8;


    private void Awake()
    {
        miniGameObjects = FindMiniGameObjects();
        InitialMiniGameStates(miniGameObjects);
    }
    
    private void OnEnable()
    {
        TicketManager.OnAvailableGames += AvailableGameRegistry;
        OnSetAvailableGame += SetMiniGamesAvailable;
    }

    private void OnDisable()
    {
        TicketManager.OnAvailableGames -= AvailableGameRegistry;
        OnSetAvailableGame -= SetMiniGamesAvailable;
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
        availableGames = games;
        foreach (var game in games)
        {
            //Debug.Log($"{game.GameName} is available to be played!");
        }

        //Invoke OnsetAvailableGame event
        OnSetAvailableGame?.Invoke(miniGameObjects);
    }

    private List<GameObject> FindMiniGameObjects()
    {
        // finds all MonoBehaviours that implement IGame and convert to a GameObject list
        var games = GameObject.FindObjectsOfType<MonoBehaviour>()
                            .OfType<IGame>()
                            .Select(g => ((MonoBehaviour)g).gameObject)
                            .ToList();

        foreach (var go in games)
        {
            Debug.Log($"{go.name} implements IGame!");
        }

        return games;
    }


    private void InitialMiniGameStates(List<GameObject> minigames)
    {
        //Sets all miniGames initially to the not playable
        for (int i = 0; i < minigames.Count; i++)
        {
            var miniGame = minigames[i];
            miniGame.layer = UnavailableLayer;
        }
    }
    

    private  void SetMiniGamesAvailable(List<GameObject> minigamesObjects)
    {
        foreach (var miniGameObject in miniGameObjects)
        {
            // Try to get the IGame component from this GameObject
            var game = miniGameObject.GetComponent<IGame>();

            if (game == null)
                continue; // Skip if no IGame script is attached (just in case)

            // Check if this game is in the availableGames list
            if (availableGames.Contains(game))
            {
                miniGameObject.layer = InteractableLayer;
                Debug.Log($"{game.GameName} is now playable!");
            }
            else
            {
                miniGameObject.layer = 8;
            }
        }
    }
}


//Title: Searching for objects that implement IGame
//Author: ChatGPT
//url: https://chatgpt.com/c/68fa86d1-1ec4-8324-a780-650d40a7c40e
//date accessed: 24/10/2025

// I struggled searching for all object that implement IGame so I needed the help of Ai ...this linq method seemed the cleanest looking method. Therefore I will learn how to use linq in c#
