using UnityEngine;
using System.Collections;

/// <summary>
/// Controls layout object.
/// 
/// This is the Base object for controls layout on the screen.  
/// 
/// Derive off of this object for different behaviors for different devices.
/// </summary>
public class ControlsLayoutObject : MonoBehaviour {
	
	public Transform Shoot, Shield, Overheat;
	public BoxCollider ShootButton,ShieldButton,OverheatButton;

	public TweenScale ShootTween, ShieldTween, OverheatTween;
	
	// Use this for initialization
	void Start () {
		
		//Grab data from player data.
		Vector3 cache = Vector3.zero;
		
			
		//Grab shoot scale.
		cache = GameObjectTracker.instance._PlayerData.GameOptions.ShootButtonScale;
		if(cache != Vector3.zero)
			Shoot.localScale = cache;
	
		//Grab shoot position
		cache = GameObjectTracker.instance._PlayerData.GameOptions.ShootButtonPosition;
		if(cache != Vector3.zero)
			Shoot.localPosition = cache;
		
		//Grab shoot scale.
		cache = GameObjectTracker.instance._PlayerData.GameOptions.ShieldButtonScale;
		if(cache != Vector3.zero)
			Shield.localScale = cache;
	
		//Grab shield position
		cache = GameObjectTracker.instance._PlayerData.GameOptions.ShieldButtonPosition;
		if(cache != Vector3.zero)
			Shield.localPosition = cache;



		//Grab the scale transforms for notifications.
//		ShootTween = Shoot.GetComponent<TweenScale>();
//		ShieldTween = Shield.GetComponent<TweenScale>();
//		OverheatTween = Overheat.GetComponent<TweenScale>();


	}

	public void AnimateShoot()
	{
		ShootTween.Reset();
		ShootTween.Play(true);
	}

	public void AnimateShield()
	{
		ShieldTween.Reset();
		ShieldTween.Play(true);
	}

	public void AnimateOverheat()
	{
		OverheatTween.Reset();
		OverheatTween.Play(true);
	}
}
