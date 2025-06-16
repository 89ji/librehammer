using System;

namespace LibHammer.Primaries;

class TakeAndHold : PrimaryMission
{
    public TakeAndHold()
    {
        Name = "Take and Hold";
        Flavor = "Several strategic locations have been identified in your vicinity. You are ordered to assult these positions, secure and hold them at any cost.";
        Description = "At the end of each Command phase, the player whose turn it is scores 5VP for each objective marker they control (up to 15VP per turn)";
    }
    public override void EvaluatePrimary()
    {
        throw new NotImplementedException();
    }
}