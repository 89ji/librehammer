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
        p1_armee.Add(new Troop { Name = "blone", Toughness = 9000 });

        List<Troop> p2_armee = new();
        p2_armee.Add(new Troop { Name = "megablone", Toughness = 1 });

        board.State.InsertArmy(p1_armee, "Jocelyn");
        board.State.InsertArmy(p2_armee, "snas");

        this.GetTree().Root.AddChild(board);
        Reparent(board);
        Hide();
    }
}
