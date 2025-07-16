using LibHammer.Gamestate;
using LibHammer.Gamestate.InterphaseActions;

namespace LibHammer.MissionRules;

class SupplyLines : MissionRule
{
    public SupplyLines(LibHammer.Gamestate.Gamestate state)
    {
        Name = "Supply Lines";
        Flavor = "Who up supplying their lines?";
        Description = "In this mission, if a player controls the objective marker in their own deployment zone at the start of their Command phase, they roll one D6: on a 4+, that player gains 1CP.";
    }

    public override InterphaseAction BuildInterphase(Gamestate.Gamestate state)
    {
        var ret = new InterphaseAction
        {
            Name = Name,
            Where = Interphase.PreCommand,
            Action = (state, phase) =>
            {
                DoAction(state);
            }

        };
        return ret;
    }

    void DoAction(Gamestate.Gamestate state)
    {
        bool controlsApoint = false;
        foreach (var cap in state.Objective)
        {
            var influence = cap.GetInfluence();
            if ((influence.Player1_Influence > influence.Player2_Influence && state.PhaseMan.ActivePlayer == influence.Player1) ||
                (influence.Player2_Influence > influence.Player1_Influence && state.PhaseMan.ActivePlayer == influence.Player2))
            {
                
            }
        }
    }
}