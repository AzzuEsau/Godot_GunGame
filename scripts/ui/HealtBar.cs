using Godot;
using System;

public partial class HealtBar : ProgressBar {
	#region Variables
		Player player;
	#endregion

	#region Godot Methods
		// Called when the node enters the scene tree for the first time.
		public override void _Ready() {
			player = GetNode<Player>("../../../");
			player.OnHealthChange += Player_OnHealthChange;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta) {
		}
	#endregion	

	#region Events
		private void Player_OnHealthChange(float healt, float damage) {
			Value = healt;
		}
	#endregion
}
