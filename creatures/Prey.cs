using Godot;
using System;

public partial class Prey : Creature
{
	public override float Fov {
		get{return GlobalVariables.Instance.preyFov;}
		set{}
	}
	static readonly PackedScene preyScene = GD.Load<PackedScene>("res://Creatures/prey.tscn");

	public override double[] SelectTarget(double[] input){
		double[] target = ChooseTarget(input);
		visionCone.Color = new Color(1,1,1,.4F);
		for (int i = 0; i < input.Length; i+=2){
			//food
			if(input[i] == .5){
				if(visionCone.Color.B == 1)
					visionCone.Color = new Color(0,1,0,.4F);
			}
			// prey 
			else if (input[i] == 1){
				if(visionCone.Color.B == 1)
					visionCone.Color = new Color(0,1,0,.4F);
			}
			// predator
			else if (input[i] == .75){
				visionCone.Color = new Color(1,0,0,.4F);
			}
		}
		return target;
	}
	public override void Initialize(){
		nn = neuralNetwork.Call("createPrey",GlobalVariables.Instance.rayCount);
		CreateRays();
	}
	public static double[] ChooseTarget(double[] input){
		double[] target = {.5,0};
		int midRay = (input.Length/2)-1;
		for (int i = 0; i < input.Length; i+=2){
			//food
			if(input[i] == .5){
				target[0] = 1.0;
				target[1] = Math.Sign(i - midRay);
			}
			// prey 
			else if (input[i] == 1){
				target[0] = Math.Max(.75,target[0]);
				if(Math.Abs(target[1]) != 1)
					target[1] = -Math.Sign(i - midRay) / 2;
			}
			// predator
			else if (input[i] == .75){
				target[0] = 1.0;
				//if predator right in front
				if(i - midRay == 0){
					target[1] = -1;
				//if predator on both sides or if food on same side
				} else if (target[1] == -Math.Sign(i - midRay)) {
					target[1] = 0;
				} else {
					target[1] = -Math.Sign(i - midRay);
				}
			}
		}
		target[1] = (target[1]/2) +.5;
		return target;
	}
	
	private void _on_body_entered(Node body){
		if(body is Food food && ! food.eaten){
			food.eaten = true;
			body.CallDeferred("queue_free");
			Prey preyInstance = (Prey) preyScene.Instantiate();
			preyInstance.Clone(this);
			preyInstance.Position = Position;
			preyInstance.Rotation = Rotation;
			GetParent().CallDeferred("add_child",preyInstance);
			//GetParent().AddChild(preyInstance);
		}
	}
}
