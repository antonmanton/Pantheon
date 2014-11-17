using UnityEngine;
using System.Collections;

public class BattleGrid : MonoBehaviour
{
	public TeamGrid[] teamGrids;

	// Initialization
	void Start ()
	{
		//teamGrids = (TeamGrid[]) gameObject.GetComponentsInChildren<TeamGrid> (); //TODO: this is not guaranteed to be indexed by teamID
	}

	// Public accessors
	public TeamGrid GetTeamGrid(int teamID)
	{
		if (teamID >= 0 && teamID < teamGrids.Length)
		{
			return teamGrids [teamID];
		}
		Debug.Log ("Invalid Team ID");
		return null;
	}
}
