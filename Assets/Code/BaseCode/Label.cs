using UnityEngine;
using System.Collections;

public class Label : MonoBehaviour {
	
	bool captured = false;
	bool popped = true;
	bool isactive = true;
	
	//float dragallouance = 10.0f;
	float full = 100.0f;
	Rect screen;
	
	public bool Button = false;
	public bool Drag = true;
	public bool HighlightCapture = true;
	public bool Freeze = false;

	public Color HighlightColor = Color.red;
	
	//Debug Stuff.
	public float dragdistance = 0.0f;
	public float DragAction = 0.0f;
	
	public GUITexture HitTexture;
	public bool UseHitLabel = false;
	
	//Local Variables to store.
	string Action; 
	Vector3 OriginalPosition;
	Vector3 OriginalScale;
	
	
	//Store the touch that we began with.
	Touch touch; 
	
	enum LabelState
	{
		Popped,
	}
	
	LabelState activeState;
	
	// Use this for initialization
	protected virtual void Start () {
		
		Initialize();
	
	}
	
	protected void Initialize()
	{
		//Store the original position for later use.
		OriginalPosition = transform.position;
		OriginalScale = transform.localScale;
		
		//dragallouance = guiTexture.pixelInset.width * 2;
		
		//If we have a hit texture hide it.
		if(HitTexture)
		{
			HitTexture.GetComponent<GUITexture>().enabled = false;
			
		}
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		

		
		CheckInput();
		CaptureUpdate();
				
		
	}
	
	void CheckInput()
	{
		
		//Either way we unhighlight?
		if(HighlightCapture)
		{
			UnHighLight();
		}
		
		
		//We  test if the button is pressed down and we are in the boundary of the lable.
		//We only test for the first touch, maybe more will be checked depending on the scope.
		if(LabelTouch())
		{

			//If our action has passed 1 we go for it. 
			if(!Button && isactive)
			{
				SendMessageUpwards(Action, SendMessageOptions.DontRequireReceiver);
			}
						
			//we are not popped anymore
			popped = false;
			
			//If we arent dragging the button, return and skip the dragging code.  
			if(Drag )
			{
				captured = true;	
			}
			
			
			//Highlight if need be
			if(HighlightCapture)
			{
				Highlight();
			}
			
			//Update the drag action based on the delta of 
			if(Input.multiTouchEnabled)
			{
				foreach(Touch t in Input.touches)
				{
					dragdistance += t.deltaPosition.magnitude;
				}
				
			}
			
			
		}
			
		//And of course we test for the mouse up. 
		if(LabelTouchUp() )
		{
			//If its a button and the button has not popped. 
			if(Button && !popped)
			{	
				//Temp bool for if the we touch up on the button.
				bool uponbutton = false;
			
				//Temp storage for the hit position, equal to the mouse position which we should laways have.
				Vector3 hitposition;
			
				//Set the hit position to the touch position.
				hitposition = touch.position;
				
				if(UseHitLabel)
				{
					if(HitTexture.HitTest(hitposition))
					{
						uponbutton = true;
					}
				}
				
				if(UseHitLabel)
				{
					if(HitTexture.HitTest(Input.mousePosition))
					{
						uponbutton = true;
					}
				}
				
				//Perform the test.
				if( GetComponent<GUITexture>().HitTest(hitposition) )
				{
					uponbutton = true;
				}
					
				//Test for upbutton on mouse.
				if(GetComponent<GUITexture>().HitTest(Input.mousePosition) )
				{
					uponbutton = true;
				}
				
				//Should only return true if we pass the hit test. which should execute the message only if we end the touch on the label.					
				if(uponbutton && isactive)
				{
					SendMessageUpwards(Action, SendMessageOptions.DontRequireReceiver);
				}
					
					
			}
			
			//End Touch with label.
			popped = true;
			captured = false;
			DragAction = 0.0f;
		
			
		}
		
	}
	
	
	void CaptureUpdate()
	{
		//We get the screen width and height here incase it changes the next or previous frame.
		screen = new Rect(0.0f,0.0f,Screen.width,Screen.height);
		
		if(captured)
		{		
			//We need to calculate teh position of the mouse into screen percentage
			float x = Input.mousePosition.x/screen.width;
			float y = Input.mousePosition.y/screen.height;
			
			
			//Temp storage for the new position.
			Vector3 Position = Vector3.zero;
			Position.Set(x,y,0.0f);
				
			//Tranfering the temp position to the transform.
			transform.position = Position;
			
		}
		else
		{
			
		}
		
	}
	
	public bool CollidesWith(Label c)
	{
		return false;
		
	}
	
	/// <summary>
	/// Sets the action by string This will send a message upwards based on the name of the action.
	/// This function will also enable the label to be a button as well.
	/// </summary>
	/// <param name='name'>
	/// Name of the action.
	/// </param>
	public void SetAction(string name)
	{
		//Set the name of the action for the button to be called if this lavel is a button.
		Action = name;
		
		//Since we are setting the aciton name we will assume you want this to be a button!
		//Button = true;
	}
	
	#region Accessors
	
	public string GetAction()
	{
		//Get the name of an action if need be.
		return Action;
	}
	
	void Highlight()
	{
		GetComponent<GUITexture>().color = HighlightColor;
	}
	
	void UnHighLight()
	{
		GetComponent<GUITexture>().color = Color.gray;
	}
	
	public bool IsCaptured()
	{
		return captured;
	}
	
	public void SetEnable(bool b)
	{
		
		isactive = b;
		GetComponent<GUITexture>().enabled = b;
	}
	
	public Vector3 GetOriginalPosition()
	{
		return OriginalPosition;
	}
	
	public Vector3 GetOriginalScale()
	{
		return OriginalScale;
	}
	
	public float GetDragAction()
	{
		return (DragAction/full);
	}
	
	public Color GetHighlightColor()
	{
		return HighlightColor;
	}
	
	public void SetHighlightColor(Color32 color)
	{
		HighlightColor = color;
	}
	
	private bool LabelTouch()
	{
		//Return right away if the button is frozen.
		if(Freeze)
		{
			return false;
		}
	
		//Temp storage for the hit position, equal to the mouse position which we should laways have.
		Vector3 hitposition;
		
		//First we test if any of the touches is interacting with the label then we return.
		foreach(Touch t in Input.touches)
		{
			//Set the hit position to the touch position.
			hitposition = t.position;
			
			
			//If a hit texture is present we test against that first.
			if(UseHitLabel)
			{
				//Perform the test.
				if( HitTexture.HitTest(hitposition) )
				{
					//Set the touch that we collided with.
					touch = t;
	
					//Return true that we are touching the label.
					return true;
				}
				
			}
			
			//Perform the test.
			if( GetComponent<GUITexture>().HitTest(hitposition) )
			{
				//Set the touch that we collided with.
				touch = t;

				//Return true that we are touching the label.
				return true;
			}
		

			
			
		}
		
		if(Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
		{
		
			//Store value of the mouse position.
			hitposition = Input.mousePosition;
			
			if(UseHitLabel)
			{
				//Then we test if we clicked on the label and return.
				if( HitTexture.HitTest(hitposition) && Input.GetMouseButton(0) ) 
				{	
					return true;
				}
			}
			
			//Then we test if we clicked on the label and return.
			if( GetComponent<GUITexture>().HitTest(hitposition) && Input.GetMouseButton(0) ) 
			{	
				return true;
			}
			
		}		
		
		
		//If we fail all the tests return false for no input detected.
		return false;
		
	}
	
	private bool LabelTouchUp()
	{
		
		//We have stored the touch on the touch down so check if the phase has ended.
		if(touch.phase == TouchPhase.Ended)
		{
			return true;
		}
		
		
		if( Input.GetMouseButtonUp(0) ) 
		{
			return true;
		}
		
		
		return false;
	}
	
	
	#endregion
}
