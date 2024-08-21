using Godot;
using System;
using static MLtest;

public partial class Prey : Creature
{
	public override float fov {get;set;} = 1.75F;
	static PackedScene preyScene = GD.Load<PackedScene>("res://creatures/prey.tscn");
	public bool eaten = false;
	
	private void _on_body_entered(Node body){
		if(body is Food && ! ((Food) body).eaten){
			((Food) body).eaten = true;
			body.CallDeferred("queue_free");
			Prey preyInstance = (Prey) preyScene.Instantiate();
			preyInstance.clone(this);
			preyInstance.Position = Position;
			GetParent().AddChild(preyInstance);
		}
	}
}


