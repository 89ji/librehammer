using LibHammer.Gamestate;
using LibHammer.Gamestate.InterphaseActions;

namespace LibHammer.MissionRules;

class ChillingRain : MissionRule
{
	public ChillingRain()
	{
		Name = "Chilling Rain";
		Flavor = "IDK what goes here";
		Description = "In this mission, no additional mission rules apply.";
	}

	public override InterphaseAction BuildInterphase(Gamestate.Gamestate state)
	{
		var ret = new InterphaseAction
		{
			Name = Name,
			Where = Interphase.PreCommand,
			Action = (state, phase) =>
			{
				DoAction(state, phase);
			}
		};

		return ret;
	}

	void DoAction(Gamestate.Gamestate state, Phase phase)
	{
		return;
	}
}