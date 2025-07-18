using Godot;
using LibHammer.Structs;
using System;

public partial class MovementManager : Node3D
{
	Gameboard board;
	[Export] Camera cam;
	[Export] UnitPopup popup;

	public override void _Ready()
	{
		Node node = this;
		while (board == null)
		{
			node = node.GetParent();
			if (node is Gameboard gboard) board = gboard;
		}

		cam.SelectedTroopUpdated += OnTroopSelect;
		cam.SelectedLocationUpdated += OnLocationSelect;
	}

	public override void _Process(double delta)
	{
		if (cam.SelectedTroop != null & cam.HoveredLocation != null)
		{
			float distanceToPointer = cam.SelectedTroop.Position.DistanceTo(cam.HoveredLocation.Value);
			popup.AboutToMove = distanceToPointer;
			//GD.Print(distanceToPointer);
			// GD.Print($"{cam.SelectedTroop.Position.DistanceTo(cam.HoveredLocation.Value):F2}");
		}
		else popup.AboutToMove = null;

	}

	void OnTroopSelect()
	{
		if (cam.SelectedTroop == null) return;
		popup.troop = cam.SelectedTroop.Troop;
		popup.Show();
	}

	void OnLocationSelect()
	{
		if (cam.SelectedTroop == null) return;
		float distanceToPointer = cam.SelectedTroop.Position.DistanceTo(cam.SelectedLocation.Value);

		// No adjustment needed, movement is within spec
		if (distanceToPointer <= cam.SelectedTroop.Troop.AvailableMovement)
		{
			cam.SelectedTroop.Troop.AvailableMovement -= distanceToPointer;
			cam.SelectedTroop.Troop.MoveState = LibHammer.Gamestate.MoveStatus.Moved;
			cam.SelectedTroop.Position = cam.SelectedLocation.Value;
		}

		// Moving outside of range, move to closest point within range
		else
		{

			cam.SelectedTroop.Troop.MoveState = LibHammer.Gamestate.MoveStatus.Moved;
			var dirToPtr = cam.SelectedTroop.Position.DirectionTo(cam.SelectedLocation.Value);
			dirToPtr *= cam.SelectedTroop.Troop.AvailableMovement;
			cam.SelectedTroop.Position += dirToPtr;
			cam.SelectedTroop.Troop.AvailableMovement = 0;
		}
	}
}
