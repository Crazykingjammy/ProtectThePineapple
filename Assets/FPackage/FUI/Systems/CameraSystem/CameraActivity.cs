using UnityEngine;
using System.Collections;

public class CameraActivity : FUIActivity {
	
	

	// Use this for initialization
	 new void Start () {
	
		base.Start();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnPOP()
	{
		ActivityManager.Instance.PullCurtain();

		//ActivityManager.Instance.PopActivity();
		ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.Pause);

	}
	
	void FocusBot()
	{
		GameObjectTracker.instance.World.PausedCamera.MyFocus = PauseCamera.FocusObject.Bot;
	}
	
	void FocusCenter()
	{
		GameObjectTracker.instance.World.PausedCamera.MyFocus = PauseCamera.FocusObject.Center;
	}
	
	public override void OnActivate ()
	{
		
		//Activate the camera
		GameObjectTracker.instance.World.PausedCamera.gameObject.SetActive(true);
		
		Debug.LogError("CAMERA SWITCH");
		//Switch the camera.
		Camera.SetupCurrent(GameObjectTracker.instance.World.PausedCamera.GetComponent<Camera>());
		
		GameObjectTracker.instance.World.PausedCamera.Enter();
	}
	
	
	public override void OnDeActivate ()
	{
		//base.OnDeActivate ();
		
		//Switch the camera.
		//Camera.SetupCurrent(GameObjectTracker.instance.World.CurrentCamera);
		
		//Deactivate the camera
		GameObjectTracker.instance.World.PausedCamera.Exit();
		
		//ActivityManager.Instance.PullCurtain();
	}
}
