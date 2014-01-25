using UnityEngine;
using System.Collections;

public class UseCardScript : MonoBehaviour
{
	public Camera UICamera;
	public LayerMask mouseRayLayerMask;

	CardUI hoveredCard;

	void Update()
	{
		// Check for a card under the mouse cursor
		var cursorRay = UICamera.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		if (Physics.Raycast(cursorRay, out hit, 2, mouseRayLayerMask))
		{
			var cardOver = hit.collider.gameObject.GetComponent<CardUI>();

			ChangeHoveredCard(cardOver);
		}
		else
		{
			ChangeHoveredCard(null);
		}
	}

	void ChangeHoveredCard(CardUI card)
	{
		if (hoveredCard == card)
		{
			return;
		}

		if (null != hoveredCard)
		{
			hoveredCard.SetExpanded(false);
		}
		hoveredCard = card;

		if (null != hoveredCard)
		{
			hoveredCard.SetExpanded(true);
		}
	}
}
