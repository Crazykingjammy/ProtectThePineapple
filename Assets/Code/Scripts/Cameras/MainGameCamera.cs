using UnityEngine;
using System.Collections;

public class MainGameCamera : MonoBehaviour {
	
	public Vector3 RestPosition;
	
	//Values for the camera if we are following a specific bot.
	public Bot PlayerToFollow;
	public bool IsFollowingBot;
	
	
	public GameObject PositionLock;
	
	
	public float BaseSpeed = 0.0f;
	public float dampspeed = 0.1f;
	
	public float FOVIncrease = 1.0f;
	public float FOVShieldIncrease = 3.0f;
	public float MaxZoonIn = 20.0f;
	public Vector3 MoveAway;
	Vector3 _moveStart;
		
	public float rotation = 18.0f;
	
	public bool GameOver = false;
	public bool Die = false;

	bool isAnimatingCannonConnecting = false;
	
	GameObject DieLookAtObject;
	float movetimer = 0.0f;
	float originalFOV = 0.0f;
	Vector3 botPositionAtGameOver = Vector3.zero;

	public float testpoint = 0.005f;
	
	Camera _myCamera = null;
	
	// Use this for initialization
	void Start () {
	
		originalFOV = GetComponent<Camera>().fieldOfView;
		
		_myCamera = GetComponent<Camera>();
		
		_moveStart = MoveAway;
	}
	
	
	public Camera MyCamera
	{
		get{
			return _myCamera;
		}
	}
	
	
	void FollowBot()
	{
		//Make sure that we have a bot attached! If not, then return.
		if(!PlayerToFollow)
		{
			print("Error! There is no bot to look at");
			return;
		}
		
		//Set the look position to the position of the bot.
		Vector3 lookposition = PlayerToFollow.transform.localPosition;
		
		//Set the location to the current location, but to the bots X position.
		Vector3 location = Vector3.zero;
		
		location.Set(lookposition.x,RestPosition.y,RestPosition.z);
		
		//Write the new location
		transform.position = location;
		
		
		//Set the look position to the position lock's location with the exception to the location on the X (which is the bot)
		lookposition.Set(location.x, PositionLock.transform.position.y,PositionLock.transform.position.z);
		
		//Perform the look at function.
		transform.LookAt(lookposition);
		
		
		//handle zooming if the bot is shielded
		if(PlayerToFollow.IsShieldActive())
		{
			//print("Increase");
			
			if(GetComponent<Camera>().fieldOfView > MaxZoonIn)
			GetComponent<Camera>().fieldOfView -= FOVShieldIncrease * Time.deltaTime;
		}
		else
		{
			//print("decrease");
			
			if(GetComponent<Camera>().fieldOfView < originalFOV && PlayerToFollow.IsCannonAttached())
			GetComponent<Camera>().fieldOfView += (FOVShieldIncrease * 2.0f) * Time.deltaTime;
		}
		
		
		
	}
	
	
	public void ZoomOut(float delta, float time)
	{
		
		GetComponent<Animation>().Play("ZoomOut");
		//IsFollowingBot = false;
		return;
		
		

	}

	public void AttachCannon(float delta, float time)
	{
			
		isAnimatingCannonConnecting = true;

		GetComponent<Animation>().Play("AttachCannon");
		//IsFollowingBot = false;
		return;
		
		
		
	}

	public void DetachCannon(float delta, float time)
	{
		
		isAnimatingCannonConnecting = true;
		
		GetComponent<Animation>().Play("DetachCannon");
		//IsFollowingBot = false;
		return;		
	}

	void FinishZoomOut()
	{
		//Play the yes sound!
		//AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Yes);

		//IsFollowingBot = true;
	}

	void FinishZoomIn()
	{
		//Play some crowd sounds.
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.CrowdSuspense);
	}

	void FinishAttachingCannon()
	{
		//Play the yes sound!
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Yes);
		
		isAnimatingCannonConnecting = false;
		//IsFollowingBot = true;
	}


	void TriggerEject()
	{
		PlayerToFollow.TriggerEjectCannon();
	}
	void FinishDetachingCannon()
	{
		// Play the yes sound!
		// AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Yes);
		
		isAnimatingCannonConnecting = false;
		//IsFollowingBot = true;

		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.CrowdSuspense);
	}

	
	public void ZoomIn(float delta, float time)
	{
		GetComponent<Animation>().Play("ZoomIn");
		return;
		

	}
	
	
	// Update is called once per frame
	void Update () {
		
		//Time.timeScale = 1.0f;
		
		if(GameOver)
		{
			GameOverView();
				return;
		}
		
		if(Die)
		{
			DieView();
			return;
		}
		
		//Follow the bot if we should.
		if(IsFollowingBot)
		{
			FollowBot();
		}
		else
		{
			LookAtObject();
		}
		
	
		// Here we do some checks if the bot has a cannon or not.
		
		if(PlayerToFollow)
		{
			if(!PlayerToFollow.IsCannonAttached() )
			{
			
				transform.Rotate(transform.forward,rotation);
			
			}
		
			
		}

		if (isAnimatingCannonConnecting){
			// we're in the middle of the attaching cannon animation.
			// we want to be updating our lookat to look at the cannon connector position
			// all other times we then want to do where ever the default game look at
			this.MyCamera.transform.LookAt(ToyBox.GetPandora().Bot_01.CannonPosition);

		}
		//Tilt the camera according to velocity.
		
		
		
			
	}
	
	
	void DieView()
	{
		movetimer += Time.deltaTime / 4.0f;
		
		//Set the look position to the position of the bot.
		Vector3 lookposition = botPositionAtGameOver;
		
		//Set the location to the current location, but to the bots X position.
		Vector3 location = Vector3.zero;
		
		float x = Mathf.Lerp(lookposition.x,DieLookAtObject.transform.position.x,movetimer);
		
		//Alter the x position from the bot to the die object.
		location.Set(x,transform.position.y,transform.position.z);
		
		//Write the new location
		transform.position = location;
		
		float z = Mathf.Lerp(PositionLock.transform.position.z,DieLookAtObject.transform.transform.position.z,movetimer * 2);
		
		//Set the look position to the position lock's location with the exception to the location on the X (which is the bot)
		lookposition.Set(location.x, PositionLock.transform.position.y,z);
		
		//Perform the look at function.
		transform.LookAt(lookposition);
		
		//Tilt when we die.
		transform.Rotate(transform.forward,12.0f);
		
		GetComponent<Camera>().fieldOfView += FOVIncrease * Time.deltaTime;
		
	}
	
	
	void GameOverView()
	{
		if(!PlayerToFollow)
			return;
		
		
		Vector3 objectattraction = PlayerToFollow.transform.position;
		//Vector3 objectattraction = DieLookAtObject.transform.position;
		
		GetComponent<Camera>().fieldOfView  = originalFOV;
		
		transform.position = objectattraction;
		transform.position += MoveAway;
		
		transform.LookAt(objectattraction);
		
		float d = BaseSpeed;
		
		//If we reach a position, slow dwn the movement.
		if(MoveAway.z > 1.3f)
		{
			d *= dampspeed;
		}
		
		if(MoveAway.z < 10.0f)
		{
			MoveAway.z += Time.deltaTime * d;
		}
			
	}
	
	void LookAtObject()
	{
		//Set the look position.
		Vector3 lookposition = PositionLock.transform.position;		
		
		//This is the location of the camera. We set it to the position of the players X.
		Vector3 location = Vector3.zero;
		location.Set(PlayerToFollow.transform.position.x,transform.position.y,transform.position.z);
		//location = PlayerToFollow.transform.position;
		
		//And here we actually write to the camera the caluclated position based ont he botx location.
		transform.position = location;
		
		//Then we look at the look position.
		transform.LookAt(PlayerToFollow.transform.position);
		
	}
	
	public void SetDie(GameObject dieobject, bool die)
	{
		DieLookAtObject = dieobject;
		Die = die;
		movetimer = 0.0f;
		
		botPositionAtGameOver = PlayerToFollow.transform.position;
	}
	public void SetGameOver(bool gameover)
	{
		GameOver = gameover;
	}
	
	public void SetBotToFollow(Bot b)
	{
		PlayerToFollow = b;
	}
	
	
	public void Reset()
	{
		PlayerToFollow = null;
		transform.position = RestPosition;
		IsFollowingBot = true;
		

		GameOver = false;
		Die = false;
		
		MoveAway = _moveStart;
	}
}
