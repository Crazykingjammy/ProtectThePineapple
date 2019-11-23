using UnityEngine;
using System.Collections;

public class CounterResetNotification : MonoBehaviour {

	private TweenTransform CounterResetNotificationAnimation = null;
	bool hasanimation = false;

	private UIPanel CounterResetNotificationUIPanel = null;
	bool hasUIPanel = false;

	public float AlphaVisible, AlphaFaded = 1.0f;

	// Use this for initialization
	void Start () {

		// the notification is a panel with a tween transform


		CounterResetNotificationAnimation = this.gameObject.GetComponent<TweenTransform>();
		if (CounterResetNotificationAnimation == null){
			Debug.LogError("Counter Reset Notification, this GO has no TweenTransform");
			hasanimation = false;
			return;
		}

		CounterResetNotificationUIPanel = this.gameObject.GetComponent<UIPanel>();
		if (CounterResetNotificationUIPanel == null){
			Debug.LogError("Counter Reset Notification, this GO has no UI Panel");
			hasUIPanel = false;
			return;
		}

		hasanimation = true;
		hasUIPanel = true;
		// we can use it!
	}
	
	// Update is called once per frame
	void Update () {
		// shouldn't need an update
	}

	public bool PlayCounterReset(){

		if (hasUIPanel){
			//CounterResetNotificationUIPanel.enabled = true;
			CounterResetNotificationUIPanel.alpha = AlphaVisible;
		}


		if (hasanimation){
			CounterResetNotificationAnimation.Reset();
			CounterResetNotificationAnimation.Play(true);
		}
		return true;
	}

	public void EndCounterReset(){
		Debug.Log("End Counter Reset()");
		if (hasUIPanel){
			CounterResetNotificationUIPanel.alpha = AlphaFaded;
			//CounterResetNotificationUIPanel.enabled = false;
		}

	}



}
