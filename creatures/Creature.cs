using Godot;
using System;
using static MLtest;

public partial class Creature : RigidBody2D
{
	public static int layerSize = 10;
	public static int inputSize = 10;
	public static int outSize = 4;
	public static int maxSpeed = 35;
	public static int mutatetionChance = 10;
	public static float turningRate = .2F;
	public static int speedUpRate = 5;
	public static int rayCount = 5;
	public virtual float fov {get;set;} = .785F;
	public static float sightLength = 100F;
	int speed = 0;
	float facingAngle = 0;
	double[,] hiddenLayer;
	double[,] output;
	RayCast2D[] rays;
	public void clone(Creature parent){
		hiddenLayer = parent.hiddenLayer;
		output = parent.output;
		if(GD.Randi() % 100 == mutatetionChance){
			hiddenLayer = MLtest.mutate(hiddenLayer);
		}
		if(GD.Randi() % 100 == mutatetionChance){
			output = MLtest.mutate(output);
		}
		createRays();
	}
	public Creature(){
		ContactMonitor = true;
		this.MaxContactsReported = 10;
		hiddenLayer = new double[layerSize,inputSize];
		output = new double[outSize,layerSize];
	}
	public void createRays(){
		float startingAngle = -fov/2;
		float raySpread = fov/(rayCount-1);
		rays = new RayCast2D[rayCount];
		for(int i = 0; i < rayCount; i++){
			RayCast2D ray = new RayCast2D();
			ray.TargetPosition = new Vector2(0,-sightLength).Rotated(startingAngle + (i*raySpread));
			ray.SetCollisionMaskValue(1,true);
			ray.SetCollisionMaskValue(2,true);
			rays[i] = ray;
			AddChild(ray);
			//Ray visualization 
			Line2D debugLine = new Line2D();
			debugLine.AddPoint(new Vector2(0,0));
			debugLine.Width = 2F;
			debugLine.AddPoint(new Vector2(0,-sightLength).Rotated(startingAngle + (i*raySpread)));
			AddChild(debugLine);
		}
	}
	public void initialize(){
		hiddenLayer = MLtest.setRandom(hiddenLayer);
		output = MLtest.setRandom(output);
		createRays();
	}
	public override void _Process(double delta){
		double[] input = new double[inputSize];
		int node = 0;
		for (int i = 0; i < rayCount; i++){
			RayCast2D ray = rays[i];
			Node2D obj = (Node2D) ray.GetCollider();
			if(obj is null){
				input[node++] = 0;
				input[node++] = 0;
			} else if (obj is Food) {
				input[node++] = 1;
				input[node++] = (Position - obj.Position).Length();
			} else if (obj is Prey){
				input[node++] = 2;
				input[node++] = (Position - obj.Position).Length();
			} else {
				input[node++] = -1;
				input[node++] = (Position - obj.Position).Length();
			}
		}
		int choice = MLtest.getOutput(input,hiddenLayer,output);
		switch (choice)
		{
			case 0:
				//facingAngle -= turningRate;
				break;
			case 1:
				//facingAngle += turningRate;
				break;
			case 2:
				ApplyImpulse(new Vector2(0,-1));
				break;
			//case 3:
				//LinearVelocity = new Vector2(8,0);
				//break;
		}
		//LinearVelocity = new Vector2(0,-speed).Rotated(facingAngle);
		//Rotation = facingAngle;
		ApplyImpulse(new Vector2(0,-1));
		//LookAt(LinearVelocity);
	}
}
