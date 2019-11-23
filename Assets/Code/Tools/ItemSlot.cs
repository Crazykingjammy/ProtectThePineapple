using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemSlot {

	public EntityFactory.CannonTypes Type;
	public bool StartActive = false;

	public int Cost = 25;



	//Internal variables for use.
	bool _bIsOpen = false;
	bool _bIsAvailable = false;

	string _iconName = "Thermometer-Units";
	string _titleName = "name";
	int _myIndex = -1;


	BaseItemCard _myItem = null;

	public BaseItemCard MyItem
	{
		get{return _myItem;}
		set{
		
			_myItem = value;
			_iconName = value.DisplayInfo.IconName;
			Type = value.ContainedCannonType;
		}
	}



	public void Clear()
	{
		//Set clearing values.
		IsOpen = false;
		IsAvailable = true;

		Type = EntityFactory.CannonTypes.NULL;

		if(StartActive)
		{
			IsOpen = true;
			Type = EntityFactory.CannonTypes.Zebra;

		}

		_myItem = null;
		_iconName = "Thermometer-Units";
	}

	public string IconName
	{
		get{return _iconName;}
		set{_iconName = value;}
	}


	public bool IsOpen
	{
		get{
			return _bIsOpen;

		}
		set {
			_bIsOpen = value;
			//_bIsAvailable = value;
		}
	}

	public bool IsAvailable
	{
		get{
			return _bIsAvailable;
			
		}
		set {
			_bIsAvailable = value;
		}
	}


}
