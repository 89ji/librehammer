using Godot;
using LibHammer.Gamestate;
using LibHammer.Gamestate.Serviceproviders;
using LibHammer.Structs;
using System;
using System.Collections.Generic;

public partial class GameTroop : Node3D
{
	public BoardTroop Troop;

	public float BaseRadius = 1f;
	public float Height = 3f;

	[Export] MeshInstance3D Base;
	[Export] public Area3D Hitbox;
	[Export] StandardMaterial3D AttackColor;
	[Export] StandardMaterial3D DefenseColor;
	[Export] Sprite3D Image;

	public override void _Ready()
	{
		// Setting up the base shape
		var newshape = new CylinderMesh();
		newshape.TopRadius = BaseRadius;
		newshape.BottomRadius = BaseRadius;
		newshape.Height = .25f;
		newshape.Material = Troop.Owner.isDefender ? DefenseColor : AttackColor;
		Base.Mesh = newshape;

		// Editing the height of the hitbox
		var hboxShape = Hitbox.GetNode<CollisionShape3D>("Shape");
		((CylinderShape3D)(hboxShape.Shape)).Height = Height;
		hboxShape.Translate(new(0, -1.5f, 0));
		hboxShape.Translate(new(0, Height / 2, 0));

		// Setting up the troop image
		Image.Translate(new(0, -1.5f, 0));
		Image.Translate(new(0, Height / 4, 0));

		// DEMO PLS REMOVE
		/*
		Troop = new BoardTroop(new LibHammer.Structs.Troop {ObjectiveControl=100});
		Troop.Owner = new();
		Troop.Owner.Name = "jocelyn";
		Troop.Owner.isDefender = true;
		*/
	}

	public List<BoardTroop> FindEngagements()
	{
		List<BoardTroop> engagements = new();

		var intersections = Hitbox.GetOverlappingAreas();
		foreach (var intersection in intersections)
		{
			var parent = intersection.GetParent();
			if (parent is GameTroop troop)
			if (troop.Troop.Owner != Troop.Owner) engagements.Add(troop.Troop);
		}
		return engagements;
	}

	public ViewClearance CalculateView(Node3D target)
	{
		throw new NotImplementedException("Chikawagga");
	}
}
