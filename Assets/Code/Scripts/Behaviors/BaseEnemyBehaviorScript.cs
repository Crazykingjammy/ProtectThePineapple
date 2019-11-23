using UnityEngine;
using System.Collections;

public class BaseEnemyBehaviorScript : MonoBehaviour {

	// who am I behaving for?
	protected Target self = null;
	protected BaseEnemy selfAsEnemy = null;
	protected bool isSelfAsEnemy;

	// Use this for initialization
	protected void Start () {
		//Set teh host. or me.
		self = GetComponent<Target>();

		selfAsEnemy = GetComponent<BaseEnemy>();
		if (selfAsEnemy){
			isSelfAsEnemy = true;
		}
		else
			isSelfAsEnemy = false;
	}

	// Update is called once per frame
	protected void FixedUpdate () {
		// if this script object is in the scene, make sure it knows who its controlling
		if (self){
			
		}
		else{
			// we don't control anyone, we shouldn't be here!
			print("I'm a " + this.name +", no self, I had to kill myself");
			//Destroy(gameObject);
		}
	}

	protected void OnCollisionEnter(Collision col)
	{

	}

	protected void OnCollisionStay(Collision col)
	{	
	}//All Collisions ON Stay Check
}
