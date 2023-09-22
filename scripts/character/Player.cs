using Godot;
using System;

public partial class Player : CharacterBody2D {
	#region Variables
		float speed = 400F;
		float life = 100;
		bool flagBool = false;
		PackedScene bullet = (PackedScene)ResourceLoader.Load("res://prefabs/environment/bullet.tscn");
		Area2D collider;
		CollisionShape2D collisionEnemy;
		Timer collisionTimer;
	#endregion

	#region Signals
		[Signal] public delegate void OnHealthChangeEventHandler(float life, float damage);
	#endregion

	#region Godot Methods
		// Called when the node enters the scene tree for the first time.
		public override void _Ready() {
			collider = GetNode<Area2D>("EnemyCollilder");
			collider.BodyEntered += Collider_BodyEntered;

			collisionEnemy = collider.GetNode<CollisionShape2D>("EnemyCollision");
			collisionTimer = collider.GetNode<Timer>("CollisionTimer");

			collisionTimer.Timeout += CollisionTimer_Timeout;

			EmitSignal("OnHealthChange", life, 0);
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta) {
		}

		public override void _Input(InputEvent @event) {
			// Comprobamos que el evento sea un click del mouse
			if(@event is InputEventMouseButton mouseEvent && @event.IsActionPressed("shoot")) {
				Bullet newBullet = (Bullet)bullet.Instantiate();
				// Obtenemos la posicion del mouse
				// newBullet.direction = (mouseEvent.Position - GlobalPosition).Normalized();
				newBullet.direction = (GetGlobalMousePosition() - GlobalPosition).Normalized();
				newBullet.GlobalPosition = this.GlobalPosition;
				GetParent().AddChild(newBullet);
			}
		}

		// Better movement here
		public override void _PhysicsProcess(double delta) {
			// Detect the input to move the player
			// float horVector = Input.GetAxis("ui_left", "ui_right");
			// float verVector = Input.GetAxis("ui_up", "ui_down");
			// Vector2 movement = new Vector2(horVector, verVector).Normalized();

			// Create the vector of the movement and detect the input
			Vector2 movement = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
			// Normalize the movement
			movement = movement.Normalized();
			// Asign the movement to the player
			Velocity = movement * speed;
			// Move the player
			this.MoveAndSlide();
		}
	#endregion

	#region Events
		private void Collider_BodyEntered(Node2D body) {
			if(body.IsInGroup("Enemy") && !flagBool) {
				flagBool = true;
				HurtPlayer(((Enemy)body).damage);
			}
		}

		private void CollisionTimer_Timeout() {
			collisionEnemy.SetDeferred("disabled", false);
			flagBool = false;
		}
	#endregion

	#region Health
		private void HurtPlayer(float damage) {
			life -= damage;

			collisionEnemy.SetDeferred("disabled", true);
			collisionTimer.Start();

			EmitSignal("OnHealthChange", life, damage);
			if(life <= 0)
				GetTree().ReloadCurrentScene();


		}
	#endregion
}
