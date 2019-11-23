using UnityEngine;
using System.Collections;

/// <summary>
/// Infinite Terrain - by Looney Lizard.
/// 
/// Usage
/// -----
///
/// 1) Create a Terrain that can be tiled, ie. the opposite sides of the Terrain must
///    match up perfectly.
///
/// 2) Link the InfiniteTerrain.cs script to the Terrain.
///
/// 3) Set CloneTerrainChildren to indicate whether or not child objects of the
///    Terrain will be copied when constructing the Terrain grid.
///
/// 4) Link the GameObject that represents the player to the PlayerObject property.
///
/// That's it!
/// 
/// See ReadMe.txt for additional information.
/// </summary>
public class InfiniteTerrain : MonoBehaviour
{
	#region Public Properties
	
	/// <summary>
	/// Specifies whether or not to clone the terrain children when constructing the Terrain grid.
	/// NOTE: Changing this property at runtime will have no effect as the Terrain grid is constructed during initialisation.
	/// </summary>
	public bool CloneTerrainChildren = true;
	
	/// <summary>
	/// The player object. The script will ensure that this object is always on the centre Terrain in the grid.
	/// </summary>
	public GameObject PlayerObject;

	#endregion
	
	#region Private Fields
	
	private Terrain[,] _terrainGrid = new Terrain[3,3];
	
	private bool _initialised = false;
	
	private bool _playerObjectSet = true;
	
	#endregion
	
	#region MonoBehaviour Methods
	
	void Start()
	{
		Terrain originalTerrain = gameObject.GetComponent<Terrain>();
		if (originalTerrain == null)
		{
			Debug.LogError("InfiniteTerrain: This script can only be linked to a Game Object of type Terrain.");
		}
		else if (!originalTerrain.name.EndsWith("(Clone)"))
		{
			_terrainGrid[0,0] = CloneTerrain(originalTerrain);
			_terrainGrid[0,1] = CloneTerrain(originalTerrain);
			_terrainGrid[0,2] = CloneTerrain(originalTerrain);
			_terrainGrid[1,0] = CloneTerrain(originalTerrain);
			_terrainGrid[1,1] = originalTerrain;
			_terrainGrid[1,2] = CloneTerrain(originalTerrain);
			_terrainGrid[2,0] = CloneTerrain(originalTerrain);
			_terrainGrid[2,1] = CloneTerrain(originalTerrain);
			_terrainGrid[2,2] = CloneTerrain(originalTerrain);
			UpdateTerrainPositions();
			_initialised = true;
		}
	}
	
	void Update ()
	{
		if (_initialised)
		{
			if (_playerObjectSet && (PlayerObject == null))
			{
				_playerObjectSet = false;
				Debug.LogWarning("InfiniteTerrain: The Terrain grid will not scroll unless PlayerObject is set to an existing GameObject.");
			}
			else if (PlayerObject != null)
			{
				_playerObjectSet = true;
				Vector3 playerPosition = new Vector3(PlayerObject.transform.position.x, PlayerObject.transform.position.y, PlayerObject.transform.position.z);
				Terrain playerTerrain = null;
				int xOffset = 0;
				int yOffset = 0;
				for (int x = 0; x < 3; x++)
				{
					for (int y = 0; y < 3; y++)
					{
						if ((playerPosition.x >= _terrainGrid[x,y].transform.position.x) &&
							(playerPosition.x <= (_terrainGrid[x,y].transform.position.x + _terrainGrid[x,y].terrainData.size.x)) &&
							(playerPosition.z >= _terrainGrid[x,y].transform.position.z) &&
							(playerPosition.z <= (_terrainGrid[x,y].transform.position.z + _terrainGrid[x,y].terrainData.size.z)))
						{
							playerTerrain = _terrainGrid[x,y];
							xOffset = 1 - x;
							yOffset = 1 - y;
							break;
						}
					}
					if (playerTerrain != null)
						break;
				}
				
				if (playerTerrain != _terrainGrid[1,1])
				{
					Terrain[,] newTerrainGrid = new Terrain[3,3];
					for (int x = 0; x < 3; x++)
						for (int y = 0; y < 3; y++)
						{
							int newX = x + xOffset;
							if (newX < 0)
								newX = 2;
							else if (newX > 2)
								newX = 0;
							int newY = y + yOffset;
							if (newY < 0)
								newY = 2;
							else if (newY > 2)
								newY = 0;
							newTerrainGrid[newX, newY] = _terrainGrid[x,y];
						}
					_terrainGrid = newTerrainGrid;
					UpdateTerrainPositions();
				}
			}
		}
	}
	
	#endregion
	
	#region Private Methods
	
	private Terrain CloneTerrain(Terrain originalTerrain)
	{
		if (CloneTerrainChildren)
			return ((Terrain)Instantiate(originalTerrain));
		return Terrain.CreateTerrainGameObject(originalTerrain.terrainData).GetComponent<Terrain>();
	}
	
	private void UpdateTerrainPositions()
	{
		_terrainGrid[0,0].transform.position = new Vector3(
			_terrainGrid[1,1].transform.position.x - _terrainGrid[1,1].terrainData.size.x,
			_terrainGrid[1,1].transform.position.y,
			_terrainGrid[1,1].transform.position.z + _terrainGrid[1,1].terrainData.size.z);
		_terrainGrid[0,1].transform.position = new Vector3(
			_terrainGrid[1,1].transform.position.x - _terrainGrid[1,1].terrainData.size.x,
			_terrainGrid[1,1].transform.position.y,
			_terrainGrid[1,1].transform.position.z);
		_terrainGrid[0,2].transform.position = new Vector3(
			_terrainGrid[1,1].transform.position.x - _terrainGrid[1,1].terrainData.size.x,
			_terrainGrid[1,1].transform.position.y,
			_terrainGrid[1,1].transform.position.z - _terrainGrid[1,1].terrainData.size.z);
		
		_terrainGrid[1,0].transform.position = new Vector3(
			_terrainGrid[1,1].transform.position.x,
			_terrainGrid[1,1].transform.position.y,
			_terrainGrid[1,1].transform.position.z + _terrainGrid[1,1].terrainData.size.z);
		_terrainGrid[1,2].transform.position = new Vector3(
			_terrainGrid[1,1].transform.position.x,
			_terrainGrid[1,1].transform.position.y,
			_terrainGrid[1,1].transform.position.z - _terrainGrid[1,1].terrainData.size.z);
		
		_terrainGrid[2,0].transform.position = new Vector3(
			_terrainGrid[1,1].transform.position.x + _terrainGrid[1,1].terrainData.size.x,
			_terrainGrid[1,1].transform.position.y,
			_terrainGrid[1,1].transform.position.z + _terrainGrid[1,1].terrainData.size.z);
		_terrainGrid[2,1].transform.position = new Vector3(
			_terrainGrid[1,1].transform.position.x + _terrainGrid[1,1].terrainData.size.x,
			_terrainGrid[1,1].transform.position.y,
			_terrainGrid[1,1].transform.position.z);
		_terrainGrid[2,2].transform.position = new Vector3(
			_terrainGrid[1,1].transform.position.x + _terrainGrid[1,1].terrainData.size.x,
			_terrainGrid[1,1].transform.position.y,
			_terrainGrid[1,1].transform.position.z - _terrainGrid[1,1].terrainData.size.z);
		
		_terrainGrid[0,0].SetNeighbors(             null,              null, _terrainGrid[1,0], _terrainGrid[0,1]);
		_terrainGrid[0,1].SetNeighbors(             null, _terrainGrid[0,0], _terrainGrid[1,1], _terrainGrid[0,2]);
		_terrainGrid[0,2].SetNeighbors(             null, _terrainGrid[0,1], _terrainGrid[1,2],              null);
		_terrainGrid[1,0].SetNeighbors(_terrainGrid[0,0],              null, _terrainGrid[2,0], _terrainGrid[1,1]);
		_terrainGrid[1,1].SetNeighbors(_terrainGrid[0,1], _terrainGrid[1,0], _terrainGrid[2,1], _terrainGrid[1,2]);
		_terrainGrid[1,2].SetNeighbors(_terrainGrid[0,2], _terrainGrid[1,1], _terrainGrid[2,2],              null);
		_terrainGrid[2,0].SetNeighbors(_terrainGrid[1,0],              null,              null, _terrainGrid[2,1]);
		_terrainGrid[2,1].SetNeighbors(_terrainGrid[1,1], _terrainGrid[2,0],              null, _terrainGrid[2,2]);
		_terrainGrid[2,2].SetNeighbors(_terrainGrid[1,2], _terrainGrid[2,1],              null,              null);
	}
	
	#endregion
}
