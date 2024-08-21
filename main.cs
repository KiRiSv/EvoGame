using Godot;
using System;

public partial class main : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	static PackedScene preyScene = GD.Load<PackedScene>("res://creatures/prey.tscn");
	static PackedScene predatorScene = GD.Load<PackedScene>("res://creatures/predator.tscn");
	static PackedScene grassScene = GD.Load<PackedScene>("res://creatures/food.tscn");
	
	static bool mouse_down = false;
	static Vector2 last_placement;
	static int selectedButton = 0;
	static bool mouseInBox = false;
	
	private void ToggleCreature (bool toggledOn, int creatureID){
		if(toggledOn){
			selectedButton = creatureID;
		}
		else{
			selectedButton = 0;
		}
	}
	
	private void _on_v_box_container_mouse_entered()
	{
		main.mouseInBox = true;
	}


	private void _on_v_box_container_mouse_exited()
	{
		main.mouseInBox = false;
	}

	
	public override void _Input(InputEvent @event)
	{
		// Mouse in viewport coordinates.
		if (@event is InputEventMouseButton eventMouseButton)
			if ((int)eventMouseButton.ButtonIndex == 1 & eventMouseButton.Pressed == true)
				main.mouse_down = true;
				
			else if ((int)eventMouseButton.ButtonIndex == 1 & eventMouseButton.Pressed == false)
				main.mouse_down = false;
	}
	
		// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(mouse_down == true& 
			(GetGlobalMousePosition().DistanceTo(last_placement) > 1) &
			main.mouseInBox == false){
			last_placement = GetGlobalMousePosition();
			//TextureButton preyButton = GetNode<TextureButton>("SideMenu/VBoxContainer/SpawnPrey");
			if (selectedButton==1){
				Prey preyInstance = (Prey) main.preyScene.Instantiate();
				preyInstance.Position = last_placement;
				preyInstance.initialize();
				AddChild(preyInstance);
			}	
			else if (selectedButton==2){
				Predator predatorInstance = (Predator) main.predatorScene.Instantiate();
				predatorInstance.initialize();
				predatorInstance.Position = last_placement;
				AddChild(predatorInstance);
			}
			else if (selectedButton==3){
				Node2D grassInstance = (Node2D) main.grassScene.Instantiate();
				grassInstance.Position = last_placement;
				AddChild(grassInstance);
			}
		}
	}
}

