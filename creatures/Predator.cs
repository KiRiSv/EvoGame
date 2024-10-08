using Godot;
using System;

public partial class Predator : Creature
{
	public override float Fov {
		get{return GlobalVariables.Instance.predFov;}
		set{}
	}
	static readonly PackedScene predatorScene = GD.Load<PackedScene>("res://Creatures/predator.tscn");
	public override double[] SelectTarget(double[] input){
		double[] target = ChooseTarget(input);
		if(target[0] == 1){
			visionCone.Color = new Color(1,0,0,.4F);
		} else if (target[0] == .75){
			visionCone.Color = new Color(0,1,0,.4F);
		} else {
			visionCone.Color = new Color(1,1,1,.4F);
		}
		return target;
	}
	public static double[] ChooseTarget(double[] input){
		double[] target = {.5,0};
		int midRay = (input.Length/2)-1;
		for (int i = 0; i < input.Length; i+=2){
			//food
			if(input[i] == .5){
				target[0] = Math.Max(.75,target[0]);
				if(Math.Abs(target[1]) != 1)
					target[1] = Math.Sign(i - midRay) / 2;
			}
			// prey 
			else if (input[i] == 1){
				target[0] = 1.0;
				target[1] = Math.Sign(i - midRay);
			}
		}
		target[1] = (target[1]/2) +.5;
		return target;
	}
	public override void Initialize(){
		nn = neuralNetwork.Call("createPredator",GlobalVariables.Instance.rayCount);
		eatingTimer = GetNode<Timer>("EatingTimer");
		starvingTimer = GetNode<Timer>("StarvingTimer");
		CreateRays();
	}

	private void _on_starving_timer_timeout(){
		CallDeferred("queue_free");
	}
	private void _on_body_entered(Node body){
		if(eatingTimer.IsStopped() && body is Prey prey && ! prey.eaten){
			prey.eaten = true;
			body.CallDeferred("queue_free");
			Predator predatorInstance = (Predator) predatorScene.Instantiate();
			predatorInstance.Clone(this);
			predatorInstance.Position = Position;
			predatorInstance.Rotation = Rotation;
			GetParent().CallDeferred("add_child",predatorInstance);
			eatingTimer.Start();
			starvingTimer.Start();
		}
	}
}
