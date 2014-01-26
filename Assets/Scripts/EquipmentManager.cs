using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentManager : MonoBehaviour
{
	static public EquipmentManager instance;

	EquipmentDefinition[] equipment;

	void Awake()
	{
		instance = this;

		// Build the list of possible equipment
		equipment = GetComponentsInChildren<EquipmentDefinition>();
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
}
