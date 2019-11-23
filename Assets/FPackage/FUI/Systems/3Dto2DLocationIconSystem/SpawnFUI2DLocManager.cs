using UnityEngine;
using System.Collections;

public class SpawnFUI2DLocManager : MonoBehaviour {
	
	public FUI2dLocManager FUI2dLocManagerPrefab;
	
	// Use this for initialization
	void Start () {
		
		if (FUI2dLocManagerPrefab != null){
			NGUITools.AddChild(this.gameObject, FUI2dLocManagerPrefab.gameObject);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
