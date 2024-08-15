using Godot;
using System;

public partial class main : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

 	static PackedScene PreyScene = GD.Load<PackedScene>("res://prey.tscn");
	
	static bool mouse_down = false;
	static Vector2 last_placement = new Vector2(0,0);
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
		if(mouse_down & (GetViewport().GetMousePosition().DistanceTo(last_placement) > 50)){
			last_placement = GetViewport().GetMousePosition();
			var preyInstance = main.PreyScene.Instantiate();
			preyInstance.Position = last_placement;
			AddChild(preyInstance);
		}
	}
}
