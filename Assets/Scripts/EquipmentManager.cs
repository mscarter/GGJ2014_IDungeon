using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentManager : MonoBehaviour
{
	static public EquipmentManager instance;

	public Material[] equipmentTypeIconMaterials;

	public Material[] cardGrantedIconMaterials;

	EquipmentDefinition[] equipment;

	List<EquipmentDefinition>[] equipmentRandomPiles;
	

	void Awake()
	{
		instance = this;

		// Build the list of possible equipment
		equipment = GetComponentsInChildren<EquipmentDefinition>();

		BuildEquipmentRandomPiles();
	}

	void OnDestroy()
	{
		instance = null;
	}

	public IEnumerator<EquipmentDefinition> GetEquipmentList(EquipmentSlot slot)
	{
		foreach (var piece in equipment)
		{
			if (piece.slot == slot)
			{
				yield return piece;
			}
		}
	}

	void BuildEquipmentRandomPiles()
	{
		// Generate random piles
		equipmentRandomPiles = new List<EquipmentDefinition>[(int)EquipmentSlot.MAX];
		
		for (int i = 0; i < (int)EquipmentSlot.MAX; ++i)
		{
			equipmentRandomPiles[i] = new List<EquipmentDefinition>();
		}
		
		foreach (var piece in equipment)
		{
			equipmentRandomPiles[(int)piece.slot].Add(piece);
		}
	}

	public EquipmentDefinition PullRandomEquipment(EquipmentSlot slot)
	{
		var equipmentPile = equipmentRandomPiles[(int)slot];
		
		int pullIndex = Random.Range(0, equipmentPile.Count);
		
		var retVal = equipmentPile[pullIndex];
		
		equipmentPile.RemoveAt(pullIndex);
		
		return retVal;
	}
}
