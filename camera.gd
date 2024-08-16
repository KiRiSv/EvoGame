extends Camera2D

@export var zoomSpeed : float = 10;

var zoomTarget: Vector2
var dragStartMousePos = Vector2.ZERO
var dragStartCameraPos = Vector2.ZERO
var isDragging : bool = false



# Called when the node enters the scene tree for the first time.
func _ready():
	zoomTarget = zoom
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	Zoom(delta)
	WASDPan(delta)
	DragPan()
	
func Zoom(delta):
	if Input.is_action_just_pressed("cam_zoom_in"):
		zoomTarget *= 1.2
		
	if Input.is_action_just_pressed("cam_zoom_out"):
		zoomTarget *= 0.8
	
	zoom = zoom.slerp(zoomTarget, zoomSpeed * delta)
	
	pass




func WASDPan(delta):
	var moveAmount = Vector2.ZERO
	if Input.is_action_pressed("cam_right"):
		moveAmount.x += 1
		
	if Input.is_action_pressed("cam_left"):
		moveAmount.x -= 1
		
	if Input.is_action_pressed("cam_up"):
		moveAmount.y -= 1
		
	if Input.is_action_pressed("cam_down"):
		moveAmount.y += 1
		
	moveAmount = moveAmount.normalized()
	position += moveAmount * delta * 1000 * (1/zoom.x)


func DragPan():
	if !isDragging and Input.is_action_just_pressed("cam_pan"):
		dragStartMousePos = get_viewport().get_mouse_position()
		dragStartCameraPos = position
		isDragging = true
		
	if isDragging and Input.is_action_just_released("cam_pan"):
		isDragging = false
		
	if isDragging:
		var moveVector = get_viewport().get_mouse_position() - dragStartMousePos
		position = dragStartCameraPos - moveVector * 1/zoom.x	

