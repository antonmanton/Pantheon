using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
	public int row;
	public int column;
	public Character character;

	// Use this for initialization
	void Start ()
	{
	
	}

	public TeamGrid GetTeamGrid()
	{
		return (TeamGrid)GetComponentInParent(typeof(TeamGrid));
	}
}
