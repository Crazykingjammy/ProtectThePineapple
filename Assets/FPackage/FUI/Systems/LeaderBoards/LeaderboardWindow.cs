using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeaderboardWindow : MonoBehaviour {
	
	public UITable TopScores;
	public PlayerScoreObject PlayerObjectPrefab;
	
	//List of the contained player objects.
	public List<PlayerScoreObject> _PlayersObjects;
	
	bool populated = false;
	
	// Use this for initialization
	void Start () {
	
		
		PopulateList();
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if(!populated)
		{
			PopulateList();
			return;
		}
		
	}
	
	public void Acivate()
	{

		//Clear and refresh the list upon activate
		if(populated)
		{
			ClearList();

			//After clearing, re populate.
//			PopulateList();
//			Acivate();

			Debug.Log("List Populated");
		}
		else
		{
			Debug.Log("List NOT populated!!");
			PopulateList();
			Acivate();
			return;
		}
		
		if(!SocialCenter.Instance)
		{
			Debug.LogError("WHere is SOCIAL CENTER!");
			return;
		}
		
		int i = 0;
		//Reassign the list upon activate. if we need to.
		foreach(SocialCenter.LeaderBoardScore lscore in SocialCenter.Instance._loadedScores)
		{
			Debug.Log("Activating score:" + lscore.Alias);
			
			//Only assign valid information... the info will be valid when assigned by teh plugin.
			//We wil return if we hit a invalid entry to hint we are at the end of the list.
			if(lscore == null)
				return;
			
			//Assign, activate and itarate.
			_PlayersObjects[i].MyScore = lscore;
			_PlayersObjects[i].gameObject.SetActive(true);
			i++;
			
		}

		//Resort?
		TopScores.Reposition();
		
	}
	
	
	void PopulateList()
	{

		if(populated == true)
			return;
		
		//Create a list at the size of friends.
		for(int i = 0; i < SocialCenter.Instance.FriendSize; i++)
		{
			PlayerScoreObject newObject = NGUITools.AddChild(TopScores.gameObject, PlayerObjectPrefab.gameObject).GetComponent<PlayerScoreObject>();
			
			//Deactivate and add to the list.
			newObject.gameObject.SetActive(false);
			
			_PlayersObjects.Add(newObject);
		}
		
		
		populated = true;
		
	}
	
	void ClearList()
	{
		foreach(PlayerScoreObject pso in _PlayersObjects)
		{
			pso.gameObject.SetActive(false);
		}
	}
}
