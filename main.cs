using Godot;
using System;

public partial class main : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<CanvasLayer>("Pause").Hide();
		Creature.Pretrain();
		selectedButton = 0;
	}

	static readonly PackedScene preyScene = GD.Load<PackedScene>("res://creatures/prey.tscn");
	static readonly PackedScene predatorScene = GD.Load<PackedScene>("res://creatures/predator.tscn");
	static readonly PackedScene grassScene = GD.Load<PackedScene>("res://creatures/food.tscn");
	
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

	private void _on_pause_pressed()
	{
		GetTree().Paused = true;
		GetNode<CanvasLayer>("Pause").Show();
		GetNode<CanvasLayer>("UI").Hide();
	}
	private void _on_resume_pressed()
	{
		
		GetNode<CanvasLayer>("Pause").Hide();
		GetNode<CanvasLayer>("UI").Show();
		GetTree().Paused = false;
	}
	
	private void _on_quit_pressed()
	{
		GetTree().Quit();
	}
	
	private void _on_reset_pressed()
	{
		_on_resume_pressed();
		GetTree().ReloadCurrentScene();
	}
	
	private void _on_main_menu_pressed()
	{
		_on_resume_pressed();
		GetTree().ChangeSceneToFile("res://menus/main_menu.tscn");
	}

	public override void _Input(InputEvent @event)
	{
		// Mouse in viewport coordinates.
		if (@event is InputEventMouseButton eventMouseButton){
			if ((int)eventMouseButton.ButtonIndex == 1 & eventMouseButton.Pressed == true)
				main.mouse_down = true;
			else if ((int)eventMouseButton.ButtonIndex == 1 & eventMouseButton.Pressed == false)
				main.mouse_down = false;
		}
	}
	
		// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(mouse_down == true & 
			(GetGlobalMousePosition().DistanceTo(last_placement) > 1) &
			main.mouseInBox == false){
			last_placement = GetGlobalMousePosition();
			//TextureButton preyButton = GetNode<TextureButton>("SideMenu/VBoxContainer/SpawnPrey");
			if (selectedButton==1){
				Prey preyInstance = (Prey) main.preyScene.Instantiate();
				preyInstance.Position = last_placement;
				preyInstance.Initialize();
				GetNode<Node2D>("Preys").AddChild(preyInstance);
			}	
			else if (selectedButton==2){
				Predator predatorInstance = (Predator) main.predatorScene.Instantiate();
				predatorInstance.Initialize();
				predatorInstance.Position = last_placement;
				GetNode<Node2D>("Predators").AddChild(predatorInstance);
			}
			else if (selectedButton==3){
				Node2D grassInstance = (Node2D) main.grassScene.Instantiate();
				grassInstance.Position = last_placement;
				GetNode<Node2D>("Plants").AddChild(grassInstance);
			}
		}
		
		GetNode<RichTextLabel>("UI/Pop/VBoxContainer/pop").Text = "Prey: " + GetNode<Node2D>("Preys").GetChildCount()
			+ "\nPredator: " + GetNode<Node2D>("Predators").GetChildCount()
			+ "\nPlants: " + GetNode<Node2D>("Plants").GetChildCount();
	}
}
