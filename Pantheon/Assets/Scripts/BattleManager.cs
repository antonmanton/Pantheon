using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour
{
	public UIManager uiManager;

	// Game Option type stuff
	public int moveRange;
	public bool canMoveDiagonal;

	public BattleGrid battleGrid;
	public Character[] characters;
	public bool battleActive { get; private set; }
	private int activeCharacterIndex;

	// Use this for initialization
	void Start ()
	{
		activeCharacterIndex = -1;
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
					if (teamGrid.IsValidTile(row, column))
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

	// Update is called once per frame
	void Update ()
	{
	
	}
}
