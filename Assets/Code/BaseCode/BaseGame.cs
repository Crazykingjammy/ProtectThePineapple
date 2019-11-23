using UnityEngine;
using System.Collections;

 public class BaseGame : MonoBehaviour {

	//Toy Box that needs to be created.
	ToyBox _pandoraBox = null;
		
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	

	protected ToyBox PandoraBox
	{
		get{
			
			if(_pandoraBox == null)
				_pandoraBox = ToyBox.GetPandora();
			
			return _pandoraBox;
		}
	}
	
	
	#region Events
	
	
	virtual public void CannonPickedUp(){}
	
	virtual public int GetCurrentMultiplierModifier(){return 0;}
	virtual public string GetCurrentPhaseName(){return "n/a";}

	virtual public void CannonPushed() {}
	virtual public void BotOverheat() {}
	
	
	virtual public void TargetDestroyed(Target t) {}
	
	virtual public void BossStart(){}
	virtual public void BossCompleted(){}

	virtual public void Clear() {}
	
	
	#endregion
	
}
