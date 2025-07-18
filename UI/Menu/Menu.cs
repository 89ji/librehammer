using Godot;
using LibHammer.Structs;
using System;
using System.Collections.Generic;

public partial class Menu : Control
{
    [Export] PackedScene Gameboard;

    void LoadGame()
    {
        Gameboard board = Gameboard.Instantiate<Gameboard>();

        List<Troop> p1_armee = new();
        p1_armee.Add(new Troop { Name = "blone", Movement = 10, Toughness = 12, ArmorSave = 3, Wounds = 22, Leadership = 6, ObjectiveControl = 5 });

        List<Troop> p2_armee = new();
        p2_armee.Add(new Troop { Name = "megablone", Movement = 6, Toughness = 5, ArmorSave = 5, Wounds = 1, Leadership = 7, ObjectiveControl = 2 });

        board.State.InsertArmy(p1_armee, "Jocelyn");
        board.State.InsertArmy(p2_armee, "snas");

        this.GetTree().Root.AddChild(board);
        Reparent(board);
        Hide();
    }
}
