using UnityEngine;
using System.Collections;

public class MainMenu : MenuSystem {
	
	
	//Objects to control and do functions with.
	public Label Play;
	public GameObject Background;
	public GUITexture CoinSlot;
	
	public GameObject Finger;
	public Camera MainCamera;
	
	//Random Display stuff.
	public GUIText LoadingText;
	
	//Public variables to tweak
	public float ScrollSpeed = -0.07f;
	float lifetime = 0.0f;
	
	enum MenuState
	{
		Display,
		Load
	}
	
	MenuState currentState;
	
	// Use this for initialization
	void Start () {
		
		Time.timeScale = 1.0f;
		
		Play.SetAction("OnPlay");
		
		
		//Disable some stuff 
		CoinSlot.enabled  = false;
		LoadingText.enabled = false;
		
		
		
	
	}
	
	// Update is called once per frame
	void Update () {
		
		lifetime += Time.deltaTime;
	
		
		//Calculate the float offset and apply it
		float offset = ScrollSpeed * lifetime;
		Background.GetComponent<Renderer>().material.SetTextureOffset("_MainTex",new Vector2(offset,offset));
		
		offset = (ScrollSpeed/2.0f) * lifetime;
		Background.GetComponent<Renderer>().material.SetTextureOffset("_DecalTex",new Vector2(offset * 1.5f,offset));
		
		if(Input.GetMouseButton(0))
		{
			//Enable stats
			Finger.GetComponent<Collider>().isTrigger = false;
			Finger.GetComponent<Light>().enabled = true;
			CoinSlot.enabled = true;
			
			//Get the position
			Vector3 screenpoint = Input.mousePosition;
			screenpoint.z = 14.0f;
			
			//Get teh position
			Vector3 position = MainCamera.ScreenToWorldPoint(screenpoint);
			
			//Set the forward fo the finger.
			Finger.transform.up = MainCamera.transform.forward;
			Finger.transform.position = position;
			Finger.GetComponent<Light>().transform.position = position;
			
			
			
		}
		else
		{
			//Turn off the stuff
			Finger.GetComponent<Collider>().isTrigger = true;
			Finger.GetComponent<Light>().enabled = false;
			CoinSlot.enabled = false;
		}
		
	}
	
	
	void OnPlay()
	{
		//Check if the mouse position is on the coin slot when the function is called.
		if(CoinSlot.HitTest(Input.mousePosition))
		{
			LoadingText.enabled = true;
			Application.LoadLevel("TestLevel");
		}
	}
	

}
