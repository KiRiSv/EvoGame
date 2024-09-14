using Godot;
using System;



public partial class Food : RigidBody2D
{
	public bool eaten = false;

	public void spawnFood(){
		var random = new RandomNumberGenerator();
		random.Randomize();
		Node2D grassInstance = (Node2D) main.grassScene.Instantiate();
		grassInstance.Position = GlobalPosition + new Vector2(x: random.RandfRange(-10, 10),y: random.RandfRange(-10, 10));
		GetParent().AddChild(grassInstance);
	}

}
