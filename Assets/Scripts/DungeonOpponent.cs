using UnityEngine;
using System.Collections;

public enum OpponentType
{
	None,
	Trap,
	Mook,
	Monster,
	MAX
}

public class DungeonOpponent : MonoBehaviour
{
	public OpponentType opponentType;
	public int fightersNeeded;
	public int magesNeeded;
	public int thievesNeeded;

	public Material opponentImage;
}
