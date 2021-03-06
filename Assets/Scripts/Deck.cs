﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CardIcons
{
	None,
	Fighter,
	Mage,
	Thief,
	MAX
}

public class Deck
{
	public CardIcons[] currentHand = new CardIcons[6];

	List<CardIcons> deckList = new List<CardIcons>();

	public int cardsRemaining { get { return deckList.Count; } }

	public void GenerateDeck(int fighterCards, int mageCards, int thiefCards)
	{
		int[] cardFrequency = new int[4] { 0, fighterCards, mageCards, thiefCards };

		for (int i = 0; i < cardFrequency.Length; ++i)
		{
			for (int j = 0; j < cardFrequency[i]; ++j)
			{
				deckList.Add((CardIcons)i);
			}
		}

		ShuffleDeck();
	}

	void ShuffleDeck()
	{
		for (int i = deckList.Count - 1; i >= 0; --i)
		{
			CardIcons swap = deckList[i];
			int swapIndex = Random.Range(0, i);
			deckList[i] = deckList[swapIndex];
			deckList[swapIndex] = swap;
		}
	}

	// Returns false if there aren't enough cards to fill the hand
	public bool DrawCardsToHand()
	{
		for (int i = 0; i < currentHand.Length; ++i)
		{
			if (deckList.Count == 0)
			{
				return false;
			}
			if (currentHand[i] == CardIcons.None)
			{
				currentHand[i] = deckList[0];
				deckList.RemoveAt(0);
			}
		}

		return true;
	}
}
