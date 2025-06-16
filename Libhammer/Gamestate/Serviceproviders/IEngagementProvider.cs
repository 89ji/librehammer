using System.Collections.Generic;

namespace LibHammer.Gamestate.Serviceproviders;

public interface IEngagementProvider
{
    public List<BoardTroop> FindEngagements(BoardTroop from);
}