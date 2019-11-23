using UnityEngine;
using System.Collections;

/// <summary>
/// FUI root panel.
/// This is going to be access to the 3D world, to keep the GUI stuff inside
/// the 3D GUI Heirarchy
/// </summary>
public class FUIRootPanel : MonoBehaviour {
	
	private static FUIRootPanel _rootPanel = null;
	public static  FUIRootPanel RootPanel {
		get {
			if (_rootPanel == null)
				_rootPanel = new FUIRootPanel();
			return _rootPanel;
		}
		set {
			_rootPanel = value;
		}
	}
	
	public UIPanel myPanel;
	
	~FUIRootPanel(){
		_rootPanel = null;
		
	}
	
}
