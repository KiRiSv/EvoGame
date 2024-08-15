using Godot;
using System;
using static MLtest;

public partial class Prey : Node2D
{
	static int layerSize = 10;
	static int inputSize = 10;
	static int outSize = 10;
	static int mutatetionChance = 10;
	double[,] hiddenLayer = new double[layerSize,inputSize];
	double[,] output = new double[outSize,layerSize];
	public Prey clone(){
		Prey clone = new Prey(hiddenLayer, output);
		if(GD.Randi() % 100 == mutatetionChance){
			hiddenLayer = MLtest.mutate(hiddenLayer);
		}
		if(GD.Randi() % 100 == mutatetionChance){
			output = MLtest.mutate(output);
		}
		return clone;
	}
	public Prey(){
		hiddenLayer = MLtest.setRandom(hiddenLayer);
		output = MLtest.setRandom(output);
	}
	public Prey(double[,] inheritedHiddenLayer, double[,] inheritedOutput){
		hiddenLayer = inheritedHiddenLayer;
		output = inheritedOutput;
	}
}
