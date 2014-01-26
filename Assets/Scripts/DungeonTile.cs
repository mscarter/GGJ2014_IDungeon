using UnityEngine;
using System.Collections;

public class DungeonTile : MonoBehaviour
{
	public MeshRenderer attributeSide;
	public MeshRenderer dungeonSide;

	public bool dungeonSideDisplayed;

	public int tileIndex;

	public MeshRenderer opponentRenderer;

	public MeshRenderer attributeGlow;

	public MeshRenderer dungeonGlow;

	public void Flip()
	{
		if (dungeonSideDisplayed)
		{
			FlipToAttribute();
		}
		else
		{
			FlipToDungeon();
		}
	}

	public void FlipToDungeon()
	{
		animation.Play("CardFlipDungeon");
		dungeonSideDisplayed = true;
	}

	public void FlipToAttribute()
	{
		animation.Play("CardFlipAttribute");
		dungeonSideDisplayed = false;
	}
}
