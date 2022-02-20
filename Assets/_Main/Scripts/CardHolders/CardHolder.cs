using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardHolder : MonoBehaviour, IOwnable, IHaveCards
{
    protected HashSet<Card> cards = new HashSet<Card>();
    protected int ownerId;

    public int OwnerId { get => ownerId; set => ownerId = value; }

    public virtual void AddCard(Card card)
    {
        if (card.cardHolder != null)
            card.cardHolder.RemoveCard(card);
        card.SetCardHolder(this);
        card.ChangeCardState(ECardState.InGraveYard);
        cards.Add(card);
        card.SetIndex(cards.Count);
    }

    public virtual void RemoveCard(Card card)
    {
        cards.Remove(card);
        UpdateCardsPositions();
    }

    public bool HasCard(Card card)
    {
        return cards.Contains(card);
    }

    protected abstract void UpdateCardsPositions();
}
