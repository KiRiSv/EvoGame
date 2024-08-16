using Godot;
using System;
using static MLtest;

public partial class Predator : Creature
{
	static PackedScene predatorScene = GD.Load<PackedScene>("res://predator.tscn");
	
	private void _on_body_entered(Node body){
		if(body is Prey){
			body.CallDeferred("queue_free");
			Predator predatorInstance = (Predator) predatorScene.Instantiate();
			predatorInstance.clone(this);
			predatorInstance.Position = Position;
			GetParent().AddChild(predatorInstance);
		}
	}
}


