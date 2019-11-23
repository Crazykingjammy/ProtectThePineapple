using UnityEngine;
using System.Collections;


/// <summary>
/// FUI enabler script.
/// Used to Toggle its gameobjects and children
/// </summary>

public class FUIEnablerScript : MonoBehaviour {
	
	public bool startActive = true;
	bool bActive = false;

	public bool Active {
		get { return bActive; }
		set { bActive = value; }

	}
	
	// Use this for initialization
	protected void Start () {
		
		if (startActive){
			bActive = startActive;
		}
		
		NGUITools.SetActive(this.gameObject, bActive);
		//this.gameObject.isStatic = true;
	}
	
	/// <summary>
	/// Start this instance.
	/// if its on, it goes off, if off, goes on.
	/// </summary>
	/// 
	public bool TogglePanel(){
		bActive = !bActive;
		NGUITools.SetActive(this.gameObject, bActive);
		
		return bActive;
	}
	
	public bool TogglePanel(bool isOn){
		bActive = isOn;
		NGUITools.SetActive(this.gameObject, bActive);
		
		return bActive;
	}
}
