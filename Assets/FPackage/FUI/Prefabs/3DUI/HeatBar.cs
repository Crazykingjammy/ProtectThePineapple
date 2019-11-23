using UnityEngine;
using System.Collections;

public class HeatBar : MonoBehaviour {
	
	/// <summary>
	/// My bot.
	/// If you want  a heat bar, you better be a bot
	/// </summary>
	Bot myBot = null;
		
	public HeatBarObject refHeatBarObjectPrefab;
	private HeatBarObject myHeatBar;
	
	// Use this for initialization
	void Start () {
		myBot = GetComponent<Bot>();
		if (myBot == null){
			Debug.Log("HeatBar: Obj not a Bot");
			return;
		}
		
		myHeatBar = Instantiate(refHeatBarObjectPrefab, myBot.transform.position, myBot.transform.rotation) as HeatBarObject;
		
		myHeatBar.MyTarget = myBot.gameObject;
	
	}

}
