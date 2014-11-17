using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour
{
	public UIManager uiManager;
	public BattleGrid battleGrid;
	private Character[] characters;

	// Movement Rules
	public int moveRange;
	public bool canMoveDiagonal;

	// Battle state
	public bool battleActive { get; private set; }
	private int activeCharacterIndex;

	// Use this for initialization
	void Start ()
	{
		activeCharacterIndex = -1;
		characters = (Character[]) GameObject.FindObjectsOfType<Character> (); //TODO: Use tags, or build from input to battle scene
	}

	public Character GetActiveCharacter()
	{
		if (activeCharacterIndex >= 0 && activeCharacterIndex < characters.Length)
		{
			return characters[activeCharacterIndex];
		}
		return null;
	}

	bool CheckBattleEnd()
	{
		// TODO: more things
		return false;
	}

	public void InitializeBattle(/*data go here*/)
	{
		battleActive = true;
		SetupCharacterQueue ();
		PlaceCharacters ();
		activeCharacterIndex = 0;
		StartCharacterTurn(activeCharacterIndex);
		/*
		while (!CheckBattleEnd())
		{
			// dequeue next character to move
			
			// do character turn
			
			// enqueue character that just finished
		}
		*/
		//battleActive = false;
	}

	void SetupCharacterQueue()
	{
		// TODO: setup character turn queue based on initiative, team ID, etc.
	}

	void PlaceCharacters()
	{
		foreach(Character character in characters)
		{
			TeamGrid teamGrid = battleGrid.teamGrids[character.teamID];
			bool startLocationFound = false;

			int[] columnOrder = { 0, 1, 2 };
			if (character.teamID == 0)
			{
				columnOrder[0] = 2;
				columnOrder[1] = 1;
				columnOrder[2] = 0;
			}

			foreach (int column in columnOrder)
			{
				for (int row = 0; row <= 2; ++row)
				{
					if (IsValidTile(character.teamID, row, column))
					{
						Tile tile = teamGrid.GetTile(row, column);
						if (tile.character == null)
						{
							character.transform.position = tile.gameObject.transform.position;
							character.currentTile = tile;
							character.previewTile = tile;
							tile.character = character;

							startLocationFound = true;
							break;
						}
					}
				}
				if (startLocationFound)
				{
					break;
				}
			}
		}
	}
	
	void StartCharacterTurn(int characterIndex)
	{
		Character character = characters [characterIndex];
		if (character != null)
		{
			Debug.Log ("TURN START: " + character.properName + " (team " + character.teamID + ")");
			uiManager.HandleStartCharacterTurn(character);
		}
	}

	void EndCharacterTurn ()
	{
		Character character = characters[activeCharacterIndex];
		if (character != null)
		{
			uiManager.HandleEndCharacterTurn(character);
			activeCharacterIndex = (activeCharacterIndex + 1) % characters.Length;
			StartCharacterTurn (activeCharacterIndex);
		}
	}

	public void CommitCharacterMove(Character character)
	{
		if (character.previewTile != null)
		{
			// Old tile no longer has a character
			character.currentTile.character = null;

			// Switch to our new tile
			character.currentTile = character.previewTile;
			character.currentTile.character = character;
		}
		EndCharacterTurn ();
	}

	public void KillCharacter(Character character)
	{
		//TODO: Link up with Mark's character code
	}
	
	public bool IsValidTile(int teamID, int row, int column)
	{
		TeamGrid teamGrid = battleGrid.teamGrids[teamID];
		if (teamGrid != null)
		{
			return (teamGrid.GetTile (row, column) != null);
		}
		return false;
	}
	
	public bool CanMoveToTile(Tile startTile, int row, int column)
	{
		TeamGrid teamGrid = battleGrid.teamGrids[startTile.GetTeamID()];
		if (teamGrid != null)
		{
			Tile endTile = teamGrid.GetTile (row, column);
			return CanMoveToTile (startTile, endTile);
		}
		return false;
	}
	
	public bool CanMoveToTile(Tile startTile, Tile endTile)
	{
		if (endTile == null)
		{
			return false;
		}
		if (startTile.GetTeamID() != endTile.GetTeamID ())
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
		
		
		if (!canMoveDiagonal)
		{
			return (rowDelta + columnDelta <= moveRange);
		}
		else
		{
			int moveRadius = Mathf.Max(rowDelta, columnDelta);
			return (moveRadius <= moveRange);
		}
	}
}
