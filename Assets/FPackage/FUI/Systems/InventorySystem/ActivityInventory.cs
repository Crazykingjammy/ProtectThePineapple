using UnityEngine;
using System.Collections;

public class ActivityInventory : FUIActivity {
	
	//Keeping track of the cannon list mananager.
	public FUICannonListManager Manager;
	
	// Use this for initialization
	new void Start () {
	
		base.Start();
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void OnActivate() { 
		
		Manager.Refresh();

		ActivityManager.Instance.PullCurtain();
		
	}

	void OnBack()
	{
		ActivityManager.Instance.PopActivity();
	}
}
