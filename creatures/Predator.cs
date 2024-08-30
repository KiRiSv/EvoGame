using Godot;
using System;
using static MLtest;

public partial class Predator : Creature
{
	static PackedScene predatorScene = GD.Load<PackedScene>("res://creatures/predator.tscn");
	
	private void _on_body_entered(Node body){
		if(body is Prey && ! ((Prey) body).eaten){
			((Prey) body).eaten = true;
			body.CallDeferred("queue_free");
			Predator predatorInstance = (Predator) predatorScene.Instantiate();
			predatorInstance.clone(this);
			predatorInstance.Position = Position;
			GetParent().CallDeferred("add_child",predatorInstance);
			//GetParent().AddChild(predatorInstance);
		}
	}
}


