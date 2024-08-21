using Godot;
using System;

public partial class main_menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Button button = GetNode<Button>("GridContainer/Start"); 
		button.Pressed += StartGame;
	}

	private void StartGame()
	{
		GetTree().ChangeSceneToFile("res://main.tscn");
	}
}
