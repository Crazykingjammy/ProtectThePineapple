using UnityEngine;
using System.Collections;

public class FUITogglePUSpawner : MonoBehaviour {
	// public PowerupSpawner spawner;
	public UILabel myLabel;
		
	void Start(){
		PowerupFactory puf = PowerupFactory.GetPUF();
		if (puf == null){
			return;
		}
		bool isActive = puf.IsSpawning();
		
		if (myLabel){
			if (isActive){
			myLabel.text = "Spawner On"; }
				else{
			myLabel.text = "Spawner Off";}
		}
	}
	
	void Update(){
//		bool isActive = PowerupFactory.GetPUF().IsSpawning();
//		//spawner.Toggle();
//		if (myLabel){
//			if (isActive){
//			myLabel.text = "Spawner On"; }
//				else{
//			myLabel.text = "Spawner Off";}
//		}
	}
	
	void OnClick(){
		
		//if (spawner){
			//PowerupFactory.GetPUF().allowSpawn = !PowerupFactory.GetPUF().allowSpawn;
			bool isActive = PowerupFactory.GetPUF().ToggleFactory();
			//spawner.Toggle();
			if (myLabel){
				if (isActive){
				myLabel.text = "Spawner On"; }
					else{
				myLabel.text = "Spawner Off";}
			}
		//}
	}
}
