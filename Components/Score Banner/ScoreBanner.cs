using Godot;
using LibHammer.Gamestate;
using System;

public partial class ScoreBanner : Control
{
	public Gamestate? State;

	[Export] RichTextLabel DefVP;
	[Export] RichTextLabel DefCP;
	[Export] RichTextLabel AtkVP;
	[Export] RichTextLabel AtkCP;

	public override void _Process(double delta)
	{
		if (State != null)
		{
			if (State.Player1.isDefender)
			{
				DefVP.Text = State.Player1.VP.ToString();
				DefCP.Text = State.Player1.CP.ToString();

				AtkVP.Text = State.Player2.VP.ToString();
				AtkCP.Text = State.Player2.CP.ToString();
			}

			else
			{
				DefVP.Text = State.Player2.VP.ToString();
				DefCP.Text = State.Player2.CP.ToString();

				AtkVP.Text = State.Player1.VP.ToString();
				AtkCP.Text = State.Player1.CP.ToString();
			}
		}
	}
}
