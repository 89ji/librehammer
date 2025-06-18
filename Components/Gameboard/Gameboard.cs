using Godot;
using LibHammer.Gamestate;
using LibHammer.Gamestate.Serviceproviders;
using System;
using System.Collections.Generic;

/// <summary>
/// Node representing the entirety of the warhammer game, ui, and everything.
/// To use, instantiate scene via godot packedscene, setup armies and names, then add to tree.
/// </summary>
public partial class Gameboard : Node3D
{
	// Instantiable stuff
	[ExportGroup("Instantiables")]
	[Export] PackedScene Gametroop;

	// Internal ref
	[ExportGroup("Internal References")]
	[Export] Node3D TroopGroup;
	[Export] DeployTemplate deployTemplate;


	public Gamestate State = new();

	/// <summary>
	///  At this point, armies and players should be setup
	/// </summary>
	public override void _Ready()
	{
		// Making a node for each troop
		Dictionary<BoardTroop, GameTroop> troop2node = new();

		foreach (BoardTroop troop in State.Player1.Army)
		{
			var btr = Gametroop.Instantiate<GameTroop>();
			btr.Troop = troop;
			TroopGroup.AddChild(btr);
			troop2node.Add(troop, btr);
		}

		foreach (BoardTroop troop in State.Player2.Army)
		{
			var btr = Gametroop.Instantiate<GameTroop>();
			btr.Troop = troop;
			TroopGroup.AddChild(btr);
			troop2node.Add(troop, btr);
		}

		// Registering providers
		State.DistanceProvider = new DistanceResolver(troop2node);
		State.EngagementProvider = new EngagementResolver(troop2node);
		State.ViewClearanceProvider = new ViewClearanceResolver(troop2node);

		State.SetupMission();
		var deploymentScene = deployTemplate.LoadDeployment(State.Deploy).Instantiate();
		AddChild(deploymentScene);

		// While not typed, all deployment scenes will have their control points in ./Control Points
		var controlPoints = deploymentScene.GetNode("Control Points").GetChildren();
		for (int idx = 0; idx < 5; idx++)
		{
			ControlPoint obj = (ControlPoint)controlPoints[idx];

			LibHammer.ControlPoint.Controlpoint lhcap = new();
			lhcap.Resolver = new CPResolver(obj);
			State.Objective[idx] = lhcap;
		}

		var chooser = State.GetChoosingPlayer();
		State.ChooseRole(true);
		NextPhase();
	}


	void NextPhase()
	{
		State.PhaseMan.NextPhase();

		foreach (var obj in State.Objective)
		{
			GD.Print(obj.GetInfluence());
		}
	}
}
