using UnityEngine;
using System.Collections.Generic;

public enum AbilityCategory
{
	Attack,
	Magic,
	Item,
	RunAway,
};

// TODO: this probably will need something beefier than an enum
public enum AbilityType
{
	Melee,
	Ranged,
	AOE,
};

public enum AbilityTargetingType
{
	Unit,
	Tile,
	Direction,
	Row,
	Column,
};

public struct AbilityTile
{
	int RowDelta;
	int ColumnDelta;
};

// Abstract class for defining character abilities on the battlefield
public abstract class Ability : MonoBehaviour
{
	// Ability Properties
	public abstract AbilityCategory 		Category{ get; } // Determines sub-menu on the battle screen
	public abstract AbilityType				AbType{ get; }
	public abstract AbilityTargetingType	TargetingType{ get;}
	public abstract int						DamageOrHealAmount{ get; } // Base amount modified by character/weapon stats
	public int								CastingSpeed; // turns needed to "charge up" ability
	public int								ManaCost;
	public int								CooldownTime; // num of this character's turns
	public bool								bTargetsEnemies;
	public bool								bTargetsAllies;
	public List<AbilityTile>				TargetTiles;
	public List<AbilityTile>				RequiredFormation; // May need different struct, could be defined by pattern not relative to character's tile
	
	// Ability State
	public bool					bOnCooldown;
	public int					CooldownTimeRemaining;
	public bool					bCurrentlySelected; // Is this ability currently being used?
	public Tile					TargetedTile;
	public Character			TargetedCharacter;

	// TODO: Targeted row/column

	//#####################################################################################
	// ------------------- METHOD USEABILITY ----------------------------------------------
	//#####################################################################################

	// Can the ability be used at all?
	public abstract bool IsValid();

	// Some ability types may have certain target requirements (e.g. enemy on same row)
	public abstract bool HasValidTargets();

	// If the ability costs mana, can the character using it afford the cost?
	public abstract bool CanAffordCost();

	//#####################################################################################
	// ------------------- TARGETING ------------------------------------------------------
	//#####################################################################################	

	// Get all targetable tiles
	public virtual List<Tile> GetTargetableTiles()
	{
		List<Tile> Tiles = new List<Tile>();

		return Tiles;
	}

	// Get all targetable characters
	public virtual List<Character> GetTargetableCharacters()
	{
		List<Character> Characters = new List<Character>();

		return Characters;
	}

	// Assign target tile
	public void TargetTile(Tile TargetTile)
	{
		TargetedTile = TargetTile;
	}

	// Assign target character
	public void TargetCharacter(Character TargetCharacter)
	{
		TargetedCharacter = TargetCharacter;
	}

	//#####################################################################################
	// ------------------- COOLDOWN -------------------------------------------------------
	//#####################################################################################

	// Does the ability have a cooldown?
	public bool HasCooldown()
	{
		return (CooldownTime > 0);
	}

	// Should only be called if HasCooldown
	public void StartCooldown()
	{
		if(CooldownTime != 0)
		{
			bOnCooldown = true;
			CooldownTimeRemaining = CooldownTime;
		}
	}

	// Should only be called if on Cooldown
	public void UpdateCooldown()
	{
		if(bOnCooldown)
		{
			CooldownTimeRemaining--;

			if(CooldownTimeRemaining <= 0)
			{
				bOnCooldown = false;
				CooldownTimeRemaining = 0;
			}
		}
	}

	//#####################################################################################
	// ------------------- USE ABILITY ----------------------------------------------------
	//#####################################################################################

	// Fire off the ability
	public virtual void UseAbility()
	{
		if(HasCooldown())
		{
			StartCooldown();
		}

		// Child classes handle rest of implementation (call super.UseAbility() first)
	}

	//#####################################################################################
	// ------------------- AOE ------------------------------------------------------------
	//#####################################################################################

	// Get all tiles affected by the ability
	public virtual List<Tile> GetAffectedTiles()
	{
		List<Tile> Tiles = new List<Tile>();
		
		return Tiles;
	}
	//{
	//	Tile[] AffectedTiles;
	//
	//	for(int idx = 0; idx < TargetTiles.Length; idx++)
	//	{
	//	
	//	}
	//}
}
