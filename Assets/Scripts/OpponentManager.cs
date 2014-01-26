using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpponentManager : MonoBehaviour
{
	static public OpponentManager instance;

	public Material[] opponentIconMaterials;
	
	DungeonOpponent[] opponents;

	List<DungeonOpponent>[] opponentRandomPiles;

	void Awake()
	{
		instance = this;
		SetupLists();
	}
	
	void OnDestroy()
	{
		instance = null;
	}

	void SetupLists()
	{
		// Look up opponents
		opponents = GetComponentsInChildren<DungeonOpponent>();

		// Generate random piles
		opponentRandomPiles = new List<DungeonOpponent>[(int)OpponentType.MAX];

		for (int i = 0; i < (int)OpponentType.MAX; ++i)
		{
			opponentRandomPiles[i] = new List<DungeonOpponent>();
		}

		foreach (var badGuy in opponents)
		{
			opponentRandomPiles[(int)badGuy.opponentType].Add(badGuy);
		}
	}
	
	public IEnumerator<DungeonOpponent> GetOpponents(OpponentType opponentType)
	{
		foreach (var opponent in opponents)
		{
			if (opponent.opponentType == opponentType)
			{
				yield return opponent;
			}
		}
	}

	public DungeonOpponent PullRandomOpponent(OpponentType opponentType)
	{
		var opponentPile = opponentRandomPiles[(int)opponentType];

		int pullIndex = Random.Range(0, opponentPile.Count);

		var retVal = opponentPile[pullIndex];

		opponentPile.RemoveAt(pullIndex);

		return retVal;
	}
}
