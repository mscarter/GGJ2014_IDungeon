using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
	static public CardManager instance;

	public Camera UICamera;
	public LayerMask mouseRayLayerMask;
	
	public CardUI[] cardsInHand;

	public GameObject completeFightButton;

	public TextMesh deckCount;

	public OpponentCard opponentCard;

	CardUI hoveredCard;

	Deck deck;

	void Awake()
	{
		instance = this;
		SetGUIActive(false);
	}

	void OnDestroy()
	{
		instance = null;
	}

	public void SetGUIActive(bool active)
	{
		gameObject.SetActive(active);
	}

	public void CreateDeck(int fighters, int mages, int thieves)
	{
		deck = new Deck();

		deck.GenerateDeck(fighters, mages, thieves);

		if (!deck.DrawCardsToHand())
		{
			Debug.LogError("Didn't even have enough cards in deck for 1 hand!");
		}

		SetCardVisualizations();
	}
	
	void SetCardVisualizations()
	{
		for (int i = 0; i < cardsInHand.Length; ++i)
		{
			if (i >= deck.currentHand.Length)
			{
				cardsInHand[i].gameObject.SetActive(false);
			}
			else
			{
				cardsInHand[i].gameObject.SetActive(true);
				cardsInHand[i].SetCardIcon(deck.currentHand[i]);
			}
		}

		deckCount.text = deck.cardsRemaining.ToString();

		completeFightButton.SetActive(false);
	}	

	void Update()
	{
		bool overCompleteFight = false;

		// Check for a card under the mouse cursor
		var cursorRay = UICamera.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		if (Physics.Raycast(cursorRay, out hit, 2, mouseRayLayerMask))
		{
			var cardOver = hit.collider.gameObject.GetComponent<CardUI>();

			ChangeHoveredCard(cardOver);

			overCompleteFight = hit.collider.gameObject == completeFightButton;
		}
		else
		{
			ChangeHoveredCard(null);
		}

		// Check for click on hovered card
		if (Input.GetMouseButtonDown(0))
		{
			ToggleCardSelected();

			if (overCompleteFight)
			{
				CompleteFight();
			}
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

	void ToggleCardSelected()
	{
		if (null == hoveredCard)
		{
			return;
		}

		hoveredCard.SetSelected(!hoveredCard.selected);

		if (RoomState.instance.DoSelectedCardsDefeatOpponent())
		{
			foreach (var card in cardsInHand)
			{
				card.glow = card.selected;
			}

			completeFightButton.SetActive(true);
		}
		else
		{
			foreach (var card in cardsInHand)
			{
				card.glow = false;
			}
			
			completeFightButton.SetActive(false);
		}
	}

	void CompleteFight()
	{
		// Double check that we have the fight ready to complete
		if (!RoomState.instance.DoSelectedCardsDefeatOpponent())
		{
			Debug.LogWarning("Somehow clicked defeate opponent without having a good card set");
			return;
		}

		RoomState.instance.OpponentDefeated();

		for (int i = 0; i < cardsInHand.Length; ++i)
		{
			if (cardsInHand[i].selected)
			{
				deck.currentHand[i] = CardIcons.None;
				cardsInHand[i].RedrawCard();
			}

			if (!deck.DrawCardsToHand())
			{
				RoomState.instance.DisplayYouLose();
			}

			SetCardVisualizations();
		}
	}
	
	public IEnumerable<CardIcons> GetSelectedCards()
	{
		for (int i = 0; i < cardsInHand.Length; ++i)
		{
			if (cardsInHand[i].selected)
			{
				yield return deck.currentHand[i];
			}
		}
	}
}
