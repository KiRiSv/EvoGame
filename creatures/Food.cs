using Godot;
using System;


public partial class Food : RigidBody2D
{
	public Food(){
		//position = new Vector2(GD.Randf()*10,GD.Randf()*10);
	}
	public static void spawnFood(int count){
		for(int i = 0; i < count; i++){
			Food food = new Food();
		}
	}
}
