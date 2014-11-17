using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
	public int row;
	public int column;
	public Character character;

	// Initialization
	void Start ()
	{
	
	}

	// Public accessors
	public TeamGrid GetTeamGrid()
	{
		return (TeamGrid)GetComponentInParent(typeof(TeamGrid));
	}

	public int GetTeamID()
	{
		return GetTeamGrid ().teamID;
	}
}
