using Godot;
using System;
using static MLtest;

public partial class Creature : RigidBody2D
{
	static int layerSize = 10;
	static int inputSize = 10;
	static int outSize = 4;
	static int speed = 15;
	static int mutatetionChance = 10;
	double[,] hiddenLayer = new double[layerSize,inputSize];
	double[,] output = new double[outSize,layerSize];
	public void clone(Creature parent){
		hiddenLayer = parent.hiddenLayer;
		output = parent.output;
		if(GD.Randi() % 100 == mutatetionChance){
			hiddenLayer = MLtest.mutate(hiddenLayer);
		}
		if(GD.Randi() % 100 == mutatetionChance){
			output = MLtest.mutate(output);
		}
	}
	public Creature(){
		hiddenLayer = MLtest.setRandom(hiddenLayer);
		output = MLtest.setRandom(output);
		LinearVelocity = new Vector2(speed,0);
		ContactMonitor = true;
		this.MaxContactsReported = 10;
	}
	public Creature(double[,] inheritedHiddenLayer, double[,] inheritedOutput){
		hiddenLayer = inheritedHiddenLayer;
		LinearVelocity = new Vector2(speed,0);
		output = inheritedOutput;
	}
	public override void _Process(double delta){
		double[] randomInput = new double[inputSize];
		for(int i=0; i< inputSize; i++){
			randomInput[i] = GD.RandRange(-1,1);
		}
		int choice = MLtest.getOutput(randomInput,hiddenLayer,output);
		switch (choice)
		{
			//case 0:
				//LinearVelocity = new Vector2(0,0);
				//break;
			case 1:
				LinearVelocity = LinearVelocity.Rotated(.2F);
				break;
			case 0:
				LinearVelocity = LinearVelocity.Rotated(-.2F);
				break;
			//case 3:
				//LinearVelocity = new Vector2(8,0);
				//break;
		}
	}
}
