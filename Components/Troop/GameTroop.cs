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
		// newshape.Material = Troop.Owner.isDefender ? DefenseColor : AttackColor;
		Base.MaterialOverride = Troop.Owner.isDefender ? DefenseColor : AttackColor;
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

	// Samples one or more times, will use the most PERMISSIVE sample found, clear -> partial obstruction -> fully obstructed
	public ViewClearance CalculateView(Node3D target)
	{
		if (target is not GameTroop) throw new Exception("Node not of type gametroop!");
		GameTroop other = (GameTroop)target;
		for (int i = 0; i < 100; i++) CalculateRay(other);
		var result = CalculateRay(other);
		return result;
	}

	// Performs one sample, returns the most OBSTRUCTIVE sample found
	ViewClearance CalculateRay(GameTroop target)
	{
		RayCast3D sampler = new();
		AddChild(sampler);

		// TODO add sampler randomization
		float randMean = 0;
		float randStd = .25f;

		Vector3 sourceRand = new((float)GD.Randfn(randMean, randStd), (float)GD.Randfn(randMean, randStd), (float)GD.Randfn(randMean, randStd));
		Vector3 destRand = new((float)GD.Randfn(randMean, randStd), (float)GD.Randfn(randMean, randStd), (float)GD.Randfn(randMean, randStd));

		sampler.TopLevel = true;

		sampler.Position = Position + new Vector3(0, Height - 1, 0) + sourceRand;

		sampler.TargetPosition = -sampler.Position + target.Position + new Vector3(0, Height - 1, 0) + destRand;

		sampler.CollisionMask = 0b0110;
		sampler.CollideWithAreas = true;
		sampler.CollideWithBodies = false;
		sampler.ForceRaycastUpdate();

		bool isPartiallyObscured = false;

		while (sampler.IsColliding())
		{
			// Layer 2, full obstruction
			if (sampler.GetCollisionMaskValue(2)) return ViewClearance.Obstructed;

			// Layer 1, partial obstruction
			else if (sampler.GetCollisionMaskValue(1))
			{
				isPartiallyObscured = true;
				sampler.AddException((CollisionObject3D)sampler.GetCollider());
				sampler.ForceRaycastUpdate();
			}

			else throw new Exception("wtf did this thing detect");
		}

		if (isPartiallyObscured) return ViewClearance.InCover;
		else return ViewClearance.FullyVisible;
	}
}
