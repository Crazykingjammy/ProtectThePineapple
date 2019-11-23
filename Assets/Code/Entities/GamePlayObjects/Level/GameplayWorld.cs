using UnityEngine;
using System.Collections;

public class GameplayWorld : MonoBehaviour {

//	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}
//	
	
	public SmartBeam _objectView;
	public MainGameCamera _camera;
	
	public PauseCamera _pauseCamera;

//	void Start()
//	{
//		_camera.camera.transparencySortMode = TransparencySortMode.Orthographic;
//	}

	public void ShakeWorld()
	{
		//Play the shaking animation
		GetComponent<Animation>().Play("GameplayWorld_Shake");
	}
	
	public void Twitch()
	{
		//Play the shaking animation
		GetComponent<Animation>().Play("GameplayWorld_Twitch");
	}
	
	public void Nudge()
	{
		
		//Play
		GetComponent<Animation>().Play("GameplayWorld_Nudge");
	}
	
	public SmartBeam ObjectSceneView
	{
		get{return _objectView;}
	}
	
	public Camera CurrentCamera
	{
		get{
			return _camera.MyCamera;
		}
	}
	
	public void SetGameCamera()
	{
		Camera.SetupCurrent(_camera.MyCamera);
	}
	
	public MainGameCamera GameCamera
	{
		get{ return _camera;}
	}
	public PauseCamera PausedCamera
	{
		get{return _pauseCamera;}
	}
	
	public void Reset()
	{
		//Call the objects reset functions.
		_objectView.Reset();

		_camera.Reset();
	}
	
	void OnGameCamera()
	{
		
		Debug.LogError("ONGameCAMERA");
		
		//Only swap came camera if paused camera is active while calling this.
		if(_pauseCamera.Open)
			return;
		
		Camera.SetupCurrent(_camera.MyCamera);
		_pauseCamera.gameObject.SetActive(false);
	}
}
