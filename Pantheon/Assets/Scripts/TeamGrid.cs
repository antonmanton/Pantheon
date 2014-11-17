using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamGrid : MonoBehaviour
{
	public Tile[] tiles;
	private BattleManager battleManager;

	// Use this for initialization
	void Start ()
	{
		battleManager = (BattleManager) GameObject.FindObjectOfType (typeof(BattleManager));
	}

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

	public bool IsValidTile(int row, int column)
	{
		return (GetTile (row, column) != null);
	}

	public bool CanMoveToTile(Tile startTile, int row, int column)
	{
		Tile endTile = GetTile (row, column);
		return CanMoveToTile (startTile, endTile);
	}

	public bool CanMoveToTile(Tile startTile, Tile endTile)
	{
		if (endTile == null)
		{
			return false;
		}
		int rowDelta = Mathf.Abs (startTile.row - endTile.row);
		int columnDelta = Mathf.Abs (startTile.column - endTile.column);

		if (rowDelta == 0 && columnDelta == 0)
		{
			return true;
		}
		else if (endTile.character != null)
		{
			return false;
		}

		
		if (!battleManager.canMoveDiagonal)
		{
			return (rowDelta + columnDelta <= battleManager.moveRange);
		}
		else
		{
			int moveRadius = Mathf.Max(rowDelta, columnDelta);
			return (moveRadius <= battleManager.moveRange);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
