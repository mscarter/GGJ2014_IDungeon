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

	bool dungeonSideSelected;
	bool attributeSideSelected;

	public bool DungeonSideSelected
	{
		set {
			dungeonGlow.gameObject.SetActive(value);
			dungeonSideSelected=false;
		}

		get { return dungeonSideSelected;}
	}

	public bool AttributeSideSelected
	{
		set {
			attributeGlow.gameObject.SetActive(value);
			attributeSideSelected=value;
		}
		get { return attributeSideSelected;}

	}
	

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

	public void SetDungeonSideSelect(bool value) 
	{
		dungeonGlow.gameObject.SetActive (value);

	}

	public void SetAttributeSideSelect(bool value)
	{
		attributeGlow.gameObject.SetActive (value);
	}

	public void SetEquipmentDefinition(EquipmentDefinition equipment)
	{
		// TODO: actually change quad materials and stuff
	}
}
