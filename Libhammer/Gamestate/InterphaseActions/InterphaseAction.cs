using System;

namespace LibHammer.Gamestate.InterphaseActions;

public class InterphaseAction
{
    required public string Name;
    required public Interphase Where;
    required public Action<Gamestate, Phase> Action;
}