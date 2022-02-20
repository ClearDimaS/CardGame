using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGraveYard : CardHolder
{
    public override void AddCard(Card card)
    {
        base.AddCard(card);
        card.MoveTo(transform.position, 0.5f, () => UpdateCardsPositions());
        UpdateCardsPositions();
    }

    protected override void UpdateCardsPositions()
    {
        foreach (var item in cards)
        {
            item.RotateTo(Quaternion.Euler(90f, 0, 0), 0.3f);
        }
    }
}
