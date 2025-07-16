using Godot;
using LibHammer.Structs;
using System;

public partial class Camera : Marker3D
{
	[Export] int minFov;
	[Export] int maxFov;
	[Export] float fovAdjustInterval;
	[Export] Camera3D camera;
	[Export] RayCast3D ray;
	[Export] RichTextLabel crosshair;
	[Export] Node3D crosshair3d;
	[Export] Node3D positionMarker;

	[Export] float moveSpeed = 16f;
	[Export] float sprintMultiplier = 1.5f;
	[Export] float xSpeed;
	[Export] float ySpeed;

	public GameTroop SelectedTroop;
	public GameTroop HoveredTroop;
	public Vector3? SelectedLocation;
	public Vector3? HoveredLocation;

	Vector2 lastMousePosition;
	Gameboard board;

	[Signal] public delegate void SelectedTroopUpdatedEventHandler();
	[Signal] public delegate void SelectedLocationUpdatedEventHandler();

	public override void _Ready()
	{
		// Finding the game board
		board = GetParent<Gameboard>();
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("move_camera"))
		{
			lastMousePosition = GetViewport().GetMousePosition();
			crosshair.Show();
		}

		if (Input.IsActionPressed("move_camera")) Input.MouseMode = Input.MouseModeEnum.Captured;
		else Input.MouseMode = Input.MouseModeEnum.Visible;

		if (Input.IsActionJustReleased("move_camera"))
		{
			GetViewport().WarpMouse(lastMousePosition);
			crosshair.Hide();
		}


		Vector3 movement = new();

		if (Input.IsActionPressed("move_forward")) movement.Z -= 1f;
		if (Input.IsActionPressed("move_back")) movement.Z += 1f;

		if (Input.IsActionPressed("move_left")) movement.X -= 1f;
		if (Input.IsActionPressed("move_right")) movement.X += 1f;

		if (movement != Vector3.Zero) movement = movement.Normalized();


		movement *= moveSpeed;
		if (Input.IsActionPressed("sprint")) movement *= sprintMultiplier;


		movement *= (float)delta;

		Translate(movement);
	}

	public override void _PhysicsProcess(double delta)
	{
		CalculateHover();

		if (GD.Randf() < .1)
		{
			var selName = SelectedTroop == null ? "none" : SelectedTroop.Troop.Stats.Name;
			var hovName = HoveredTroop == null ? "none" : HoveredTroop.Troop.Stats.Name;

			GD.Print($"Selected Troop: {selName}\nHovered Troop: {hovName}\n");
		}
	}

	void CalculateHover()
	{
		crosshair3d.Hide();

		HoveredTroop = null;
		HoveredLocation = null;

		var selectedObj = ray.GetCollider();
		if (selectedObj == null) return;

		// Assumes the direct parent of an area is a troop, but checks
		if (selectedObj is Area3D area)
		{
			var parent = area.GetParent();
			if (parent is GameTroop troop) TroopHover(troop);
			else TerrainHover();
		}

		else if (selectedObj is StaticBody3D body) TerrainHover();
	}

	void TroopHover(GameTroop troop)
	{
		HoveredTroop = troop;
		if (Input.IsActionJustPressed("select"))
		{
			SelectedTroop = troop;
			EmitSignal(SignalName.SelectedTroopUpdated);
		}
	}

	void TerrainHover()
	{
		crosshair3d.Show();

		var position = ray.GetCollisionPoint();
		HoveredLocation = position;
		crosshair3d.Position = position;
		if (Input.IsActionJustPressed("select"))
		{
			SelectedLocation = position;
			positionMarker.Position = position;
			EmitSignal(SignalName.SelectedLocationUpdated);
		}
	}


	public override void _Input(InputEvent vent)

	{
		if (vent is InputEventMouseButton mouse)
		{
			/*
			if (mouse.ButtonIndex == MouseButton.WheelDown) camera.Fov = (camera.Fov + fovAdjustInterval <= maxFov) ? camera.Fov + fovAdjustInterval : maxFov;
			else if (mouse.ButtonIndex == MouseButton.WheelUp) camera.Fov = (camera.Fov - fovAdjustInterval >= minFov) ? camera.Fov - fovAdjustInterval : minFov;
			*/

			if (mouse.ButtonIndex == MouseButton.WheelDown) Translate(new(0, -.25f * fovAdjustInterval, 0));
			else if (mouse.ButtonIndex == MouseButton.WheelUp) Translate(new(0, .25f * fovAdjustInterval, 0));

			if (Position.Y < 1) Position = new(Position.X, 1, Position.Z);
			if (Position.Y > 20) Position = new(Position.X, 20, Position.Z);

		}

		if (Input.IsActionPressed("move_camera") && vent is InputEventMouseMotion motion)
		{
			camera.RotateX(-motion.Relative.Y * ySpeed * .002f);
			var rot = camera.RotationDegrees;
			rot.X = Mathf.Clamp(rot.X, -75f, 0f);
			camera.RotationDegrees = rot;

			RotateY(-motion.Relative.X * xSpeed * .002f);
		}
	}
}
