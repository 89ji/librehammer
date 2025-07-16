using Godot;
using LibHammer.Gamestate;
using System;

public partial class RulesAndObjectives : Control
{
	[Export] Gameboard board;
	[Export] RichTextLabel rulesTitle;
	[Export] RichTextLabel rulesText;
	[Export] RichTextLabel primaryTitle;
	[Export] RichTextLabel primaryText;

	[Export] RichTextLabel secondary1Title;
	[Export] RichTextLabel secondary1Text;

	[Export] RichTextLabel secondary2Title;
	[Export] RichTextLabel secondary2Text;


	Gamestate state;

	public override void _Ready()
	{
		state = board.State;
	}

	public override void _Process(double delta)
	{
		rulesTitle.Text = state.Rule.Name;
		rulesText.Text = state.Rule.Description;

		primaryTitle.Text = state.Primary.Name;
		primaryText.Text = state.Primary.Description;

		secondary1Title.Text = state.PhaseMan.ActivePlayer.Secondaries[0].Name;
		secondary1Text.Text = state.PhaseMan.ActivePlayer.Secondaries[0].Description;

		secondary2Title.Text = state.PhaseMan.ActivePlayer.Secondaries[1].Name;
		secondary2Text.Text = state.PhaseMan.ActivePlayer.Secondaries[1].Description;
	}
}
