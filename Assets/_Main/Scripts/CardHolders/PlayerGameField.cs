using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameField : CardHolder, ICardTarget
{
    [SerializeField] private float cardsSpacing = 1f;

    public override void AddCard(Card card)
    {
        base.AddCard(card);
        card.ChangeCardState(ECardState.OnGameField);
        card.RotateTo(Quaternion.Euler(-90, 180, 0), 1f);
        card.MoveTo(transform.position, 1f, UpdateCardsPositions);
    }

    public bool TryInteract(Card card)
    {
        bool canInteract = ownerId == card.OwnerId && card.cardState != ECardState.OnGameField;
        if(canInteract)
            AddCard(card);
        return canInteract;
    }

    protected override void UpdateCardsPositions()
    {
        int index = 0;
        foreach (var card in cards)
        {
            card.MoveTo(transform.position + cardsSpacing * Vector3.right * (index - cards.Count / 2f), 1f);
            index++;
        }
    }
}
