using Godot;
using System;
using static MLtest;

public partial class Creature : RigidBody2D
{
	public int layerSize = 10;
	public int inputSize = 10;
	public int outSize = 4;
	public int maxSpeed = 35;
	public int mutatetionChance = 10;
	public float turningRate = .2F;
	public int speedUpRate = 5;
	public int rayCount = 5;
	public virtual float fov {get;set;} = .785F;
	public float sightLength = 50F;
	int speed = 0;
	float facingAngle = 0;
	double[,] hiddenLayer;
	double[,] output;
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
		for(int i = 0; i < rayCount; i++){
			RayCast2D ray = new RayCast2D();
			ray.TargetPosition = new Vector2(0,-sightLength).Rotated(startingAngle + (i*raySpread));
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
		double[] randomInput = new double[inputSize];
		int node = 0;
		for (; node < rayCount; node++){
			
		}
		randomInput[node++] = speed;
		for(; node< inputSize; node++){
			randomInput[i] = GD.RandRange(-1,1);
		}
		int choice = MLtest.getOutput(randomInput,hiddenLayer,output);
		switch (3)
		{
			case 0:
				facingAngle -= turningRate;
				LinearVelocity = new Vector2(0,-speed).Rotated(facingAngle);
				break;
			case 1:
				facingAngle += turningRate;
				LinearVelocity = new Vector2(0,-speed).Rotated(facingAngle);
				break;
			case 2:
				speed = Math.Min(maxSpeed, speed+speedUpRate);
				break;
			//case 3:
				//LinearVelocity = new Vector2(8,0);
				//break;
		}
		Rotation = facingAngle;
		//LookAt(LinearVelocity);
	}
}
