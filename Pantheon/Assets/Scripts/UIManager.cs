using UnityEngine;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
	public bool debugMouseRay;
	public BattleManager battleManager;

	// Assets
	public Material tileBaseMaterial;
	public Material moveRangeMaterial;
	public GameObject activeCharacterArrow;
	public GameObject abilityRangeArrow;
	private List<GameObject> markers;

	// Initialization
	void Start ()
	{
		markers = new List<GameObject> ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (debugMouseRay)
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			Color color = Color.yellow;
			if (Physics.Raycast(ray))
			{
				color = Color.green;
			}
			Debug.DrawRay (ray.origin, ray.direction * 10, color);
		}

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
						if (battleManager.CanMoveToTile(activeCharacter.currentTile, hitTile))
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
			// Indicate active character
			if (activeCharacterArrow != null)
			{
				MarkCharacter(character, activeCharacterArrow);
			}

			// Preview move range
			if (moveRangeMaterial != null)
			{
				List<Tile> tilesToHighlight = new List<Tile>();
				TeamGrid teamGrid = character.currentTile.GetTeamGrid();
				foreach (Tile tile in teamGrid.tiles)
				{
					if (battleManager.CanMoveToTile(character.currentTile, tile))
					{
						tilesToHighlight.Add (tile);
					}
				}
				HighlightTiles (tilesToHighlight, moveRangeMaterial);
			}

			// Preview attack range
			//TODO: refresh after move commit
			if (abilityRangeArrow != null)
			{
				Ability_BasicMelee meleeAbility = character.GetComponentInParent<Ability_BasicMelee>();
				if (meleeAbility != null && meleeAbility.IsValid())
				{
					List<Character> targetableCharacters = meleeAbility.GetTargetableCharacters();
					MarkCharacters(targetableCharacters, abilityRangeArrow);
				}
			}
		}
	}

	public void HandleEndCharacterTurn(Character character)
	{
		UnhighlightAll ();
		UnmarkAll ();
	}

	void PreviewCharacterMove(Character character, int rowDelta, int columnDelta)
	{
		if (character != null)
		{
			int destinationRow = character.previewTile.row + rowDelta;
			int destinationColumn = character.previewTile.column + columnDelta;
			if (battleManager.IsValidTile (character.teamID, destinationRow, destinationColumn))
			{
				TeamGrid teamGrid = battleManager.battleGrid.teamGrids[character.teamID];
				Tile destinationTile = teamGrid.GetTile(destinationRow, destinationColumn);
				PreviewCharacterMove(character, destinationTile);
			}
		}
	}

	void PreviewCharacterMove(Character character, Tile destinationTile)
	{
		if (character != null)
		{
			if (battleManager.CanMoveToTile(character.currentTile, destinationTile))
			{
				// Don't set the character's current tile until we confirm
				character.previewTile = destinationTile;
				character.transform.position = destinationTile.gameObject.transform.position;
			}
		}
	}

	void MarkCharacters(List<Character> characters, GameObject marker)
	{
		foreach (Character character in characters)
		{
			MarkCharacter (character, marker);
		}
	}

	void MarkCharacter(Character character, GameObject marker)
	{
		if (character.currentTile != null)
		{
			MarkTile(character.currentTile, marker);
		}
	}

	void MarkTile(Tile tile, GameObject marker)
	{
		Vector3 position = tile.transform.position + new Vector3 (0f, 1.5f, 0f); //TODO: make as variable, or anchor point in world
		GameObject markerInstance = (GameObject) Instantiate(marker, position, Quaternion.identity) as GameObject;
		markers.Add (markerInstance);
	}

	void UnmarkAll()
	{
		foreach (GameObject marker in markers)
		{
			GameObject.Destroy (marker);
		}
	}

	void HighlightTiles(List<Tile> tiles, Material material)
	{
		foreach (Tile tile in tiles)
		{
			MeshRenderer tileRenderer = (MeshRenderer) tile.GetComponentInChildren<MeshRenderer>();
			tileRenderer.material = material;
		}
	}

	void UnhighlightAll()
	{
		foreach (TeamGrid teamGrid in battleManager.battleGrid.teamGrids)
		{
			foreach (Tile tile in teamGrid.tiles)
			{
				MeshRenderer tileRenderer = (MeshRenderer) tile.GetComponentInChildren<MeshRenderer>();
				tileRenderer.material = tileBaseMaterial;
			}
		}
	}
		
}
