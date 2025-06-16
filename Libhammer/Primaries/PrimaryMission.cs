namespace LibHammer.Primaries;

public abstract class PrimaryMission
{
    public string Name;
    public string Flavor;
    public string Description;


    public abstract void EvaluatePrimary();
}