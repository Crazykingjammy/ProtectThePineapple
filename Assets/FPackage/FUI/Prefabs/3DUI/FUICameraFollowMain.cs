using UnityEngine;
using System.Collections;

public class FUICameraFollowMain : MonoBehaviour {
	
	public Camera mainCamera = null;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = mainCamera.transform.localPosition;
		transform.localRotation = mainCamera.transform.localRotation;
		transform.localScale = mainCamera.transform.localScale;
	}
}
