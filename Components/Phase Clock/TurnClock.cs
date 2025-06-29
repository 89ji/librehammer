using Godot;
using LibHammer.Gamestate;
using System;

public partial class TurnClock : Control
{
    [Export] Gameboard board;
    [Export] Sprite2D cmd;
    [Export] Sprite2D move;
    [Export] Sprite2D shoot;
    [Export] Sprite2D charge;
    [Export] Sprite2D fight;
    [Export] Panel panel;
    [Export] RichTextLabel text;

    [Export] Color Defender;
    [Export] Color Attacker;

    public override void _Ready()
    {
        //ShowInfo(new Player { Name = "Jocelyn", isDefender = false }, Phase.Fight, 3);
    }

    public override void _Process(double delta)
    {
        ShowInfo(board.State.PhaseMan.ActivePlayer, board.State.PhaseMan.CurrentPhase, board.State.PhaseMan.Turn);
    }

    public void ShowInfo(Player activeplayer, Phase phase, int turn)
    {
        if (activeplayer.isDefender) panel.Modulate = Defender;
        else panel.Modulate = Attacker;

        HideAll();

        switch (phase)
        {
            case Phase.Command:
                cmd.Show();
                break;
            case Phase.Move:
                move.Show();
                break;
            case Phase.Charge:
                charge.Show();
                break;
            case Phase.Shoot:
                shoot.Show();
                break;
            case Phase.Fight:
                fight.Show();
                break;
            default:
                HideAll();
                break;
        }

        text.Text = $"{activeplayer.Name}'s {phase} Phase\nTurn {turn}";
        panel.Size = new(text.Size.X + 90, 74);
    }

    void HideAll()
    {
        cmd.Hide();
        move.Hide();
        shoot.Hide();
        charge.Hide();
        fight.Hide();
    }
}
