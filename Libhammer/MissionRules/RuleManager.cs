using System;

namespace LibHammer.MissionRules;

static class RuleManager
{
    public static MissionRule RollMission()
    {
        Random rng = new Random();
        int rand = rng.Next(0, 1);

        // Add one for each thingy
        switch (rand)
        {
            case 0:
                return new ChillingRain();

            default:
                return new ChillingRain();
        }
    }
}