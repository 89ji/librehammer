using Godot;
using LibHammer.Gamestate;
using System;

public partial class NextPhaseButton : Button
{
    Gamestate state;

    public override void _Ready()
    {
        state = GetParent().GetParent<Gameboard>().State;
    }

    public override void _Process(double delta)
    {
        if (state.PhaseMan.GetNextPhase() == Phase.Postgame) Text = "End Game";
        else if (state.PhaseMan.GetNextPhase() == Phase.Command) Text = "Next Turn";
        else Text = $"To {state.PhaseMan.GetNextPhase()} Phase";
    }
}
