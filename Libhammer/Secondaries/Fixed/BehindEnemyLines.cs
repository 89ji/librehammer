namespace LibHammer.Secondaries.Fixed;

class BehindEnemyLines : SecondaryMission
{
    public BehindEnemyLines()
    {
        MissionType = SecondaryType.Fixed;
        Name = "Behind Enemy Lines";
        Flavor = "Idk, kill people";
        Description = "At the end of your turn, if two or more units from your army (excluding Aircraft) are wholly within your opponent's deployment zone, this Secondary Mission is achieved and you score 4VPIf, at the end of your turn, only one unit from your army (excluding Aircraft) is wholly within your opponent’s deployment zone, this Secondary Mission is still achieved, but in this instance you score 2VP instead of 4VP. If you are using Tactical Missions, then when this Secondary Mission is achieved you score an extra 1VP (for a maximum of 5VP).";
    }
}