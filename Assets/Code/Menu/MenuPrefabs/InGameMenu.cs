using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InGameMenu : MenuSystem {

	//The public labels that compose the menu.
	public Label AccessPoint;
	public Label ResetButton;
	public Label OptionsButton;
	public Label QuitButton;
	//public Label OpenButton;
	
	//Close the menu on start. 
	bool menuactive;
	
	//Local variables.
	enum MenuState
	{
		Open, 
		Closed,
		Paused,
		Quit,
		Reset,
		
	}
	
	
	
	//State
	MenuState CurrentState;
	

	

	
		
	// Use this for initialization
	void Start () {
	
		//Set Position to 0
		transform.position = Vector3.zero;
		
		//Create button actions.
		AccessPoint.SetAction("Open");
		ResetButton.SetAction("ResetAction");
		OptionsButton.SetAction("OptionsAction");
		QuitButton.SetAction("QuitAction");
		
		//Populate the button list.
		//ButtonsList.Add(AccessPoint);
		ButtonsList.Add(OptionsButton);
		ButtonsList.Add(ResetButton);
		ButtonsList.Add(QuitButton);
	
		
		//Close the menu and dont show the animation when started
		CurrentState = MenuState.Closed;
		timer.Fill();
		menuactive = false;
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		UpdateMenuState();	
		
		timer.Update();
		
	}
	

	
	void UpdateMenuState()
	{
		switch(CurrentState)
		{
		case MenuState.Open:
			{
			
			//From the very start if we are ever in this state the menu wwill be considered active.
			menuactive = true;
			
			//Move all the buttons to the layout for an open menu.
			foreach(Label l in ButtonsList)
			{
				MoveToOriginal(l,AccessPoint);	
			}
			
			//Keep track of a freeze timer to freze the subbuttons with the animation.
			if(timer.IsMessageTriggered())
			{
				foreach(Label l in ButtonsList)
				{
					l.Freeze = false;
				}
			}
			
			break;
			}
			
		case MenuState.Closed:
		{
			//Close all the buttons to an access point.
			foreach(Label l in ButtonsList)
			{
				MoveToPoint(l,AccessPoint);
			}
			
			//Keep track of the freeze button to freeze the subbuttons with the animation. 
			if(timer.IsMessageTriggered())
			{
				foreach(Label l in ButtonsList)
				{
					l.Freeze = true;
				}
			}
			
			if(timer.IsFull() )
			{
				//We will only de activate the menu once we are sure that we are 100% closed.
				menuactive = false;
				
			}
		
			break;
			
		}
		case MenuState.Quit:
		{
			Application.LoadLevel("MainMenu");
				
			break;
		}
			
		case MenuState.Reset:
		{
			Application.LoadLevel("TestLevel");
			
			break;
		}
			
		default:
			return;
		}
	}
	
	

	
	#region Actions
	
	//This is an access Function
	void Open()
	{
		
		ResetAnimationTimer();
		
		if(CurrentState == MenuState.Open)
		{
			CurrentState = MenuState.Closed;
			return;
		}
		
		//Set the bool
		CurrentState = MenuState.Open;
	}
	
	void ResetAction()
	{
		menuactive = false;
		CurrentState = MenuState.Reset;
	}
	
	void OptionsAction()
	{
		
	}
	
	void QuitAction()
	{
		menuactive = false;
		CurrentState = MenuState.Quit;
		
	}
	
	public bool IsActive()
	{
		return menuactive;
	}
	
	public void Activate()
	{
		//Reset the timer and open the menu. This is a function call to the button.
		ResetAnimationTimer();
		CurrentState = MenuState.Open;
	}
	
	
	#endregion
}
