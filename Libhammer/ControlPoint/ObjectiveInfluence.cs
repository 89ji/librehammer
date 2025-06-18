using LibHammer.Gamestate;

namespace LibHammer.ControlPoint;

public struct ObjectiveInfluence
{
    public Player Player1;
    public int Player1_Influence;

    public Player Player2;
    public int Player2_Influence;

    public override string ToString()
    {
        if (Player1_Influence == 0 && Player2_Influence == 0) return "No influence";
        return $"{Player1.Name}: {Player1_Influence}\n{Player2.Name}: {Player2_Influence}";
    }
}