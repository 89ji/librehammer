using Godot;
using LibHammer.Gamestate;
using System;

public partial class UnitPopup : Control
{
	[Export] Camera cam;
	[Export] RichTextLabel Name;
	[Export] RichTextLabel Movement;
	[Export] RichTextLabel Toughness;
	[Export] RichTextLabel Armor;
	[Export] RichTextLabel Wounds;
	[Export] RichTextLabel Leadership;
	[Export] RichTextLabel ObjControl;
	[Export] RichTextLabel ShowMore;

	public BoardTroop troop;
	public float? AboutToMove;

	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("alt"))
		{
			ShowMore.Hide();
			Toughness.Show();
			Armor.Show();
			Wounds.Show();
			Leadership.Show();
			ObjControl.Show();
		}

		else
		{
			ShowMore.Show();
			Toughness.Hide();
			Armor.Hide();
			Wounds.Hide();
			Leadership.Hide();
			ObjControl.Hide();
		}

		if (troop == null) return;

		Name.Text = troop.Stats.Name;

		if (AboutToMove.HasValue) Movement.Text = $"{troop.AvailableMovement:F1}({troop.AvailableMovement - AboutToMove:F1})/{troop.Stats.Movement} Meters";
		else Movement.Text = $"{troop.AvailableMovement}/{troop.Stats.Movement} Meters";

		Toughness.Text = $"Toughness: {troop.Stats.Toughness}";
		Armor.Text = $"Armor Save: {troop.Stats.ArmorSave}+";
		Wounds.Text = $"Wounds: {troop.Stats.Wounds - troop.DamageTaken}/{troop.Stats.Wounds}";
		Leadership.Text = $"Leadership: {troop.Stats.Leadership}+";
		ObjControl.Text = $"Objective Control: {troop.Stats.ObjectiveControl}";

	}
}
