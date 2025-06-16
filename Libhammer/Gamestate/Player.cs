using System.Collections.Generic;
using LibHammer.Gamestate.InterphaseActions;
using LibHammer.Secondaries;

namespace LibHammer.Gamestate;

public class Player
{
    public string? Name;
    public List<BoardTroop> Army = new();
    public bool isDefender;
    public int CP = 0;
    public int VP = 0;
    public readonly SecondaryMission[] Secondaries = new SecondaryMission[2];
    public readonly InterphaseManager Interphase = new();


    public static bool operator ==(Player left, Player right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (ReferenceEquals(left, null)) return false;
        if (ReferenceEquals(right, null)) return false;

        return left.Name == right.Name;
    }

    public static bool operator !=(Player left, Player right)
    {
        if (ReferenceEquals(left, right)) return false;
        if (ReferenceEquals(left, null)) return true;
        if (ReferenceEquals(right, null)) return true;

        return left.Name != right.Name;
    }
}