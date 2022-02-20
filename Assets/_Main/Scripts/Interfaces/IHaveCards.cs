using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHaveCards
{
    public void RemoveCard(Card card);

    public void AddCard(Card card);

    public bool HasCard(Card card);
}
