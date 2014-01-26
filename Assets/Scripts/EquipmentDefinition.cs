using UnityEngine;
using System.Collections;

public enum EquipmentSlot
{
	Head,
	Torso,
	Gauntlets,
	Legs,
	Shoes,
	Misc,
	MAX
}

public class EquipmentDefinition : MonoBehaviour
{
	public EquipmentSlot slot;
	public int fighterCardsGenerated;
	public int mageCardsGenerated;
	public int thiefCardsGenerated;

	public OpponentType opponentType;

	public Material equipmentIcon;
}
