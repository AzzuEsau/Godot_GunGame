using Godot;
using System;

public partial class Game : Node {
	#region Variables
		PackedScene enemy = (PackedScene)ResourceLoader.Load("res://prefabs/enemies/enemy.tscn");
		Timer spawnEnemyTimer;
		int enemiesToKill;
		Label enemiesToKillLabel;


		CanvasLayer winCanvas;
		Button restartButton;
		Button exitButton;
	#endregion

	#region Godot Methods
		// Called when the node enters the scene tree for the first time.
		public override void _Ready() {
			spawnEnemyTimer = GetNode<Timer>("EnemySpawnTimer");
			spawnEnemyTimer.Timeout += SpawnEnemyTimer_Timeout;

			enemiesToKillLabel = GetNode<Label>("CanvasLayer/HBoxContainer/enemiesLabel");
			RestartEnemiesToKill();


			winCanvas = GetNode<CanvasLayer>("CanvasFinish");
			winCanvas.Visible = false;
			restartButton = GetNode<Button>("CanvasFinish/Win/Restart");
			restartButton.Pressed += RestartButton_Pressed;
			exitButton = GetNode<Button>("CanvasFinish/Win/Exit");
			exitButton.Pressed += ExitButton_Pressed;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta) {
			
		}
	#endregion

	#region Labels
		private void UpdateEnemiesToKill(int enemies) {
			this.enemiesToKill = enemies;
			enemiesToKillLabel.Text = this.enemiesToKill.ToString();
		}

		private void RestartEnemiesToKill() {
			UpdateEnemiesToKill(GD.RandRange(1, 5));
		}

		private void Pause(bool paused) {
			winCanvas.Visible = paused;
			GetTree().Paused = paused;
		}
	#endregion

	#region Events
		private void SpawnEnemyTimer_Timeout() {
			Enemy newEnemy = (Enemy)enemy.Instantiate();
			newEnemy.OnDeath += NewEnemy_OnDeath;
			newEnemy.GlobalPosition = new Vector2(GD.RandRange(-650, 1900), GD.RandRange(-333, 1080));
			AddChild(newEnemy);
		}


		private void NewEnemy_OnDeath() {
			UpdateEnemiesToKill(enemiesToKill - 1);
			if(enemiesToKill <= 0) {
				Pause(true);
			}
		}

		private void RestartButton_Pressed() {
			Pause(false);
			GetTree().ReloadCurrentScene();
		}

		private void ExitButton_Pressed() {
			GetTree().Quit();
		}
	#endregion
}
