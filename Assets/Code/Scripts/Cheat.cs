using UnityEngine;
using System.Collections;

public class Cheat : MonoBehaviour {

	bool adding = false;
	
	void OnPress(bool down)
	{
		if(down)
			adding = true;
		else
			adding = false;
	}
	
	void Update()
	{
		if(adding)
			GameObjectTracker.instance._PlayerData.GemBank += 700;
	}
	
	
	void UnlockAll()
	{
		//Unlock all cannons
		
		foreach(BaseItemCard card in GameObjectTracker.instance._PlayerData.CardDeck)
			card.UnlockCard(true);
	}
			
}

