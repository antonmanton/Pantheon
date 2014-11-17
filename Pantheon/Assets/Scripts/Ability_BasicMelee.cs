using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof (Character))]
public class Ability_BasicMelee : Ability 
{
	public override AbilityCategory 	 Category{get{return AbilityCategory.Attack;}}
	public override AbilityType 		 AbType{get{return AbilityType.Melee;}}
	public override AbilityTargetingType TargetingType{get{return AbilityTargetingType.Unit;}}
	public override int 				 DamageOrHealAmount{get{return 1;}}

	//#####################################################################################
	// ------------------- METHOD USEABILITY ----------------------------------------------
	//#####################################################################################
	
	// Can the ability be used at all?
	public override bool IsValid()
	{
		return (HasValidTargets() && CanAffordCost());
	}
	
	// Some ability types may have certain target requirements (e.g. enemy on same row)
	public override bool HasValidTargets()
	{
		return (GetTargetableCharacters().Count > 0);
	}
	
	// If the ability costs mana, can the character using it afford the cost?
	public override bool CanAffordCost()
	{
		return true;
	}
	
	//#####################################################################################
	// ------------------- TARGETING ------------------------------------------------------
	//#####################################################################################	
	
	// Get all targetable characters
	public override List<Character> GetTargetableCharacters()
	{
		// Get My Character
		Character MyCharacter = GetComponent<Character>();

		// List toReturn of targets
		List<Character> TargetCharacters = new List<Character>();

		// Grab all characters
		Character[] TargetCharArray = GameObject.FindObjectsOfType<Character>() as Character[];

		// filter out characters from our team
		for(int idx = 0; idx < TargetCharArray.Length; idx++)
		{
			if(TargetCharArray[idx].teamID != MyCharacter.teamID)
			{
				TargetCharacters.Add(TargetCharArray[idx]);
			}
		}

		return TargetCharacters;
	}
	
	//#####################################################################################
	// ------------------- USE ABILITY ----------------------------------------------------
	//#####################################################################################
	
	// Fire off the ability
	public override void UseAbility()
	{
		base.UseAbility();

		// Make sure you have a target
		if(TargetedCharacter == null)
		{
			return;
		}

		TargetedCharacter.TakeDamage(DamageOrHealAmount);
	}
}
