using System.Collections.Generic;
using Godot;
using LibHammer.Gamestate;
using LibHammer.Gamestate.Serviceproviders;

class EngagementResolver : IEngagementProvider
{
	Dictionary<BoardTroop, GameTroop> troops;

	public EngagementResolver(Dictionary<BoardTroop, GameTroop> ters)
	{
		troops = ters;
	}

	public List<BoardTroop> FindEngagements(BoardTroop from)
	{
		return troops[from].FindEngagements();
	}
}