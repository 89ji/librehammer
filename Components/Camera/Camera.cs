using Godot;
using System;

public partial class Camera : Marker3D
{
	[Export] int minFov;
	[Export] int maxFov;
	[Export] float fovAdjustInterval;
	[Export] Camera3D camera;
	[Export] RayCast3D ray;

	[Export] float moveSpeed = 16f;
	[Export] float sprintMultiplier = 1.5f;
	[Export] float xSpeed;
	[Export] float ySpeed;

	Vector2 lastMousePosition;

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("move_camera")) lastMousePosition = GetViewport().GetMousePosition();

		if (Input.IsActionPressed("move_camera")) Input.MouseMode = Input.MouseModeEnum.Captured;
		else Input.MouseMode = Input.MouseModeEnum.Visible;
		
		if (Input.IsActionJustReleased("move_camera")) GetViewport().WarpMouse(lastMousePosition);

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
		RaycastToMouse();
	}

	void RaycastToMouse()
	{
		Vector2 mousePos = GetViewport().GetMousePosition();

		Vector3 from = camera.ProjectRayOrigin(mousePos);
		Vector3 to = from + camera.ProjectRayNormal(mousePos) * 1000f;

		ray.GlobalPosition = from;
		ray.TargetPosition = to - from;
		ray.ForceRaycastUpdate();

		if (ray.IsColliding())
		{
			GD.Print("Hit: ", ray.GetCollider());
			GD.Print("Position: ", ray.GetCollisionPoint());
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
			rot.X = Mathf.Clamp(rot.X, -80f, 0f);
			camera.RotationDegrees = rot;

			RotateY(-motion.Relative.X * xSpeed * .002f);
		}
	}
}
