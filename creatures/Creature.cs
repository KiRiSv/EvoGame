using Godot;
using System;
public partial class Creature : RigidBody2D
{
	private static int inputSize = 10;
	private static int maxSpeed = 10;
	private static float turningRate = .8F;
	private static int rayCount = 5;
	public virtual float Fov {get;set;} = .785F;
	private static float sightLength = 100F;
	public bool eaten = false;
	public Variant nn;
	private static readonly GDScript neuralNetwork = GD.Load<GDScript>("res://creatures/neuralNet.gd");
	private double[] chosenMove = {0,0};
	Line2D debugTarget;
	Line2D debugChoice;
	public Polygon2D visionCone;
	RayCast2D[] rays;
	public void Clone(Creature parent){
		CreateRays();
		nn = neuralNetwork.Call("clone", parent.nn);
	}
	public void CreateRays(){
		float startingAngle = -Fov/2;
		float raySpread = Fov/(rayCount-1);
		rays = new RayCast2D[rayCount];
		visionCone = new();
		int coneSegmentFactor = 10;
		Vector2[] nodes = new Vector2[(rayCount*coneSegmentFactor)+1];
		nodes[0] = new Vector2(0,0);
		int nodeIndex = 1;
		for(int i = 0; i < rayCount; i++){
			RayCast2D ray = new();
			ray.TargetPosition = new Vector2(0,-sightLength).Rotated(startingAngle + (i*raySpread));
			for (int j = 0; j < coneSegmentFactor; j++){
				nodes[nodeIndex++] = new Vector2(0,-sightLength).Rotated(startingAngle + ((i+(j*(1F/coneSegmentFactor)))*raySpread));
				if(i == rayCount-1)
					break;
			}
			ray.SetCollisionMaskValue(1,true);
			ray.SetCollisionMaskValue(2,true);
			rays[i] = ray;
			AddChild(ray);
			//Ray visualization 
			Line2D debugLine = new();
			debugLine.AddPoint(new Vector2(0,0));
			debugLine.Width = 2F;
			debugLine.AddPoint(new Vector2(0,-sightLength).Rotated(startingAngle + (i*raySpread)));
			AddChild(debugLine);
		}
		visionCone.Polygon = nodes;
		visionCone.Color = new Color(1,1,1,.4F);
		AddChild(visionCone);
		debugTarget = new Line2D();
		debugTarget.AddPoint(new Vector2(0,0));
		debugTarget.Width = 2F;
		debugTarget.AddPoint(new Vector2(0,-.5F));
		debugTarget.DefaultColor = new Color(0,0,1,1);
		AddChild(debugTarget);
		debugChoice = new Line2D();
		debugChoice.DefaultColor = new Color(1,0,0,1);
		debugChoice.AddPoint(new Vector2(0,0));
		debugChoice.Width = 2F;
		debugChoice.AddPoint(new Vector2(0,-.5F));
		AddChild(debugChoice);
	}
	public void Initialize(){
		int[] layerSizes = {inputSize,5,5,2};
		nn = neuralNetwork.Call("createNetwork", layerSizes);
		CreateRays();
	}
	// Makes them look where they are moiving
	private void LookWhereGo(){
		float movementAngle = (LinearVelocity.Angle() + ((float) Math.PI / 2 * 3)) % ((float) Math.PI*2);
		float currentRotation = (float)((Rotation + Math.PI)) % ((float) Math.PI*2);
		if(LinearVelocity.Length() != 0 && Math.Abs(movementAngle - currentRotation)  % ((float) Math.PI*2) > 0.04){
			AngularVelocity = Math.Sign((movementAngle - currentRotation)  % ((float) Math.PI*2)) * .1F;
		}
		GD.Print(movementAngle,"  ", currentRotation);
	}
	public override void _IntegrateForces(PhysicsDirectBodyState2D state){
		LookWhereGo();
		ApplyForce(new Vector2(0, (float)-chosenMove[0] * maxSpeed).Rotated(Rotation + (float)((chosenMove[1] -.5F)*turningRate)));
	}
	public virtual double[] ChooseTarget(double[] input){
		double[] target = new double[] {0,0};
		return target;
	}
	public override void _Process(double delta){
		if(eaten){
			return;
		}
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
		chosenMove = (double[]) neuralNetwork.Call("getOutput", nn, input);
		double[] target = ChooseTarget(input);
		chosenMove[0] = 0.1;
		chosenMove[1] = 0;
		// double[] target = {1,0};
		// chosenMove[0] = 1;
		// chosenMove[1] = 1;
		// Rotation = (float)Math.PI/2;
		Vector2 targetDirection = new Vector2(0, (float)-target[0] * maxSpeed).Rotated((float)((target[1] -.5)*turningRate));
		Vector2 choiceDirection = new Vector2(0, (float)-chosenMove[0] * maxSpeed).Rotated((float)( (chosenMove[1] -.5)*turningRate));
		debugTarget.RemovePoint(1);
		debugChoice.RemovePoint(1);
		debugTarget.AddPoint(targetDirection*15);
		debugChoice.AddPoint(choiceDirection*15);
		// debugTarget.AddPoint(new Vector2((float)target[0], (float)target[1])*50);
		// debugChoice.AddPoint(new Vector2((float)chosenMove[0], (float)chosenMove[1])*50);
		// GD.Print(new Vector2((float)target[0], (float)target[1]));
		// GD.Print(new Vector2((float)chosenMove[0], (float)chosenMove[1]));
		// GD.Print();
		if(target[0] == .5 && Pyth(chosenMove[0] -.5,chosenMove[1]) < .1){}
		else
			neuralNetwork.Call("train", nn, input, target);
	}
	static double Pyth(double a, double b){
		return Math.Sqrt((a*a)+ (b*b));
	}
}
