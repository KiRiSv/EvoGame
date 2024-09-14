using Godot;
using System;

public partial class settings : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_back_pressed()
	{
	GetTree().ChangeSceneToFile("res://menus/main_menu.tscn");
	}
	
	private void _on_prey_fov_value_changed(int fov){

		GlobalVariables.Instance.preyFov = fov * Mathf.Pi / 180;

	}
	
	private void _on_pred_fov_value_changed(int fov){
		GlobalVariables.Instance.predFov = fov * Mathf.Pi / 180;
	}

	private void _on_raycast_value_changed(int rayCount){
		GlobalVariables.Instance.rayCount = rayCount;
	}
}
