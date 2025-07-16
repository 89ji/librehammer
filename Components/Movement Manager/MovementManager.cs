using Godot;
using System;

public partial class MovementManager : Node3D
{
    Gameboard board;
    [Export] Camera cam;

    public override void _Ready()
    {
        var node = (Node)this;
        while (board == null)
        {
            node = node.GetParent();
            if (node is Gameboard gboard) board = gboard;
        }

        cam.SelectedTroopUpdated += OnTroopSelect;
        cam.SelectedLocationUpdated += OnLocationSelect;
    }

    void OnTroopSelect()
    {

    }

    void OnLocationSelect()
    {
        cam.SelectedTroop.Position = cam.SelectedLocation.Value;
    }
}
