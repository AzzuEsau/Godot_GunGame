using Godot;
using System;

public partial class Enemy : CharacterBody2D {
	#region Variables
		Player player;
		float speed = 200;
		public float damage = 10;
	#endregion

	#region Signals
		[Signal] public delegate void OnDeathEventHandler();
	#endregion

	#region Godot Methods
		// Called when the node enters the scene tree for the first time.
		public override void _Ready() {
			player = (Player)GetTree().GetFirstNodeInGroup("Player");
		}

		public override void _PhysicsProcess(double delta) {
			if(player != null) {
				Velocity = (player.GlobalPosition - this.GlobalPosition).Normalized() * speed;
				this.MoveAndSlide();
			}
		}
	#endregion

	public void Death() {
		EmitSignal("OnDeath");
		QueueFree();
	}
}
