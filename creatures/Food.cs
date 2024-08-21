using Godot;
using System;


public partial class Food : RigidBody2D
{
	public bool eaten = false;
	public static void spawnFood(int count){
		for(int i = 0; i < count; i++){
			Food food = new Food();
		}
	}
}
