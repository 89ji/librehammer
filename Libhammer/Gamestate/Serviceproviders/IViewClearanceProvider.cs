namespace LibHammer.Gamestate.Serviceproviders;

public interface IViewClearanceProvider
{
    public ViewClearance DetermineVisibility(BoardTroop from, BoardTroop to);
}

public enum ViewClearance
{
    FullyVisible,
    InCover,
    Obstructed
}