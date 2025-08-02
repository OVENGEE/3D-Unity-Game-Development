using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStateMachine
{
    public PlayerState CurrentPlayerState { get; set; }

    public void Initialise(PlayerState initialState)
    {
        // function that starts with a state
        CurrentPlayerState = initialState;
        CurrentPlayerState.EnterState();
    }

    public void SwitchState(PlayerState newState)
    {
        //function that switches from one state to another taking new state as input
        CurrentPlayerState.ExitState();
        CurrentPlayerState = newState;
        CurrentPlayerState.ExitState();
    }
}
//Code reference:
// The statemachine logic comes from:https://www.youtube.com/watch?v=RQd44qSaqww