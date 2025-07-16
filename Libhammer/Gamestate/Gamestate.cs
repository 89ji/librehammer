using System;
using System.Collections.Generic;
using LibHammer.Deployments;
using LibHammer.Gamestate.Serviceproviders;
using LibHammer.MissionRules;
using LibHammer.Primaries;
using LibHammer.Secondaries;
using LibHammer.Structs;
using LibHammer.ControlPoint;

namespace LibHammer.Gamestate;

public class Gamestate
{
    // State managers
    public PhaseManager PhaseMan;
    public readonly Controlpoint[] Objective = new Controlpoint[5];

    // Stuff for external resolvers
    public IDistanceProvider DistanceProvider;
    public IEngagementProvider EngagementProvider;
    public IViewClearanceProvider ViewClearanceProvider;


    // Stuff for player 1
    public Player Player1 = new();


    // Stuff for player 2
    public Player Player2 = new();

    public PrimaryMission Primary;
    public MissionRule Rule;
    public Deployment Deploy;

    public bool PlayersReady
    {
        get
        {
            return Player1.Name != null && Player2.Name != null;
        }
    }


    public void InsertArmy(List<Troop> army, string player)
    {
        if (Player1.Name == null)
        {
            Player1.Name = player;
            Player1.Army = Army2Board(army, Player1);
        }

        else if (Player2.Name == null)
        {
            Player2.Name = player;
            Player2.Army = Army2Board(army, Player2);
        }

        else throw new Exception("Both players already added!");
    }

    List<BoardTroop> Army2Board(List<Troop> army, Player player)
    {
        List<BoardTroop> boardArmy = new();
        foreach (Troop troop in army)
        {
            var btroop = new BoardTroop(troop);
            btroop.Owner = player;
            boardArmy.Add(btroop);
        }
        return boardArmy;
    }

    // Setup deployments, mission rule, and primary mission
    public void SetupMission()
    {
        if (!PlayersReady) throw new Exception("Players are not yet initialized!");

        PhaseMan = new(Player1, Player2, this);

        Primary = PrimaryManager.RollPrimary();
        Rule = RuleManager.RollMission();
        Deploy = DeploymentManager.RollDeployment();
    }

    Player? ChoosingPlayer;
    Player? OtherPlayer;
    public Player GetChoosingPlayer()
    {
        if (ChoosingPlayer == null)
        {
            Random rng = new();
            if (rng.Next(0, 2) == 0)
            {
                ChoosingPlayer = Player1;
                OtherPlayer = Player2;
            }
            else
            {
                ChoosingPlayer = Player2;
                OtherPlayer = Player1;
            }
        }
        return ChoosingPlayer;
    }


    public void ChooseRole(bool isAttacking)
    {
        if (isAttacking)
        {
            ChoosingPlayer.isDefender = false; ;
            OtherPlayer.isDefender = true;
        }
        else
        {
            ChoosingPlayer.isDefender = true; ;
            OtherPlayer.isDefender = false;
        }
    }

    public void SetMissionRule(MissionRule missionRule)
    {
        // If the mission slot is occupied, deregister its actions
        if (missionRule != null)
        {
            
        }
    }
}