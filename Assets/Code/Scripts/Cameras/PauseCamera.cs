using UnityEngine;
using System.Collections;

public class PauseCamera : MonoBehaviour {
	
	public float Speed = 10.0f;
	public float decay = 0.95f;
	public float RadiusCap = 27.0f;
	public float HeightCap = 1.2f;
	
	
	public TweenPosition tweenE;
	
	public float dead = 0.005f; // this is from trial and error
    float minPinchDistance = 0;
 
	
	float _radius = 20.0f;
	float _angle = 270.0f;
	float _height = 7.5f;
	
	GameObject focus = null;
	
	
	Vector3 prevPosition = Vector3.zero;
	Vector3 velocity = Vector3.zero;
	
	bool opened = false;
	
	public enum FocusObject
	{
		Bot = 0,
		Center,
		count
	}
	
	FocusObject _focus = FocusObject.Center;
	
	public FocusObject MyFocus
	{
		get{return _focus;}
		set{
		
			//Calculate the position before switching focus for old position storing.
			CalculatePosition();
			tweenE.from = transform.position;
			
			switch(value)
			{
			case FocusObject.Bot:
			{
				focus = GameObjectTracker.instance.World._objectView.BotView.gameObject;
				break;
			}
				
			case FocusObject.Center:
			{
				focus = GameObjectTracker.instance.World._objectView.gameObject;
				
				break;
			}
				
			default:
			{
				break;
			}
				
			}
			
		_focus = value;
			
		CalculatePosition();
		tweenE.to = transform.position;
		
		tweenE.Reset();
		tweenE.Play(true);
		
			
		}
	}
	
	public void Exit()
	{
		CalculatePosition();
		
		tweenE.from = transform.position;
		tweenE.to = GameObjectTracker.instance.World.CurrentCamera.transform.position;
		
		tweenE.Reset();
		tweenE.Play(true);
		
		opened = false;
		
		
	}
	
	public void Enter()
	{
		CalculatePosition();
		
		tweenE.to = transform.position;
		tweenE.from = GameObjectTracker.instance.World.CurrentCamera.transform.position;
		
		tweenE.Reset();
		tweenE.Play(true);
		
		opened = true;
	}
	
	public bool Open
	{
		get
		{
			return opened;
		}
	}
	
	// Use this for initialization
	void Start () {
	
		
		
	}
	
	void CalculatePosition()
	{
		
		if(!focus)
			return;
		
		Vector3 point = focus.transform.position;
		Vector3 position = Vector3.zero;
		
		float r = Radius;
		float a = Angle;
		float h = Height;
				
		position.x = point.x + r * Mathf.Cos(a) * Mathf.Sin(h);
		position.z = point.z + r * Mathf.Sin(a) * Mathf.Sin(h);
		position.y = point.y + r * Mathf.Cos(h) ;
		
		
		transform.position = position;
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		float t = Time.fixedDeltaTime;
		float s = Speed * t;
		
		if(focus)
		{
			transform.LookAt(focus.transform);
		}
		
		
		if(Input.GetKey(KeyCode.Q))
		{
			Height += s;
		}
		
		if(Input.GetKey(KeyCode.E))
		{
			Height -= s;
		}
		
		if(Input.GetKey(KeyCode.W))
		{
			Radius += s * 6;
		}
		
		if(Input.GetKey(KeyCode.S))
		{
			Radius -= s * 6;
		}
		
		
		if(Input.GetKey(KeyCode.A))
		{
			Angle += s;
		}
		
		if(Input.GetKey(KeyCode.D))
		{
			Angle -= s;
		}
		
		
		//If we have a pinch value. then apply pinch zoom and nothing else.
		float p = Pinch;
		if(Mathf.Abs(p) > 1.0f)
		{
			Radius -= p * 0.05f;
			CalculatePosition();
			return;
			
		}
		
		
		if(Input.GetMouseButton(0))
		{	
			//Get the position
			Vector3 screenpoint = Input.mousePosition;
			
			//Find the distance between last position.
			Vector3 dir = screenpoint - prevPosition;
			dir.Normalize();
			
			
//			Debug.LogError(dir);
			
			//Set the velocity
			//velocity = dir;
			
			if(Mathf.Abs(dir.x) > dead)
			{
				velocity.x = dir.x * s;
				prevPosition = screenpoint;
				
			}
			
			if(Mathf.Abs(dir.y) > dead)
			{
				velocity.y = dir.y * s;
				prevPosition = screenpoint;
				
			}
			
			
		}
		else
		{
		
			prevPosition = Input.mousePosition;
			
		}
		
		//Else button down glide through velocity at a decay.
		Angle -= velocity.x * s;
		Height += velocity.y * s;
		
		
		//Decay the velocity
		velocity *= (decay);
		
		
		
		//CalcualtePinch();
		CalculatePosition();
		
	}
	
	
	void OnEnable()
	{
		//Debug.LogError("Enabled?????");
		focus = GameObjectTracker.instance.World._objectView.gameObject;		
	}
	
	float Pinch
	{
		get{
			
			float pinchAmmount = 0f;
 
       
			// if two fingers are touching the screen at the same time and ...       
			if (Input.touchCount > 1)
			{
				Touch touch1 = Input.touches[0];
				Touch touch2 = Input.touches[1];
				
				if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
				{ 
					// ... pinching, prepare ZOOM
					float pinchDistance = Vector2.Distance(touch1.position, touch2.position);
					float prevDistance = Vector2.Distance(touch1.position - touch1.deltaPosition,
                                               touch2.position - touch2.deltaPosition);
					
					float pinchDistanceDelta = pinchDistance - prevDistance;
					
					if (Mathf.Abs(pinchDistanceDelta) > minPinchDistance)
					{
					
						pinchAmmount = pinchDistanceDelta; 
				
					}
					
					Debug.LogError("Pinch:" + pinchAmmount);
					return pinchAmmount;
         
				}
	
		
			}
	
			return 0;
		}
		
		
	}
	
	float Radius
	{
		set
		{
			//_radius = value;
			_radius = Mathf.Clamp(value,4.2f,RadiusCap);		
			
		}
		get 
		{
			return _radius;
		}
	}
	
	float Angle
	{
		set
		{
			
			_angle = value;
		}
		get 
		{
			return _angle;
		}
	}
	
	float Height
	{
		set
		{
			//_height = value;
			_height = Mathf.Clamp(value,HeightCap,7.7f);			
		}
		get 
		{
			return _height;
		}
	}
}
