using UnityEngine;
using System.Collections;

public class FUIBossNotification : MonoBehaviour {

	FUIWindowToggles myWindow = null;
	public TweenScale BGTween;
	public TweenTransform LabelTween;

	public TweenAlpha WindowTween;

	// Use this for initialization
	void Start () {
	
		myWindow = GetComponent<FUIWindowToggles>();
	}
	
	// Update is called once per frame
	void Update () {
	

		if(Input.GetKeyDown(KeyCode.Z))
		{
			if(!myWindow._isVisible)
				ActivateNotification();
			else
				CloseWindow();

		}

	}

	public void CloseWindow()
	{
		myWindow.ToggleWindowOff();
	}


	public void ActivateNotification()
	{
		//Turn on window.
		myWindow.ToggleWindowOn();
		WindowTween.Reset();
		WindowTween.Play(true);

		//Play the animations.
		BGTween.gameObject.SetActive(true);
		BGTween.Reset();
		BGTween.Play(true);

		//Label tweening animation reset can be done here.
		LabelTween.Reset();
		//BGTween.Play(true);
	}


	void OnBGEnter()
	{
		//Deactivate the BG
		BGTween.gameObject.SetActive(false);

		//Play the label tween animation here
		LabelTween.Play(true);
	}

}
