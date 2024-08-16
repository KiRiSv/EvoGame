using Godot;
using System;
using static MLtest;

public partial class Predator : Creature
{
	static int layerSize = 10;
	static int inputSize = 10;
	static int outSize = 4;
	static int speed = 15;
	static int mutatetionChance = 10;
	double[,] hiddenLayer = new double[layerSize,inputSize];
	double[,] output = new double[outSize,layerSize];
	
	private void _on_body_entered(Node body)
	{
		if(body is Prey){
			body.CallDeferred("queue_free");
			PackedScene predatorScene = GD.Load<PackedScene>("res://predator.tscn");
			Predator predatorInstance = (Predator) predatorScene.Instantiate();
			predatorInstance.clone(this);
			AddChild(predatorInstance);
		}
	}
}


