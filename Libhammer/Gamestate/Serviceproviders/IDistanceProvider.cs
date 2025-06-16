namespace LibHammer.Gamestate.Serviceproviders;

public interface IDistanceProvider
{
    public float Measure(BoardTroop from, BoardTroop to);
}