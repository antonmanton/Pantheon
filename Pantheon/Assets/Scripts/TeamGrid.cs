using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamGrid : MonoBehaviour
{
	public int teamID;
	public Tile[] tiles { get; private set; }

	// Initialization
	void Start ()
	{
		tiles = (Tile[]) gameObject.GetComponentsInChildren<Tile> ();
	}

	// Public accessors
	public Tile GetTile(int row, int column)
	{
		for (int index = 0; index < tiles.Length; ++index)
		{
			Tile tile = tiles [index];
			if (tile.row == row && tile.column == column) {
				return tile;
			}
		}
		return null;
	}
}
