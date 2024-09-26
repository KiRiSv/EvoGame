using Godot;
using System;

public partial class GlobalVariables : Node
{
	public static GlobalVariables Instance { get; private set; }
	
	public float preyFov { get; set; }
	
	public float predFov { get; set; }
	
	public int rayCount { get; set; } = 5;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

}
