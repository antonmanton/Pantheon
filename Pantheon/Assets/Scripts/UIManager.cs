using UnityEngine;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
	public BattleManager battleManager;

	// Materials and asset stuff
	public Material activeCharacterMaterial;
	public Material moveRangeMaterial;
	public Material abilityRangeMaterial;
	public GameObject activeCharacterArrow;
	public GameObject abilityRangeArrow;

	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update ()
	{
		/*
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Color color = Color.yellow;
		if (Physics.Raycast(ray))
		{
			color = Color.green;
		}
		Debug.DrawRay (ray.origin, ray.direction * 10, color);
		*/

		if (!battleManager.battleActive)
		{
			if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
			{
				battleManager.InitializeBattle ();
			}
		}
		else if (battleManager.battleActive && battleManager.GetActiveCharacter() != null)
		{
			// Mouse controls
			if (Input.GetMouseButtonDown(0))
			{
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit))
				{
					Tile hitTile = (Tile)hit.collider.GetComponentInParent(typeof(Tile));
					Character activeCharacter = battleManager.GetActiveCharacter();
					if (activeCharacter.currentTile != null)
					{
						TeamGrid teamGrid = activeCharacter.currentTile.GetTeamGrid();
						if (teamGrid.CanMoveToTile(activeCharacter.currentTile, hitTile))
						{
							PreviewCharacterMove(activeCharacter, hitTile);
							battleManager.CommitCharacterMove(activeCharacter);
						}
					}
				}
			}

			// Keyboard controls
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				PreviewCharacterMove(battleManager.GetActiveCharacter(), -1, 0);
			}
			else if (Input.GetKeyDown (KeyCode.DownArrow))
			{
				PreviewCharacterMove(battleManager.GetActiveCharacter(), 1, 0);
			}
			else if (Input.GetKeyDown (KeyCode.LeftArrow))
			{
				PreviewCharacterMove(battleManager.GetActiveCharacter(), 0, -1);
			}
			else if (Input.GetKeyDown (KeyCode.RightArrow))
			{
				PreviewCharacterMove(battleManager.GetActiveCharacter(), 0, 1);
			}
			else if (Input.GetKeyDown (KeyCode.Space))
			{
				battleManager.CommitCharacterMove(battleManager.GetActiveCharacter());
			}
		}
	}

	public void HandleStartCharacterTurn(Character character)
	{
		if (character != null)
		{
			if (activeCharacterMaterial != null)
			{
				List<Character> characters = new List<Character>();
				characters.Add(character);
				HighlightCharacters(characters, activeCharacterMaterial);
			}

			// Preview move range
			if (moveRangeMaterial != null)
			{
				List<Tile> tilesToHighlight = new List<Tile>();
				TeamGrid teamGrid = character.currentTile.GetTeamGrid();
				for (int row = 0; row <= 2; ++row)
				{
					for (int column = 0; column <= 2; ++column)
					{
						if (teamGrid.IsValidTile(row, column))
						{
							Tile tile = teamGrid.GetTile(row, column);
							if (teamGrid.CanMoveToTile(character.currentTile, tile))
							{
								tilesToHighlight.Add(tile);
							}
						}
					}
				}
				HighlightTiles (tilesToHighlight, moveRangeMaterial);
			}

			// Preview attack range
			if (abilityRangeMaterial != null)
			{
				Ability_BasicMelee meleeAbility = character.GetComponentInParent<Ability_BasicMelee>();
				if (meleeAbility != null && meleeAbility.IsValid())
				{
					List<Character> targetableCharacters = meleeAbility.GetTargetableCharacters();
					HighlightCharacters(targetableCharacters, abilityRangeMaterial);
				}
			}
		}
	}

	public void HandleEndCharacterTurn(Character character)
	{
		if (character != null)
		{
			if (activeCharacterMaterial != null)
			{
				MeshRenderer characterRenderer = (MeshRenderer) character.GetComponentInChildren(typeof(MeshRenderer));
				characterRenderer.material.color = Color.grey;
			}
			UnhighlightTiles(character.currentTile.GetTeamGrid().tiles);
			UnhighlightCharacters(battleManager.characters);
		}
	}

	void PreviewCharacterMove(Character character, int rowDelta, int columnDelta)
	{
		if (character != null)
		{
			TeamGrid teamGrid = character.currentTile.GetTeamGrid ();
			int destinationRow = character.previewTile.row + rowDelta;
			int destinationColumn = character.previewTile.column + columnDelta;
			if (teamGrid.IsValidTile (destinationRow, destinationColumn))
			{
				Tile destinationTile = teamGrid.GetTile(destinationRow, destinationColumn);
				PreviewCharacterMove(character, destinationTile);
			}
		}
	}

	void PreviewCharacterMove(Character character, Tile destinationTile)
	{
		if (character != null)
		{
			TeamGrid teamGrid = character.currentTile.GetTeamGrid ();
			if (teamGrid.CanMoveToTile(character.currentTile, destinationTile))
			{
				// Don't set the character's current tile until we confirm
				character.previewTile = destinationTile;
				character.transform.position = destinationTile.gameObject.transform.position;
			}
		}
	}

	void HighlightCharacters(List<Character> characters, Material material)
	{
		foreach (Character character in characters)
		{
			MeshRenderer characterRenderer = (MeshRenderer) character.GetComponentInChildren(typeof(MeshRenderer));
			characterRenderer.material = material;
		}
	}

	void UnhighlightCharacters(Character[] characters)
	{
		foreach (Character character in characters)
		{
			MeshRenderer characterRenderer = (MeshRenderer) character.GetComponentInChildren(typeof(MeshRenderer));
			characterRenderer.material.color = Color.grey;
		}
	}

	void HighlightTiles(Tile[] tiles, Material material)
	{
		foreach (Tile tile in tiles)
		{
			MeshRenderer tileRenderer = (MeshRenderer) tile.GetComponentInChildren(typeof(MeshRenderer));
			tileRenderer.material = material;
		}
	}

	void HighlightTiles(List<Tile> tiles, Material material)
	{
		foreach (Tile tile in tiles)
		{
			MeshRenderer tileRenderer = (MeshRenderer) tile.GetComponentInChildren(typeof(MeshRenderer));
			tileRenderer.material = material;
		}
	}

	void UnhighlightTiles(Tile[] tiles)
	{
		foreach (Tile tile in tiles)
		{
			MeshRenderer tileRenderer = (MeshRenderer) tile.GetComponentInChildren(typeof(MeshRenderer));
			tileRenderer.material.color = Color.grey;
		}
	}
}
