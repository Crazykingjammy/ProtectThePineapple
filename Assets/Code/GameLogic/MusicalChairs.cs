using UnityEngine;
using System.Collections;

public class MusicalChairs : MonoBehaviour {
	
	//Store data for a match.
	//[System.Serializable]
	public class TurnData
	{
		//Content.
		float _pps;
		int _slotA, _slotB;
		
		//here we will store a string which stores the name of the turn data
		// This will help read the turn data and know when the turn was actually placed.
		//Proposed ID format: 
		string _id = "MATCHID_PLAYERID_ROUND#_TURN#";
		
		//Public accessors.
		public float PPS
		{
			get
			{return _pps;}
			set
			{_pps = value;}
		}
		
		public EntityFactory.CannonTypes SlotA
		{
			get{return (EntityFactory.CannonTypes)_slotA;}
			set{_slotA = (int)value;}
		}
		public EntityFactory.CannonTypes SlotB
		{
			get{return (EntityFactory.CannonTypes)_slotB;}
			set{_slotB = (int)value;}
		}
		
		public string ID
		{
			get{return _id;}
		}
		
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
#region Internal Functions
	
	void ProcessRound()
	{
		
	}
	
	void ProcessMatch(){}
	
	
	
#endregion
	
#region Interface Functions
	
	public void SubmitTurnData(float PPS, EntityFactory.CannonTypes SlotA, EntityFactory.CannonTypes SlotB)
	{
		
	}
	
	public TurnData[] GetRoundTurns()
	{
		return null;
	}
	
	
#endregion
	
}
