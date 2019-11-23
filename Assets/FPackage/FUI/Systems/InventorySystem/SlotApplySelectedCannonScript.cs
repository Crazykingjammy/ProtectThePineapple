using UnityEngine;
using System.Collections;

public class SlotApplySelectedCannonScript : MonoBehaviour {
	// TODO Make private
	public SlotSelectionObject mySlotObject = null;

	
	public Transform GParent;

	public FUIWindowToggles DraggingWindow;
	
	Vector2 dragvec = Vector2.zero;
	bool animationset = false;
	
	public void OnStart(){
		// the open cannon script better be part of a slot selection object with in an activity
		mySlotObject = gameObject.transform.parent.GetComponent<SlotSelectionObject>();
		
		
	}
	
	
	
	public void OnClick(){
		
		// first check if slot is attached
//		if (mySlotObject.MyCannon.InUse){
//			Debug.Log("Cannon is In Use, Unable to update Slot");
//			return;
//		}
//		Debug.Log("Updating Slot with + cannonname");
		
		//mySlotObject.ButtonOpenCannonListPressed();
		mySlotObject.ButtonCannonSelectedPressed();
		
		// ToyBox.GetPandora().AssignSlot(EntityFactory.CannonTypes.Fox,0);
	}
	
	
	
	public void OnDrag(Vector2 delta)
	{
		//dragvec = delta;	

		//transform.localScale = new Vector3(3.0f,3.0f,3.0f);
		DraggingWindow.ToggleWindowOn();


		foreach(CannonSelectionObject card in ActivityManager.Instance.CardSelectors)
		{
			if(GetComponent<Collider>().bounds.Intersects(card.GetComponent<Collider>().bounds))
			{
				
				//Call the button select function with forced cannon card for selection.
				card._myCheckbox.isChecked = true;
				
			}
		}

//		Debug.LogError("BINGO");
	}
	
	
	public void OnDrop()
	{
		Debug.LogError("Dropo");
	}
	
	public void OnPress(bool p)
	{
		
		DraggingWindow.ToggleWindowOff();
		
//		if(p == true && !animationset)
//		{
//			
//			animation.to = GParent.position;
//			animationset = true;
//			
//		}
//			
//		
//		if(p == false)
//		{
//			
//			
//	
//			dragvec = Vector2.zero;
//			
//			animation.from = GParent.position ;
//			
//			animation.Reset();
//			animation.Play(true);
//				
//			
////			foreach(CannonSelectionObject card in ActivityManager.Instance.CardSelectors)
////			{
////				if(collider.bounds.Intersects(card.collider.bounds))
////				{
////			
////					//Call the button select function with forced cannon card for selection.
////					mySlotObject.ButtonCannonSelectedPressed(card.MyCannonCard);
////					
////				}
////			}
////			
//			
//		}
//

	}
	

	
}
