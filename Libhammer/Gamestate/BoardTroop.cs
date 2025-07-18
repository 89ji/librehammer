using System.Collections.Generic;
using LibHammer.Structs;

namespace LibHammer.Gamestate;

// Represents a troop on the board with its current move, damage taken, etc
public class BoardTroop
{
    public Player Owner;
    public Troop Stats;
    public int DamageTaken;
    public bool BattleShocked;

    // Movement stuffs
    public MoveStatus MoveState = MoveStatus.Pending;
    public float AvailableMovement = 0;

    // Shooting stuffs
    public bool ShootingEligible = true;
    public readonly List<BoardTroop> ShootingTargets = new();

    // Charging stuffs
    public bool ChargingEligible = true;
    public readonly List<BoardTroop> ChargeTargets = new();

    public BoardTroop(Troop referenceTroop)
    {
        Stats = referenceTroop;
    }
}