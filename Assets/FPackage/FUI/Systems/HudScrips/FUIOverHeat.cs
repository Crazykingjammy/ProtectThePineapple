using UnityEngine;
using System.Collections;

public class FUIOverHeat : MonoBehaviour {

	void OnClick(){
		
		ToyBox.GetPandora().Bot_01.TriggerOverheat();
	}
}
