using System.Collections.Generic;
using LibHammer.Gamestate;
using LibHammer.Gamestate.Serviceproviders;

class ViewClearanceResolver : IViewClearanceProvider
{
    Dictionary<BoardTroop, GameTroop> troops;

    public ViewClearanceResolver(Dictionary<BoardTroop, GameTroop> troop) => troops = troop;

    public ViewClearance DetermineVisibility(BoardTroop from, BoardTroop to)
    {
        return troops[from].CalculateView(troops[from]);
    }
}