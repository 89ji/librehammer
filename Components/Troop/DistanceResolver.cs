using System.Collections.Generic;
using LibHammer.Gamestate;
using LibHammer.Gamestate.Serviceproviders;

class DistanceResolver : IDistanceProvider
{
    Dictionary<BoardTroop, GameTroop> troops;
    public DistanceResolver(Dictionary<BoardTroop, GameTroop> reg) => troops = reg;

    public float Measure(BoardTroop from, BoardTroop to)
    {
        GameTroop gFrom = troops[from];
        GameTroop gTo = troops[to];

        return gFrom.Position.DistanceTo(gTo.Position);
    }
}