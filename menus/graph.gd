extends Control

var prey_pop = []
var pred_pop = []
var plant_pop = []
var pop_length = 600

# Called when the node enters the scene tree for the first time.
func _ready():
	prey_pop.resize(pop_length)
	prey_pop.fill(0.0)
	pred_pop.resize(pop_length)
	pred_pop.fill(0.0)
	plant_pop.resize(pop_length)
	plant_pop.fill(0.0)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass



func _on_timer_timeout():
	$Graph2D.remove_all()
	prey_pop.append(get_node("../../../Preys").get_child_count())
	prey_pop.remove_at(0)
	pred_pop.append(get_node("../../../Predators").get_child_count())
	pred_pop.remove_at(0)
	plant_pop.append(get_node("../../../Plants").get_child_count())
	plant_pop.remove_at(0)	
	
	var prey_plot = $Graph2D.add_plot_item("Prey",Color.LAWN_GREEN)
	var pred_plot = $Graph2D.add_plot_item("Predator",Color.RED)
	var plant_plot = $Graph2D.add_plot_item("Plant",Color.DARK_GREEN)
	
	for i in range(0,pop_length,1):
		prey_plot.add_point(Vector2(i,prey_pop[i]))
		pred_plot.add_point(Vector2(i,pred_pop[i]))
		plant_plot.add_point(Vector2(i,plant_pop[i]))
