using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
	public string properName;
	public int maxHealth;
	public int currentHealth;
	public int teamID;
	public int initiative;
	public Tile currentTile; // actual position for game state
	public Tile previewTile; // for displaying move preview UI

	// Use this for initialization
	void Start ()
	{
		if (currentTile != null)
		{
			gameObject.transform.position = currentTile.transform.position;
			previewTile = currentTile;
		}
	}

	int GetRow()
	{
		if (currentTile != null)
		{
			return currentTile.row;
		}
		return -1;
	}

	int GetColumn()
	{
		if (currentTile != null)
		{
			return currentTile.column;
		}
		return -1;
	}

	public void TakeDamage(int DamageAmount)
	{
		currentHealth -= DamageAmount;
		Mathf.Clamp(currentHealth, 0, maxHealth);

		// Check for death
		if(currentHealth == 0)
		{
			// BattleManager kills the unit
		}
	}
}
