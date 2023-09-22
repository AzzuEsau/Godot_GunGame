using Godot;
using System;

public partial class Bullet : Area2D {
	#region Variables
		public Vector2 direction;
		float speed = 1000;
		Timer timer;
	#endregion

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		// timer = GetNode<Timer>("KillTimer");
		timer = GetChild<Timer>(2);

		BodyEntered += OnBodyEntered;
		timer.Timeout += TimeoutDestroy;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		// GlobalPosition.DirectionTo(direction);
		GlobalPosition += speed * direction * (float)delta;
	}

	#region Events
		private void OnBodyEntered(Node2D body) {
			if(body.IsInGroup("Enemy")) {
				((Enemy)body).Death();
				QueueFree();
			}
		}

		private void TimeoutDestroy() {
			QueueFree();
		}
	#endregion
}
