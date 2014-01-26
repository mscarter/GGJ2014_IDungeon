﻿using UnityEngine;
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

	public EquipmentSlot tileSlot;

	EquipmentDefinition equipment;
	DungeonOpponent opponent;

	public MeshRenderer equipmentIcon;
	public MeshRenderer[] cardTypeGranted;
	public MeshRenderer equipmentTypeIcon;
	public TextMesh equipmentName;

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
		this.equipment = equipment;

		equipmentIcon.material = equipment.equipmentIcon;

		int cardTypeIndex = 0;
		for (int i = 0; i < equipment.fighterCardsGenerated; ++i)
		{
			cardTypeGranted[cardTypeIndex].material = EquipmentManager.instance.cardGrantedIconMaterials[(int)CardIcons.Fighter];
			++cardTypeIndex;
		}
		for (int i = 0; i < equipment.mageCardsGenerated; ++i)
		{
			cardTypeGranted[cardTypeIndex].material = EquipmentManager.instance.cardGrantedIconMaterials[(int)CardIcons.Mage];
			++cardTypeIndex;
		}
		for (int i = 0; i < equipment.thiefCardsGenerated; ++i)
		{
			cardTypeGranted[cardTypeIndex].material = EquipmentManager.instance.cardGrantedIconMaterials[(int)CardIcons.Thief];
			++cardTypeIndex;
		}

		equipmentTypeIcon.material = EquipmentManager.instance.equipmentTypeIconMaterials[(int)equipment.slot];
		equipmentName.text = equipment.name;
	}

	public void ConfigureDungeonGraphics()
	{
		dungeonSide.material = DungeonManager.instance.GetBackground();
		// TODO: set door overlay
	}
}
