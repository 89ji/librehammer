using System.Collections.Generic;
using LibHammer.Secondaries.Fixed;
using LibHammer.Secondaries.Tactical;

namespace LibHammer.Secondaries;

public static class SecondaryManager
{
    public static List<SecondaryMission> GetFixedMissions()
    {
        List<SecondaryMission> missions = new();

        missions.Add(new Assassination());
        missions.Add(new BehindEnemyLines());
        // Add all fixed missions here

        return missions;
    }

    public static List<SecondaryMission> GetTacticalMissons()
    {
        List<SecondaryMission> missions = new();

        missions.Add(new NoPrisoners());
        missions.Add(new SecureNoMansLand());
        // Add all tactical missions here

        return missions;
    }
}