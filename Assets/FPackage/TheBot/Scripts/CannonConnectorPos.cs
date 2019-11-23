using UnityEngine;
using System.Collections;

public class CannonConnectorPos : MonoBehaviour {

	BaseEnemy myHostEnemy = null;
	Bot myHostBot = null;

	public GameObject CannonConnectorPosHost {
	set{
			Debug.Log("***** checking cannon Connector Pos Host set ******");
			if (value == null){
				return;
			}
			BaseEnemy tempEnemy = value.GetComponent<BaseEnemy>();
			if (tempEnemy != null){
				myHostEnemy = tempEnemy;
			}
			else{
				// its not an enemy, could be a bot
				Bot tempBot = value.GetComponent<Bot>();
				if (tempBot != null){
					myHostBot = tempBot;
				}
			}
		}
		get{
			if (myHostEnemy){
				return myHostEnemy.gameObject;
			}
			else{
				return myHostBot.gameObject;
			}
		}
	}




	void Start(){

	}

	void Update(){

		if (myHostEnemy){
			if (myHostEnemy.objectAim){
				this.transform.LookAt(myHostEnemy.objectAim.transform.position);
			}
		}
		else {
			// we're a bot... atm do nothing..

		}

	}
}
