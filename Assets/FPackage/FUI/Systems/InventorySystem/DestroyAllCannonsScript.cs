using UnityEngine;
using System.Collections;

public class DestroyAllCannonsScript : MonoBehaviour {

	void OnClick(){
		ToyBox.GetPandora().ClearFreeCannons();
	}
}

