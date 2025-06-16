namespace LibHammer.Secondaries.Fixed;

class Assassination : SecondaryMission
{
    public Assassination()
    {
        MissionType = SecondaryType.Fixed;
        Name = "Asssassination";
        Flavor = "Idk, kill people";
        Description = "If you are using Fixed Missions, then while this Secondary Mission is active, each time an enemy Character model is destroyed, you score 4VP.If you are using Tactical Missions, then at the end of the turn, if either of the conditions below are satisfied, this Secondary Mission is achieved and you score 5VP:One or more enemy Character units were destroyed this turn.All Character units from your opponent’s Army Roster have been destroyed during the battle.Note that if you are using Tactical Missions, this Secondary Mission is achieved even if such a unit was destroyed and then subsequently resurrected for any reason.";
    }
}