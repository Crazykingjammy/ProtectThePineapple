using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// FUI grid of phase results.
/// The script that manages the grid of Phase results.
/// 
/// This uses a NGUI UIGrid object and adds FUIPhaseResultObjects into it
/// </summary>
public class FUIGridOfPhaseResults : MonoBehaviour {
	
	// When this gameobject is created, get the UIGrid object attached to it.
	UIGrid myGrid = null;
	
	// the prefab of a phase result object
	public FUIPhaseResultObject refPhaseResultObject;
	
	// we're going to cache the phase results Objects
	List<FUIPhaseResultObject> phaseResults;
	// and we're going to cache the pointer to the GOT phaseData List
	List<BasePhase.PhaseData> phaseDataList;
	
	// how many phases have we added so far
	int phaseCount = 0;
	
	// the phases results title
	public UILabel labelPhaseResultsTitle;
	public List<string> phaseResultsTitles;
	
	
	
	
	// Use this for initialization
	void Start () {
		myGrid = gameObject.GetComponent<UIGrid>();
		if (!myGrid)
			Debug.LogError("GridOfPhaseResults has no UIGrid");
		
		phaseResults = new List<FUIPhaseResultObject>();
		
		if (labelPhaseResultsTitle)
		labelPhaseResultsTitle.text = phaseResultsTitles[0];					
	}
		
	// Update is called once per frame
	void Update () {
		
		phaseDataList = GameObjectTracker.GetGOT()._PlayerData.Breathless.PhaseList;
		
		
		if (phaseDataList != null && phaseCount < phaseDataList.Count){
			
			// if we have a data list, and the data list is greater than the number
			// of phases we have created, the last phasecount "Should" be the last 
			// punched phase, grab that and create the result object and add it to the grid.
			
			BasePhase.PhaseData lastPhaseData = phaseDataList[phaseCount];
			
			// after all is done, increment our phase count
			phaseCount++;
			
			FUIPhaseResultObject newPhaseResult = null;
			newPhaseResult = NGUITools.AddChild(myGrid.gameObject, refPhaseResultObject.gameObject).GetComponent<FUIPhaseResultObject>();
			if (lastPhaseData.PhaseCompletionPunch < 0){
				newPhaseResult.phaseSprite.spriteName = "phaseunknown";
				newPhaseResult.phaseLabel.text = "Failed";
			}
			else{
				
				newPhaseResult.phaseSprite.spriteName = lastPhaseData.IconTextureName;
				newPhaseResult.phaseLabel.text =  FormatSeconds(lastPhaseData.PhaseCompletionPunch);
				//newPhaseResult.PhaseData = lastPhaseData;
			}
			// regardless if the phases is punched or not, store the given phase name
			// this way when it gets punched, we can update the data
			newPhaseResult.givenSpriteName = lastPhaseData.IconTextureName;
			
			phaseResults.Add(newPhaseResult);
			
			// now we've added any new phases that we need.
			// real quick iterate through the lists, and update any new phases that 
			// have now been punched
			for (int i = 0; i <  phaseCount; i++){
				if (phaseDataList[i].PhaseCompletionPunch > 0){
					phaseResults[i].phaseSprite.spriteName = phaseResults[i].givenSpriteName;
					phaseResults[i].phaseLabel.text = FormatSeconds(phaseDataList[i].PhaseCompletionPunch);
					phaseResults[i].PhaseData = phaseDataList[i];
				}
			}
			
			myGrid.Reposition();
			
			// update the phase title
			if (labelPhaseResultsTitle)
			labelPhaseResultsTitle.text = phaseResultsTitles[phaseCount];
		}
	}
	
	string FormatSeconds(float elapsed)
	{
	   int d = (int)(elapsed * 100.0f);
	   int minutes = d / (60 * 100);
	   int seconds = (d % (60 * 100)) / 100;
	   int hundredths = d % 100;
	   return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, hundredths);
	}

}
