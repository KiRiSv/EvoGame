using Godot;
using System;

public partial class MLtest : Node
{
	private static int maxInd(double[] list){
		double max = list[0];
		int maxI = 0;
		for(int i = 1; i < list.Length; i++){
			if(max < list[i]){
				maxI = i;
				max = list[i];
			}
		}
		return maxI;
	}
	public static int getOutput(double[] input, double[,] hiddenLayer, double[,] output){
		double[] cellSums = new double[hiddenLayer.GetLength(0)];
		for(int i = 0; i< hiddenLayer.GetLength(0); i++){
			for(int j = 0; j< input.Length;j++){
				cellSums[i] += input[j] * hiddenLayer[i,j];
			}
		}
		double[] outCellSums = new double[output.GetLength(0)];
		for(int i = 0; i< output.GetLength(0); i++){
			for(int j = 0; j< cellSums.Length;j++){
				outCellSums[i] += cellSums[j] * output[i,j];
			}
		}
		return maxInd(outCellSums);
	}
	public static double[,] mutate(double[,] arr){
		long i = GD.Randi() % arr.GetLength(0);
		long j = GD.Randi() % arr.GetLength(1);
		arr[i,j] = GD.RandRange(-1,1);
		return arr;
	}
	public static double[,] setRandom(double[,] arr){
		for(int i = 0; i < arr.GetLength(0); i++){
			for(int j = 0; j < arr.GetLength(1);j++){
				arr[i,j] = GD.RandRange(-1,1);
			}
		}
		return arr;
	}
}
