using Godot;
using System;

public partial class main_menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		button.Pressed += StartGame;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void StartGame()
	{
		GetTree().ChangeSceneToFile('res://main.tscn')
	}
}
