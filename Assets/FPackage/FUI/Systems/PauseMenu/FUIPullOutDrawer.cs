using UnityEngine;
using System.Collections;

public class FUIPullOutDrawer : MonoBehaviour {
	
	public Transform pullOutPostion;
	
	void OnClick(){
		TweenPosition.Begin(this.gameObject, 2.0f, pullOutPostion.transform.position);
	}
}
