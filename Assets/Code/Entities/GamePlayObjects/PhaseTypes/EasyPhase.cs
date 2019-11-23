using UnityEngine;
using System.Collections;

public class EasyPhase : BasePhase {
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
	
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		
		HandleWaveCreation();
		CheckWaveCountsForBoss();
		CheckComboCompletion();
	
	}
}
