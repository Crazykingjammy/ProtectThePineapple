using UnityEngine;
using System.Collections;

public class PowerupSoccerBallTarget : Crate {
	
	public PowerupAffectBase myAffect;

	// Use this for initialization
	new void Start () {
		base.Start();
		
	}
	
	void Update () {

		if(IsDestroyed())
		{
			//Hide our mesh
			GetComponent<Renderer>().enabled = false;
			Reset();
		}
	
	}
	
	protected override void Reset()
	{
		base.Reset();
		
					
		if (myAffect != null){
			transform.position = myAffect.transform.position;
		}
		
//		//Get rid of the cannon.
//		
//		//Make sure the renderer is enabled.
//		renderer.enabled = true;
		
		
	}
	
	void OnCollisionEnter(Collision col)
	{
		
		

				//If we hit some walls
//		if(col.gameObject.CompareTag("Wall") )
//		{
//			if(rigidbody.velocity.magnitude > 7.0f)
//			{
//				//print("BOOM!");
//				
//				//Play sound
//				//AudioPlayer.GetPlayer().PlaySound(HitSoundEffect);
//				
//				//Deal the damage
//				//Damage(TargetMomentumDamage);
//				//rigidbody.AddForce();
//			}
//			
//		}
//		
		if(col.gameObject.CompareTag("Target"))
		{
		
			if(GetComponent<Rigidbody>().velocity.magnitude > 0.0010f)
			{
				//print("BOOM!");
				
				//Play sound
				AudioPlayer.GetPlayer().PlaySound(HitSoundEffect);
				
				//Deal the damage
				Damage(SelfMomentumDamage);
				
				//Get the target we collided with.
				Target t = col.gameObject.GetComponent<Target>();
				t.Damage(TargetMomentumDamage);
				
			}
		
		}
		
		

		
	}//OnCollisionEnter()
}
