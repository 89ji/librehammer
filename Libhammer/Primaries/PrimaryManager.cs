using System;

namespace LibHammer.Primaries;

static class PrimaryManager
{
    public static PrimaryMission RollPrimary()
    {
        Random rng = new Random();
        int rand = rng.Next(0, 1);

        // Add one for each thingy
        switch (rand)
        {
            case 0:
                return new TakeAndHold();

            default:
                return new TakeAndHold();
        }
    }
}