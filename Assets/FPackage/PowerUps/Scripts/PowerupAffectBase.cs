using UnityEngine;
using System.Collections;

public class PowerupAffectBase : MonoBehaviour {
	
	// when picked up I may become affective
	protected bool isAffective = false;

	/// how long does the effect last
	public float affectiveTimeDuration = 20.0f;
	// what was the time i first became affective
	protected float affectiveTimeStart = 0;
	

	
	// whos powerup am i affecting
	public PowerupBase basePickup = null;
	

	
	// Use this for initialization
	protected virtual void Start () {
//		foreach(Transform child in transform){
//				child.gameObject.active = false;
//		}		
	}
	
	// Update is called once per frame
	protected void FixedUpdate () {
		// check the time and see if we need to end.
		CheckTiming();
		
	}
	
	public void Activate(){
		isAffective = true;
		SetTimeStartAffective(Time.time);			// we are affective emmediately
		transform.localRotation.Set(0, 0, 0, 1);// .localPosition.Set(0.0f,0.0f,0.0f);
		transform.forward = Vector3.forward;
		
//		// Affect Objects have children for the collider and the rendering
//		foreach(Transform child in transform){
//			//child.transform.position = Vector3.zero;
//			if (child.renderer) child.renderer.enabled = isAffective;
//			if (child.rigidbody) child.rigidbody.WakeUp();
//			if (child.gameObject) child.gameObject.active = isAffective;
//		}
		
		gameObject.SetActive(isAffective);
	}
	
	public void DeActivate(){
		isAffective = false;
		SetTimeStartAffective(0);
		
//		// Affect Objects have children for the collider and the rendering
//		foreach(Transform child in transform){
//			//child.transform.position = Vector3.zero;
//			if (child.renderer) child.renderer.enabled = isAffective;
//			if (child.rigidbody) child.rigidbody.Sleep();
//			if (child.gameObject) child.gameObject.active = isAffective;
//		}
		
		gameObject.SetActive(isAffective);

	}
	
	public void CheckTiming(){
		if (isAffective && (Time.time - affectiveTimeStart) > affectiveTimeDuration){
			DeActivate();
			
			basePickup.AffectDone();
		}
	}
	
//	void OnTriggerStay(Collider col)
//	{	
//		//Check if we collide with a CommandCenter.
//		if(col.gameObject.CompareTag("CommandCenter"))
//		{
//			CommandCenter c = col.gameObject.GetComponent<CommandCenter>();
//			// if we touch the command Center add health, 
//			// but only certain amount per second
//			if (isAffective && (Time.time - timeStartedGivingHealth) > healthGivingFreq){				
//				// TODO: for now, we are going to add negative damage, should work
//				c.Damage(-healthToGive);
//				print("Giving +" + healthToGive + " to Target!");
//				timeStartedGivingHealth = Time.time;
//			}
//		}
//	
//		
//	}//All Collisions Check
		
	public void SetTimeStartAffective(float time){
		affectiveTimeStart = time;
	}
	
	public float GetTimeStartAffective(){
		return affectiveTimeStart;
	}
	
	public void SetIsAffective(bool isAffect){
		isAffective = isAffect;
	}
	public 	bool GetIsAffective(){
		return isAffective;
	}
	
	public virtual void ReceiveColliderInfo(Collider col){
		
	}
	public virtual void ReceiveCollisionInfo(Collision col){
		
	}
	
//	public void UpdatePosition(Vector3 pos){
//		foreach(Transform child in transform){
//				child.gameObject.transform.position = pos;
//		}
//	}
}
