using Godot;
using System;

public partial class main : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	static PackedScene preyScene = GD.Load<PackedScene>("res://prey.tscn");
	static PackedScene predatorScene = GD.Load<PackedScene>("res://predator.tscn");
	static PackedScene grassScene = GD.Load<PackedScene>("res://food.tscn");
	
	static bool mouse_down = false;
	static Vector2 last_placement = new Vector2(0,0);
	static int selectedButton = 0;
	
	private void ToggleCreature (bool toggledOn, int creatureID){
		if(toggledOn){
			selectedButton = creatureID;
		}
		else{
			selectedButton = 0;
		}
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
		if(mouse_down & (GetViewport().GetMousePosition().DistanceTo(last_placement) > 1)){
			last_placement = GetViewport().GetMousePosition();
			//TextureButton preyButton = GetNode<TextureButton>("SideMenu/VBoxContainer/SpawnPrey");
			if (selectedButton==1){
				Node2D preyInstance = (Node2D) main.preyScene.Instantiate();
				preyInstance.Position = last_placement;
				AddChild(preyInstance);
			}	
			else if (selectedButton==2){
				Node2D predatorInstance = (Node2D) main.predatorScene.Instantiate();
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

