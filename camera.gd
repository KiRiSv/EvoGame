extends Camera2D

@export var zoomSpeed : float = 20;

var zoomTarget: Vector2
var dragStartMousePos = Vector2.ZERO
var dragStartCameraPos = Vector2.ZERO
var isDragging : bool = false
var zoom_min: Vector2 = Vector2(1, 1)
var zoom_max: Vector2 = Vector2(10, 10)


# Called when the node enters the scene tree for the first time.
func _ready():
	zoomTarget = zoom
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	#if Input.is_action_just_pressed("cam_zoom_in") or Input.is_action_just_pressed("cam_zoom_out"):
	Zoom(delta)
	WASDPan(delta)
	DragPan()
	
func Zoom(delta):
	var zoomEvent = false
	if Input.is_action_just_pressed("cam_zoom_in"):
		zoomTarget *= 1.2
		zoomEvent = true
						
	if Input.is_action_just_pressed("cam_zoom_out"):
		zoomEvent = true
		zoomTarget *= 0.8
		
	zoomTarget = clamp(zoomTarget,zoom_min,zoom_max)
	var oldpos = get_global_mouse_position()
	zoom = zoomTarget
	#zoom = zoom.lerp(zoomTarget, zoomSpeed * delta)
	if zoomEvent:
		position += oldpos - get_global_mouse_position()
	
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
	var offset = get_viewport().size/2
	position.x = clamp(position.x, 50, 1950)
	position.y = clamp(position.y, 50, 1950)


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
		position.x = clamp(position.x, 50, 1950)
		position.y = clamp(position.y, 50, 1950)

