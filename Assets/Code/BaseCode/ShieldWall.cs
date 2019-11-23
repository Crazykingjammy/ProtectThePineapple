using UnityEngine;
using System.Collections;

public class ShieldWall : MonoBehaviour {
	
	Shield host = null;
	
	public ParticleSystem BlockEffect;
	
	// Use this for initialization
	void Start () {
		
		//BlockEffect = Instantiate(BlockEffect) as ParticleSystem;
		//BlockEffect.transform.position = transform.position;
		//BlockEffect.transform.parent = transform;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		//print("Wall Update");
	
	}
	
	public void SetHost(Shield h)
	{
		host = h;
	}
	
	public Shield GetHost()
	{
		return host;
	}


	void OnCollisionEnter(Collision col)
	{

		Debug.LogError("Wall shiled touch");
		
//		//Dont pull targets
//		if(col.gameObject.CompareTag("CommandCenter") )
//		{
//			Target t = col.gameObject.GetComponent<Target>();
//			
//			//print("Hope?");
//			host.Melee(t,18.0f);
//		}
//		
//		
//		
//		
//		if(col.gameObject.CompareTag("Target"))
//		{
//			Target t = col.gameObject.GetComponent<Target>();
//			
//			if(!t.IsDestroyed())
//				host.Melee(t);
//		}
//				
//		if(col.gameObject.CompareTag("Cannon"))
//		{
//			Cannon c = col.gameObject.GetComponent<Cannon>();
//			
//			host.Host.OnMeleeCannon(c,45.0f);
//			
//		}
//		
//		//We check if object is collidign with balls then apply to the pull of the black hole.
//		if(col.gameObject.CompareTag("Ball") )
//		{
//			Ball b = col.gameObject.GetComponent<Ball>();
//			
//			//here we check if the ball is active. If it is, we will let it be!
//			//We leave the function and go to the next object as we dont want to pull this object if its a activated ball.
//			//if(b.ID == Ball.BallSourceID.Enemy || b.ID == Ball.BallSourceID.Deflect )
//			if(b.ID != Ball.BallSourceID.Netural && b.ID != Ball.BallSourceID.Hot && b.IsActive() )
//			{
//				
//				if(host)
//				{
//					//BlockEffect.Play();
//					
//					//Send the message to the shield that we blocked a ball.
//					host.BallBlocked(b);
//				}
//				
//			}
//			
//		}
//		

		
		
	}
}
