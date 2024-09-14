using Godot;
using System;

public partial class Prey : Creature
{
	public override float Fov {get;set;} = 1.75F;
	static readonly PackedScene preyScene = GD.Load<PackedScene>("res://creatures/prey.tscn");
	
	public override double[] ChooseTarget(double[] input){
		double[] target = {.5,0};
		int midRay = input.Length/2;
		visionCone.Color = new Color(1,1,1,.4F);
		for (int i = 0; i < input.Length; i+=2){
			//food
			if(input[i] == 1){
				target[0] = 1;
				target[1] = Math.Sign(i - midRay);
				if(visionCone.Color.B == 1)
					visionCone.Color = new Color(0,1,0,.4F);
			}
			// prey 
			else if (input[i] == 2){
				target[0] = Math.Max(.75,target[0]);
				if(Math.Abs(target[1]) != 1)
					target[1] = Math.Sign(i - midRay) / 2;
				if(visionCone.Color.B == 1)
					visionCone.Color = new Color(0,1,0,.4F);
			}
			// predator
			else if (input[i] == -1){
				target[0] = 1;
				//if predator right in front
				if(i - midRay == 0){
					target[1] = -1;
				//if predator on both sides or if food on same side
				} else if (target[1] == Math.Sign(i - midRay)) {
					target[1] = 0;
				} else {
					target[1] = -Math.Sign(i - midRay);
				}
				visionCone.Color = new Color(1,0,0,.4F);
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
			GetParent().CallDeferred("add_child",preyInstance);
			//GetParent().AddChild(preyInstance);
		}
	}
}
