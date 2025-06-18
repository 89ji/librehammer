using Godot;
using System;
using LibHammer.ControlPoint;
using LibHammer.Gamestate;
using System.Collections.Generic;

public partial class ControlPoint : Node3D
{
	[Export] MeshInstance3D ColorHightlight;
	[Export] Area3D Hitbox;

	[Export] StandardMaterial3D Neutral;
	[Export] StandardMaterial3D Defender;
	[Export] StandardMaterial3D Attacker;

	public readonly List<GameTroop> Influencers = new();

	public override void _Ready()
	{
		OnHitboxUpdate();
	}

	public override void _Process(double delta)
	{
	}

	public void OnHitboxEnter(Area3D area)
	{
		var parent = area.GetParent();
		if (parent is GameTroop troop)
		{
			GD.Print("Thingy entered");
			Influencers.Add(troop);
			OnHitboxUpdate();
		}
	}

	public void OnHitboxExit(Area3D area)
	{
		var parent = area.GetParent();
		if (parent is GameTroop troop)
		{
			GD.Print("Thingy exited");
			Influencers.Remove(troop);
			OnHitboxUpdate();
		}
	}

	public void OnHitboxUpdate()
	{
		var influences = CalculateInfluence();

		if (influences.Player1_Influence == influences.Player2_Influence || influences.Player1 == influences.Player2) ColorHightlight.MaterialOverride = Neutral;
		else if (influences.Player1_Influence > influences.Player2_Influence)
		{
			if (influences.Player1.isDefender) ColorHightlight.MaterialOverride = Defender;
			else ColorHightlight.MaterialOverride = Attacker;
		}
		else
		{
			if (influences.Player2.isDefender) ColorHightlight.MaterialOverride = Defender;
			else ColorHightlight.MaterialOverride = Attacker;
		}

	}

	public ObjectiveInfluence CalculateInfluence()
	{
		Player? p1 = null;
		Player? p2 = null;

		int p1_influence = 0;
		int p2_influence = 0;

		foreach (var troop in Influencers)
		{
				if (p1 == null) p1 = troop.Troop.Owner;
				else if (p2 == null) p2 = troop.Troop.Owner;

				if (p1 == troop.Troop.Owner) p1_influence += troop.Troop.BattleShocked ? 0 : troop.Troop.Stats.ObjectiveControl;
				else p2_influence += troop.Troop.BattleShocked ? 0 : troop.Troop.Stats.ObjectiveControl;
		}

		ObjectiveInfluence influences = new();
		influences.Player1 = p1;
		influences.Player1_Influence = p1_influence;
		influences.Player2 = p2;
		influences.Player2_Influence = p2_influence;

		return influences;
	}
}
