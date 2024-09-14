class_name neuralNetwork

static func createNetwork(ray_count : int) -> NNET:
	print("t")
	var nn : NNET = NNET.new([ray_count*2,5,5,2],false)
	nn.set_loss_function(BNNET.LossFunctions.MSE)
	nn.use_Rprop(.3)
	nn.set_batch_size(1)
	return nn

static func createPredator(ray_count : int) -> NNET:
	var nn : NNET = NNET.new([ray_count*2,5,5,2],false)
	nn.load_data("res://Creatures/predator.nn")
	return nn

static func savePredator(nn : NNET):
	nn.save_binary("res://Creatures/predator.nn")

static func savePrey(nn : NNET):
	nn.save_binary("res://Creatures/prey.nn")

static func createPrey(ray_count : int) -> NNET:
	var nn : NNET = NNET.new([ray_count*2,5,5,2],false)
	nn.load_data("res://Creatures/prey.nn")
	return nn

static func cloneNetwork(nn : NNET) -> NNET:
	return nn.duplicate()

static func train(nn : NNET, input : Array[float], target : Array[float]) -> void:
	if nn == null:
		return
	nn.train([input],[target])

static func getOutput(nn: NNET, input : Array[float]) -> Array:
	if nn == null:
		return [0.,0.]
	nn.set_input(input)
	nn.propagate_forward()
	return nn.get_output()
