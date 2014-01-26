using UnityEngine;
using System.Collections;

public class OpponentCard : MonoBehaviour
{
	public MeshRenderer opponentIcon;
	public MeshRenderer[] cardTypeNeeded;
	public MeshRenderer opponentTypeIcon;
	public TextMesh opponentName;

	void Awake()
	{
		gameObject.SetActive(false);
	}

	public void ShowOpponent(DungeonOpponent opponent)
	{
		if (opponent == null)
		{
			gameObject.SetActive(false);
			return;
		}
		gameObject.SetActive(true);

		opponentIcon.material = opponent.opponentImage;

		opponentTypeIcon.material = OpponentManager.instance.opponentIconMaterials[(int)opponent.opponentType];

		for (int i = 0; i < cardTypeNeeded.Length; ++i)
		{
			cardTypeNeeded[i].material = EquipmentManager.instance.cardGrantedIconMaterials[(int)CardIcons.None];
		}

		int cardNeededIndex = 0;
		for (int i = 0; i < opponent.fightersNeeded; ++i)
		{
			cardTypeNeeded[cardNeededIndex].material = EquipmentManager.instance.cardGrantedIconMaterials[(int)CardIcons.Fighter];
			++cardNeededIndex;
		}
		for (int i = 0; i < opponent.magesNeeded; ++i)
		{
			cardTypeNeeded[cardNeededIndex].material = EquipmentManager.instance.cardGrantedIconMaterials[(int)CardIcons.Mage];
			++cardNeededIndex;
		}
		for (int i = 0; i < opponent.thievesNeeded; ++i)
		{
			cardTypeNeeded[cardNeededIndex].material = EquipmentManager.instance.cardGrantedIconMaterials[(int)CardIcons.Thief];
			++cardNeededIndex;
		}

		opponentName.text = opponent.name;
	}
}
