using UnityEngine;
using System.Collections;

public class FUIOpenInventoryButton : FUIPushControlPanelButton {
	
	// this is the current pause menu, its a bit of a hack
	public FUIEnablerScript StatsScreen = null;
	
//	// i dont want to toggle a panel, i want to trigger an Activity
//	public FUIActivity InvScreen = null;
//	public UISprite activeSprite = null;
	
	//public FUIEnablerScript InventoryScreen = null;
	
	bool isInventoryOpen = false;

}
